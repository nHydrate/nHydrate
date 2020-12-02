To setup a binding in VSCode requires two steps.

Step 1:
Open the "tasks.json" file in your workspace ".vscode" folder. Add this snippets.

    {
      "label": "BuildModel",
      "type": "shell",
      "detail" :"BuildModel",
      "command": "FOLDER\\nHydrate.Command.Core.exe",
      "args": ["--formatmodel=\"true\"", "--model=\"${file}\"", "--output=\"${fileDirname}\""]
    }


Step 2:
Open the global "keybindings.json" file in the users folder.
%USER%\AppData\Roaming\Code\User\keybindings.json

Add this command:

    {
        "key": "ctrl+n ctrl+h",
        "command": "workbench.action.tasks.runTask",
        "args": "BuildModel"
    }

This will assign the keyboard shortcut CTRL+N CTRL+H to run the task.
