using System;
using System.Runtime.InteropServices;

namespace Render
{
    class CudaUtil
    {
        static private int frame_buffer_size = 0;

        #region Console
        /// <summary>
        /// 打开控制台
        /// </summary>
        [DllImport("CudaAPI", EntryPoint = "OpenDebugConsole", CallingConvention = CallingConvention.Cdecl)]
        public static extern void OpenDebugConsole();
        #endregion

        #region Initialize Necessary Resources on CUDA
        [DllImport("CudaAPI", EntryPoint = "initializeResources", CallingConvention = CallingConvention.Cdecl)]
        unsafe private static extern void initializeResources(int _width, int _height);
        /// <summary>
        /// 指定渲染流程中的长与宽，并初始化必要的资源
        /// </summary>
        /// <param name="_width"></param>
        /// <param name="_height"></param>
        public static void InitializeResources(int _width, int _height)
        {
            frame_buffer_size = _width * _height;
            unsafe
            {
                initializeResources(_width, _height);
            }
        }
        #endregion

        #region Buffer Map Add & Free
        [DllImport("CudaAPI", EntryPoint = "addBufferMap", CallingConvention = CallingConvention.Cdecl)]
        unsafe private static extern void addBufferMap(BufferMap* bufferMap);
        /// <summary>
        /// 向C端添加BufferMap的指针, 用于Render Scene操作
        /// </summary>
        /// <param name="width"></param>
        /// <param name="height"></param>
        /// <returns></returns>
        public static void AddBufferMap(IntPtr bufferMap)
        {
            unsafe
            {
                addBufferMap((BufferMap*)bufferMap);
            }
        }

        #endregion

        #region Free Resources
        [DllImport("CudaAPI", EntryPoint = "freeResources", CallingConvention = CallingConvention.Cdecl)]
        private static extern void freeResources();
        /// <summary>
        /// 回收各种资源
        /// </summary>
        public static void FreeResources()
        {
            unsafe
            {
                freeResources();
            }
        }
        #endregion

        #region Primitive or Camera add, delete and change
        [DllImport("CudaAPI", EntryPoint = "AddSphere", CallingConvention = CallingConvention.Cdecl)]
        private static extern int AddSphere(float c_x, float c_y, float c_z, float r, float g, float b, float a, float radius);
        /// <summary>
        /// 根据参数添加球体
        /// </summary>
        /// <param name="centre"></param>
        /// <param name="materialColor"></param>
        /// <param name="radius"></param>
        public static int AddSphere(Point centre, Color materialColor, float radius)
        {
            int currentIndex=-1;
            unsafe
            {
                currentIndex = AddSphere(
                    centre.x, centre.y, centre.z,
                    materialColor.r, materialColor.g, materialColor.b, materialColor.a,
                    radius
                    );
            }
            return currentIndex;
        }
        /// <summary>
        /// 添加默认的球体
        /// </summary>
        public static int AddSphere()
        {
            int currentIndex = -1;
            currentIndex = AddSphere(new Point(0.0f,0.0f,-5.0f), Color.Red, 1.0f);
            return currentIndex;
        }

        [DllImport("CudaAPI", EntryPoint = "AddTriangle", CallingConvention = CallingConvention.Cdecl)]
        private static extern int AddTriangle(float r, float g, float b, float a, float point0_x, float point0_y, float point0_z, float point1_x, float point1_y, float point1_z, float point2_x, float point2_y, float point2_z);
        /// <summary>
        /// 根据参数添加Triangle
        /// </summary>
        /// <param name="centre"></param>
        /// <param name="materialColor"></param>
        /// <param name="points"></param>
        public static int AddTriangle(Color materialColor, Point[] points)
        {
            int currentIndex = -1;
            unsafe
            {
                currentIndex = AddTriangle(
                    materialColor.r, materialColor.g, materialColor.b, materialColor.a,
                    points[0].x, points[0].y, points[0].z,
                    points[1].x, points[1].y, points[1].z,
                    points[2].x, points[2].y, points[2].z
                    );
            }
            return currentIndex;
        }
        /// <summary>
        /// 添加默认的Triangle
        /// </summary>
        public static int AddTriangle()
        {
            int currentIndex = -1;
            Point[] points = new Point[3];
            points[0] = new Point(1.0f, 0.0f, -5.0f);
            points[1] = new Point(0.0f, 1.0f, -5.0f);
            points[2] = new Point(-1.0f, 0.0f, -5.0f);
            currentIndex = AddTriangle(new Color(), points);
            return currentIndex;
        }

