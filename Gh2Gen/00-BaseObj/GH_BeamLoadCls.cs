using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Grasshopper.Kernel;
using Grasshopper.Kernel.Types;
using Rhino.Geometry;

namespace Gh2Gen._00_BaseObj
{
    public class GH_BeamLoadCls : GH_GeometricGoo<BeamLoadCls>
    {
        public GH_BeamLoadCls()
        {

        }
        public GH_BeamLoadCls(BeamLoadCls load):base(load)
        {
        }
        public override BoundingBox Boundingbox
        {
            get
            {
                return Boundingbox;
            }
        }
        public override bool CastTo<Q>(out Q target)
        {
            Type c = new BeamLoadCls().GetType();
            object obj2 = base.m_value;
            target = (Q)obj2;
            return typeof(Q).IsAssignableFrom(c);
        }
        public override string TypeName
        {
            get
            {
                return "GH_BeamLoad";
            }
        }

        public override string TypeDescription
        {
            get
            {
                return "GH_BeamLoad";
            }
        }


        public override IGH_GeometricGoo DuplicateGeometry()
        {
            return null;
        }

        public override BoundingBox GetBoundingBox(Transform xform)
        {
            return this.Boundingbox;
        }

        public override IGH_GeometricGoo Morph(SpaceMorph xmorph)
        {
            return null;
        }

        public override string ToString()
        {

            return string.Concat(string.Format("load value is:{0} KN/m", Value.Value));
        }

        public override IGH_GeometricGoo Transform(Transform xform)
        {
            return null;
        }
    }
}
