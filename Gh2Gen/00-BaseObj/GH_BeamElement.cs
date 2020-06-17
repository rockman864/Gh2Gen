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
    public class GH_BeamElement : GH_GeometricGoo<BeamElementCls>
    {
        public GH_BeamElement()
        { }
        public GH_BeamElement(BeamElementCls beamEle) : base(beamEle)
        {

        }
        public override BoundingBox Boundingbox { get; }

        public override string TypeName { get { return "GH_BeamElement"; } }

        public override string TypeDescription { get { return "GH_BeamElement"; } }

        public override IGH_GeometricGoo DuplicateGeometry()
        {
            return null;
        }
        public override bool CastTo<T>(out T target)
        {
            Type c = new BeamElementCls().GetType();
            object obj2 = base.m_value;
            target = (T)obj2;
            return typeof(T).IsAssignableFrom(c);

        }
        public override BoundingBox GetBoundingBox(Transform xform)
        {
            return Boundingbox;
        }

        public override IGH_GeometricGoo Morph(SpaceMorph xmorph)
        {
            return null;
        }

        public override string ToString()
        {
            return "this is GH_BeamElement";
        }

        public override IGH_GeometricGoo Transform(Transform xform)
        {
            return null;
        }
    }
}
