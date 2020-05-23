using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Drawing.Imaging;
using System.Diagnostics;
using System.Threading;
namespace Render.前端部分.界面
{
    public partial class Form1 : Form
    {
        Bitmap bitmap;
        Bitmap bitmap2;
        Thread thread;

        public Form1()
        {
            InitializeComponent();
            bitmap = new Bitmap("L:\\C#\\渲染器\\Render\\Render\\前端部分\\界面\\QQ图片20200117155017.png");
            
            bitmap2 = new Bitmap("L:\\C#\\渲染器\\Render\\Render\\前端部分\\界面\\QQ图片20200117173937.png");

            ThreadStart ts = new ThreadStart(adb);
            thread = new Thread(ts);
            thread.Start();
        }

        private delegate void a();
        private void adb()
        {
            a a = new a(b);
            this.Invoke(a);

            Log.instance.writeLog("副线程");
        }

        public void b()
        {
            Thread.Sleep(3000);
            Log.instance.writeLog("主线程");
        }

        private void panel1_Click(object sender, EventArgs e)
        {
            bool a = true;
            Stopwatch sw = new Stopwatch();
            sw.Start();
            for(int i = 0; i < 100; i++)
            {
                if (a)
                {
                    //pictureBox1.Image = bitmap;
                }
                else
                    //pictureBox1.Image = bitmap2;
                a = !a;
            }
            sw.Stop();
            this.Text = sw.Elapsed.ToString();
        }
    }
}