        [DllImport("CudaAPI", EntryPoint = "AddPlane", CallingConvention = CallingConvention.Cdecl)]
        private static extern int AddPlane(float c_x, float c_y, float c_z, float r, float g, float b, float a, float normal_x, float normal_y, float normal_z);
        /// <summary>
        /// 根据参数添加Plane
        /// </summary>
        /// <param name="centre"></param>
        /// <param name="materialColor"></param>
        /// <param name="normal"></param>
        public static int AddPlane(Point centre, Color materialColor, Normal normal)
        {
            int currentIndex = -1;
            unsafe
            {
                currentIndex =  AddPlane(
                    centre.x, centre.y, centre.z,
                    materialColor.r, materialColor.g, materialColor.b, materialColor.a,
                    normal.x, normal.y, normal.z
                    );
            }
            return currentIndex;
        }
        /// <summary>
        /// 添加默认的Plane
        /// </summary>
        public static int AddPlane()
        {
            int currentIndex = -1;
            currentIndex = AddPlane(new Point(),new Color(),new Normal());
            return currentIndex;
        }

        /// <summary>
        /// 向GPU发送Camera的相关信息
        /// </summary>
        /// <param name="position_x"></param>
        /// <param name="position_y"></param>
        /// <param name="position_z"></param>
        /// <param name="rotation_x"></param>
        /// <param name="rotation_y"></param>
        /// <param name="rotation_z"></param>
        /// <param name="rotation_w"></param>
        /// <param name="width"></param>
        /// <param name="height"></param>
        /// <param name="fov"></param>
        /// <param name="aspectRatio"></param>
        /// <param name="viewDistance"></param>
        /// <param name="sampler"></param>
        [DllImport("CudaAPI", EntryPoint = "SendCamera", CallingConvention = CallingConvention.Cdecl)]
        public static extern void SendCamera(
        float position_x, float position_y, float position_z,
        float rotation_x, float rotation_y, float rotation_z, float rotation_w,
        int width, int height, float fov, float aspectRatio, float viewDistance, int sampler);
        public static void SendCamera(Camera camera,int width,int height)
        {
            SendCamera(
                camera.position.x, camera.position.y, camera.position.z,
                camera.rotation.imaginaryPart.x, camera.rotation.imaginaryPart.y, camera.rotation.imaginaryPart.z, camera.rotation.realPart,
                width, height, camera.fov, camera.aspectRatio, camera.viewDistance,0);
        }

        [DllImport("CudaAPI", EntryPoint = "deletePrimitive", CallingConvention = CallingConvention.Cdecl)]
        private static extern void deletePrimitive(int index);
        /// <summary>
        /// 删除在该index下的primitive
        /// </summary>
        /// <param name="index"></param>
        public static void DeletePrimitive(int index)
        {
            unsafe
            {
                deletePrimitive(index);
            }
        }

