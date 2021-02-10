using System.ComponentModel.Design;
using EnvDTE;
using Microsoft;
using Microsoft.VisualStudio.Shell;
using Task = System.Threading.Tasks.Task;
using EnvDTE80;
using System;

namespace TSQLtTools.Command
{
    /// <summary>
    /// Command handler
    /// </summary>
    internal sealed class addUnitTestProj
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

            var cmdId = new CommandID(PackageGuids.guidTSQLtToolsPackageCmdSet, PackageIds.addUnitTestProjectId);
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

            string message;
            object[] activeSolutionProjects;
            Project proj;
            
            activeSolutionProjects = _dte.ActiveSolutionProjects as object[];

            if (activeSolutionProjects != null)
            {
                foreach (object objActive in activeSolutionProjects)
                {
                    proj = objActive as Project;
                    if (proj != null)
                    {
                        message = $"Package: {_package.GetType().Name} Inside {typeof(addUnitTestProj).FullName} Selected Project: {proj.Name}";
                        Logger.Log(message);
                      
                    }
                }
            }
        }




        
    }
}
