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
and the repaired B2B `ApplicationAcceptedEvent` topology. The four M4 commits are restacked onto that exact
P4 without changing their boundaries and are validated as a stable preparation checkpoint. The final
post-publication restack remains mechanical because P4 itself must still be rewritten before it lands.

The M4 candidate replaces the final Auth.Contracts-to-Messaging cross-repository runtime source edge with the
`Concertable.Messaging.Contracts` package seam, exposes Payment through protocol-correct HTTP discovery in the
B2B and Customer standalone AppHosts, and makes inventory validation reject blocking runtime edges as well as
test-tier edges. Payment.Hosting retains the HTTP-schemed `https` compatibility endpoint on the container's
HTTP/1-capable port 8080 for REST, webhook, and mobile callers, and adds an HTTP-schemed `grpc` endpoint on the
separate HTTP/2-only port 8081. Payment.Client prefers that h2c endpoint and retains the legacy `https` discovery
fallback for TLS-backed project hosts. The Auth carve gate now includes both Auth-owned source roots, so it proves
Auth.Contracts restores Messaging from the package feed rather than silently omitting the contract project.
Cleartext Payment call credentials are default-deny: the B2B and Customer AppHosts inject the explicit opt-in only
for local run-mode resources, while publish-mode manifests omit it.
The complete prepared candidate is independently reviewed and security-reviewed through `this commit` with
an approved judgment and no open findings.

Existing `auth`, `b2b`, `customer`, `payment`, `search`, `infra`, and `config` repositories retain their
identities. The remaining repository boundaries are `platform-dotnet`, `platform-frontend`, and `system`.
General shared frontend code covers web and mobile; web and mobile are package tiers, not repositories. M4
creates no repository and makes no topology decision.

## Next Steps

- Blocked: yes; the prepared M4 checkpoint must not enter a PR or merge queue yet.
- Blocked by: terminal M1 PRs #942-#945, their ordered package/image publications, the final rewritten P4,
  and the accepted G0 package baseline.
- Unblock action: finish #942 and publish its package train; merge #943 and publish the new Auth image; rewrite
  #944 to pin that immutable Auth digest, qualify its four standalone Auth client rosters, and merge it; rewrite
  and merge #945 on the resulting exact stack and complete its final contract/package closure.
- Resume when: those M1 package versions are feed-visible, the Auth image is digest-pinned and qualified, P4 is
  landed on `origin/main`, and G0 is accepted. Then rebase these same four M4 commits mechanically onto exact
  main, publish the repaired Payment.Client and Payment.Hosting packages plus the dual-port Payment.Web image,
  pin its immutable B2B/Customer digest, revalidate against that published baseline, and deliver M4.

## Completed work

- Reconciled the active ledger against the corrected repository topology and current main without importing
  divergent pre-correction topology text.
- Restacked the M4 packet from obsolete P4 `4f2681974c914a15e50c6292e724e42900d3d20b` onto current P4
  `406391952840912153587406a9d04ed1fcd8cecc` while preserving its four commit boundaries.
- Replaced the Auth.Contracts `ProjectReference` to Messaging.Contracts with a centrally pinned package reference.
- Restored HTTP-schemed Payment discovery in the B2B and Customer hosts while retaining the `https` endpoint name
  for REST/mobile compatibility, added a separately named h2c-only gRPC endpoint, and made Payment.Hosting own the
  container endpoint and listener-environment contract.
- Made Payment.Client prefer the `grpc` discovery key, preserve the legacy `https` fallback for project hosts,
  and fail closed before registering call credentials for cleartext HTTP unless the owning composition explicitly
  opts in through `PaymentClient:AllowInsecureHttp=true`. B2B and Customer set that opt-in only in local run mode;
  their published manifests omit it.
- Extended the split-inventory check to fail for blocking runtime edges and regenerated the inventory.
- Extended the Auth carve workflow to include and build the Auth.Contracts owner root.

## Verification

- `scripts/local-platform.ps1 prepare` produced exact local M1 version
  `0.1.0-local.1788824636009` with all 58 packages from current P4.
- `eng/repository-split/inventory.py --check` passes with zero blocking runtime and test-tier edges.
- `eng/repository-split/validate_map.py` reports 4,794 tracked/claimed paths, 82 unclaimed paths, and zero
  duplicate claims. The unclaimed paths remain the pre-existing F0 map-admission work and are outside M4.
