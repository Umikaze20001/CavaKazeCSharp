using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Render;
namespace Render
{
    public class Scene
    {
        Camera camera;
        Sphere sphere;
        Triangle triangle;
        Plane plane;
        private static Scene instance;
        public static Scene Instance
        {
            get
            {
                if (instance == null)
                    instance = new Scene();
                return instance;
            }
        }

        private Scene()
        {
            camera = new Camera();
            sphere = new Sphere();
            triangle = new Triangle();
            plane = new Plane();
        }

        public int AddPlane()
        {
            return CudaUtil.AddPlane();
        }

        public int AddTriangle()
        {
            return CudaUtil.AddTriangle();
        }

        public int AddSphere()
        {
            return CudaUtil.AddSphere();
        }

        public bool ChangeSphere(int index,Sphere sphere)
        {
            CudaUtil.ChangePrimitiveCentre(index, sphere.centre);
            CudaUtil.ChangePrimitiveColor(index, sphere.materialColor);
            CudaUtil.ChangePrimitiveRadius(index, sphere.radius);
            Log.instance.writeLog("将球的参数变为,质心:" + sphere.centre.x.ToString() + sphere.centre.y.ToString() + sphere.centre.z.ToString());
           
            return true;
        }

        public bool ChangePlane(int index, Plane plane)
        {
            CudaUtil.ChangePrimitiveColor(index, plane.materialColor);
            CudaUtil.ChangePrimitiveNormal(index, plane.normal);
            CudaUtil.ChangePrimitiveCentre(index, plane.centre);
            Log.instance.writeLog("将平面的参数变为,质心:" + plane.centre.x.ToString() + plane.centre.y.ToString() + plane.centre.z.ToString());
            Log.instance.writeLog("法向量" + plane.normal.x.ToString() + plane.normal.y.ToString() + plane.normal.z.ToString());
            return true;
        }

        public bool ChangeTriangle(int index,Triangle triangle)
        {
            CudaUtil.ChangePrimitivePoints(index, triangle.points);
            CudaUtil.ChangePrimitiveColor(index, triangle.materialColor);
            CudaUtil.ChangePrimitiveNormal(index, triangle.normal);
            return true;
        }

        public bool DeletrPrimitive(int index)
        {
            CudaUtil.DeletePrimitive(index);
            return true;
        }

        public Sphere GetSphere(int index)
        {
            sphere.centre = CudaUtil.CheckPrimitiveCentre(index);
            sphere.materialColor = CudaUtil.CheckPrimitiveColor(index);
            sphere.radius = CudaUtil.CheckPrimitiveRadius(index);
            return sphere;
        }

        public Triangle GetTriangle(int index)
        {
            triangle.points = CudaUtil.CheckPrimitivePoints(index);
            triangle.normal = CudaUtil.CheckPrimitiveNormal(index);
            triangle.materialColor = CudaUtil.CheckPrimitiveColor(index);
            return triangle;
        }

        public Plane GetPlane(int index)
        {
            plane.centre = CudaUtil.CheckPrimitiveCentre(index);
            plane.materialColor = CudaUtil.CheckPrimitiveColor(index);
            plane.normal = CudaUtil.CheckPrimitiveNormal(index);
            return plane;
        } 

        public bool DeletePrimitive(int index)
        {
            CudaUtil.DeletePrimitive(index);
            return true;
        }
        
        public void UpdateCamera()
        {
            camera.Update(CommonInfo.accumulatedX, CommonInfo.accumulatedY);
            CudaUtil.SendCamera(camera, CommonInfo.width, CommonInfo.height);
        }

        public void SendCameraToCuda()
        {
            CudaUtil.SendCamera(camera, CommonInfo.width, CommonInfo.height);
        }

        public void CameraMoveForward()
        {
            camera.MoveForward();
             
        }
        public void CameraMoveBack()
        {
            camera.MoveBack();
             
        }
        public void CameraMoveLeft()
        {
            camera.MoveLeft();
             
        }
        public void CameraMoveRight()
        {
            camera.MoveRight();
             
        }
    }
}
