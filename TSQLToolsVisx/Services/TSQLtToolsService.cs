using System;
using System.Threading;
using Microsoft.VisualStudio.Shell;
using Task = System.Threading.Tasks.Task;

namespace TSQLtTools
{
    public class TSQLtToolsService
    {
        private EnvDTE.DTE _dte;

        public void Initialize(IServiceProvider provider)
        {
            ThreadHelper.ThrowIfNotOnUIThread();
            _dte = provider.GetService(typeof(EnvDTE.DTE)) as EnvDTE.DTE;
        }

        public async Task InitializeAsync(IAsyncServiceProvider provider, CancellationToken cancellationToken)
        {
            // Switch to the main thread - the call to AddCommand in addUnitTestProj's constructor requires
            // the UI thread.
            await ThreadHelper.JoinableTaskFactory.SwitchToMainThreadAsync(cancellationToken);
            _dte = await provider.GetServiceAsync(typeof(EnvDTE.DTE)) as EnvDTE.DTE;
        }
    }
}