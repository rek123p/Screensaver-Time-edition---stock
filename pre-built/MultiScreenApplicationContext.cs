using System;
using System.Collections.Generic;
using System.Windows.Forms;

namespace Screensaver
{
    public class MultiScreenApplicationContext : ApplicationContext
    {
        private readonly List<MainForm> forms = new();
        private readonly DateTime startTime = DateTime.Now;
        private bool isClosing = false;

        public MultiScreenApplicationContext()
        {
            foreach (var screen in Screen.AllScreens)
            {
                var form = new MainForm(screen.Bounds);
                form.FormClosed += (s, e) => CloseAll();
                forms.Add(form);
                form.Show();
            }

            LowLevelInputHook.Start();
            LowLevelInputHook.ActivityDetected += OnActivity;
        }

        private void OnActivity()
        {
            if ((DateTime.Now - startTime).TotalSeconds < 2) return;
            CloseAll();
        }

        private void CloseAll()
        {
            if (isClosing) return;
            isClosing = true;

            LowLevelInputHook.ActivityDetected -= OnActivity;
            LowLevelInputHook.Stop();

            foreach (var form in forms)
            {
                if (!form.IsDisposed) form.Close();
            }

            ExitThread();
        }
    }
}
