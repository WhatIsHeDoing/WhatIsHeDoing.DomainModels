# Contributing

Changes are welcome! Please read the following advice before submitting a new pull request:

* Search previous pull requests before adding a new one, as yours may be a duplicate.
* Create an individual pull request for each domain model, or set of related models.
* Make sure your editor respects the `.editorconfig` [configuration][editorconfig].
* Take a look through the source code and try and match its coding style to your submission.
* Add unit tests where possible, and run them before submitting :)
* Update the Web API serialisation project, `WhatIsHeDoing.DomainModels.APITests`.
* Update the `README.md` where necessary.
* Consider how the project version should be bumped: it currently follows the [SemVer][semver] scheme.
* Finally: let's try and keep this professional, and fun if possible! For more information,
see the [Contributor Covenant][covenant].

Thanks for your help!

## Releasing a new version

Only maintainers with NuGet API key access can publish a release. Publishing is triggered
automatically by CI when an annotated git tag is pushed — there is no manual upload step.

1. **Ensure CI is green** on `live` before tagging.

2. **Choose the next version** following [SemVer][semver]. The last tag can be found with:

   ```bash
   git tag --sort=-version:refname | head -1
   ```

3. **Create an annotated tag** (annotated, not lightweight — MinVer requires this for clean versioning):

   ```bash
   git tag -a v1.2.3 -m "v1.2.3"
   ```

4. **Push the tag** to trigger the publish workflow:

   ```bash
   git push origin v1.2.3
   ```

   > Do not use `git push --tags` — this pushes all local tags at once and could accidentally
   > re-trigger old versions.

5. **Verify** the [build workflow][actions] runs to completion and the package appears on NuGet.

NuGet packages are immutable once published. If a mistake is made, publish a corrected patch
release (e.g. `v1.2.4`) — the `--skip-duplicate` flag in CI prevents re-pushing the same version
from failing the pipeline, but it will not overwrite what is already on NuGet.

[actions]: https://github.com/WhatIsHeDoing/WhatIsHeDoing.DomainModels/actions
[covenant]: https://www.contributor-covenant.org/
[editorconfig]: http://editorconfig.org/
[semver]: http://semver.org/
