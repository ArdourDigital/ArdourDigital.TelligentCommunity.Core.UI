using System;
using System.Collections.Generic;
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
            #if DEBUG
                Install(new Version(1, 0, 0, 0));
            #endif
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
    }
}
