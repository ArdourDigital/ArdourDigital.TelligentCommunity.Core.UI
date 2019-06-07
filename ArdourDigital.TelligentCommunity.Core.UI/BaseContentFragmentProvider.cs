using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using Telligent.Evolution.Extensibility.UI.Version1;
using Telligent.Evolution.Extensibility.Version1;

namespace ArdourDigital.TelligentCommunity.Core.UI
{
    public abstract class BaseContentFragmentProvider : IPlugin, IScriptedContentFragmentFactoryDefaultProvider, IInstallablePlugin
    {
        public abstract string Description
        {
            get;
        }

        public abstract string Name
        {
            get;
        }

        public abstract Guid ScriptedContentFragmentFactoryDefaultIdentifier
        {
            get;
        }

        public Version Version
        {
            get
            {
                return GetType().Assembly.GetName().Version;
            }
        }

        public virtual void Initialize()
        {
            if (IsDeveloperModeEnabled())
            {
                Install(new Version(1, 0, 0, 0));
            }
        }

        public virtual void Install(Version lastInstalledVersion)
        {
            var assembly = GetType().Assembly;

            foreach (var fragment in ScriptedContentFragments)
            {
                FactoryDefaultScriptedContentFragmentProviderFiles.AddUpdateDefinitionFile(this, string.Format("{0}.xml", fragment.GetType().Name), assembly.GetManifestResourceStream(fragment.DefinitionFile));

                foreach (var supplementaryFile in fragment.SupplementaryFiles)
                {
                    FactoryDefaultScriptedContentFragmentProviderFiles.AddUpdateSupplementaryFile(this, fragment.FragmentId, supplementaryFile.Filename, assembly.GetManifestResourceStream(supplementaryFile.ResourceName));
                }
            }
        }

        public virtual void Uninstall()
        {
            FactoryDefaultScriptedContentFragmentProviderFiles.DeleteAllFiles(this);
        }

        public virtual IEnumerable<IContentFragment> ScriptedContentFragments
        {
            get
            {
                var type = GetType();

                foreach (var fragmentType in type.Assembly.GetTypes().
                    Where(t => t.IsPublic
                        && t.IsClass
                        && !t.IsAbstract
                        && t.Namespace.Equals(type.Namespace, StringComparison.OrdinalIgnoreCase)
                        && t.GetInterfaces().Contains(typeof(IContentFragment))))
                {
                    yield return (IContentFragment)Activator.CreateInstance(fragmentType);
                }
            }
        }

        protected virtual bool IsDeveloperModeEnabled()
        {
            // There are Telligent ways of getting if Developer Mode is enabled
            // but these require getting hold of services from common libraries,
            // the ways of doing this and the DLLs that need to be referenced vary
            // between versions making it awkward to do in a way that supports all
            // versions. Getting it directly is easy to support (assuming this name
            // doesn't change).
            var value = ConfigurationManager.AppSettings["EnableDeveloperMode"];

            if (bool.TryParse(value, out bool result))
            {
                return result;
            }

            return false;
        }
    }
}
