using System.Windows.Forms;

namespace Processing.Launcher.WinForms
{
    public partial class SketchForm : Form
    {
        public SketchForm()
        {
            this.BackgroundImageLayout = ImageLayout.None;
            this.SetStyle(
                ControlStyles.AllPaintingInWmPaint |
                ControlStyles.UserPaint |
                ControlStyles.DoubleBuffer,
                true);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            InitializeComponent();
        }
    }
}
