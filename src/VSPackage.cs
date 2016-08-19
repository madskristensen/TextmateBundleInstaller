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
    [Guid(PackageGuidString)]
    public sealed class VSPackage : AsyncPackage
    {
        public const string PackageGuidString = "13bea213-be01-48dc-a51e-7d148c7bec69";

        protected override async task InitializeAsync(CancellationToken cancellationToken, IProgress<ServiceProgressData> progress)
        {
            if (!BundleExtractor.HasFilesBeenCopied())
            {
                await BundleExtractor.CopyBundles();
            }
        }
    }
}
