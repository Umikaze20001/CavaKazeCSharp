using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Imaging;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Render;
using System.Threading;
 
namespace Render
{
     
    public partial class mainForm : Form
    {
        System.Drawing.Color borderColor = System.Drawing.Color.FromArgb(38, 38, 38);
        int borderWidth = 1;
        //int primitiveCount = 0;
        int sphereIndex = 0;
        int planeIndex = 0;
        int triangleIndex = 0;


        Bitmap bitmap1;
        Bitmap bitmap2;
        Bitmap bitmap3;

        //当前primitive是否进行过修改
        bool isArgumentUpdate = false;
        //摄像机是否修改过
        bool isCameraUpdate = false;
        //若修改则记录下ID
        Primitive_type updateType;
        int updateIndex;

        //判断是否窗体获得焦点
        bool argumentFocus = false;

 


        //用于协助记录argument面板的数据
        Sphere sphere;
        Triangle triangle;
        Plane plane;

        Scene scene;

        CommandPool commandPool;
        public mainForm( )
        {
            //loop是一个工具类，调度图片的方法和渲染图片的方法（都是无限循环的）我写在里面了
            InitializeComponent();
            commandPool = CommandPool.Instance;

            scene = Scene.Instance;
            int width = CommonInfo.width;
            int height = CommonInfo.height;
            bitmap1 = CommonInfo.bitmap1;
            bitmap2 = CommonInfo.bitmap2;
            bitmap3 = CommonInfo.bitmap3;
            pictureBox1.Image = bitmap1;
            // bitmap1 = new Bitmap("QQ图片20200117155017.png");


            argumentPanel_transformPanelPlane.Visible = false;
            argumentPanel_transformPanelTriangle.Visible = false;
            argumentPanel_transformPanelSphere.Visible = false;


            //空间换时间
            Sphere sphere = new Sphere();
            Triangle triangle = new Triangle();
            Plane plane = new Plane();
            
        }

        private void mainForm_Shown(object sender, EventArgs e)
        {
            CommonInfo.mainFormHaveShown.Set();
        }

        // 这里定义的都是和前端交互的事件，你不用太在意
        #region 事件
        private void panel_Paint(object sender, PaintEventArgs e)
        {
            Panel panel = sender as Panel;
             
            ControlPaint.DrawBorder(e.Graphics,
                                       panel.ClientRectangle,
                                       borderColor,
                                       borderWidth,
                                       ButtonBorderStyle.Solid,
                                       borderColor,
                                       borderWidth,
                                       ButtonBorderStyle.Solid,
                                       borderColor,
                                       borderWidth,
                                       ButtonBorderStyle.Solid,
                                       borderColor,
                                       borderWidth,
                                       ButtonBorderStyle.Solid);
        }

        private void addPrimitives_addSphere_Click(object sender, EventArgs e)
        {
            commandPool.AddCommand(new Add(AddShpere));
        }

        private void addPrimitives_addTriangle_Click(object sender, EventArgs e)
        {
            commandPool.AddCommand(new Add(AddTriangle));
        }

        private void addPrimitives_addPlane_Click(object sender, EventArgs e)
        {
            commandPool.AddCommand(new Add(AddPlane));
        }

        private void deletePrimitive_Click(object sender, EventArgs e)
        {
            int index = (scenePanel_sceneList.SelectedNode as PrimitiveNode).index;
            commandPool.AddCommand(new Delete(DeletePrimitive), new object[] { index });
        }

        private void sceneStripMenu_rename_Click(object sender, EventArgs e)
        {
            scenePanel_sceneList.SelectedNode.BeginEdit();
        }

        private void mainForm_FormClosed(object sender, FormClosedEventArgs e)
        {
            System.Environment.Exit(0);
        }

