# Deal lifecycle ownership progress

- Plan: `plans/launch/DEAL_LIFECYCLE_OWNERSHIP_PLAN.md`
- Roadmap: `plans/launch/LAUNCH_ROADMAP.md`
- Roadmap item: `launch/deal-lifecycle-ownership`
- Worktree: `C:\Users\TommySeery\source\repos\Concertable\.worktrees\Refactor-launch_deal-lifecycle-ownership`
- Branch: `Refactor/launch_deal-lifecycle-ownership`
- PR: implementation PR not opened; planning PR [#610](https://github.com/Concertable/concertable/pull/610) merged as `95b98273f59ebcadc2f9a919b8d6e04f351393d0`
- Dependency/package gates: Phase 1 is unblocked. Phase 3 requires Phase 1, the Phase 2 Payment additive package, and the additive B2B HTTP/frontend package surfaces to be merged, published or deployed, platform-synced where applicable, and restorable. The Rust decision-engine plan is downstream of Phase 3, not a blocker.
- Last reconciled: 2026-08-16 against clean implementation worktree HEAD/origin/main `95b98273f59ebcadc2f9a919b8d6e04f351393d0`

## Current state

Investigation and target design are complete. The merged baseline still models editable opportunity
terms as Deal, stores the one commercial state machine on Application, creates a stateless Booking at
Accept, and creates Concert at Booked. Contract, Invoice, workflow strategies, executors, payment
handlers, and combined dashboard queries all live in Concert. Payment correlates the same flow through
consumer-specific ApplicationId and BookingId contracts.

The investigated plan is durable on main through PR #610. Its planning worktree and the superseded
no-PR investigation worktree are closed. This clean implementation worktree is ready for Phase 1;
no product code has changed on its branch yet. The dirty main checkout and unrelated worktrees were
not modified.

## Next Steps

Implement Phase 1 as the first independently green code PR slice:

1. Add exact transition-topology characterization tests for all four current deal types, including
   failure, retry, cancellation, late-payment, and settlement recovery edges.
2. Rename the editable offer family from Deal to DealTerms across the Deal module, Opportunity
   C# consumers, seed data, and tests; rename `OpportunityEntity.DealId` to `DealTermsId` without
   changing runtime or HTTP behaviour. Keep the existing wire and frontend package names until the
   additive Phase 2 producer surfaces are published.
3. Preserve the two current module-local strategy builders in this phase; update architecture guidance
   to distinguish DealTerms from the future concrete Deal.
4. Re-scaffold B2B initial migrations, run the smallest affected Deal/Concert builds and focused unit
   and integration tests, then open the draft PR so remote CI validates the exact head.
5. Reconcile this ledger with commit, PR, verification, and review evidence. Do not begin Phase 3 until
   both published-boundary expansion gates recorded in the plan are green.

## Completed work

- Investigated the merged Deal/Concert strategy registration, lifecycle graph, entity relationships,
  workflow implementations, executors/steps, module facades, dashboard queries, frontend routes,
  Payment contracts/metadata/persistence, and the legacy Rust decision-engine plan.
- Decided the target aggregate, module boundary, Booking removal, invariant enforcement, naming, and
  multi-PR package cut-over sequence in `DEAL_LIFECYCLE_OWNERSHIP_PLAN.md`.
- Corrected the frontend delivery sequence for the published `@concertable/b2b` package and blocked
  the stale Rust extraction plan until the Deal ownership cut-over has landed and been reconciled.
- Planning baseline: `6870e8e05`; delivery-gate corrections: `ec368204c`; implementation handoff
  metadata: `688c3f966`.
- Docs-only planning PR #610 merged as `95b98273f59ebcadc2f9a919b8d6e04f351393d0`;
  its plan-managed worktree and the superseded investigation worktree were removed with repository
  automation.

## Verification

- `git fetch origin --quiet`: refreshed the planning baseline.
- `git rev-list --count HEAD..origin/main`: `0` in this worktree before plan edits.
- `git status --short --branch`: clean `Docs/DealLifecycleOwnershipPlan...origin/main` before plan edits.
- Repository searches confirmed Payment and the B2B SPAs are real consumers of Application/Booking
  identity, so Booking removal requires a package/API cut-over rather than a local entity deletion.
- `python .agents/hooks/plan_graph.py --root C:\Users\TommySeery\source\repos\Concertable\.worktrees\Docs-DealLifecycleOwnershipPlan`: 0 errors, 0 warnings.
- `git diff --check`: passed.
- Initial work push verified local HEAD, `origin/Docs/DealLifecycleOwnershipPlan`, and PR #610
  `headRefOid` all equal `c0f4e1d5274c0d8cb69865ad8708734f43adec5a`.
- PR #610 was current with main, contained only four Markdown paths, carried `skip-e2e`, and was
  admin-merged without E2E. No `api/**` path changed, so no platform-sync was created for this PR.
- `Refactor/launch_deal-lifecycle-ownership` was created cleanly from merge commit
  `95b98273f59ebcadc2f9a919b8d6e04f351393d0`; the open platform-sync PR #608 was green at creation.
- No build or test run yet: this checkpoint changes planning documents only.

## Reviews

- Docs review: clean after the published-frontend sequence, Rust prerequisite, package-cutover phase,
  and ledger evidence were corrected; final watermark `883a07dc248c7f8250f14049af194f3a8de3ea78`.
- The spent docs-review work order was deleted after PR #610 merged.
- Review the Phase 1 code diff after implementation and focused verification.

## Decisions, discoveries, blockers, and deviations

- `Deal` is the concrete artist–venue agreement-in-progress; the current `DealEntity` is DealTerms.
- Concert remains separate and is referenced by nullable `ConcertId` on Deal, giving a one-way
  Deal → Concert.Contracts module dependency.
- Booking has no independent invariant after Deal becomes the stable identity and will be removed,
  while application/booking words may survive only on honest phase-specific projections and UI copy.
- One `WorkflowRegistry` replaces separate state-machine and capability registries. Internal workflow
  and strategy infrastructure uses namespace-scoped short names instead of `ConcertDeal...` prefixes.
- `DealState` is a persistence union, not an authorization list. The immutable per-type machine,
  aggregate machine/type check, single transition path, builder validation, and exact topology tests
  enforce the state subset for each `DealType`.
- Payment must stay adapter-agnostic. Its final correlation term is `ExternalReference`, with B2B using
  `deal:{id}`; Payment does not receive a parameter called `DealId`.
- Venue and Artist consume the published `@concertable/b2b` package, so their DealTerms/Deal resource
  migration follows an additive package and HTTP expansion, consumer cut-over, then cleanup.
- The Rust engine remains stateless and downstream. Its legacy plan must be reconciled after the B2B
  ownership cut-over because its current extraction names and lifecycle model are stale.

## Resume prompt

```
cd C:\Users\TommySeery\source\repos\Concertable\.worktrees\Refactor-launch_deal-lifecycle-ownership
Read @plans/launch/DEAL_LIFECYCLE_OWNERSHIP_PLAN.md and @plans/launch/DEAL_LIFECYCLE_OWNERSHIP_PROGRESS.md and do what its `## Next Steps` says.
```
