using System;
using System.Collections.Generic;

using Grasshopper.Kernel;
using Rhino.Geometry;
using Gh2Gen._00_BaseObj;
// In order to load the result of this wizard, you will also need to
// add the output bin/ folder of this project to the list of loaded
// folder in Grasshopper.
// You can use the _GrasshopperDeveloperSettings Rhino command for that.

namespace Gh2Gen._01_Components
{
    public class BeamELement : GH_Component
    {
        /// <summary>
        /// Each implementation of GH_Component must provide a public 
        /// constructor without any arguments.
        /// Category represents the Tab in which the component will appear, 
        /// Subcategory the panel. If you use non-existing tab or panel names, 
        /// new tabs/panels will automatically be created.
        /// </summary>
        public BeamELement()
          : base("BeamEle", "BELE",
              "对线段赋予物理属性建立单元",
              "MIDAS", "01 Elements")
        {
        }

        /// <summary>
        /// Registers all the input parameters for this component.
        /// </summary>
        protected override void RegisterInputParams(GH_Component.GH_InputParamManager pManager)
        {
            pManager.AddLineParameter("List of lines", "Lines", "线组", GH_ParamAccess.item);
            pManager.AddIntegerParameter("Material Number", "Material", "材料编号", GH_ParamAccess.item, (int)1);
            pManager.AddIntegerParameter("Section Number", "Section", "截面编号", GH_ParamAccess.item, (int)2);
            pManager.AddNumberParameter("Beta Angle", "Beta", "Beta角度", GH_ParamAccess.item, (double)0);
            pManager.AddTextParameter("GroupNames", "GName", "所属组别", GH_ParamAccess.item);
            pManager[4].Optional = true;


        }

        /// <summary>
        /// Registers all the output parameters for this component.
        /// </summary>
        protected override void RegisterOutputParams(GH_Component.GH_OutputParamManager pManager)
        {
            pManager.AddGenericParameter( "BeamElements", "Beams", "具有物理属性的梁单元", GH_ParamAccess.item);
        }

        /// <summary>
        /// 对每个线段赋予材料、截面、beta角等属性，然后创建线单元对象
        /// </summary>
        /// <param name="DA">The DA object can be used to retrieve data from input parameters and 
        /// to store data in output parameters.</param>
        protected override void SolveInstance(IGH_DataAccess DA)
        {
            Line linei = new Line();
            int material = 0;
            int section = 0;
            double beta = 0;
            string gnames = null;
            List<string> gnames2 = null;



            DA.GetData(0, ref linei);
            DA.GetData(1, ref material);
            DA.GetData(2, ref section);
            DA.GetData(3, ref beta);
            bool bool_gname = DA.GetData(4, ref gnames);
            if (bool_gname)
            {
                string[] namesArray = gnames.Split(',');
                gnames2 = new List<string>(namesArray);
            }

            BeamElementCls Beami = new BeamElementCls(linei, material, section);
            Beami.Groupname = gnames2;
            DA.SetData(0, new GH_BeamElement(Beami));
        }

        /// <summary>
        /// Provides an Icon for every component that will be visible in the User Interface.
        /// Icons need to be 24x24 pixels.
        /// </summary>
        protected override System.Drawing.Bitmap Icon
        {
            get
            {
                // You can add image files to your project resources and access them like this:
                //return Resources.IconForThisComponent;
                return null;
            }
        }

        /// <summary>
        /// Each component must have a unique Guid to identify it. 
        /// It is vital this Guid doesn't change otherwise old ghx files 
        /// that use the old ID will partially fail during loading.
        /// </summary>
        public override Guid ComponentGuid
        {
            get { return new Guid("231b2911-1bd0-4cf1-93e4-18c6d794a3ef"); }
        }
    }

}