        private void InputIsNumber(object sender, KeyPressEventArgs e)
        {
            var tb = sender as TextBox;

            if (null == tb)
            {
                e.Handled = true;
                return;
            }

            if ((e.KeyChar >= '0' && e.KeyChar <= '9') || e.KeyChar == 8 || e.KeyChar == 46) //数字、Backspace、小数点
            {
                var editText = (tb.Text);

                if (e.KeyChar != 8)
                {
                    var selStart = tb.SelectionStart;
                    var selLength = tb.SelectionLength;

                    if (selLength > 0) //存在选择的内容，进行替换。
                    {
                        editText = editText.Remove(selStart, selLength);

                        tb.Text = editText;
                        tb.SelectionLength = 0;
                        tb.SelectionStart = selStart;
                    }

                    editText = editText.Insert(selStart, e.KeyChar.ToString());

                    try
                    {
                        //校验新数据是否合法。
                        var newValue = double.Parse(editText);
                        e.Handled = !(newValue >= 0);
                    }
                    catch (Exception)
                    {
                        e.Handled = true;
                        return;
                    }
                    e.Handled = false;
                }
                else
                    e.Handled = false;
            }
            else
            {
                //正负数切换
                if (e.KeyChar == 45)
                {
                    tb.Text = tb.Text.Contains("-") ? tb.Text.Replace("-", "") : tb.Text.Insert(0, "-");

                    tb.Select(tb.Text.Length, 0);
                }

                e.Handled = true;
            }
        }

        //不用管
        private void main_scenePanel_Resize(object sender, EventArgs e)
        {
            Panel p = sender as Panel;
            p.Refresh();
        }
        //这里，需要大改
        private void scenePanel_sceneList_AfterSelect(object sender, TreeViewEventArgs e)
        {
            argumentPanel_transformPanelSphere.Visible = false;
            argumentPanel_transformPanelPlane.Visible = false;
            argumentPanel_transformPanelTriangle.Visible = false;
        }

        private void argument_TextChanged(object sender, EventArgs e)
        {
            if((sender as TextBox).Focused)
            {
                updateType = GetCurrentNode().type;
                updateIndex = GetCurrentNode().index;
                isArgumentUpdate = true;
            }
           
        }

        /// <summary>
        /// 若panel重绘结束，则告诉showImage 可以继续了
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>

        #endregion

        private delegate void RefreshTransform();
        public void RefreshTransormPanel()
        {
            if (argumentFocus)
                return;
            if (scenePanel_sceneList.InvokeRequired)
            {
                RefreshTransform r = new RefreshTransform(RefreshTransormPanel);
                this.Invoke(r);
            }
            else
            {
                
                PrimitiveNode pn = GetCurrentNode();
                if (pn == null || pn.Parent == null)
                {
                    return;
                }
                int index = pn.index;
                Primitive_type p = pn.type;
                //因为目前primitive类型较少，所以我在这里用了primitive，以后不一定，也许会用字典，也许会用其他的
                switch (p)
                {
                    case Primitive_type.Default:
                        break;
                    case Primitive_type.Sphere:
                        argumentPanel_transformPanelSphere.Visible = true;
                        argumentPanel_transformPanelPlane.Visible = false;
                        argumentPanel_transformPanelTriangle.Visible = false;
                        RefreshSphereArgument(index);
                        break;
                    case Primitive_type.Triangle:
                        argumentPanel_transformPanelSphere.Visible = false;
                        argumentPanel_transformPanelPlane.Visible = false;
                        argumentPanel_transformPanelTriangle.Visible = true;
                        RefreshTriangleArgument(index);
                        break;
                    case Primitive_type.Plane:
                        argumentPanel_transformPanelSphere.Visible = false;
                        argumentPanel_transformPanelPlane.Visible = true;
                        argumentPanel_transformPanelTriangle.Visible = false;
                        RefreshPlaneArgument(index);
                        break;
                }
            }
        }

        private delegate void Refresh(int index);
        private void RefreshSphereArgument(int index)
        {
            if (sphereCenterXText.InvokeRequired)
            {
                Refresh r = new Refresh(RefreshPlaneArgument);
                this.Invoke(r, new object[] { index });
            }
            else
            {
                sphere = scene.GetSphere(index);
                sphereCenterXText.Text = sphere.centre.x.ToString();
                sphereCenterYText.Text = sphere.centre.y.ToString();
                sphereCenterZText.Text = sphere.centre.z.ToString();
                sphereColorXText.Text = sphere.materialColor.r.ToString();
                sphereColorYText.Text = sphere.materialColor.g.ToString();
                sphereColorZText.Text = sphere.materialColor.b.ToString();
                sphereRadiusText.Text = sphere.radius.ToString();
            }
            
        }

