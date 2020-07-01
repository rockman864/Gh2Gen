using System;
using System.Collections.Generic;

using Grasshopper.Kernel;
using Rhino.Geometry;
using Gh2Gen._00_BaseObj;

namespace Gh2Gen._01_Components
{
    public class BeamLoadCmp : GH_Component
    {
        /// <summary>
        /// Initializes a new instance of the AddLineLoadComonent class.
        /// </summary>
        //GH_ObjList objlist = null;
        /// 
        public BeamLoadCmp()
          : base("BeamLoad", "BeamLoad",
              "给梁单元增加线荷载",
              "Midas", "02 Model")
        {
        }

        /// <summary>
        /// Registers all the input parameters for this component.
        /// </summary>
        protected override void RegisterInputParams(GH_Component.GH_InputParamManager pManager)
        {
            pManager.AddIntegerParameter("LoadCase", "LoadCase", "荷载工况:1-表示恒载DL,2-表示活载LL", GH_ParamAccess.item, (int)1);
            pManager.AddGenericParameter("BeamElements", "Beams", "要添加梁线荷载的梁单元", GH_ParamAccess.item);
            pManager.AddIntegerParameter("CoordinateSystem", "GlabalOrLocal", "荷载坐标系:1-整体坐标系,2-局部坐标系", GH_ParamAccess.item, (int)1);
            pManager.AddIntegerParameter("Direction", "Dir", "荷载方向:1-X,2-Y,3-Z", GH_ParamAccess.item, (int)3);
            pManager.AddNumberParameter("Value(KN/M)", "Value", "荷载大小，单位KN/M", GH_ParamAccess.item);

        }

        /// <summary>
        /// Registers all the output parameters for this component.
        /// </summary>
        protected override void RegisterOutputParams(GH_Component.GH_OutputParamManager pManager)
        {
            pManager.AddParameter(new Param_BeamLoad(), "BeamLoads", "BeamLoads", "梁单元线荷载对象", GH_ParamAccess.item);
        }

        /// <summary>
        /// This is the method that actually does the work.
        /// </summary>
        /// <param name="DA">The DA object is used to retrieve from inputs and store in outputs.</param>
        protected override void SolveInstance(IGH_DataAccess DA)
        {
            int loadcase = 1;
            GH_BeamElement Beams = null;
            int coordinate = 1;
            int direction = 3;
            double value = double.NaN;

            bool bool_case = DA.GetData(0, ref loadcase);
            bool bool_beams = DA.GetData(1, ref Beams);
            bool bool_coord = DA.GetData<int>(2, ref coordinate);
            bool bool_dir = DA.GetData<int>(3, ref direction);
            bool bool_value = DA.GetData<double>(4, ref value);

            if (bool_beams && bool_value)
            {
                BeamLoadCls beamloadi = new BeamLoadCls(loadcase, Beams.Value.Line, coordinate, direction, value);
                DA.SetData(0, new GH_BeamLoadCls(beamloadi));
            }

        }

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
            get { return new Guid("ef99a984-bd64-4894-acfe-fe8e090f5aa5"); }
        }

    }
}