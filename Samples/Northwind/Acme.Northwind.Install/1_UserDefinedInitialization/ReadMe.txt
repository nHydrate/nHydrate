Please place any custom scripts that need to run prior to all generated scripts here. Make sure they are an embedded resource

There are three ways that a database install happens:
"NewDatabase" Folder - During the process of installation the user chooses to create a new database. 
"Unversioned" Folder - During the process of installation the user chooses to upgrade a database that has not been versioned by nHydrate
"Versioned" Folder - During the process of installation the user chooses to upgrade a database that has been versioned by nHydrate
"Always" Folder: After the scenario specific scripts have run. Scripts in the "Always" folder will be run regardless of scenario.

For each specific folder custom scripts are run in Alphabetical order. For this reason it is best practice to use a number based prefix as part of your naming standard example:
000_Always
001_Always
002_Always
::::::::
999_Always
