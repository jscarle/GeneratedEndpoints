# Agent Instructions

## Release Intent, Versioning, and Workflow

Treat explicit release requests—such as “Mint a new release,” “Release this,” “Publish this as a new release,” “Create a new release,” or any clear equivalent—as authorization to carry out GeneratedEndpoints' full release workflow, including committing, pushing, tagging, and publishing as needed.

If the user has not clearly requested a release, or if the appropriate change category cannot be determined confidently, ask for clarification **before** committing, pushing, creating a tag, or publishing a release.

Only change GeneratedEndpoints' package, assembly, and file versions when the user has clearly requested that a release be published. Do not bump versions during ordinary implementation, refactoring, bug-fixing, dependency updates, testing, commits, pushes, or pull-request preparation unless those actions are part of an explicitly requested release.

Choose the next version using these repository-specific rules:

- Minor, backward-compatible changes increment the patch component. Example: `10.0.5` → `10.0.6`.
- Moderate or breaking changes increment the middle component and reset the patch. Example: `10.0.5` → `10.1.0`.
- Increment the major component and reset the other components only when the newest supported .NET generation changes. Example: `10.0.5` → `11.0.0`.
- Breaking changes alone do not justify a major-version increment in this repository. If multiple categories apply, use the highest applicable category.

Update `Version` in `src/GeneratedEndpoints/GeneratedEndpoints.csproj` using three components and update `AssemblyVersion` and `FileVersion` using the matching four-component form. Use a three-component lightweight tag such as `v10.0.6`; some historical tags are annotated, but new release tags must be lightweight.

A clear release request authorizes this complete workflow:

1. Inspect the worktree, current branch, `origin`, `develop`, recent commits, pull requests, and releases before editing release metadata.
2. Determine the next version, update the three version properties in `src/GeneratedEndpoints/GeneratedEndpoints.csproj`, and verify that the tag and release do not already exist.
3. Validate the release-scoped changes using the repository's normal restore, Release build, test, and pack commands.
4. Commit all release-scoped changes on the reusable `feature/work` branch using a concise past-tense sentence ending in a period, then push `feature/work`.
5. Open a ready pull request from `feature/work` to `develop` using recent title and body conventions, enable squash auto-merge, and monitor checks and review state.
6. Fix and push any issue that prevents the pull request from merging, then wait until it is merged.
7. Fast-forward local `develop` from `origin/develop` and reset local `feature/work` to that latest `develop` commit. The remote feature branch may be deleted automatically after merge.
8. Create the lightweight `vX.Y.Z` tag on the latest `develop` commit and push that tag to `origin`.
9. Publish a non-draft GitHub release named `vX.Y.Z - Description`, using concise release-note bullets consistent with recent GeneratedEndpoints releases.
10. Monitor the release-triggered `Publish` workflow until the GeneratedEndpoints NuGet package is published successfully, fixing in-scope failures when possible.

Do not require separate confirmation for each Git or GitHub step after an unambiguous release command. Report the pull request, merge commit, tag, GitHub release URL, and NuGet publishing result when complete.