        [DllImport("CudaAPI", EntryPoint = "changePrimitive", CallingConvention = CallingConvention.Cdecl)]
        private static extern void changePrimitive(
            int index,
            Primitive_type type,
            float c_x, float c_y, float c_z,
            float r, float g, float b, float a,
            float n_x, float n_y, float n_z,
            float radius,
            float p0_x, float p0_y, float p0_z,
            float p1_x, float p1_y, float p1_z,
            float p2_x, float p2_y, float p2_z
            );
        /// <summary>
        /// 修改某个结点处的Primitive并使之变为Default
        /// </summary>
        /// <param name="index"></param>
        /// <param name="primitive"></param>
        public static void ChangePrimitive(int index, Primitive primitive)
        {
            unsafe
            {
                changePrimitive(
                    index,
                    Primitive_type.Default,//type
                    primitive.centre.x, primitive.centre.y, primitive.centre.z,//centre
                    primitive.materialColor.r, primitive.materialColor.g, primitive.materialColor.b, primitive.materialColor.a,//materialColor
                    0.0f, 1.0f, 0.0f,//Normal
                    0.0f,//radius
                    0.0f, 0.0f, 0.0f,//p0
                    0.0f, 0.0f, 0.0f,//p1
                    0.0f, 0.0f, 0.0f //p2
                    );
            }
        }
        /// <summary>
        /// 修改某个结点处的Primitive并使之变为Sphere
        /// </summary>
        /// <param name="index"></param>
        /// <param name="sphere"></param>
        public static void ChangePrimitive(int index, Sphere sphere)
        {
            unsafe
            {
                changePrimitive(
                    index,
                    Primitive_type.Sphere,//type
                    sphere.centre.x, sphere.centre.y, sphere.centre.z,//centre
                    sphere.materialColor.r, sphere.materialColor.g, sphere.materialColor.b, sphere.materialColor.a,//materialColor
                    0.0f, 1.0f, 0.0f,//Normal
                    sphere.radius,//radius
                    0.0f, 0.0f, 0.0f,//p0
                    0.0f, 0.0f, 0.0f,//p1
                    0.0f, 0.0f, 0.0f//p2
                    );
            }
        }
        /// <summary>
        /// 修改某个结点处的Primitive并使之变为Triangle
        /// </summary>
        /// <param name="index"></param>
        /// <param name="triangle"></param>
        public static void ChangePrimitive(int index, Triangle triangle)
        {
            unsafe
            {
                changePrimitive(
                    index,
                    Primitive_type.Triangle,//type
                    triangle.centre.x, triangle.centre.y, triangle.centre.z,//centre
                    triangle.materialColor.r, triangle.materialColor.g, triangle.materialColor.b, triangle.materialColor.a,//materialColor
                    triangle.normal.x, triangle.normal.y, triangle.normal.z,//Normal
                    0.0f,//radius
                    triangle.points[0].x, triangle.points[0].y, triangle.points[0].z,//p0
                    triangle.points[1].x, triangle.points[1].y, triangle.points[1].z,//p1
                    triangle.points[2].x, triangle.points[2].y, triangle.points[2].z //p2
                    );
            }
        }
        /// <summary>
        /// 修改某个结点处的Primitive并使之变为Plane
        /// </summary>
        /// <param name="index"></param>
        /// <param name="plane"></param>
        public static void ChangePrimitive(int index, Plane plane)
        {
            unsafe
            {
                changePrimitive(
                    index,
                    Primitive_type.Plane,//type
                    plane.centre.x, plane.centre.y, plane.centre.z,//centre
                    plane.materialColor.r, plane.materialColor.g, plane.materialColor.b, plane.materialColor.a,//materialColor
                    plane.normal.x, plane.normal.y, plane.normal.z,//Normal
                    0.0f,//radius
                    0.0f, 0.0f, 0.0f,//p0
                    0.0f, 0.0f, 0.0f,//p1
                    0.0f, 0.0f, 0.0f //p2
                    );
            }
        }

        [DllImport("CudaAPI", EntryPoint = "changePrimitiveType", CallingConvention = CallingConvention.Cdecl)]
        private static extern void changePrimitiveType(int index, Primitive_type type);
        /// <summary>
        /// 修改图元的类型
        /// </summary>
        /// <param name="index"></param>
        /// <param name="type"></param>
        public static void ChangePrimitiveType(int index, Primitive_type type)
        {
            unsafe
            {
                changePrimitiveType(index, type);
            }
        }

        [DllImport("CudaAPI", EntryPoint = "changePrimitiveCentre", CallingConvention = CallingConvention.Cdecl)]
        private static extern void changePrimitiveCentre(int index, float c_x, float c_y, float c_z);
        /// <summary>
        /// 修改图元的质心
        /// </summary>
        /// <param name="index"></param>
        /// <param name="centre"></param>
        public static void ChangePrimitiveCentre(int index, Point centre)
        {
            unsafe
            {
                changePrimitiveCentre(index, centre.x, centre.y, centre.z);
            }
        }

        [DllImport("CudaAPI", EntryPoint = "changePrimitiveColor", CallingConvention = CallingConvention.Cdecl)]
        private static extern void changePrimitiveColor(int index, float r, float g, float b,float a);
        /// <summary>
        /// 修改图元的颜色
        /// </summary>
        /// <param name="index"></param>
        /// <param name="materialColor"></param>
        public static void ChangePrimitiveColor(int index, Color materialColor)
        {
            unsafe
            {
                changePrimitiveColor(index, materialColor.r, materialColor.g, materialColor.b,materialColor.a);
            }
        }

