nHydrate ORM
========

This platform creates strongly-typed, extendable classes for Entity Framework. **The generated code is Entity Framework. There is no difference.** This is not a replacement for EF or a custom ORM. It is a visual, modeling layer that allows a team to manage complex database schemas. It works well with SCM software like Git, allowing multi-user access to your model with minimal conflicts.

Installation
========

The plug-in is available in the Visual Studio Gallery. You can install it from the Tools menu under "Extensions and Updates". You can also  download the latest version from the Visual Studio Gallery
https://visualstudiogallery.msdn.microsoft.com/d641354e-b15f-437c-b7eb-83c6ac423651

Running the code
========

To load and compile this project you need Visual Studio 2017 or greater. When installing Visual Studio, be sure to include the Modeling SDK. You can then load and build the solution.

When you build, the plugin will be automatically be installed as an extension in the Experimental Hive. You will not see the extension in the main Visual Studio setup. You must open Visual Studio as Experimental Hive. The Experimental Hive is a copy of Visual Studio with fresh settings that can be reset if needed. It is automatically installed and configured when you have the Modeling SDK on your machine.
