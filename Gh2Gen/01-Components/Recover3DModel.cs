using System;
using System.Collections.Generic;
using System.Linq;
using System.Collections;
using Grasshopper.Kernel;
using Rhino.Geometry;
using Grasshopper.Kernel.Data;
using Grasshopper;
using KangarooSolver;
using Rhino.Geometry.Collections;
using Gh2Gen._02_UtilityFunction;
using Gh2Gen._00_BaseObj;

namespace Gh2Gen._01_Components
{
    public class Recover3DModel : GH_Component
    {
        /// <summary>
        /// Initializes a new instance of the Recover3DModel class.
        /// </summary>
        public Recover3DModel()
          : base("Recover3DModel", "2Dto3D",
              "从二维平面图复原三维模型，需要三维节点信息、有节点编号的平面图",
              "Midas", "02 Model")
        {
        }

        /// <summary>
        /// Registers all the input parameters for this component.
        /// </summary>
        protected override void RegisterInputParams(GH_Component.GH_InputParamManager pManager)
        {
            pManager.AddPointParameter("3DPointsCloud", "3DPtCloud", "三维点云信息", GH_ParamAccess.list);
            pManager.AddIntegerParameter("2DPointsIndex", "2DPtInd", "二维点云索引", GH_ParamAccess.list);
            pManager.AddPointParameter("2DPointsCoord", "2DPtCoord", "二维点云坐标", GH_ParamAccess.list);
            pManager.AddLineParameter("2DLines", "2DLines", "二维线信息", GH_ParamAccess.list);
            pManager.AddNumberParameter("error", "err", "节点合并误差", GH_ParamAccess.item, (double)20);




        }

        /// <summary>
        /// Registers all the output parameters for this component.
        /// </summary>
        protected override void RegisterOutputParams(GH_Component.GH_OutputParamManager pManager)
        {
            pManager.AddLineParameter("3DLines", "3DLines", "三维线模型", GH_ParamAccess.list);
            pManager.AddIntegerParameter("LinePts", "LinePts", "三维线断点索引", GH_ParamAccess.tree);
            pManager.Register_GenericParam("ModelData", "ModelData", "模型数据", GH_ParamAccess.item);

        }

        /// <summary>
        /// This is the method that actually does the work.
        /// </summary>
        /// <param name="DA">The DA object is used to retrieve from inputs and store in outputs.</param>
        protected override void SolveInstance(IGH_DataAccess DA)
        {
            List<Point3d> PointsCloud_3D = new List<Point3d>();
            List<int> PointsIndex_2D = new List<int>();
            List<Point3d> PointsCoords_2D = new List<Point3d>();
            List<Line> Lines_2D = new List<Line>();
            List<Line> Lines_3D = new List<Line>();
            double err = 20;
            RTree PointsCoords_2DRtree = new RTree();

            bool bool1 = DA.GetDataList(0, PointsCloud_3D);
            bool bool2 = DA.GetDataList(1, PointsIndex_2D);
            bool bool3 = DA.GetDataList(2, PointsCoords_2D);
            bool bool4 = DA.GetDataList(3, Lines_2D);
            bool bool5 = DA.GetData(4, ref err);

            ModelCls model = new ModelCls();
            if(bool1)
            {
                model.Nodes = PointsCloud_3D;

            }
            if(bool2&&bool3)
            {
                for (int i = 0; i < PointsCoords_2D.Count; i++)
                {
                    PointsCoords_2DRtree.Insert(PointsCoords_2D[i], i);
                }
            }
            if(bool1&& bool2 && bool3&& bool4)
            {
                List<BeamElementCls> BeamElements = new List<BeamElementCls>();
                Lines_2D.ForEach(x => BeamElements.Add(new BeamElementCls(x, 1, 1)));

                int num = BeamElements.Count;
                for (int i = 0; i < num; i++)
                {
                    List<int> verticesIndex = new List<int>();
                    for (int j = 0; j < 2; j++)
                    {
                        Point3d chkPt = BeamElements[i].Line.PointAt((double)j);
                        FunctionClass.RtreeSearch(PointsCoords_2DRtree,PointsIndex_2D,chkPt, err, verticesIndex);
                    }
                    if(verticesIndex.Count==2)
                    {
                        BeamElements[i].Nodes_no = verticesIndex;
                        BeamElements[i].Line = new Line(PointsCloud_3D[verticesIndex[0]-1], PointsCloud_3D[verticesIndex[1]-1]);
                    }

                }
                model.BeamElements = BeamElements;
                BeamElements.ForEach(x => Lines_3D.Add(x.Line));
                DA.SetDataList(0, Lines_3D);
                DA.SetData(2, model);
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
            get { return new Guid("455506f5-04cd-42df-90fc-1fd6118d33c6"); }
        }
    }
}