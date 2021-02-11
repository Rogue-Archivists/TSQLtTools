using System;
using System.Diagnostics;
using Microsoft;
using Shell = Microsoft.VisualStudio.Shell;
using Microsoft.VisualStudio.Shell.Interop;
using Task = System.Threading.Tasks.Task;
using System.Diagnostics.CodeAnalysis;

internal static class Logger
{
    private static string _name;
    private static IVsOutputWindowPane _pane;
    private static IVsOutputWindow _output;

    public static async Task InitializeAsync(Shell.IAsyncServiceProvider provider, string name)
    {
        await Shell.ThreadHelper.JoinableTaskFactory.SwitchToMainThreadAsync();
        _output = await provider.GetServiceAsync(typeof(SVsOutputWindow)) as IVsOutputWindow;
        Assumes.Present(_output);
        _name = name;
    }

    [SuppressMessage("Microsoft.Usage", "CA1806:DoNotIgnoreMethodResults", MessageId = "Microsoft.VisualStudio.Shell.Interop.IVsOutputWindowPane.OutputString(System.String)")]
    public static void Log(string message)
    {
        if (string.IsNullOrEmpty(message))
            return;

        try
        {
            if (EnsurePane())
            {
                _pane.OutputString(DateTime.Now.ToString() + ": " + message + Environment.NewLine);
            }
        }
        catch (Exception ex)
        {
            System.Diagnostics.Debug.Write(ex);
        }
    }


    public static void Log(Exception ex)
    {
        if (ex != null)
        {
            Log(ex.ToString());
        }
    }

    public static async Task LogToOutputWindowAsync(object message)
    {
        try
        {
            await Shell.ThreadHelper.JoinableTaskFactory.SwitchToMainThreadAsync();

            if (EnsurePane())
            {
                _pane.OutputStringThreadSafe(message + Environment.NewLine);
            }
        }
        catch (Exception ex)
        {
            Debug.Write(ex);
        }
    }

    private static bool EnsurePane()
    {
        Shell.ThreadHelper.ThrowIfNotOnUIThread();
        if (_pane == null)
        {
            var guid = Guid.NewGuid();
            _output.CreatePane(ref guid, _name, 1, 1);
            _output.GetPane(ref guid, out _pane);
        }

        return _pane != null;
    }
}