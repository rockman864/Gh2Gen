using System;
using System.Collections.Generic;

using Grasshopper.Kernel;
using Rhino.Geometry;
using Grasshopper;
using Grasshopper.Kernel.Data;

namespace Gh2Gen
{
    public class CombineComponent : GH_Component
    {
        /// <summary>
        /// Initializes a new instance of the CombineObjects class.
        /// </summary>
        public CombineComponent()
          : base("CombineObjects", "CBOBJ",
              "将线单元和面单元合并到一个模型",
              "Midas", "Elements")
        {
        }

        /// <summary>
        /// Registers all the input parameters for this component.
        /// </summary>
        protected override void RegisterInputParams(GH_Component.GH_InputParamManager pManager)
        {
            pManager.AddGenericParameter("LineObjs", "LObjs", "具有物理属性的线对象", GH_ParamAccess.list);
            pManager.AddNumberParameter("error", "err", "节点合并误差", GH_ParamAccess.item,(double)0.004);
        }

        /// <summary>
        /// Registers all the output parameters for this component.
        /// </summary>
        protected override void RegisterOutputParams(GH_Component.GH_OutputParamManager pManager)
        {
            pManager.AddPointParameter("List of points", "Points", "合并相近点后独立的点", GH_ParamAccess.list);
            pManager.Register_GenericParam("LineObjs", "LObjs", "线对象", GH_ParamAccess.list);
            pManager.AddIntegerParameter("Line-Point data", "P0-P1", "每个线的起始点索引", GH_ParamAccess.tree);
        }

        /// <summary>
        /// This is the method that actually does the work.
        /// </summary>
        /// <param name="DA">The DA object is used to retrieve from inputs and store in outputs.</param>
        protected override void SolveInstance(IGH_DataAccess DA)
        {
            
            List<LineObjClass> lineobjs = new List<LineObjClass>();
            List<Point3d> points = new List<Point3d>();
            double err = 0;
            int pointsCount = 0;//添加到points列表中点的个数
            int pointsNo = 0;//线段端点的索引
            DataTree<int> tree = new DataTree<int>();

            DA.GetDataList(0, lineobjs);
            DA.GetData(1, ref err);
            int num = lineobjs.Count;
            for(int i=0;i<num;i++)
            {
                List<Point3d> lineV = new List<Point3d>();
                List<int> vInd = new List<int>();
                GH_Path path = new GH_Path(tree.BranchCount);

                Point3d p0 = lineobjs[i].Line.PointAt(0);
                Point3d p1 = lineobjs[i].Line.PointAt(1);
                lineV.Add(p0);
                lineV.Add(p1);
                for(int j=0;j<lineV.Count;j++)
                {
                    bool bo;
                    int ind;
                    FunctionClass.BigThanErr(points, lineV[j], err, out bo, out ind);
                    if (bo)//如果点lineV[j]不跟其他点重合
                    {
                        points.Add(lineV[j]);
                        pointsCount += 1;
                        pointsNo = pointsCount;
                    }
                    else
                    {
                        pointsNo = ind+1;
                    }
                    vInd.Add(pointsNo);
                    lineobjs[i].Vertices = vInd;
                    tree.Add(vInd[j], path);
                }
            }
            DA.SetDataList(0,points);
            DA.SetDataList(1, lineobjs);
            DA.SetDataTree(2, tree);
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
            get { return new Guid("faa85724-6d45-42e3-b960-56a05d981598"); }
        }
    }
}