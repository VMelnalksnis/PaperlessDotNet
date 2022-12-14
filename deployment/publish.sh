#!/bin/bash
version=$(cat version)
publish_dir="./source/$1/bin/Release"
full_version="$version.$2"
package_name="$1.$version.nupkg"
symbols_name="$1.$version.snupkg"

dotnet pack \
	./source/"$1"/"$1".csproj \
	--configuration Release \
	-p:AssemblyVersion="$full_version" \
	-p:AssemblyFileVersion="$full_version" \
	-p:PackageVersion="$version" \
	-p:InformationalVersion="$version""$3" \
	/warnAsError \
	/nologo ||
	exit

echo "artifact-name=$package_name" >> "$GITHUB_OUTPUT"
echo "artifact=$publish_dir/$package_name" >> "$GITHUB_OUTPUT"

echo "symbols-name=$symbols_name" >> "$GITHUB_OUTPUT"
echo "symbols=$publish_dir/$symbols_name" >> "$GITHUB_OUTPUT"
