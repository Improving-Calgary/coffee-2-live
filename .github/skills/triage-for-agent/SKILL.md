---
name: triage-for-agent
description: Triages a user story to determine the right agent workflow — autonomous, guided with an implementation plan, or human pairing. Use when deciding how to assign a story.
---

# Triage Story for Agent Workflow

Read the spec file, then assess it against the project instructions already in your context and the existing codebase patterns. Classify the story into one of three labels:

## Labels

- **agent-sweet-spot** — The spec, acceptance criteria, and existing project instructions and codebase patterns provide enough clarity and context that a coding agent has a strong chance of completing this autonomously.
- **agent-guided** — The spec is sound but complex enough that an implementation plan should be drafted first and added to the story before handing it to a coding agent. The user must create this plan as a separate step — do not generate it here.
- **agent-pairing** — The spec is too complex, ambiguous, or requires too much contextual judgment. This should be developed locally by a human using AI as a pair programmer.

## Evaluation Criteria

Consider these factors when classifying:

1. **Spec clarity** — Are the acceptance criteria specific and testable? Are inputs/outputs well-defined?
2. **Pattern coverage** — Do similar patterns already exist in the codebase that the agent can follow?
3. **Scope** — How many files, layers, or systems does this touch? Cross-cutting changes are harder.
4. **Ambiguity** — Are there decisions that require human judgment, domain knowledge, or stakeholder input?
5. **Risk** — Could a wrong implementation cause data loss, security issues, or breaking changes?

## Output

1. **Label** — One of: `agent-sweet-spot`, `agent-guided`, `agent-pairing`.
2. **Reasoning** — A brief explanation covering which evaluation criteria drove the classification.
3. **If agent-guided** — Indicate that an interactive session should be started to indicate how the story should be implemented.
