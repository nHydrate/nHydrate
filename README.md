nHydrate ORM for Entity Framework Core
========

This platform creates strongly-typed, extendable classes for Entity Framework. **The generated code is Entity Framework Core. There is no difference.** This is not a replacement for EF or a custom ORM. It is a visual, modeling layer that allows a team to manage complex database schemas. It works well with SCM software like Git, allowing multi-user access to your model with minimal conflicts.

The main home page is [nHydrate.com](https://nHydrate.com)

Questions or comments contact [feedback@nhydrate.org](mailto:feedback@nhydrate.org)

nHydrate is now for Entity Framework Core only. The previous Entity Framework 4.x-6.x support can be downloaded here. It must be manually installed, as it is no longer available in the Gallery.
[Legacy Version](http://nhydrate.org/downloads/nHydrate.DslPackage.vsix.zip)

Installation
========

The plug-in is available in the Visual Studio Gallery. You can install it from the Tools menu under "Extensions and Updates". You can also  download the latest version from the [Visual Studio Gallery](https://visualstudiogallery.msdn.microsoft.com/d641354e-b15f-437c-b7eb-83c6ac423651)

Features
========
- SQL Server and PostgreSQL
- Modeler for industry standard Entity Framework
- Flexible mapping strategies
- Multi-tenant database support
- Full LINQ capabilities
- Support for audit fields (created/modified)
- Support for audit shadow tables (full row logging)
- Logging support
- Works with stored procedures and views
- Outer-join fetching, when supported by database
- Code facade naming for database entities
- Support for dynamic model
- Lazy fetching for associations
- Support for Composite Keys
- Support for Composite Indexes
- Flexible association mapping
- Supports bidirectional associations
- Metadata API


Running the code
========

To load and compile this project you need Visual Studio 2017 or greater. When installing Visual Studio, be sure to include the Modeling SDK. You can then load and build the solution.

When you build, the plugin will be automatically be installed as an extension in the Experimental Hive. You will not see the extension in the main Visual Studio setup. You must open Visual Studio as Experimental Hive. The Experimental Hive is a copy of Visual Studio with fresh settings that can be reset if needed. It is automatically installed and configured when you have the Modeling SDK on your machine.

<p align="center" style="padding-top:20px;">
<img src="http://nhydrate.org/images/nhydrate-medium.png" title="nHydrate for Entity Framework" >
</p>
