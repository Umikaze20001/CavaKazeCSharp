using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Render;
namespace Render
{
    public enum Sampler
    {
        regular
    }

    public class Camera
    {
        public float rotateScale = 0.005f;
        public float moveScale = 0.1f;
        public Point position;
        public Vec3 forward;
        public Vec3 right;
        public Vec3 up;
        public float phi = (float)Math.PI;
        public float theta = 0.5f * (float)Math.PI;
        public Quaternion rotation = new Quaternion();
        public float fov = (1.0f / 3.0f) * (float)Math.PI;
        public float aspectRatio = 16.0f / 9.0f;
        public float viewDistance = 5.0f;
       //    Sampler sampler = Sampler.regular;
        public Camera()
        {
            position = new Point();
            forward = new Vec3(0, 0, -1);
            right = new Vec3(1, 0, 0);
            up = new Vec3(0, 1, 0);
        }
        public void Update(int X,int Y)
        {
            phi = -rotateScale * X + (float)Math.PI;
            theta = rotateScale * Y + 0.5f * (float)Math.PI;
            if (theta <= 0.00001f)
            {
                theta = 0.00001f;
                Y = -(int)((0.5f * Math.PI) / rotateScale);
            }
            else if (theta >= Math.PI)
            {
                theta = (float)Math.PI;
                Y = (int)((0.5f * (float)Math.PI) / rotateScale);
            }
            forward = new Vec3((float)(Math.Sin(theta) * Math.Sin(phi)), (float)(Math.Cos(theta)), (float)(Math.Sin(theta) * Math.Cos(phi)));
            forward.normalize();
            right = new Vec3(Vec3.cross(new Vec3(forward.x, 0, forward.z), forward));
            right.normalize();

            Quaternion rotationPhi = new Quaternion(new Vec3(0.0f, 1.0f, 0.0f), phi);
            float axisTheta = phi + 0.5f * (float)Math.PI;
            Vec3 rotateThetaAxis = new Vec3((float)Math.Sin(axisTheta), 0.0f, (float)Math.Cos(axisTheta));
            Quaternion rotationTheta = new Quaternion(rotateThetaAxis, theta);
            rotation = rotationTheta * rotationPhi;
        }
        public void MoveForward()
        {
            position += moveScale * forward;
        }
        public void MoveBack()
        {
            position -= moveScale * forward;

        }
        public void MoveLeft()
        {
            position -= moveScale * right;
        }
        public void MoveRight()
        {
            position += moveScale * right;
        }
        public void SendToCuda()
        {
            //CudaUtil.SendCamera(
            //    position.x, position.y, position.z,
            //    rotation.imaginaryPart.x, rotation.imaginaryPart.y, rotation.imaginaryPart.z, rotation.realPart,
            //    CommonInfo.Width, CommonInfo.Heght, fov, aspectRatio, viewDistance, (int)Sampler.regular);
        }
    }
}
