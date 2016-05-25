using System;

namespace ArdourDigital.TelligentCommunity.Core.UI
{
    public class SupplementaryFile
    {
        public SupplementaryFile(string fileName, Type type)
            : this(fileName, string.Format("{0}.Resources.{1}.{2}", type.Namespace, type.Name, fileName))
        {
        }

        public SupplementaryFile(string fileName, string resourceName)
        {
            Filename = fileName;
            ResourceName = resourceName;
        }

        public string Filename { get; private set; }

        public string ResourceName { get; private set; }
    }
}
