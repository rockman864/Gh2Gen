using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Grasshopper.Kernel.Types;

namespace Gh2Gen._00_BaseObj
{
    public class GH_ObjList : GH_Goo<ObjList>
    {
        public GH_ObjList(ObjList objlist):base(objlist)
        {

        }
        public override bool IsValid => throw new NotImplementedException();

        public override string TypeName => throw new NotImplementedException();

        public override string TypeDescription => throw new NotImplementedException();

        public override IGH_Goo Duplicate()
        {
            return null;
        }

        public override string ToString()
        {
            return "this is GH_ObjList ";
        }
    }
}
