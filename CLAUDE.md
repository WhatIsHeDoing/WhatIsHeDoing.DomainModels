# CLAUDE.md

Project context for Claude Code sessions.

## Project overview

**WhatIsHeDoing.DomainModels** is a .NET 10 NuGet library of strongly-typed domain model value objects (barcodes, locations, etc.). It is published to NuGet.org on git tag pushes.

Three projects in the solution:

| Project | Purpose |
|---|---|
| `WhatIsHeDoing.DomainModels` | The library — this is what gets packed and published |
| `WhatIsHeDoing.DomainModels.Tests` | xunit v3 unit tests |
| `WhatIsHeDoing.DomainModels.APITest` | ASP.NET Core demo showing the models in a Web API |

## Key commands

All day-to-day tasks are in the `justfile`. Run `just` (or `just --choose`) to pick interactively.

```
just restore      # dotnet restore + dotnet tool restore
just build        # debug build
just test         # run tests
just coverage     # run tests + produce coverage.xml (Cobertura)
just build_release
just test_release
just pack         # produce .nupkg in ./nuget/
just ci           # build_release + test_release + pack
just api          # run the demo API (Swagger UI at http://localhost:5xxx)
just outdated     # upgrade all NuGet packages
just upgrade      # interactive framework upgrade via upgrade-assistant
```

## Architecture

- **`IDomainModel<T>`** — core interface; every model implements this
- **`DomainModelBase<T>`** — abstract base handling XML, JSON, equality, hashing
- **`DomainModelJSONConverter<T>`** — Newtonsoft.Json converter
- **`DomainModelTypeConverter<T>`** — `TypeConverter` for model binding
- Models live in namespaces: `Barcodes` (EAN, ISBN), `Locations` (CountryCode, UKPostcode)

## Code standards

- **Nullable**: `<Nullable>enable</Nullable>` everywhere — all nullable warnings are errors
- **Analyzers**: StyleCop, RoslynSecurityGuard, Microsoft.CodeAnalysis.Analyzers, xunit.analyzers
- **Ruleset**: `WhatIsHeDoing.ruleset` — all CA rules enabled, `TreatWarningsAsErrors=true`
- **Suppressed globally**: `AD0001`, `CA1014`, `CA2214`
- **Tests suppress**: `CA1034` (nested types used for test class grouping)

## Testing

- **Framework**: xunit v3 via `Microsoft.Testing.Platform` (MTP) runner
- **Runner config**: `dotnet.config` sets `[dotnet.test.runner] name = "Microsoft.Testing.Platform"`
- **CRITICAL**: The MTP runner is **incompatible** with `coverlet.collector`, `coverlet.msbuild`, and `Microsoft.Testing.Extensions.CodeCoverage` (version mismatch with xunit.v3's embedded MTP). Do not add these.
- **Coverage**: Use `dotnet-coverage` (local tool, see `.config/dotnet-tools.json`). The command is `dotnet tool run dotnet-coverage collect "dotnet test" --output ./coverage.xml --output-format cobertura`
- **Coverage gate**: Codecov enforces ≥80% via `codecov.yml`

## Dependencies

- **Central Package Management**: all versions in `Directory.Packages.props` — never set a version directly in a `.csproj`
- **Dotnet tools**: all pinned in `.config/dotnet-tools.json` — run `dotnet tool restore` after cloning
- **Dependabot**: configured for NuGet (monthly) and GitHub Actions (monthly) with auto-approve + squash merge on green builds

## Versioning and publishing

- **MinVer**: versions are derived from git tags (e.g. tag `v1.2.3` → package `1.2.3`; untagged → `1.2.3-alpha.0.N`)
- **NuGet push**: only runs in CI when the triggering ref is a `v*` tag (`if: startsWith(github.ref, 'refs/tags/')`)

## CI

Two workflows in `.github/workflows/`:

- **`build.yml`**: restore → build (Release) → test with coverage → Codecov upload → pack → NuGet push (tags only)
- **`dependabot-approve.yml`**: verifies Dependabot PRs build and test, then auto-approves and squash-merges

## Adding a new domain model

1. Create the class in the appropriate namespace folder under `WhatIsHeDoing.DomainModels/`
2. Extend `DomainModelBase<T>` and implement `Construct(object? value)`
3. Add `IsValid`, `TryParse`, and an implicit cast operator following the pattern in `CountryCode.cs`
4. Add a test class in `WhatIsHeDoing.DomainModels.Tests/` mirroring the folder structure
5. Optionally add a controller in `WhatIsHeDoing.DomainModels.APITest/` for the Swagger demo
