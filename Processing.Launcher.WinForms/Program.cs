using System;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Windows.Forms;
using Processing.Core;

namespace Processing.Launcher.WinForms
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            var form = new SketchForm();
            Action<Bitmap> setImage = image =>
                {
                    form.Invoke(
                        new MethodInvoker(
                            () =>
                                {
                                    form.BackgroundImage = image;
                                    form.Invalidate();
                                }
                            )
                        );
                };
            Assembly userAssembly = Assembly.LoadFile(Path.Combine(Environment.CurrentDirectory, @"Sketch.dll"));
            Canvas canvas =
                    (Canvas)
                    userAssembly
                    .ExportedTypes
                    .First(t => t.BaseType == typeof(Canvas))
                    .GetConstructor(new Type[] { })
                    .Invoke(null);
            form.Shown += (sender, args) =>
            {

                canvas.SetConnections(setImage,
                    handler => form.MouseDown += handler,
                    handler => form.MouseUp += handler,
                    handler => form.MouseMove += handler);
                form.Width = canvas.Width;
                form.Height = canvas.Height;
                form.FormBorderStyle = FormBorderStyle.FixedDialog;
                canvas.Start();
            };
            Application.Run(form);
        }
    }
}
