using System;
using System.ComponentModel.Design;
using System.IO;
using EnvDTE;
using EnvDTE80;
using Microsoft.VisualStudio.Shell;

namespace TextmateBundleInstaller
{
    internal sealed class ReportMissingLanguage
    {
        private const string _urlFormat = "https://github.com/madskristensen/TextmateBundleInstaller/issues/new?title={0}&body={1}";

        private DTE2 _dte;
        private string _ext;

        private ReportMissingLanguage(OleMenuCommandService commandService, DTE2 dte)
        {
            _dte = dte;

            var id = new CommandID(PackageGuids.guidVSPackageCmdSet, PackageIds.ReportMissingLanguage);
            var command = new OleMenuCommand(Execute, id);
            command.BeforeQueryStatus += BeforeQueryStatus;
            commandService.AddCommand(command);
        }

        public static ReportMissingLanguage Instance { get; private set; }

        public static async System.Threading.Tasks.Task Initialize(AsyncPackage package)
        {
            if (Instance == null)
            {
                var commandService = await package.GetServiceAsync(typeof(IMenuCommandService)) as OleMenuCommandService;
                var dte = await package.GetServiceAsync(typeof(DTE)) as DTE2;
                Instance = new ReportMissingLanguage(commandService, dte);
            }
        }

        private void BeforeQueryStatus(object sender, EventArgs e)
        {
            var button = (OleMenuCommand)sender;
            button.Enabled = button.Visible = false;

            var language = _dte.ActiveDocument?.Language;

            if (string.IsNullOrEmpty(language) || !language.Equals("Plain Text", StringComparison.OrdinalIgnoreCase))
                return;

            if (!CommandRegistration.IsFileSupported(_dte.ActiveDocument.FullName))
                return;

            _ext = Path.GetExtension(_dte.ActiveDocument.FullName);
            button.Text = $"Report missing language for {_ext} files...";
            button.Enabled = button.Visible = true;
        }

        private void Execute(object sender, EventArgs e)
        {
            string title = Uri.EscapeUriString($"Missing support for {_ext} files");
            string body = Uri.EscapeUriString("Optionally, please give more details such as language name and homepage URL");
            string url = string.Format(_urlFormat, title, body);

            System.Diagnostics.Process.Start(url);
        }
    }
}
