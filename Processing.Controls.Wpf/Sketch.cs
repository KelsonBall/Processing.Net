using System;
using System.Drawing;
using System.Windows;
using System.Windows.Forms;
using System.Windows.Input;
using System.Windows.Interop;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using Application = System.Windows.Application;
using Canvas = Processing.Core.Canvas;
using Image = System.Windows.Controls.Image;
using MouseEventArgs = System.Windows.Input.MouseEventArgs;

namespace Processing.Controls.Wpf
{
    public class Sketch : Image
    {
        public static readonly DependencyProperty CanvasProperty = DependencyProperty.Register("Canvas",
                typeof(Canvas),
                typeof(Sketch),
                new FrameworkPropertyMetadata(Sketch.CanvasChanged));

        public Canvas Canvas
        {
            get { return (Canvas)GetValue(Sketch.CanvasProperty); }
            set { SetValue(Sketch.CanvasProperty, value); }
        }

        static Sketch()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(Sketch), new FrameworkPropertyMetadata(typeof(Sketch)));
        }

        private void Subscribe(Canvas canvas)
        {
            if (!this.IsLoaded)
                this.Loaded += (sender, args) => LoadCanvas(canvas);
            else
                LoadCanvas(canvas);
        }

        private void LoadCanvas(Canvas canvas)
        {
            Action<Bitmap> setImage = image =>
            {
                if (Application.Current != null)
                    DispatchBitmapSet(image);
                else
                    canvas.Stop();
            };
            canvas.SetConnections(setImage,
                                handler => PreviewMouseDown += (s, a) => invoke(canvas, a, handler),
                                handler => PreviewMouseUp += (s, a) => invoke(canvas, a, handler),
                                handler => PreviewMouseMove += (s, a) => invoke(canvas, a, handler));
            canvas.Start();
        }

        private void DispatchBitmapSet(Bitmap image)
        {
            Application.Current.Dispatcher.Invoke(() => Source = Sketch.Bitmap2BitmapImage(image));
        }

        public static void CanvasChanged(DependencyObject sketch, DependencyPropertyChangedEventArgs args)
        {
            ((Canvas)args.OldValue)?.Stop();
            if (args.NewValue != null)
                ((Sketch)sketch).Subscribe(((Canvas)args.NewValue));
        }

        private void invoke(object sender, MouseEventArgs args, System.Windows.Forms.MouseEventHandler handler)
        {
            MouseButtons buttons = MouseButtons.None;
            if (args.LeftButton == MouseButtonState.Pressed)
                buttons |= MouseButtons.Left;
            if (args.MiddleButton == MouseButtonState.Pressed)
                buttons |= MouseButtons.Middle;
            if (args.RightButton == MouseButtonState.Pressed)
                buttons |= MouseButtons.Right;
            var position = args.GetPosition(this);
            Canvas canvas = (Canvas) sender;
            int x = (int) (position.X * (canvas.Width / ActualWidth));
            int y = (int)(position.Y * (canvas.Height / ActualHeight));
            System.Windows.Forms.MouseEventArgs formsArgs = new System.Windows.Forms.MouseEventArgs(buttons, 1, x, y, 0);
            handler.Invoke(sender, formsArgs);
        }

        [System.Runtime.InteropServices.DllImport("gdi32.dll")]
        public static extern bool DeleteObject(IntPtr hObject);

        private static ImageSource Bitmap2BitmapImage(Bitmap bitmap)
        {
            IntPtr hBitmap = bitmap.GetHbitmap();
            ImageSource retval;

            try
            {
                retval = Imaging.CreateBitmapSourceFromHBitmap(
                             hBitmap,
                             IntPtr.Zero,
                             Int32Rect.Empty,
                             BitmapSizeOptions.FromEmptyOptions());
            }
            finally
            {
                DeleteObject(hBitmap);
            }

            return retval;
        }
    }
}
