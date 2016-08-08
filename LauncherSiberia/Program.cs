using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Drawing;
using System.Windows.Forms;
using System.Reflection;
using System.IO;
namespace LauncherSiberia
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
            AppDomain.CurrentDomain.AssemblyResolve += new ResolveEventHandler(CurrentDomain_AssemblyResolve);
            Application.Run(new Form1());
        }
        static Assembly CurrentDomain_AssemblyResolve(object sender, ResolveEventArgs args)
        {
            using (var stream = Assembly.GetExecutingAssembly().GetManifestResourceStream("LauncherSiberia.Ionic.Zip.dll"))
            {
                byte[] assemblyData = new byte[stream.Length];
                stream.Read(assemblyData, 0, assemblyData.Length);
                return Assembly.Load(assemblyData);
            }
        }
        
    }

    class FsProgressBar : Control
    {
        public enum FsProgressTextType
        {
            AsIs, Percent
        }

        //Cal when Value was changed
        public event EventHandler ValueChanged;
        //Call when Value == MaxValue
        public event EventHandler ValuesIsMaximum;

        private Int32 minValue;
        public Int32 MinValue
        {
            get
            {
                return this.minValue;
            }
            set
            {
                if (value >= this.MaxValue)
                {
                    throw new Exception("MinValue must be less than MaxValue");
                }
                this.minValue = value;
                this.Invalidate();
            }
        }

        private Int32 maxValue;
        public Int32 MaxValue
        {
            get
            {
                return this.maxValue;
            }
            set
            {
                if (value <= this.MinValue)
                {
                    throw new Exception("MaxValue must be more than MinValue");
                }
                this.maxValue = value;
                this.Invalidate();
            }
        }

        private Int32 value;
        public Int32 Value
        {
            get
            {
                return this.value;
            }
            set
            {
                if (value < this.MinValue || value > this.maxValue)
                {
                    throw new Exception("Value must be between MinValue and MaxValue");
                }
                this.value = value;
                if (this.value == this.MaxValue && this.ValuesIsMaximum != null)
                {
                    this.ValuesIsMaximum(this, new EventArgs());
                }
                if (this.ValueChanged != null)
                {
                    this.ValueChanged(this, new EventArgs());
                }
                this.Invalidate();
            }
        }

        private Int32 borderWidth;
        public Int32 BorderWidth
        {
            get
            {
                return this.borderWidth;
            }
            set
            {
                if (value < 0)
                {
                    throw new Exception("Border width can not be negative");
                }
                this.borderWidth = value;
            }
        }

        public System.Drawing.Color BorderColor { get; set; }
        public System.Drawing.Color ProgressColor { get; set; }
        public Boolean ShowProgressText { get; set; }
        public FsProgressTextType ProgressTextType { get; set; }

        public FsProgressBar()
        {
            SetStyle(ControlStyles.UserPaint, true);
            SetStyle(ControlStyles.OptimizedDoubleBuffer, true);
            SetStyle(ControlStyles.SupportsTransparentBackColor, true);
            this.minValue = 0;
            this.maxValue = 100;
            this.value = 0;
            this.BorderWidth = 0;
            this.BorderColor = Color.Black;
            this.BackColor = SystemColors.Control;
            this.ForeColor = Color.Black;
            this.ProgressColor = Color.Yellow;
            this.ShowProgressText = true;
            this.Paint += new PaintEventHandler(FsProgressBar_Paint);
            this.Size = new Size(200, 30);
            this.ProgressTextType = FsProgressTextType.AsIs;
        }

        protected void FsProgressBar_Paint(object sender, PaintEventArgs e)
        {
            e.Graphics.FillRectangle(new SolidBrush(this.BackColor), new Rectangle(0, 0, this.Width, this.Height));
            e.Graphics.FillRectangle(new SolidBrush(this.ProgressColor), new Rectangle(0, 0, (this.Value * this.Width) / this.MaxValue, this.Height));
            if (this.BorderWidth > 0)
            {
                e.Graphics.DrawRectangle(new Pen(this.BorderColor, this.BorderWidth), this.DisplayRectangle);
            }
            if (this.ShowProgressText)
            {
                string text = String.Empty;
                switch (this.ProgressTextType)
                {
                    case FsProgressTextType.AsIs:
                        text = this.Value + " / " + this.MaxValue;
                        break;
                    case FsProgressTextType.Percent:
                        text = ((this.Value * 100) / this.MaxValue).ToString() + "%";
                        break;
                }
                System.Drawing.SizeF size = e.Graphics.MeasureString(text, this.Font);
                e.Graphics.DrawString(text, this.Font, new SolidBrush(this.ForeColor), new PointF(this.Width / 2 - size.Width / 2, this.Height / 2 - size.Height / 2));
            }
        }
    }
}
