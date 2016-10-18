using System;
using System.ComponentModel.Composition;
using System.IO;
using System.Linq;
using Microsoft.VisualStudio.Shell;
using Microsoft.VisualStudio.Shell.Interop;
using Microsoft.VisualStudio.Text;
using Microsoft.VisualStudio.Text.Editor;
using Microsoft.VisualStudio.Utilities;

namespace TextmateBundleInstaller
{
    [Export(typeof(IWpfTextViewCreationListener))]
    [ContentType(_contentType)]
    [TextViewRole(PredefinedTextViewRoles.Document)]
    public class ShowMessagePrompt : IWpfTextViewCreationListener
    {
        private const string _contentType = "plaintext";
        private static readonly string[] _ignoredFiles = { ".txt", ".rtf", ".log" };

        [Import]
        ITextDocumentFactoryService DocumentService { get; set; }

        [Import(typeof(SVsServiceProvider))]
        IServiceProvider ServiceProvider { get; set; }

        public void TextViewCreated(IWpfTextView textView)
        {
            if (!TextmateBundlerInstallerPackage.IsInitialized)
                return;

            ITextDocument doc = null;

            if (!DocumentService.TryGetTextDocument(textView.TextBuffer, out doc) || !IsFileSupported(doc.FilePath))
                return;

            if (ReportMissingLanguage.Instance == null && textView.TextBuffer.ContentType.TypeName == _contentType)
            {
                ShowMessageBox(doc);

                ThreadHelper.JoinableTaskFactory.Run(async () =>
                {
                    await ReportMissingLanguage.Initialize(TextmateBundlerInstallerPackage.Instance);
                });
            }
        }

        private void ShowMessageBox(ITextDocument doc)
        {
            if (!TextmateBundlerInstallerPackage.Instance.Options.ShowPromptOnPlaintextFiles)
                return;

            ThreadHelper.Generic.BeginInvoke(() =>
            {
                string ext = Path.GetExtension(doc.FilePath);
                string message = $"You can report missing language support for {ext} and other files not currently supported by Visual studio by right-clicking inside the editor";
                VsShellUtilities.ShowMessageBox(ServiceProvider, message, Vsix.Name, OLEMSGICON.OLEMSGICON_INFO, OLEMSGBUTTON.OLEMSGBUTTON_OK, OLEMSGDEFBUTTON.OLEMSGDEFBUTTON_FIRST);

                TextmateBundlerInstallerPackage.Instance.Options.ShowPromptOnPlaintextFiles = false;
                TextmateBundlerInstallerPackage.Instance.Options.SaveSettingsToStorage();
            });
        }

        public static bool IsFileSupported(string fileName)
        {
            if (string.IsNullOrEmpty(fileName))
                return false;

            var ext = Path.GetExtension(fileName)?.ToLowerInvariant();

            if (string.IsNullOrEmpty(ext))
                return false;

            return !_ignoredFiles.Contains(ext);
        }
    }
}