- The Auth clean carve builds both Auth-owned roots plus the runtime/unit/integration closure with zero errors.
  Its restored asset graph resolves `Concertable.Messaging.Contracts/0.1.0-local.1788824636009` as a package and
  contains no source reference to that cross-repository contract.
- The B2B and Customer clean archive carves build their package-only solutions with zero errors against that
  exact local M1 package set. The Payment clean archive carve, including its client, hosting, web, AppHost, and
  compatibility closure, also builds with zero errors.
- All 35 B2B and all 9 Customer standalone architecture/host-graph tests pass against that package set.
- M4R1 targeted AppHost graph verification passes 4/4 B2B tests and 4/4 Customer tests against exact local
  platform version `0.1.0-local.1788721241736`; both suites assert the compatibility endpoint's HTTP scheme.
- M4R2 live Payment transport verification passes 1/1. A real Kestrel host serves HTTP/1.1 REST on one listener
  and generated Payment gRPC over a separate HTTP/2-only listener; the public Payment.Client registration prefers
  the `grpc` key over a deliberately unusable legacy endpoint and delivers the service bearer metadata.
- Local platform version `0.1.0-local.1788730449876` was freshly prepared with all 57 packages, including the
  repaired Payment.Client and Payment.Hosting. Against that feed, all 4 B2B and all 4 Customer `AppHost_` tests
  pass, covering production, Stripe publish, and mobile tunnel graphs; all 13 Payment architecture tests pass.
- M4R3 validation prepared a new exact 57-package set at `0.1.0-local.1788732761225`. Against that feed,
  Payment architecture passes 13/13, B2B architecture passes 13/13, and Customer architecture passes 9/9.
  The focused Payment transport suite passes 2/2: the explicit composition opt-in completes a real generated
  client/server bearer handshake, while the default cleartext path is rejected before client registration.
- Current-P4 focused verification passes 13/13 Payment architecture tests and 2/2 Payment transport tests;
  the latter cover the live h2c bearer handshake and default-deny cleartext registration behavior.
- `python .agents/hooks/plan_graph.py --root <M4-worktree>` passes with zero errors and zero warnings after the
  checkpoint reconciliation.
- Payment.Web builds with zero warnings and zero errors after the split-listener change.
- Evaluated SDK container metadata exposes TCP ports 8080 and 8081 and bakes the matching
  `ASPNETCORE_HTTP_PORTS` and `PaymentTransport__GrpcPort` defaults into the Payment.Web image.
- Windows verification used a temporary short drive mapping because the isolated worktree plus the longest B2B
  project path is 265 characters. Fresh archive carves eliminated the path-length artifact; no source workaround
  or reduced graph was used.
- No local E2E suite was run; E2E remains a remote merge-queue diagnostic gate.

## Reviews

The canonical review artifact `reviews/Refactor-RepoSplit-M4-Closure-Repair.md` retains the obsolete-base review
history and records a fresh full pass for the exact restacked range. The prepared candidate is approved through
`this commit` with no open findings. Its M4R1 false-HTTPS and M4R2 mixed-listener findings remain repaired by the
restacked commits; any later post-M1 movement requires an incremental watermark before delivery.

## Decisions, discoveries, blockers, and deviations

- The Payment container terminates no TLS. Port 8080 remains HTTP/1-capable for REST, webhooks, mobile tunnelling,
  and the `https` compatibility endpoint name. Port 8081 is a distinct HTTP/2-only h2c listener exposed as `grpc`.
  Payment.Client enables insecure-channel call credentials only for an `http` address carrying the explicit
  `PaymentClient:AllowInsecureHttp=true` composition opt-in; otherwise registration fails closed. Aspire does not
  terminate TLS on behalf of either cleartext container target.
- M4 delivery requires a coordinated Payment publication: consumers must not receive the new `grpc`-preferring
  Payment.Client/Hosting packages until a Payment.Web image containing the 8081 listener exists, and B2B/Customer
  must pin that immutable image digest before their standalone AppHosts are delivered.
- Auth.Contracts owns its package pin because it is a separately mapped root in the retained Auth repository.
  Local M1 validation overrides that pin with the exact locally prepared platform version.
- Initial in-worktree B2B/Customer build attempts failed in MSBuild copy targets because the 265-character B2B
  path crossed the Windows path limit. Short-mounted clean archive carves proved the identical candidate; this
  was an execution-environment artifact, not a test assertion or repository-closure failure.
- Package publication, repository creation/import, and G0, C1, F0, or R1 gate execution are outside M4.
