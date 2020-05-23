using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing.Imaging;
using System.Drawing;
namespace Render.Logic
{
    class FrameBuffer
    {
        Bitmap bitmap;
        IntPtr bitptr;
        int index;
        FrameBuffer(int width,int height,PixelFormat format,int i)
        {
            bitmap = new Bitmap(width, height, format);
            BitmapData bd = bitmap.LockBits(new Rectangle(0, 0, width, height), ImageLockMode.ReadWrite, format);
            bitptr = bd.Scan0;
            bitmap.UnlockBits(bd);
            index = i;
        }

        
    }

    class FrameBufferControl
    {
        private static FrameBufferControl instance;
        public static FrameBufferControl Instance
        {
            get
            {
                if (instance == null)
                    instance = new FrameBufferControl();
                return instance;
            }
        }

        private FrameBufferControl()
        {

        }
    }
}
