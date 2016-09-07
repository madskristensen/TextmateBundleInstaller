using System;
using System.Runtime.InteropServices;
using System.Threading;
using Microsoft.VisualStudio.Shell;
using Microsoft.VisualStudio.Shell.Interop;
using task = System.Threading.Tasks.Task;

namespace TextmateBundleInstaller
{
    [PackageRegistration(UseManagedResourcesOnly = true, AllowsBackgroundLoading = true)]
    [InstalledProductRegistration("#110", "#112", Vsix.Version, IconResourceID = 400)]
    [ProvideAutoLoad(UIContextGuids80.NoSolution, PackageAutoLoadFlags.BackgroundLoad)]
    [ProvideOptionPage(typeof(Options), Vsix.Name, "General", 101, 102, true, new[] { "textmate", "language" })]
    [Guid(PackageGuids.guidVSPackageString)]
    [ProvideMenuResource("Menus.ctmenu", 1)]
    public sealed class TextmateBundlerInstallerPackage : AsyncPackage
    {
        private Options _options;

        public static TextmateBundlerInstallerPackage Instance
        {
            get;
            private set;
        }

        public Options Options
        {
            get
            {
                if (_options == null)
                    _options = (Options)GetDialogPage(typeof(Options));

                return _options;
            }
        }

        public static bool IsInitialized
        {
            get;
            private set;
        }

        protected override async task InitializeAsync(CancellationToken cancellationToken, IProgress<ServiceProgressData> progress)
        {
            Instance = this;

            if (!await BundleExtractor.HasFilesBeenCopied())
            {
                await BundleExtractor.CopyBundles();
            }

            IsInitialized = true;
        }
    }
}
