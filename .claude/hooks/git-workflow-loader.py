#!/usr/bin/env python3
"""
Hook: Auto-load git-workflow when user mentions commit
Triggered by: UserPromptSubmit
"""
import json
import sys
import os

def main():
    try:
        input_data = json.load(sys.stdin)
    except:
        sys.exit(0)

    prompt = input_data.get("prompt", "").lower()

    # Detect commit-related keywords
    commit_keywords = ["commit", "提交", "git commit", "commit this"]

    if any(keyword in prompt for keyword in commit_keywords):
        # Read git-workflow.md content
        project_dir = os.environ.get("CLAUDE_PROJECT_DIR", ".")
        workflow_path = os.path.join(project_dir, ".claude/instructions/git-workflow.md")

        try:
            with open(workflow_path, "r", encoding="utf-8") as f:
                workflow_content = f.read()
        except FileNotFoundError:
            workflow_content = "Git workflow file not found."

        # Inject as additional context
        output = {
            "hookSpecificOutput": {
                "hookEventName": "UserPromptSubmit",
                "additionalContext": f"=== Git Workflow (Auto-loaded) ===\n{workflow_content}"
            }
        }
        print(json.dumps(output))

    sys.exit(0)

if __name__ == "__main__":
    main()
