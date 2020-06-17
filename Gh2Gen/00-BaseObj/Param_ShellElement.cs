using System;
using System.Collections.Generic;

using Grasshopper.Kernel;
using Rhino.Geometry;
using Grasshopper;
using System.Collections;

namespace Gh2Gen._00_BaseObj
{
    public class Param_ShellElement : GH_Param<GH_ShellElement>, IGH_PreviewObject
    {
        /// <summary>
        /// Initializes a new instance of the Param_ShellElement class.
        /// </summary>
        /// 
        private bool m_hidden;
        public Param_ShellElement() : base(new GH_InstanceDescription("ShellElement", "ShellElement", "ShellElement", "Midas", "00 Params"))
        {
            m_hidden = false;
        }

        /// <summary>
        /// Registers all the input parameters for this component.
        /// </summary>

        /// <summary>
        /// Provides an Icon for the component.
        /// </summary>
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
            get { return new Guid("7ead12f4-7a69-410e-a800-9a868e5ad7f6"); }
        }

        public bool Hidden
        {
            get { return m_hidden; }
            set { m_hidden = value; }
        }

        public bool IsPreviewCapable { get { return true; } }

        public BoundingBox ClippingBox { get { return base.Preview_ComputeClippingBox(); } }

        public void DrawViewportWires(IGH_PreviewArgs args)
        {
            IEnumerator er = base.m_data.GetEnumerator();
            while (er.MoveNext())
            {
                GH_ShellElement current = (GH_ShellElement)er.Current;
                if (current != null)
                {
                    current.Value.preview(args);
                }
            }
        }

        public void DrawViewportMeshes(IGH_PreviewArgs args)
        {
            if ((args.Document.PreviewMode == GH_PreviewMode.Shaded) && args.Display.SupportsShading)
            {
                base.Preview_DrawMeshes(args);
            }

        }
    }
}