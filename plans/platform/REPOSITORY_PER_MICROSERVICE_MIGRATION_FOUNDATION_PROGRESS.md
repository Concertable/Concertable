# Repository-per-microservice foundation progress

- Plan: `plans/platform/REPOSITORY_PER_MICROSERVICE_MIGRATION_PLAN.md`
- Roadmap: `plans/platform/POLYREPO_ROADMAP.md`
- Roadmap item: `platform/polyrepo-cut`
- Active packet: M4, monorepo closure repair
- Worktree: `C:\Users\tommy\source\repos\Concertable\.worktrees\Refactor-RepoSplit-M4-Closure-Repair`
- Branch: `Refactor/RepoSplit-M4-Closure-Repair`
- PR: not opened; remote preparation checkpoint only
- Base: current exact M1 P4 candidate and PR #945 head
  `406391952840912153587406a9d04ed1fcd8cecc` (`Refactor/M1-Platform-Contract`). P4 will be rewritten after
  PR #943 publishes the new Auth image and PR #944 pins its immutable digest.
- Dependency/package gates: the current M1 package/API shape is present locally. M4 publication and delivery
  remain gated on terminal PRs #942-#945, their ordered package/image publications, the final exact P4 head,
  and the G0 package baseline. This packet does not publish packages or images.
- Last reconciled: 2026-09-08 against `origin/main`
  `12efedd68da08d92b08990a30e76dab5546b5ed4`, corrected topology commits `82bf5dbbb` and `bb59d9ba3`,
  and current PR #945 head `406391952840912153587406a9d04ed1fcd8cecc`.

## Current state

Checkpoint 6A is terminal. M3 PR #948 and B2B producer PR #949 have landed on current main. M1 P4 provides
the exact current local platform package/API boundary required to prepare M4, including the 58-package train
and the repaired B2B `ApplicationAcceptedEvent` topology. The four M4 commits are being restacked without
changing their boundaries; final review and validation belong to the resulting exact head.

The M4 candidate replaces the final Auth.Contracts-to-Messaging cross-repository runtime source edge with the
`Concertable.Messaging.Contracts` package seam, exposes Payment through an HTTPS proxy endpoint in the B2B and
Customer standalone AppHosts, and makes inventory validation reject blocking runtime edges as well as test-tier
edges. The Auth carve gate now includes both Auth-owned source roots, so it proves Auth.Contracts restores
Messaging from the package feed rather than silently omitting the contract project.

Existing `auth`, `b2b`, `customer`, `payment`, `search`, `infra`, and `config` repositories retain their
identities. The remaining repository boundaries are `platform-dotnet`, `platform-frontend`, and `system`.
General shared frontend code covers web and mobile; web and mobile are package tiers, not repositories. M4
creates no repository and makes no topology decision.

## Next Steps

- Complete the four-commit restack onto current P4, validate and review the exact prepared candidate, and push
  one explicit remote checkpoint without opening a PR or entering the merge queue.
- After M1 PRs #942-#945 and their ordered package/image publication are terminal, restack mechanically onto
  the exact landed P4/main, revalidate against the published package baseline and pinned images, then deliver M4.

## Completed work

- Reconciled the active ledger against the corrected repository topology and current main without importing
  divergent pre-correction topology text.
- Restacked the M4 packet from obsolete P4 `4f2681974c914a15e50c6292e724e42900d3d20b` toward current P4
  `406391952840912153587406a9d04ed1fcd8cecc` while preserving its four commit boundaries.
- Replaced the Auth.Contracts `ProjectReference` to Messaging.Contracts with a centrally pinned package reference.
- Corrected the B2B and Customer Payment resource endpoints to terminate HTTPS at the Aspire proxy while keeping
  container target port 8080, and updated the owner host-graph assertions.
- Extended the split-inventory check to fail for blocking runtime edges and regenerated the inventory.
- Extended the Auth carve workflow to include and build the Auth.Contracts owner root.

## Verification

- The previous exact local M1 set `0.1.0-local.1788721241736` contained 57 packages and proved the original
  M4 Auth package-only carve plus B2B and Customer standalone closures. The current 58-package P4 restack
  requires fresh validation before its evidence is current.
- `eng/repository-split/inventory.py --check` previously passed with zero blocking runtime and test-tier edges.
- `eng/repository-split/validate_map.py` previously reported zero duplicate claims; its 79 unclaimed paths remain
  the pre-existing F0 map-admission work and are outside M4.
- No local E2E suite was run; E2E remains a remote merge-queue diagnostic gate.

## Reviews

The existing review artifact `reviews/Refactor-RepoSplit-M4-Closure-Repair.md` approved the obsolete-base
candidate through `8e46afb0d16465cf518786ffb03a0ad07ed89094`. The exact restacked head requires an incremental review
watermark before publication or delivery.

## Decisions, discoveries, blockers, and deviations

- The Payment container continues to listen on target port 8080. `WithHttpsEndpoint` changes the Aspire proxy
  discovery scheme to HTTPS; it does not require TLS inside the Payment container.
- Auth.Contracts owns its package pin because it is a separately mapped root in the retained Auth repository.
  Local M1 validation overrides that pin with the exact locally prepared platform version.
- Initial in-worktree B2B/Customer build attempts failed in MSBuild copy targets because the 265-character B2B
  path crossed the Windows path limit. Short-mounted clean archive carves proved the identical candidate; this
  was an execution-environment artifact, not a test assertion or repository-closure failure.
- Package publication, repository creation/import, and G0, C1, F0, or R1 gate execution are outside M4.