        private void RefreshPlaneArgument(int index)
        {
            if (planeCenterXText.InvokeRequired)
            {
                Refresh r = new Refresh(RefreshPlaneArgument);
                this.Invoke(r, new object[] { index });
            }
            else
            {
                plane = scene.GetPlane(index);

                planeCenterXText.Text = plane.centre.x.ToString();
                planeCenterYText.Text = plane.centre.y.ToString();
                planeCenterZText.Text = plane.centre.z.ToString();
                planeColorXText.Text = plane.materialColor.r.ToString();
                planeColorYText.Text = plane.materialColor.g.ToString();
                planeColorZText.Text = plane.materialColor.b.ToString();
                planeNormalXText.Text = plane.normal.x.ToString();
                planeNormalYText.Text = plane.normal.y.ToString();
                planeNormalZText.Text = plane.normal.z.ToString();
            }
            

        }

        private void RefreshTriangleArgument(int index)
        {
            if (triangleColorXText.InvokeRequired)
            {
                Refresh r = new Refresh(RefreshTriangleArgument);
                this.Invoke(r, new object[] { index });
            }
            else
            {
                triangle = scene.GetTriangle(index);

                triangleColorXText.Text = triangle.materialColor.r.ToString();
                triangleColorYText.Text = triangle.materialColor.g.ToString();
                triangleColorZText.Text = triangle.materialColor.b.ToString();
                trianglePoint1XText.Text = triangle.points[0].x.ToString();
                trianglePoint1YText.Text = triangle.points[0].y.ToString();
                trianglePoint1ZText.Text = triangle.points[0].z.ToString();
                trianglePoint2XText.Text = triangle.points[1].x.ToString();
                trianglePoint2YText.Text = triangle.points[1].y.ToString();
                trianglePoint2ZText.Text = triangle.points[1].z.ToString();
                trianglePoint3XText.Text = triangle.points[2].x.ToString();
                trianglePoint3YText.Text = triangle.points[2].y.ToString();
                trianglePoint3ZText.Text = triangle.points[2].z.ToString();
                triangleNormalXText.Text = triangle.normal.x.ToString();
                triangleNormalYText.Text = triangle.normal.y.ToString();
                triangleNormalZText.Text = triangle.normal.z.ToString();
            }   

        }

        //切换图片的函数，由调度线程调用。
        private delegate void UpdateImg(int index);
        public void UpdateImage(int index)
        {
            if (pictureBox1.InvokeRequired)
            {
                UpdateImg u = new UpdateImg(UpdateImage);
                this.Invoke(u, new object[] { index });
            }
            else
            {
                switch (index)
                {
                    case 0: pictureBox1.Image = bitmap1;  break;
                    case 1: pictureBox1.Image = bitmap2;  break;
                    case 2: pictureBox1.Image = bitmap3;  break;
                    default: pictureBox1.Image = bitmap1; break;
                }
            }
            
        }

        #region primitive的增删改查，需要C端提供对应的API

        private delegate void Delete(int index);
        public void DeletePrimitive(int index)
        {
            if (scenePanel_sceneList.InvokeRequired)
            {
                Delete d = new Delete(DeletePrimitive);
                this.Invoke(d, new object[] { index });
            }
            else
            {
                scene.DeletePrimitive(index);

                scenePanel_sceneList.SelectedNode.Remove();
            }
            
        }

        private delegate void Add();
        public void AddShpere( )
        {
            if (scenePanel_sceneList.InvokeRequired)
            {
                Add a = new Add(AddShpere);
                this.Invoke(a);
            }
            else
            {
                int index = scene.AddSphere();
                if (index == -1)
                {
                    MessageBox.Show("添加失败");
                    return;
                }

                PrimitiveNode p = new PrimitiveNode(Primitive_type.Sphere, index);

                p.Text = "sphere" + sphereIndex;

                scenePanel_sceneList.TopNode.Nodes.Add(p);
                scenePanel_sceneList.ExpandAll();
                ++sphereIndex;
            }
        }

