using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using Render;
using System.Threading;
using Render.前端部分.界面;
namespace Render
{
    static class WorkFlow
    {
      

        /// <summary>
        /// 应用程序的主入口点。
        /// </summary>
        [STAThread]
        static void Main()
        {
            
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            CudaUtil.InitializeResources(CommonInfo.width, CommonInfo.height);

            CommonInfo.MallocMap();
            CommonInfo.AddMapToCuda();
            mainForm mainForm = new mainForm( );
             
 
            
            
            
            Loop loop = new Loop(mainForm);
            ThreadStart renderStart = new ThreadStart(loop.loop);
            Thread renderLoop = new Thread(renderStart);
            //开启调度线程（切换图片的函数）
            ThreadStart showStart = new ThreadStart(loop.ShowImage);
            Thread showLoop = new Thread(showStart);
            renderLoop.Start();
            showLoop.Start();
            Application.Run(mainForm);
            CudaUtil.FreeResources();
        }
    }
}
