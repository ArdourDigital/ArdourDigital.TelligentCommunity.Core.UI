# Core UI Library for Telligent Community

This library provides common UI related functionality for use when working with Telligent Community.

## Supported Versions
This library should work with the following versions of Telligent Community
- Telligent Community 9.x
- Telligent Community 8.x

## Installation

You can install the library by [downloading and referencing the latest version](https://github.com/ArdourDigital/ArdourDigital.TelligentCommunity.Core.UI/releases/latest), or by using nuget:

```
Install-Package ArdourDigital.TelligentCommunity.Core.UI
```

## Features

### Base Content Fragment Provider

The abstract `BaseContentFragmentProvider` provides a common way of managing the installation of factory default widgets, which have been created as Embedded Resources.

This is intended for use when creating a Plugin library that contains content fragments, and uses some conventions when you create widgets.

#### Creating a widget

In your class library project, create a folder to contain the widgets to be installed, and create a new class that inherits from `BaseContentFragmentProvider`. In this example the folder will be called `ContentFragments` and the class `TestContentFragmentProvider`, but you can call these whatever you would like.

In `TestContentFragmentProvider` implement the `Description`, `Name` and `Id` properties, this should give you a class similar to this:

```
using ArdourDigital.TelligentCommunity.Core.UI;
using System;
using Telligent.Evolution.Extensibility.UI.Version1;
using Telligent.Evolution.Extensibility.Version1;

namespace ArdourDigital.TestPlugin.ContentFragments
{
    public class TestContentFragmentProvider : BaseContentFragmentProvider, IPlugin, IScriptedContentFragmentFactoryDefaultProvider, IInstallablePlugin
    {
        public override string Description
        {
            get
            {
                return "Test Content Fragment Provider Implementation";
            }
        }

        public override string Name
        {
            get
            {
                return "Test Content Fragments";
            }
        }

        public override Guid ScriptedContentFragmentFactoryDefaultIdentifier
        {
            get
            {
                return new Guid("c97ad084-26d5-49bc-92f4-c9db754a0dd1");
            }
        }
    }
}
```

For each content fragment to include, create a new class in the `ContentFragments` folder that implements `IContentFragment`, for this example the class will be called `TestContentFragment`.

```
using ArdourDigital.TelligentCommunity.Core.UI;
using System;
using System.Collections.Generic;
using System.Linq;

namespace ArdourDigital.TestPlugin.ContentFragments
{
    public class TestContentFragment : IContentFragment
    {
        public string DefinitionFile
        {
            get
            {
                return "ArdourDigital.TestPlugin.ContentFragments.Resources.TestContentFragment.TestContentFragment.xml";
            }
        }

        public Guid FragmentId
        {
            get
            {
                return new Guid("6b6d83ae-99b5-4630-bf8a-c91ed7efdab1");
            }
        }

        public IEnumerable<SupplementaryFile> SupplementaryFiles
        {
            get
            {
                return Enumerable.Empty<SupplementaryFile>();
            }
        }
    }
}
``` 

In the `ContentFragments` folder create a folder named `Resources` and inside that one with the same name as your widget class (e.g. `TestContentFragment`).

Your widget definition file should be created in this folder with the same name as the widget class (e.g. `TestContentFragment.xml`), and be an Embedded Resource.

This should give a folder structure such as:

- ContentFragments
    - Resources
        - TestContentFragment
            - TestContentFragment.xml
    - TestContentFragment.cs
    - TestContentFragmentProvider.cs
    
If you build the provider and install your plugin, the widget files should be correctly installed to the filestorage.

If your widget has additional files, for example a `ui.js` file, add this to the same folder as the widget definition file as an Embedded Resource, and update the widget definition to include this as a Supplementary File, for example:

```
public IEnumerable<SupplementaryFile> SupplementaryFiles
{
    get
    {
        yield return new SupplementaryFile("ui.js", GetType());
    }
}
```