        [DllImport("CudaAPI", EntryPoint = "changePrimitiveNormal", CallingConvention = CallingConvention.Cdecl)]
        private static extern void changePrimitiveNormal(int index, float x, float y, float z);
        /// <summary>
        /// 修改平面的法线朝向
        /// </summary>
        /// <param name="index"></param>
        /// <param name="normal"></param>
        public static void ChangePrimitiveNormal(int index, Normal normal)
        {
            unsafe
            {
                changePrimitiveNormal(index, normal.x,normal.y,normal.z);
            }
        }

        [DllImport("CudaAPI", EntryPoint = "changePrimitiveRadius", CallingConvention = CallingConvention.Cdecl)]
        private static extern void changePrimitiveRadius(int index, float radius);
        /// <summary>
        /// 修改球的半径
        /// </summary>
        /// <param name="index"></param>
        /// <param name="radius"></param>
        public static void ChangePrimitiveRadius(int index, float radius)
        {
            unsafe
            {
                changePrimitiveRadius(index, radius);
            }
        }

        [DllImport("CudaAPI", EntryPoint = "changePrimitivePoints", CallingConvention = CallingConvention.Cdecl)]
        private static extern void changePrimitivePoints(int index,
            float p0_x, float p0_y, float p0_z,
            float p1_x, float p1_y, float p1_z,
            float p2_x, float p2_y, float p2_z
            );
        /// <summary>
        /// 修改三角形的三个顶点
        /// </summary>
        /// <param name="index"></param>
        /// <param name="points"></param>
        public static void ChangePrimitivePoints(int index, Point[] points)
        {
            unsafe
            {
                changePrimitivePoints(index, 
                    points[0].x,points[0].y,points[0].z,
                    points[1].x,points[1].y,points[1].z,
                    points[2].x,points[2].y,points[2].z
                    );
            }
        }
        #endregion

        #region Check primitive
        [DllImport("CudaAPI", EntryPoint = "checkPrimitiveType", CallingConvention = CallingConvention.Cdecl)]
        private static extern Primitive_type checkPrimitiveType(int index);
        /// <summary>
        /// 查看在该index下图元的类型
        /// </summary>
        /// <param name="index"></param>
        /// <returns></returns>
        public static Primitive_type CheckPrimitiveType(int index)
        {
            unsafe
            {
                return checkPrimitiveType(index);
            }
        }

        [DllImport("CudaAPI", EntryPoint = "checkPrimitiveCentre", CallingConvention = CallingConvention.Cdecl)]
        private static extern float checkPrimitiveCentre(int index,int c_idx);
        /// <summary>
        /// 查看在该index下图元的类质心
        /// </summary>
        /// <param name="index"></param>
        /// <returns></returns>
        public static Point CheckPrimitiveCentre(int index)
        {
            Point result = new Point();
            for (int i = 0; i < 3; i++)
            {
                switch (i)
                {
                    case 0: result.x = checkPrimitiveCentre(index, i);break;
                    case 1: result.y = checkPrimitiveCentre(index, i);break;
                    case 2: result.z = checkPrimitiveCentre(index, i);break;
                }
            }
            return result;
        }

        [DllImport("CudaAPI", EntryPoint = "checkPrimitiveColor", CallingConvention = CallingConvention.Cdecl)]
        private static extern float checkPrimitiveColor(int index, int c_idx);
        /// <summary>
        /// 查看在该index下的materialColor
        /// </summary>
        /// <param name="index"></param>
        /// <returns></returns>
        public static Color CheckPrimitiveColor(int index)
        {
            Color result = new Color();
            for (int i = 0; i < 3; i++)
            {
                switch (i)
                {
                    case 0: result.r = checkPrimitiveColor(index, i); break;
                    case 1: result.g = checkPrimitiveColor(index, i); break;
                    case 2: result.b = checkPrimitiveColor(index, i); break;
                    case 3: result.a = checkPrimitiveColor(index, i); break;
                }
            }
            return result;
        }

