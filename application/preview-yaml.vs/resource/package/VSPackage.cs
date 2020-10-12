
using Microsoft.VisualStudio;
using Microsoft.VisualStudio.Shell;
using System;
using System.Runtime.InteropServices;
using System.Threading;
using Task = System.Threading.Tasks.Task;

namespace resource.package
{
    [PackageRegistration(UseManagedResourcesOnly = true, AllowsBackgroundLoading = true)]
    [InstalledProductRegistration(CONSTANT.NAME, CONSTANT.DESCRIPTION, CONSTANT.VERSION)]
    [Guid(CONSTANT.GUID)]
    [ProvideAutoLoad(VSConstants.UICONTEXT.ShellInitialized_string, PackageAutoLoadFlags.BackgroundLoad)]
    public sealed class PreviewYAMLPackage : AsyncPackage
    {
        internal static class CONSTANT
        {
            public const string COPYRIGHT = "Copyright (c) 2020 by Viacheslav Lozinskyi. All rights reserved.";
            public const string DESCRIPTION = "Quick preview of YAML files";
            public const string EXTENSION1 = ".YML";
            public const string EXTENSION2 = ".YAML";
            public const string GUID = "C34A1A6D-E95B-4EB2-A0CB-7CF85D72AAF0";
            public const string NAME = "Preview-YAML";
            public const string VERSION = "1.0.5";
        }

        protected override async Task InitializeAsync(CancellationToken cancellationToken, IProgress<ServiceProgressData> progress)
        {
            {
                cartridge.AnyPreview.Connect();
                cartridge.AnyPreview.Register(cartridge.AnyPreview.MODE.PREVIEW, CONSTANT.EXTENSION1, new preview.YAML());
                cartridge.AnyPreview.Register(cartridge.AnyPreview.MODE.PREVIEW, CONSTANT.EXTENSION2, new preview.YAML());
            }
            {
                await JoinableTaskFactory.SwitchToMainThreadAsync(cancellationToken);
            }
        }

        protected override int QueryClose(out bool canClose)
        {
            {
                cartridge.AnyPreview.Disconnect();
                canClose = true;
            }
            return VSConstants.S_OK;
        }
    }
}
