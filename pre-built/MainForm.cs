using System;
using System.Drawing;
using System.IO;
using System.Windows.Forms;
using Microsoft.Web.WebView2.Core;
using Microsoft.Web.WebView2.WinForms;

namespace Screensaver
{
    public class MainForm : Form
    {
        private WebView2 webView;

        public MainForm(Rectangle bounds)
        {
            FormBorderStyle = FormBorderStyle.None;
            StartPosition = FormStartPosition.Manual;
            Bounds = bounds;
            TopMost = true;
            Cursor.Hide();
            BackColor = Color.Black;

            webView = new WebView2();
            webView.Dock = DockStyle.Fill;
            Controls.Add(webView);

            Load += MainForm_Load;
        }

        private async void MainForm_Load(object? sender, EventArgs e)
        {
            string baseDir = AppDomain.CurrentDomain.BaseDirectory;
            string webFolder = Path.Combine(baseDir, "web");

            string webView2RuntimePath = Path.Combine(baseDir, "WebView2Runtime");
            string? runtimeToUse = Directory.Exists(webView2RuntimePath) ? webView2RuntimePath : null;

            try
            {
                var envOptions = new CoreWebView2EnvironmentOptions();
                var environment = await CoreWebView2Environment.CreateAsync(
                    runtimeToUse,
                    Path.Combine(Path.GetTempPath(), "ScreensaverData"),
                    envOptions);

                await webView.EnsureCoreWebView2Async(environment);
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    "WebView2 initialization error:\n" + ex.Message,
                    "Screensaver - Debug",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
                Close();
                return;
            }

            webView.CoreWebView2.SetVirtualHostNameToFolderMapping(
                "screensaver.app",
                webFolder,
                CoreWebView2HostResourceAccessKind.Allow);

            webView.CoreWebView2.Navigate("https://screensaver.app/index.html");
        }
    }
}
