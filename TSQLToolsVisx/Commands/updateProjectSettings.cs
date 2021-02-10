using EnvDTE;
using EnvDTE80;
using Microsoft;
using Microsoft.VisualStudio.Shell;
using System;
using System.ComponentModel.Design;
using Task = System.Threading.Tasks.Task;

namespace TSQLtTools.Command
{
    /// <summary>
    /// Command handler
    /// </summary>
    public sealed class updateProjectSettings
    {
        private static DTE2 _dte;
        private static AsyncPackage _package;

        public static async Task InitializeAsync(AsyncPackage package)
        {
            _package = package;

            // Switch to the main thread - the call to AddCommand in addUnitTestProj's constructor requires
            // the UI thread.
            await ThreadHelper.JoinableTaskFactory.SwitchToMainThreadAsync();

            var menuCommandService = await package.GetServiceAsync(typeof(IMenuCommandService)) as IMenuCommandService;
            Assumes.Present(menuCommandService);

            _dte = await package.GetServiceAsync(typeof(DTE)) as DTE2;
            Assumes.Present(_dte);

            var cmdId = new CommandID(PackageGuids.guidTSQLtToolsPackageCmdSet, PackageIds.ProjSettingsID);
            var cmd = new MenuCommand(Execute, cmdId);
            menuCommandService.AddCommand(cmd);
        }

        /// <summary>
        /// This function is the callback used to execute the command when the menu item is clicked.
        /// See the constructor to see how the menu item is associated with this function using
        /// OleMenuCommandService service and MenuCommand class.
        /// </summary>
        /// <param name="sender">Event sender.</param>
        /// <param name="e">Event args.</param>
        private static void Execute(object sender, EventArgs e)
        {
            ThreadHelper.ThrowIfNotOnUIThread();

            string message = $"Package: {_package.GetType().Name} inside {typeof(updateProjectSettings).FullName}";
            Logger.Log(message);
        }
    }
}
