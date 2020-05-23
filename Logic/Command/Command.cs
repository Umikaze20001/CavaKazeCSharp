using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Render 
{
    //public interface ICommand
    //{
    //    void Excute();
     
    //}
    public class Command
    {
        private Delegate dele;
        private object[] param;
        public Command(Delegate x,object[] b)
        {
            dele = x;
            param = b;
        }

        public Command(Delegate x)
        {
            dele = x;
            param = null;
        }

        public void SetCommand(Delegate x,object[] b)
        {
            dele = x;
            param = b;
        }

        public void SetCommand(Delegate x)
        {
            dele = x;
            param = null;
        }

        public void Clear()
        {
            dele = null;
            param = null;
        }

        public void Excute()
        {
            dele.DynamicInvoke(param);
        }
    }

    //public abstract class Command
    //{
    //    protected Command(mainForm mf)
    //    {
    //        mainForm = mf;
    //    }
    //    public abstract void Excute();
    //    protected mainForm mainForm;

    //}

    //public class DeleteCommand :Command
    //{

    //    public override void Excute()
    //    {
    //        mainForm.DeletePrimitive(index);
    //    }

    //    public DeleteCommand(int index, mainForm mf) : base(mf)
    //    {
    //        this.index = index;
    //    }
    //    int index;
    //}
 
    //public class AddSphereCommand : Command
    //{
    //    public AddSphereCommand(mainForm mf) : base(mf)
    //    {

    //    }

    //    public override void Excute()
    //    {
    //        mainForm.AddShpere();
    //    }
    //}

    //public class AddTriangleCommand : Command
    //{
    //    public AddTriangleCommand(mainForm mf) : base(mf)
    //    {
    //    }

    //    public override void Excute()
    //    {
    //        mainForm.AddTriangle();
    //    }
    //}

    //public class AddPlaneCommand : Command
    //{


    //    public AddPlaneCommand(mainForm mf) : base(mf)
    //    {
    //    }

    //    public override void Excute()
    //    {
    //        mainForm.AddPlane();
    //    }
    //}

    //public class UpdateCameraCommand : Command
    //{

    //}
}
