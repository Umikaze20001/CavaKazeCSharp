using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using System.Drawing.Imaging;
using System.Windows.Forms;
namespace Render
{
    class Controller
    {
        Bitmap bitmap1 = CommonInfo.bitmap1, bitmap2 = CommonInfo.bitmap2, bitmap3 = CommonInfo.bitmap3, pickmap = CommonInfo.pickmap;
        IntPtr bitPtr1 = CommonInfo.frame_buffer0_ptr, bitPtr2 = CommonInfo.frame_buffer1_ptr, bitPtr3 = CommonInfo.frame_buffer2_ptr, pkDataPtr = CommonInfo.picking_buffer_ptr;
        mainForm form;
        Scene scene;
        Sphere sphere;
        Triangle triangle;
        Plane plane;
        //当前primitive是否进行过修改
        public bool isArgumentUpdate = false;
        //摄像机是否修改过
        public bool isCameraUpdate = false;
        //修改的primitive的类型
        public Primitive_type updateType;
        public int updateIndex;
        private Controller()
        {
            scene = Scene.Instance;
        }

        private static Controller instance;
        public static Controller Instance
        {
            get
            {
                if (instance == null)
                    instance = new Controller();
                return instance;
            }
        }

        

        public void SetResolution()
        {

        }

        public void AddBufferMap()
        {
            CudaUtil.AddBufferMap(CommonInfo.frame_buffer0_ptr);
            CudaUtil.AddBufferMap(CommonInfo.frame_buffer1_ptr);
            CudaUtil.AddBufferMap(CommonInfo.frame_buffer2_ptr);
            CudaUtil.AddBufferMap(CommonInfo.picking_buffer_ptr);
        }

        public void InitializeFrameBuffer()
        {
            Rectangle rec = new Rectangle(0, 0, width, height);
            // bitmap1 = new Bitmap("QQ图片20200117155017.png");

            bitmap1 = new Bitmap(width, height, PixelFormat.Format24bppRgb);

            bd1 = bitmap1.LockBits(rec, ImageLockMode.ReadWrite, PixelFormat.Format24bppRgb);
            bitPtr1 = bd1.Scan0;
            bitmap1.UnlockBits(bd1);

            bitmap2 = new Bitmap(width, height, PixelFormat.Format24bppRgb);
            bd2 = bitmap2.LockBits(rec, ImageLockMode.ReadWrite, PixelFormat.Format24bppRgb);
            bitPtr2 = bd2.Scan0;
            bitmap2.UnlockBits(bd2);

            bitmap3 = new Bitmap(width, height, PixelFormat.Format24bppRgb);
            bd3 = bitmap3.LockBits(rec, ImageLockMode.ReadWrite, PixelFormat.Format24bppRgb);
            bitPtr3 = bd3.Scan0;
            bitmap3.UnlockBits(bd3);

            pickmap = new Bitmap(width, height, PixelFormat.Format24bppRgb);
            pkData = pickmap.LockBits(rec, ImageLockMode.ReadWrite, PixelFormat.Format24bppRgb);
            pkDataPtr = pkData.Scan0;
            pickmap.UnlockBits(pkData);
            pictureBox1.Image = bitmap1;
        }

        public void SetForm(mainForm mainForm)
        {
            form = mainForm;
        }

        public void SetMFMap()
        {
            form.bitmap1 = bitmap1;
            form.bitmap2 = bitmap2;
            form.bitmap3 = bitmap3;
        }

        public void SetMainformHaveShown()
        {
            CommonInfo.mainFormHaveShown.Set();
        }

        public void AddSphere()
        {
            int index = scene.AddSphere();
            if (index == -1)
            {
                MessageBox.Show("添加失败");
                return;
            }
            form.AddSceneListNode(Primitive_type.Sphere, index);
        }

        public void AddTriangle()
        {
            int index = scene.AddTriangle();
            if (index == -1)
            {
                MessageBox.Show("添加失败");
                return;
            }
            form.AddSceneListNode(Primitive_type.Triangle, index);
        }

        public void AddPlane()
        {
            int index = scene.AddPlane();
            if(index == -1)
            {
                MessageBox.Show("添加失败");
                return;
            }
            form.AddSceneListNode(Primitive_type.Plane, index);
        }

