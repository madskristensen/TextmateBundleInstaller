using System.ComponentModel.Composition;
using Microsoft.VisualStudio.Shell;
using Microsoft.VisualStudio.Text.Editor;
using Microsoft.VisualStudio.Utilities;

namespace TextmateBundleInstaller.Commands
{
    [Export(typeof(IWpfTextViewCreationListener))]
    [ContentType("plaintext")]
    [TextViewRole(PredefinedTextViewRoles.Document)]
    public class CommandRegistration : IWpfTextViewCreationListener
    {
        public void TextViewCreated(IWpfTextView textView)
        {
            if (ReportMissingLanguage.Instance != null)
                return;

            ThreadHelper.JoinableTaskFactory.Run(async () =>
            {
                await ReportMissingLanguage.Initialize(TextmateBundlerInstallerPackage.Instance);
            });
        }
    }
}
