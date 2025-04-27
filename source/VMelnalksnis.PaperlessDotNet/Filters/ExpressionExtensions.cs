// Copyright 2022 Valters Melnalksnis
// Licensed under the Apache License 2.0.
// See LICENSE file in the project root for full license information.

using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Runtime.Serialization;
using System.Text.Encodings.Web;
using System.Text.Json.Serialization;

using VMelnalksnis.PaperlessDotNet.Documents;

using static System.Linq.Expressions.ExpressionType;

namespace VMelnalksnis.PaperlessDotNet.Filters;

/// <summary>Extensions methods for <see cref="Expression"/>.</summary>
public static class ExpressionExtensions
{
	/// <summary>Gets a query string for documents for the given expressions.</summary>
	/// <param name="filterExpression">Expression for filtering the results.</param>
	/// <param name="orderExpression">Expression for selecting the field by which to order the results.</param>
	/// <returns>A URL formatted query string.</returns>
	public static string GetQueryString(
		this Expression<Func<DocumentFilter, bool>> filterExpression,
		Expression<Func<Document, object>>? orderExpression = null)
	{
		return filterExpression.GetQueryStringCore(orderExpression);
	}

	private static string GetQueryStringCore<TFilter, TOrder>(
		this Expression<Func<TFilter, bool>> filterExpression,
		Expression<Func<TOrder, object>>? orderExpression = null)
	{
		var parameters = filterExpression
			.Body
			.FlattenExpression()
			.Select(ToKeyValuePair);

		if (orderExpression is { Body: UnaryExpression { Operand: MemberExpression memberExpression } })
		{
			parameters = parameters.Append(new("ordering", memberExpression.GetOrderMemberName()));
		}

		return string.Join("&", parameters.Select(pair => $"{pair.Key}={UrlEncoder.Default.Encode(pair.Value)}"));
	}

	private static IEnumerable<Expression> FlattenExpression(this Expression expression) => expression switch
	{
		MethodCallExpression methodCall => [methodCall],

		BinaryExpression binary => binary.NodeType switch
		{
			AndAlso => binary.Left.FlattenExpression().Concat(binary.Right.FlattenExpression()),

			GreaterThan or GreaterThanOrEqual or LessThan or LessThanOrEqual or Equal or NotEqual => [binary],

			_ => throw new ArgumentOutOfRangeException(
				nameof(binary),
				binary,
				$"Unsupported binary expression node type {binary.NodeType}"),
		},

		_ => throw new ArgumentOutOfRangeException(
			nameof(expression),
			expression,
			$"Expression of type {expression.GetType().Name} is not supported"),
	};

	private static KeyValuePair<string, string> ToKeyValuePair(this Expression expression)
	{
		if (expression is BinaryExpression binaryExpression)
		{
			var suffix = binaryExpression.GetSuffix();

			if (binaryExpression.Left is MemberExpression memberExpression)
			{
				var value = binaryExpression.Right.Evaluate();
				if (value is bool boolValue)
				{
					value = binaryExpression.NodeType is NotEqual
						? !boolValue
						: boolValue;
				}
				else if (value is null)
				{
					value = binaryExpression.NodeType is Equal;
				}

				var memberName = memberExpression.GetFilterMemberName();

				if (suffix is not null)
				{
					memberName += $"__{suffix}";
				}

				return new(
					memberName,
					value switch
					{
						DateTime dateTime when memberName.Contains("__date") => dateTime.ToString(
							"yyyy-MM-dd",
							CultureInfo.InvariantCulture),
						DateTime dateTime => dateTime.ToString("yyyy-MM-ddTHH:mm:ss", CultureInfo.InvariantCulture),
						bool boolean => boolean.ToString().ToLowerInvariant(),
						_ => value.ToString() ?? string.Empty,
					});
			}
		}

		if (expression is MethodCallExpression methodCallExpression)
		{
			var suffix = methodCallExpression.TranslateMethod();
			if (methodCallExpression.Object is { } obj)
			{
				var arg = methodCallExpression.Arguments.Single();
				var (memberExpression, valueExpression) = (arg, obj) switch
				{
					(MemberExpression member, var other) => (member, other),
					(var other, MemberExpression member) => (member, other),
					_ => throw new NotImplementedException(),
				};

				var value = valueExpression.Evaluate() ?? throw new NotSupportedException("Method calls with null arguments are not supported");
				if (value is IEnumerable<int> values)
				{
					suffix = "in";
					value = string.Join(",", values);
				}

				var memberName = memberExpression.GetFilterMemberName();
				memberName += $"__{suffix}";

				return new(memberName, value.ToString() ?? string.Empty);
			}
			else
			{
				var value = methodCallExpression.Arguments[0].Evaluate() ?? throw new InvalidOperationException("Extension method calls on null instances are not supported");
				if (value is IEnumerable<int> values)
				{
					suffix = "in";
					value = string.Join(",", values);
				}

				var memberExpression = (MemberExpression)methodCallExpression.Arguments[1];
				var memberName = memberExpression.GetFilterMemberName();

				memberName += $"__{suffix}";

				return new(memberName, value.ToString() ?? string.Empty);
			}
		}

		throw new ArgumentOutOfRangeException(
			nameof(expression),
			expression,
			$"Unsupported expression type {expression.GetType().Name}");
	}

	private static string GetFilterMemberName(this MemberExpression expression)
	{
		var member = expression.Member;
		var memberName = member.GetCustomAttribute<DataMemberAttribute>()?.Name ?? member.Name.ToLowerInvariant();

		if (expression.Expression is not MemberExpression innerExpression)
		{
			return memberName;
		}

		var innerMemberName = innerExpression.GetFilterMemberName();
		return innerExpression.Type.IsNullableValueType()
			? innerMemberName
			: $"{innerMemberName}__{memberName}";
	}

	private static string GetOrderMemberName(this MemberExpression expression)
	{
		var member = expression.Member;
		return member.GetCustomAttribute<JsonPropertyNameAttribute>()?.Name ?? member.Name.ToLowerInvariant();
	}

	private static string TranslateMethod(this MethodCallExpression expression) => expression.Method.Name switch
	{
		nameof(string.Contains) => "icontains",
		nameof(string.EndsWith) => "iendswith",
		nameof(string.Equals) => "iexact",
		nameof(string.StartsWith) => "istartswith",
		_ => throw new ArgumentOutOfRangeException(
			nameof(expression),
			expression.Method.Name,
			"Unsupported method in method call expression"),
	};

	private static string? GetSuffix(this BinaryExpression expression) => expression switch
	{
		{ NodeType: GreaterThan } => "gt",
		{ NodeType: GreaterThanOrEqual } => "gte",
		{ NodeType: LessThan } => "lt",
		{ NodeType: LessThanOrEqual } => "lte",
		{ NodeType: Equal } when
			expression.Left.Type == typeof(string) ||
			expression.Right.Type == typeof(string) => "iexact",
		{ NodeType: Equal or NotEqual } when
			expression.Right is ConstantExpression { Value: null } ||
			expression.Left is ConstantExpression { Value: null } => "isnull",
		{ NodeType: Equal or NotEqual } => null,
		_ => throw new ArgumentOutOfRangeException(nameof(expression), expression.NodeType, string.Empty),
	};

	private static object? Evaluate(this Expression expression)
	{
		return Expression.Lambda(expression).Compile().DynamicInvoke();
	}
}
