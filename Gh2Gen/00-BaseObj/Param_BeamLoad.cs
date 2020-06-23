using System;
using System.Collections.Generic;
using System.Collections;
using System.Drawing;
using Grasshopper.Kernel;
using Rhino.Geometry;
using Grasshopper.Kernel.Types;
namespace Gh2Gen._00_BaseObj
{
    /// <summary>
    /// GH_Param，类似GH_Component,GH中另一种组件，参数组件
    /// </summary>
    public class Param_BeamLoad : GH_Param<GH_BeamLoadCls>, IGH_PreviewObject
    {

        private bool m_hidden;
        public Param_BeamLoad() : base(new GH_InstanceDescription("BeamLoad", "BeamLoad", "BeamLoad", "Midas", "00 Params"))
        {
            m_hidden = false;

        }


        protected override System.Drawing.Bitmap Icon
        {
            get
            {
                //You can add image files to your project resources and access them like this:
                // return Resources.IconForThisComponent;
                return null;
            }
        }


        public override Guid ComponentGuid
        {
            get { return new Guid("e890798d-5a39-434c-bbd2-e7715dcc3da3"); }
        }
        public bool Hidden { get { return this.m_hidden; } set { this.m_hidden = value; } }

        public bool IsPreviewCapable { get { return true; } }

        public BoundingBox ClippingBox
        {
            get
            {
                return base.Preview_ComputeClippingBox();
            }
        }

        public void DrawViewportWires(IGH_PreviewArgs args)
        {
            IEnumerator er = base.m_data.GetEnumerator();
            while (er.MoveNext())
            {
                GH_BeamLoadCls current = (GH_BeamLoadCls)er.Current;
                if(current!=null)
                {
                    current.Value.preview(args);
                }
            }
        }

        public void DrawViewportMeshes(IGH_PreviewArgs args)
        {
        }
    }
}