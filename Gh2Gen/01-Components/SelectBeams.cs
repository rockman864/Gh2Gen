using System;
using System.Collections.Generic;

using Grasshopper.Kernel;
using Rhino.Geometry;
using Gh2Gen._00_BaseObj;
namespace Gh2Gen._01_Components
{
    public class SelectBeams : GH_Component
    {
        /// <summary>
        /// Initializes a new instance of the SelectComponent class.
        /// </summary>
        public SelectBeams()
          : base("SelectBeams", "SelectB",
              "通过封闭的线建立选区，选择线单元",
              "Midas", "01 Elements")
        {
        }

        /// <summary>
        /// Registers all the input parameters for this component.
        /// </summary>
        protected override void RegisterInputParams(GH_Component.GH_InputParamManager pManager)
        {
            pManager.AddGenericParameter("Beams", "Beams", "要被选的梁单元", GH_ParamAccess.item);
            pManager.AddCurveParameter("ClosedCrv", "ClosedCrv", "用于建立选区的封闭曲线", GH_ParamAccess.item);
            pManager.AddNumberParameter("tolerance", "t", "选择精度", GH_ParamAccess.item, (double)0.001);
        }

        /// <summary>
        /// Registers all the output parameters for this component.
        /// </summary>
        protected override void RegisterOutputParams(GH_Component.GH_OutputParamManager pManager)
        {
            pManager.AddParameter(new Param_BeamElement(), "Selected Beams", "Beams", "被选中的梁单元", GH_ParamAccess.item);
        }

        /// <summary>
        /// This is the method that actually does the work.
        /// </summary>
        /// <param name="DA">The DA object is used to retrieve from inputs and store in outputs.</param>
        protected override void SolveInstance(IGH_DataAccess DA)
        {
            GH_BeamElement beami = null;
            Curve c = null;
            double err = 0.001;

            bool boolL = DA.GetData(0, ref beami);
            bool boolC = DA.GetData<Curve>(1, ref c);
            bool boolClosed = c.IsClosed;
            DA.GetData(2, ref err);
            if (boolL && boolC && boolClosed)
            {
                List<Point3d> pts = new List<Point3d>();
                pts.Add(beami.Value.Line.PointAt(0));
                pts.Add(beami.Value.Line.PointAt(1));
                BoundingBox bx = c.GetBoundingBox(false);
                Plane curvePlane = Plane.WorldXY;
                c.TryGetPlane(out curvePlane, bx.Min.DistanceTo(bx.Max));
                List<Point3d> planePts = new List<Point3d>();
                for (int j = 0; j < 2; j++)
                {
                    planePts.Add(curvePlane.ClosestPoint(pts[j]));//获得杆件在平面内的投影节点
                }

                if (c.Contains(planePts[0], curvePlane, err) == PointContainment.Inside && c.Contains(planePts[1], curvePlane, err) == PointContainment.Inside)//起点和终点都在曲线内
                {
                    DA.SetData(0, beami);

                }
            }
            else { this.AddRuntimeMessage(GH_RuntimeMessageLevel.Error, "用于建立选区的curve必须是封闭曲线"); }
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
            get { return new Guid("a5d4ac1c-ceb8-438e-8906-ae75a34e28bf"); }
        }
        public override GH_Exposure Exposure
        {
            get
            {
                return GH_Exposure.secondary;
            }
        }

    }
}