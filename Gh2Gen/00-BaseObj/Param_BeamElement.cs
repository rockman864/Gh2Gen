using System;
using System.Collections.Generic;
using System.Collections;
using System.Drawing;
using Grasshopper.Kernel;
using Rhino.Geometry;
using Grasshopper.Kernel.Types;

namespace Gh2Gen._00_BaseObj
{
    public class Param_BeamElement : GH_Param<GH_BeamElement>,IGH_PreviewObject
    {
        /// <summary>
        /// Initializes a new instance of the Param_BeamElement class.
        /// </summary>
        private bool m_hidden;
        private bool m_preview;
        public Param_BeamElement( ):base(new GH_InstanceDescription("BeamElement", "BeamElement", "BeamElement", "Midas", "00 Params"))
        {
            m_hidden = false;
            m_preview = true;
        }

        /// <summary>
        /// Registers all the input parameters for this component.
        /// </summary>

        public void DrawViewportWires(IGH_PreviewArgs args)
        {
            IEnumerator er = base.m_data.GetEnumerator();
            while (er.MoveNext())
            {
                GH_BeamElement current = (GH_BeamElement)er.Current;
                if (current != null)
                {
                    current.Value.preview(args);
                }
            }
        }

        public void DrawViewportMeshes(IGH_PreviewArgs args)
        {

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

        /// <summary>
        /// Gets the unique ID for this component. Do not change this ID after release.
        /// </summary>
        public override Guid ComponentGuid
        {
            get { return new Guid("06cf691a-1c76-4453-b0ce-22d86f090fc4"); }
        }

        public bool Hidden
        {
            get
            {
                return m_hidden;
            }
            set
            {
                m_hidden = value;
            }
        }
        public override string ToString()
        {
            return "this is a beamElement";
        }
        public bool IsPreviewCapable
        {
            get
            {
                return m_preview;
            }
        }

        public BoundingBox ClippingBox { get; set; }
    }
}