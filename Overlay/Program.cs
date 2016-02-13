using System;
using System.Collections.Generic;
using System.Drawing;
using System.Runtime.InteropServices;
using System.Windows.Forms;
using Overlay.Native;

namespace Overlay
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            var app = new App();



            // MessageBox provides the necessary mesage loop that SetWinEventHook requires.
            // In real-world code, use a regular message loop (GetMessage/TranslateMessage/
            // DispatchMessage etc or equivalent.)
            //            MessageBox.Show("Tracking name changes on HWNDs, close message box to exit.");
            app.Run();


        }

    }
}