        public void AddPlane()
        {
            if (scenePanel_sceneList.InvokeRequired)
            {
                Add a = new Add(AddPlane);
                this.Invoke(a);
            }
            else
            {
                int index = scene.AddPlane();
                if (index == -1)
                {
                    MessageBox.Show("添加失败");
                    return;
                }

                PrimitiveNode p = new PrimitiveNode(Primitive_type.Plane, index);
                p.Text = "plane" + planeIndex;
                scenePanel_sceneList.TopNode.Nodes.Add(p);
                scenePanel_sceneList.ExpandAll();
                ++planeIndex;
            }
            
        }

        public void AddTriangle()
        {
            if (scenePanel_sceneList.InvokeRequired)
            {
                Add a = new Add(AddTriangle);
                this.Invoke(a);
            }
            else
            {
                int index = scene.AddTriangle();
                if (index == -1)
                {
                    MessageBox.Show("添加失败");
                    return;
                }

                PrimitiveNode p = new PrimitiveNode(Primitive_type.Triangle, index);
                p.Text = "triangle" + triangleIndex;
                scenePanel_sceneList.TopNode.Nodes.Add(p);
                scenePanel_sceneList.ExpandAll();
                ++triangleIndex;
            }
        }

        


        public void ChangeSphere(int index)
        {
            sphere.centre.x = TextUtil.GetFloat(sphereCenterXText.Text);
            sphere.centre.y = TextUtil.GetFloat(sphereCenterYText.Text);
            sphere.centre.z = TextUtil.GetFloat(sphereCenterZText.Text);
            sphere.materialColor.r = TextUtil.GetFloat(sphereColorXText.Text);
            sphere.materialColor.g = TextUtil.GetFloat(sphereColorYText.Text);
            sphere.materialColor.b = TextUtil.GetFloat(sphereColorZText.Text);
            sphere.radius = TextUtil.GetFloat(sphereRadiusText.Text);
            scene.ChangeSphere(index, sphere);
        }

        public void ChangePlane(int index)
        {
            plane.centre.x = TextUtil.GetFloat(planeCenterXText.Text);
            plane.centre.y = TextUtil.GetFloat(planeCenterYText.Text);
            plane.centre.z = TextUtil.GetFloat(planeCenterZText.Text);
            plane.materialColor.r = TextUtil.GetFloat(planeColorXText.Text);
            plane.materialColor.g = TextUtil.GetFloat(planeColorYText.Text);
            plane.materialColor.b = TextUtil.GetFloat(planeColorZText.Text);
            plane.normal.x = TextUtil.GetFloat(planeNormalXText.Text);
            plane.normal.y = TextUtil.GetFloat(planeNormalYText.Text);
            plane.normal.z = TextUtil.GetFloat(planeNormalZText.Text);
            scene.ChangePlane(index, plane);
        }

        public void ChangeTriangle(int index)
        {
            triangle.points[0].x = TextUtil.GetFloat(trianglePoint1XText.Text);
            triangle.points[0].y = TextUtil.GetFloat(trianglePoint1YText.Text);
            triangle.points[0].z = TextUtil.GetFloat(trianglePoint1ZText.Text);
            triangle.points[1].x = TextUtil.GetFloat(trianglePoint2XText.Text);
            triangle.points[1].z = TextUtil.GetFloat(trianglePoint2YText.Text);
            triangle.points[1].z = TextUtil.GetFloat(trianglePoint2ZText.Text);
            triangle.points[2].x = TextUtil.GetFloat(trianglePoint3XText.Text);
            triangle.points[2].y = TextUtil.GetFloat(trianglePoint3YText.Text);
            triangle.points[2].z = TextUtil.GetFloat(trianglePoint3ZText.Text);

            triangle.materialColor.r = TextUtil.GetFloat(triangleColorXText.Text);
            triangle.materialColor.g = TextUtil.GetFloat(triangleColorYText.Text);
            triangle.materialColor.b = TextUtil.GetFloat(triangleColorZText.Text);
            triangle.normal.x = TextUtil.GetFloat(triangleNormalXText.Text);
            triangle.normal.y = TextUtil.GetFloat(triangleNormalYText.Text);
            triangle.normal.z = TextUtil.GetFloat(triangleNormalZText.Text);

            scene.ChangeTriangle(index, triangle);
        }

