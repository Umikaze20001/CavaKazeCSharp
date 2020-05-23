using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
using Render;
namespace Render
{
    public class ImageBuffer
    {
        
        public ImageBuffer(ManualResetEvent e)
        {
            signal = e;
 
        }
        public int index;
        public ManualResetEvent signal;
        public ImageBuffer next;
        public ImageBuffer theOther;
    }

    public class Loop
    {
        public Loop(mainForm m)
        {
            mf = m;
            renderBuffer = r2;  //bitmap1在一开始是一张默认图片，所以从bitmap2开始渲染
            showBuffer = s1;  // 从bitmap1开始显示
            //以下这些是为了方便定义的
            r1.next = r2;
            r2.next = r3;
            r3.next = r1;
            s1.next = s2;
            s2.next = s3;
            s3.next = s1;
            r1.theOther = s1;
            r2.theOther = s2;
            r3.theOther = s3;
            s1.theOther = r1;
            s2.theOther = r2;
            s3.theOther = r3;
            s1.index = 0;
            s2.index = 1;
            s3.index = 2;
            r1.index = 0;
            r2.index = 1;
            r3.index = 2;
             
        }
   
        public mainForm mf;
        private  ImageBuffer r1 = new ImageBuffer(new ManualResetEvent(false));
        private  ImageBuffer r2 = new ImageBuffer(new ManualResetEvent(true));
        private  ImageBuffer r3 = new ImageBuffer(new ManualResetEvent(true));
        private  ImageBuffer s1 = new ImageBuffer(new ManualResetEvent(true));
        private  ImageBuffer s2 = new ImageBuffer(new ManualResetEvent(false));
        private  ImageBuffer s3 = new ImageBuffer(new ManualResetEvent(false));
        
        private ImageBuffer renderBuffer;
        private ImageBuffer showBuffer;
        private AutoResetEvent paintOver = new AutoResetEvent(false);
        CommandPool commandPool = CommandPool.Instance;

        public void loop()
        {
            CommonInfo.mainFormHaveShown.WaitOne();
            while (true)
            {
                if (CommonInfo.isResolutionChanged)
                    CommonInfo.ChangeResolution();
                //更新scene
                mf.UpdatePrimitives();
                if (commandPool.Excute() > 0)
                    CudaUtil.GenerateScene();
                
                //更新面板
                mf.RefreshTransormPanel();
                //渲染图片
                renderBuffer.signal.WaitOne();  // 若当前bitmap为不可渲状态则等待，反之进行下一步

                CudaUtil.RenderScene(renderBuffer.index,3); // 这里应该是 renderScene（）
                renderBuffer.signal.Reset(); //渲染完成后，当前bitmap变为不可渲
                renderBuffer.theOther.signal.Set(); //渲染完成后，当前bitmap变为 可被前端显示（简称可显） 状态
                renderBuffer = renderBuffer.next;  // 切换到下一个bitmap
                 
            }
             
        }

        public void ShowImage()
        {
            CommonInfo.mainFormHaveShown.WaitOne();
            while (true)
            {
                showBuffer.signal.WaitOne();  //若当前bitmap为不可显则等待
                mf.UpdateImage(showBuffer.index); // 让前端显示当前bitmap
                showBuffer.signal.Reset(); // 显示完成，当前bitmap变为 不可显
                showBuffer.theOther.signal.Set(); //显示完成，当前bitmap变为可渲
                showBuffer = showBuffer.next; //切换到下一个bitmap
                
            }
        }

        //前端显示完成后会调用这个函数，告诉showImage()函数，前端已经显示完了，它可以进行下一步了
        public void SetPaintOver()
        {
            paintOver.Set();
        }



    }
}
