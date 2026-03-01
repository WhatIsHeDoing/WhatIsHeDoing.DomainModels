# 🧐 Default interactive selection command.
default:
    @just --choose

# 🚀 Runs a full CI build.
ci: build_release test_release pack

# 🛜 Installs dependencies.
restore:
    dotnet restore && dotnet tool restore

# 🔨 Builds the library in debug.
[group("Debug")]
build:
    dotnet build

# 🧪 Runs the debug test project.
[group("Debug")]
test:
    dotnet test

# 🔨 Builds the library in release.
[group("Release")]
build_release:
    dotnet build --configuration Release

# 🧪 Runs the released test project.
[group("Release")]
test_release:
    dotnet test --configuration Release

# 📊 Runs tests and collects code coverage.
[group("Release")]
coverage:
    dotnet tool run dotnet-coverage collect "dotnet test" --output ./coverage.xml --output-format cobertura

# 📦 Packages the project ready to publish to NuGet.
[group("Release")]
pack:
    dotnet pack --configuration Release --no-restore --output ./nuget

# 👀 Runs the demo API project.
api:
    dotnet run --project WhatIsHeDoing.DomainModels.APITest

# 🎨 Fixes code style issues automatically.
[group("Debug")]
format:
    dotnet format style

# 🔍 Checks for code style issues without making changes.
[group("Debug")]
check-format:
    dotnet format style --verify-no-changes

# ⬆️ Upgrades all library dependencies.
outdated:
    dotnet outdated --upgrade

# ⬆️ Runs an interactive framework upgrade.
upgrade:
    # https://learn.microsoft.com/en-us/dotnet/core/porting/upgrade-assistant-how-to-upgrade#upgrade-a-project-from-the-cli
    upgrade-assistant upgrade