        public PrimitiveNode GetCurrentNode()
        {
            return (scenePanel_sceneList.SelectedNode as PrimitiveNode);
        }
    
        public void UpdatePrimitives()
        {
            if(isArgumentUpdate)
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
                scene.UpdateCamera();
                isCameraUpdate = false;
            }

        }


        #endregion

        private void ArgumentText_Enter(object sender, EventArgs e)
        {
            argumentFocus = true;
        }

        private void ArgumentText_Leave(object sender, EventArgs e)
        {
            argumentFocus = false;
        }

  
        private void pictureBox1_MouseMove(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                
                CommonInfo.currentMouseX = e.X;
                CommonInfo.currentMouseY = e.Y;

                CommonInfo.accumulatedX += CommonInfo.currentMouseX - CommonInfo.lastMouseX;
                CommonInfo.accumulatedY += CommonInfo.currentMouseY - CommonInfo.lastMouseY;

                CommonInfo.lastMouseX = CommonInfo.currentMouseX;
                CommonInfo.lastMouseY = CommonInfo.currentMouseY;

                isCameraUpdate = true;
                
            }
        }

        private void pictureBox1_MouseUp(object sender, MouseEventArgs e)
        {
            switch (e.Button)
            {
                case MouseButtons.Left:
                    CommonInfo.isPressingMouseLeft = false;
                    break;
                case MouseButtons.None:
                    break;
                case MouseButtons.Right:
                    break;
                case MouseButtons.Middle:
                    break;
                case MouseButtons.XButton1:
                    break;
                case MouseButtons.XButton2:
                    break;
                default:
                    break;
            }

        }

        private void pictureBox1_MouseDown(object sender, MouseEventArgs e)
        {
            switch (e.Button)
            {
                case MouseButtons.Left:
                    CommonInfo.isPressingMouseLeft = true;
                    CommonInfo.lastMouseX = e.X;
                    CommonInfo.lastMouseY = e.Y;
                    CommonInfo.currentMouseX = e.X;
                    CommonInfo.currentMouseY = e.Y;
                    break;
                case MouseButtons.None:
                    break;
                case MouseButtons.Right:
                    break;
                case MouseButtons.Middle:
                    break;
                case MouseButtons.XButton1:
                    break;
                case MouseButtons.XButton2:
                    break;
                default:
                    break;
            }
        }

        private void mainForm_KeyPress(object sender, KeyPressEventArgs e)
        {
            switch (e.KeyChar)
            {
                case 'w':
                    if (CommonInfo.isPressingMouseLeft)
                    {
                        scene.CameraMoveForward();
                        isCameraUpdate = true;
                    }
                    break;
                case 'a':
                    if (CommonInfo.isPressingMouseLeft)
                    {
                        scene.CameraMoveLeft();
                        isCameraUpdate = true;
                    }
                    break;
                case 's':
                    if (CommonInfo.isPressingMouseLeft)
                    {
                        scene.CameraMoveBack();
                        isCameraUpdate = true;
                    }
                    break;
                case 'd':
                    if (CommonInfo.isPressingMouseLeft)
                    {
                        scene.CameraMoveRight();
                        isCameraUpdate = true;
                    }
                    break;
            }
        }

        private void ResolutionItem_Click(object sender, EventArgs e)
        {
            ToolStripMenuItem item = sender as ToolStripMenuItem;
            int tag = int.Parse(item.Tag.ToString());
            CommonInfo.isResolutionChanged = true;
            CommonInfo.resolution = (Resolution)tag;
        }
    }

}