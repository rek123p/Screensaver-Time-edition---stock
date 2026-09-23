using System;
using System.Diagnostics;
using System.IO;
using System.Windows.Forms;

namespace Screensaver
{
  static class Program
  {
    [STAThread]
    static void Main(string[] args)
    {
      Application.SetUnhandledExceptionMode(UnhandledExceptionMode.CatchException);
      Application.ThreadException += (s, e) =>
        MessageBox.Show(e.Exception.ToString(), "Screensaver - Thread Exception");
      AppDomain.CurrentDomain.UnhandledException += (s, e) =>
        MessageBox.Show(e.ExceptionObject?.ToString() ?? "unknown error", "Screensaver - Fatal Exception");

      Application.EnableVisualStyles();
      Application.SetCompatibleTextRenderingDefault(false);

      string arg = args.Length > 0 ? args[0].Trim().ToLower() : "/s";

      if (arg.StartsWith("/c"))
      {
        string baseDir = AppDomain.CurrentDomain.BaseDirectory;
        string configPath = Path.Combine(baseDir, "web", "config.json");

        try
        {
          Process.Start("notepad.exe", configPath);
        }
        catch (Exception ex)
        {
          MessageBox.Show(
            "Couldn't open config.json automatically:\n" + ex.Message +
            "\n\nYou can edit it manually here:\n" + configPath,
            "Screensaver - Configuration",
            MessageBoxButtons.OK,
            MessageBoxIcon.Information);
        }
        return;
      }
      else if (arg.StartsWith("/p"))
      {
        return;
      }
      else
      {
        Application.Run(new MultiScreenApplicationContext());
      }
    }
  }
}
