using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Render;
using System.Threading;
using System.Windows.Forms;
using System.Drawing.Imaging;
using System.Drawing;

namespace Render
{
    public enum Resolution
    {
        P128X72,
        P256X144,
        P512X288,
        P1024X576,
        P1280X720
    }
    public static class CommonInfo
    {
        public static readonly float PI = 3.14158f;
        

        public static bool isInitialized = false;
        public static bool isPressingMouseLeft = false;
        public static bool isResolutionChanged = false;
        public static Resolution resolution = Resolution.P128X72;

        public static int currentMouseX = 0;
        public static int currentMouseY = 0;
        public static int lastMouseX = 0;
        public static int lastMouseY = 0;
        public static int accumulatedX = 0;
        public static int accumulatedY = 0;

        public static ManualResetEvent mainFormHaveShown = new ManualResetEvent(false);

        public static int width = 512;
        public static int height = 288;
   

        public static bool SetPixel(int _width,int _height)
        {
            if (width * height % 4 != 0)
                return false;
            width = _width;
            height = _height;
            return true;
        }

      //  public string DebugInfo;

        public static IntPtr frame_buffer0_ptr { get; internal set; }
        public static IntPtr frame_buffer1_ptr { get; internal set; }
        public static IntPtr picking_buffer_ptr { get; internal set; }
        public static IntPtr frame_buffer2_ptr { get; internal set; }

        public static Bitmap bitmap1, bitmap2, bitmap3, pickmap;

        public static void MallocMap()
        {
            Rectangle rec = new Rectangle(0, 0, width, height);
            bitmap1 = new Bitmap(width, height, PixelFormat.Format24bppRgb);
            BitmapData bd1 = bitmap1.LockBits(rec, ImageLockMode.ReadWrite, PixelFormat.Format24bppRgb);
            frame_buffer0_ptr = bd1.Scan0;
            bitmap1.UnlockBits(bd1);

            bitmap2 = new Bitmap(width, height, PixelFormat.Format24bppRgb);
            BitmapData bd2 = bitmap2.LockBits(rec, ImageLockMode.ReadWrite, PixelFormat.Format24bppRgb);
            frame_buffer1_ptr = bd2.Scan0;
            bitmap2.UnlockBits(bd2);

            bitmap3 = new Bitmap(width, height, PixelFormat.Format24bppRgb);
            BitmapData bd3 = bitmap3.LockBits(rec, ImageLockMode.ReadWrite, PixelFormat.Format24bppRgb);
            frame_buffer2_ptr = bd3.Scan0;
            bitmap3.UnlockBits(bd3);

            pickmap = new Bitmap(width, height, PixelFormat.Format24bppRgb);
            BitmapData pkData = pickmap.LockBits(rec, ImageLockMode.ReadWrite, PixelFormat.Format24bppRgb);
            picking_buffer_ptr = pkData.Scan0;
            pickmap.UnlockBits(pkData);
            
        }

        public static void ResetMap()
        {
            Rectangle rec = new Rectangle(0, 0, width, height);
            bitmap1.SetResolution(width, height);
            BitmapData bd1 = bitmap1.LockBits(rec, ImageLockMode.ReadWrite, PixelFormat.Format24bppRgb);
            frame_buffer0_ptr = bd1.Scan0;
            bitmap1.UnlockBits(bd1);

            bitmap2.SetResolution(width, height);
            BitmapData bd2 = bitmap2.LockBits(rec, ImageLockMode.ReadWrite, PixelFormat.Format24bppRgb);
            frame_buffer1_ptr = bd2.Scan0;
            bitmap2.UnlockBits(bd2);

            bitmap3.SetResolution(width, height);
            BitmapData bd3 = bitmap3.LockBits(rec, ImageLockMode.ReadWrite, PixelFormat.Format24bppRgb);
            frame_buffer2_ptr = bd3.Scan0;
            bitmap3.UnlockBits(bd3);

            pickmap = new Bitmap(width, height, PixelFormat.Format24bppRgb);
            BitmapData pkData = pickmap.LockBits(rec, ImageLockMode.ReadWrite, PixelFormat.Format24bppRgb);
            picking_buffer_ptr = pkData.Scan0;
            pickmap.UnlockBits(pkData);
        }

        public static void AddMapToCuda()
        {
            CudaUtil.AddBufferMap(frame_buffer0_ptr);
            CudaUtil.AddBufferMap(frame_buffer1_ptr);
            CudaUtil.AddBufferMap(frame_buffer2_ptr);
            CudaUtil.AddBufferMap(picking_buffer_ptr);
        }

        public static void ChangeResolution()
        {
            switch (resolution)
            {
                case Resolution.P128X72:
                    width = 128;
                    height = 72; 
                    break;
                case Resolution.P256X144:
                    width = 256;
                    height = 144;
                    break;
                case Resolution.P512X288:
                    width = 512;
                    height = 288;
                    break;
                case Resolution.P1024X576:
                    width = 1024;
                    height = 576;
                    break;
                case Resolution.P1280X720:
                    width = 1280;
                    height = 720;
                    break;
            }
            CudaUtil.InitializeResources(width, height);
            ResetMap();
            AddMapToCuda();
            isResolutionChanged = false;
        }
    }
}