        public void DeletePrimitive(int primitiveIndex,int nodeIndex)
        {
            if (scene.DeletePrimitive(primitiveIndex))
            {
                form.DeleteSceneListNode(nodeIndex);
            }
            else
            {
                MessageBox.Show("删除失败");
                
            }
        }

        public void UpdatePrimitives()
        {
            if (isArgumentUpdate)
            {
                switch (updateType)
                {
                    case Primitive_type.Default:
                        break;
                    case Primitive_type.Sphere:
                        ChangeSphere(updateIndex);
                        break;
                    case Primitive_type.Triangle:
                        ChangeTriangle(updateIndex);
                        break;
                    case Primitive_type.Plane:
                        ChangePlane(updateIndex);
                        break;
                }
                isArgumentUpdate = false;
            }
            if (isCameraUpdate)
            {
                scene.UpdateCamera(ref CommonInfo.accumulatedX, ref CommonInfo.accumulatedY);
                isCameraUpdate = false;
            }

        }

        public void ChangeSphere(int index)
        {
            sphere.centre.x = TextUtil.GetFloat(form.sphereCenterXText.Text);
            sphere.centre.y = TextUtil.GetFloat(form.sphereCenterYText.Text);
            sphere.centre.z = TextUtil.GetFloat(form.sphereCenterZText.Text);
            sphere.materialColor.r = TextUtil.GetFloat(form.sphereColorXText.Text);
            sphere.materialColor.g = TextUtil.GetFloat(form.sphereColorYText.Text);
            sphere.materialColor.b = TextUtil.GetFloat(form.sphereColorZText.Text);
            sphere.radius = TextUtil.GetFloat(form.sphereRadiusText.Text);
            scene.ChangeSphere(index, sphere);
        }

        public void ChangePlane(int index)
        {
            plane.centre.x = TextUtil.GetFloat(form.planeCenterXText.Text);
            plane.centre.y = TextUtil.GetFloat(form.planeCenterYText.Text);
            plane.centre.z = TextUtil.GetFloat(form.planeCenterZText.Text);
            plane.materialColor.r = TextUtil.GetFloat(form.planeColorXText.Text);
            plane.materialColor.g = TextUtil.GetFloat(form.planeColorYText.Text);
            plane.materialColor.b = TextUtil.GetFloat(form.planeColorZText.Text);
            plane.normal.x = TextUtil.GetFloat(form.planeNormalXText.Text);
            plane.normal.y = TextUtil.GetFloat(form.planeNormalYText.Text);
            plane.normal.z = TextUtil.GetFloat(form.planeNormalZText.Text);
            scene.ChangePlane(index, plane);
        }

        public void ChangeTriangle(int index)
        {
            triangle.points[0].x = TextUtil.GetFloat(form.trianglePoint1XText.Text);
            triangle.points[0].y = TextUtil.GetFloat(form.trianglePoint1YText.Text);
            triangle.points[0].z = TextUtil.GetFloat(form.trianglePoint1ZText.Text);
            triangle.points[1].x = TextUtil.GetFloat(form.trianglePoint2XText.Text);
            triangle.points[1].z = TextUtil.GetFloat(form.trianglePoint2YText.Text);
            triangle.points[1].z = TextUtil.GetFloat(form.trianglePoint2ZText.Text);
            triangle.points[2].x = TextUtil.GetFloat(form.trianglePoint3XText.Text);
            triangle.points[2].y = TextUtil.GetFloat(form.trianglePoint3YText.Text);
            triangle.points[2].z = TextUtil.GetFloat(form.trianglePoint3ZText.Text);

            triangle.materialColor.r = TextUtil.GetFloat(form.triangleColorXText.Text);
            triangle.materialColor.g = TextUtil.GetFloat(form.triangleColorYText.Text);
            triangle.materialColor.b = TextUtil.GetFloat(form.triangleColorZText.Text);
            triangle.normal.x = TextUtil.GetFloat(form.triangleNormalXText.Text);
            triangle.normal.y = TextUtil.GetFloat(form.triangleNormalYText.Text);
            triangle.normal.z = TextUtil.GetFloat(form.triangleNormalZText.Text);

            scene.ChangeTriangle(index, triangle);
        }
    }
}
