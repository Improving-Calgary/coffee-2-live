---
name: resolve-pr-comments
description: Reads PR review comments, validates each one, and either fixes the code or responds explaining why no change is needed. Use when asked to address or resolve PR feedback.
---

# Resolve PR Review Comments

Use `gh` CLI to fetch all review comments from the PR, then for each unresolved comment:

1. **Read the referenced code** in full context.

2. **Validate** — Is this a real problem per the project's technical designs, engineering principles, and security guide? Style/formatting nits that linters handle are not worth a code change.

3. **Act** based on your assessment:
   - **Fix** — Make the minimal code change. Run relevant tests. Batch related fixes.
   - **Respond** — If incorrect, already handled, or subjective, reply explaining why. Reference specific docs or code.
   - **Clarify** — If ambiguous, ask for clarification.

4. **Summarize** — How many fixed vs. responded vs. need clarification. List files changed. Flag anything uncertain for human review.

## Rules

- Address every comment — never silently skip one.
- Do not make changes beyond what the comment asks for.
- If a comment contradicts the technical designs, side with the designs and explain why.
