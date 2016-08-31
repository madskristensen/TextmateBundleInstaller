using System.ComponentModel;
using Microsoft.VisualStudio.Shell;

namespace TextmateBundleInstaller
{
    public class Options : DialogPage
    {
        [Category("General")]
        [DisplayName("Show message box")]
        [Description("Shows the message box explaining how to report missing language support.")]
        [DefaultValue(true)]
        public bool ShowPromptOnPlaintextFiles { get; set; } = true;
    }
}
