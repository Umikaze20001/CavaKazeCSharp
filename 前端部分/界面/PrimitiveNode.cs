using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
namespace Render
{

    public class PrimitiveNode : TreeNode
    {
        public PrimitiveNode(Primitive_type type,int i)
        {
            this.type = type;
            index = i;
        }

        public PrimitiveNode(string text) : base(text)
        {
        }

        public PrimitiveNode(string text, TreeNode[] children) : base(text, children)
        {
        }

        public PrimitiveNode(string text, int imageIndex, int selectedImageIndex) : base(text, imageIndex, selectedImageIndex)
        {
        }

        public PrimitiveNode(string text, int imageIndex, int selectedImageIndex, TreeNode[] children) : base(text, imageIndex, selectedImageIndex, children)
        {
        }

        protected PrimitiveNode(SerializationInfo serializationInfo, StreamingContext context) : base(serializationInfo, context)
        {
        }



        public Primitive_type type;
        public readonly int index;
    }
}
