build:
	dotnet build
test:
	dotnet test
build.release:
	dotnet build --configuration Release

# Call after changing version number
prep.release:
	git tag v$(grep -oP -m 1 '(?<=<Version>).*(?=</Version>)' Sethealth.net/Sethealth.net.csproj)
	git push && git push --tags


