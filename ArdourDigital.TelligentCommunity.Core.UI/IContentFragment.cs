using System;
using System.Collections.Generic;

namespace ArdourDigital.TelligentCommunity.Core.UI
{
    public interface IContentFragment
    {
        Guid FragmentId { get; }

        string DefinitionFile { get; }

        IEnumerable<SupplementaryFile> SupplementaryFiles { get; }
    }
}