        [DllImport("CudaAPI", EntryPoint = "checkPrimitiveNormal", CallingConvention = CallingConvention.Cdecl)]
        private static extern float checkPrimitiveNormal(int index, int n_idx);
        /// <summary>
        /// 查看在该index下的normal
        /// </summary>
        /// <param name="index"></param>
        /// <returns></returns>
        public static Normal CheckPrimitiveNormal(int index)
        {
            Normal result = new Normal();
            for (int i = 0; i < 3; i++)
            {
                switch (i)
                {
                    case 0: result.x = checkPrimitiveNormal(index, i); break;
                    case 1: result.y = checkPrimitiveNormal(index, i); break;
                    case 2: result.z = checkPrimitiveNormal(index, i); break;
                }
            }
            return result;
        }

        [DllImport("CudaAPI", EntryPoint = "checkPrimitiveRadius", CallingConvention = CallingConvention.Cdecl)]
        private static extern float checkPrimitiveRadius(int index);
        /// <summary>
        /// 查看在该index下的radius
        /// </summary>
        /// <param name="index"></param>
        /// <returns></returns>
        public static float CheckPrimitiveRadius(int index)
        {
            unsafe
            {
                return checkPrimitiveRadius(index);
            }
        }

        [DllImport("CudaAPI", EntryPoint = "checkPrimitivePoints", CallingConvention = CallingConvention.Cdecl)]
        private static extern float checkPrimitivePoints(int index, int p_idx);
        /// <summary>
        /// 查看在该index下的radius
        /// </summary>
        /// <param name="index"></param>
        /// <returns></returns>
        public static Point[] CheckPrimitivePoints(int index)
        {
            Point[] result = new Point[3];
            result[0] = new Point();
            result[1] = new Point();
            result[2] = new Point();
            for (int i = 0; i < 9; i++)
            {
                switch (i)
                {
                    case 0: result[0].x = checkPrimitivePoints(index, i); break;
                    case 1: result[0].y = checkPrimitivePoints(index, i); break;
                    case 2: result[0].z = checkPrimitivePoints(index, i); break;

                    case 3: result[1].x = checkPrimitivePoints(index, i); break;
                    case 4: result[1].y = checkPrimitivePoints(index, i); break;
                    case 5: result[1].z = checkPrimitivePoints(index, i); break;

                    case 6: result[2].x = checkPrimitivePoints(index, i); break;
                    case 7: result[2].y = checkPrimitivePoints(index, i); break;
                    case 8: result[2].z = checkPrimitivePoints(index, i); break;
                }
            }
            return result;
        }

        #endregion

        #region Generate scene
        [DllImport("CudaAPI", EntryPoint = "generateScene", CallingConvention = CallingConvention.Cdecl)]
        private static extern void generateScene();
        /// <summary>
        /// GenerateScene()应该在每一次场景中的图元的数量改动时执行一次，因为需要重新给GPU分配内存。
        /// 但是修改某个图元的数据时不需要执行。
        /// </summary>
        public static void GenerateScene()
        {
            unsafe
            {
                generateScene();
            }
        }

        #endregion

        #region Render
        [DllImport("CudaAPI", EntryPoint = "renderScene", CallingConvention = CallingConvention.Cdecl)]
        private static extern void renderScene(int frame_buffer_index, int picking_buffer_index);
        /// <summary>
        /// 指定一张Frame Buffer用于渲染最终图片，指定一张Picking Buffer用于快速拾取
        /// </summary>
        /// <param name="frame_buffer_index"></param>
        /// <param name="picking_buffer_index"></param>
        public static void RenderScene(int frame_buffer_index, int picking_buffer_index)
        {
            unsafe
            {
                renderScene(frame_buffer_index, picking_buffer_index);
                //for (int i = 0; i < data.Height; i++)
                //{
                //    for (int j = 0; j < data.Width; j++)
                //    {
                //        *ptr = *
                //        ptr += 3;
                //    }
                //    ptr += data.Stride - data.Width * 3;
                //}
            }
        }
        #endregion

        #region Debug&Test
        [DllImport("CudaAPI", EntryPoint = "test", CallingConvention = CallingConvention.Cdecl)]
        public static extern int test();
        /// <summary>
        /// Debug&Test用的，不用管
        /// </summary>
        /// <returns></returns>
        public static int Test()
        {
            return test();
        }
        #endregion

        
    }

    

    public enum Primitive_type
    {
        Default,
        Sphere,
        Triangle,
        Plane
    }
    public struct BufferMap
    {
        byte r, g, b;
    };
}
