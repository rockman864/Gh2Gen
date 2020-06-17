using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Grasshopper.Kernel.Types;

namespace Gh2Gen._00_BaseObj
{
    public class GH_ShellElement : GH_Goo<ShellElementCls>
    {
        public GH_ShellElement(ShellElementCls shelli):base(shelli)
        {

        }
        public override bool IsValid
        {
            get { return true; }
        }

        public override string TypeName { get { return "GH_ShellElement"; } }

        public override string TypeDescription { get { return "GH_ShellElement"; } }

        public override IGH_Goo Duplicate()
        {
            return null;
        }

        public override string ToString()
        {
            return "this is GH_ShellElement";
        }
        public override bool CastTo<Q>(ref Q target)
        {
            Type c = new ShellElementCls().GetType();
            object obj2 = base.m_value;
            target = (Q)obj2;
            return typeof(Q).IsAssignableFrom(c);
        }

    }
}
