using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CVARC.V2
{
    public class ConfigurationProposal
    {
        public LoadingData LoadingData { get; set; }
        public SettingsProposal SettingsProposal { get; set; }

        public ConfigurationProposal()
        {
            LoadingData = new LoadingData();
            SettingsProposal = new SettingsProposal();
        }
    }
}
