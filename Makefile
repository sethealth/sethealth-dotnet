build:
	dotnet build
test:
	dotnet test
build.release: test
	dotnet build --configuration Release

# Call after changing version number in Sethealth.net.csproj
release: build.release
	git tag v${shell grep -oP -m 1 "(?<=<Version>).*(?=</Version>)" Sethealth.net/Sethealth.net.csproj}
	git push && git push --tags


