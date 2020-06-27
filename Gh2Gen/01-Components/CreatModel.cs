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
    public class CreatModel : GH_Component
    {
        /// <summary>
        /// Initializes a new instance of the CombineComponent2 class.
        /// </summary>
        public CreatModel()
          : base("ModelData", "ModelData",
              "将线单元和面单元合并到一个模型",
              "Midas", "02 Model")
        {
        }

        /// <summary>
        /// Registers all the input parameters for this component.
        /// </summary>
        protected override void RegisterInputParams(GH_Component.GH_InputParamManager pManager)
        {
            pManager.AddGenericParameter("BeamElements", "Beams", "具有各种参数的梁单元", GH_ParamAccess.list);
            pManager.AddGenericParameter("ShellElements", "Shells", "具有各种参数的壳单元", GH_ParamAccess.list);
            pManager[1].Optional = true;
            pManager.AddGenericParameter("BeamLoads", "BeamLoads", "梁单元线荷载", GH_ParamAccess.list);
            pManager[2].Optional = true;
            pManager.AddGenericParameter("ShellLoads", "ShellLoads", "壳单元线荷载", GH_ParamAccess.list);
            pManager[3].Optional = true;
            pManager.AddGenericParameter("Supports", "Supports", "边界约束", GH_ParamAccess.list);
            pManager[4].Optional = true;
            pManager.AddNumberParameter("error", "err", "节点合并误差", GH_ParamAccess.item, (double)0.01);
        }

        /// <summary>
        /// Registers all the output parameters for this component.
        /// </summary>
        protected override void RegisterOutputParams(GH_Component.GH_OutputParamManager pManager)
        {
            pManager.AddPointParameter("List of points", "Points", "合并相近点后独立的点", GH_ParamAccess.list);
            pManager.Register_GenericParam("ModelData", "ModelData", "模型数据", GH_ParamAccess.list);
            pManager.AddTextParameter("Information", "info", "模型数据概况", GH_ParamAccess.list);


        }

        /// <summary>
        /// 主要逻辑过程包括三步：1.建立所有单元的端点列表pointsOrigin 2.利用kangaroo的去重功能对pointsOrigin进行去重得到pointsAfter，然后存入Rtree，便于下一步搜索。
        ///3.循环每个线段的端点，在Rtree里搜索端点。
        /// </summary>
        /// <param name="DA">The DA object is used to retrieve from inputs and store in outputs.</param>
        protected override void SolveInstance(IGH_DataAccess DA)
        {


            //建立局部变量存储输入参数
            List<BeamElementCls> BeamList = new List<BeamElementCls>();
            List<ShellElementCls> ShellList = new List<ShellElementCls>();
            List<BaseElementCls> ElementsList = new List<BaseElementCls>();
            List<BeamLoadCls> BeamLoadList = new List<BeamLoadCls>();
            List<ShellLoadCls> ShellLoadList = new List<ShellLoadCls>();
            List<SupportCls> SupportList = new List<SupportCls>();

            double err = 0.01;
            DA.GetData(5, ref err);
            //获取输入参数传给局部变量，并返回是否成功或取数据
            bool bool1 = DA.GetDataList(0, BeamList);
            bool bool2 = DA.GetDataList(1, ShellList);
            bool boolBL = DA.GetDataList(2, BeamLoadList);

            BeamLoadList.RemoveAll(j=> { return j == null; });
            bool boolSL = DA.GetDataList(3, ShellLoadList);
            bool boolSPT = DA.GetDataList(4, SupportList);
            ModelCls model = new ModelCls();
            RTree beamMidPts = new RTree();
            if (bool1)
            {
                ElementsList.AddRange(BeamList);
                for (int k = 0; k < BeamList.Count; k++)
                {
                    BeamList[k].Ele_no = k + 1;//对线单元赋予单元编号
                }

                ///
                ///将线单元和面单元的节点进行归并统一 得到pointsAfter
                ///
                //建立所有线段端点组成的列表
                List<Point3d> pointsOrigin = FunctionClass.getPoints(BeamList);
                if (bool2)
                {
                    //获得所有面单元对象的顶点列表
                    List<Point3d> pointsMesh = FunctionClass.getPoints(ShellList);
                    pointsOrigin.AddRange(pointsMesh);
                }
                //1.利用kangaroo中的节点去重功能进行去重，效率比自己判断节点距离高
                //2.注意：如果err大于某些线的长度，会造成线段数据丢失，导出的模型没有这部分线段
                List<Point3d> pointsAfter = Util.RemoveDupPts2(pointsOrigin, err);//测距离方法：x，y，z方向差同时小于err的点。而不是真实距离，真实距离可能大于err，但是速度比算真实距离快。
                //List<Point3d> pointsAfter = FunctionClass.deleteDuplicatPts(pointsOrigin, err);
                //将去重后的点存入Rtree，由列表转变为树形结构数据，以便高效的进行空间点的搜索，建立线段端点索引
                RTree pointsAfterRt = new RTree();
                for (int j = 0; j < pointsAfter.Count; j++)
                {
                    pointsAfterRt.Insert(pointsAfter[j], j);
                }
                for (int k = 0; k < BeamList.Count; k++)
                {
                    beamMidPts.Insert(BeamList[k].Line.PointAt(0.5),k);
                }

                SetLineVerInd(pointsAfterRt, BeamList, err);//对线单元赋予节点索引***核心功能**
                DA.SetDataList(0, pointsAfter);

                if (bool2)
                {
                    ElementsList.AddRange(ShellList);
                    for (int m = 0; m < ShellList.Count; m++)
                    {
                        ShellList[m].Ele_no = BeamList.Count + 1 + m;//对面单元赋予单元编号
                    }
                    SetMeshVerInd(pointsAfterRt, ShellList, err);//对面单元赋予节点索引***核心功能**
                }
                model.Nodes = pointsAfter;
                model.BeamElements = BeamList;
                model.ShellElements = ShellList;
                model.Elements = ElementsList;
                model.Groups = FunctionClass.getGroupObjects0(ElementsList);
                List<string> infoList = new List<string>();
                string info1 = string.Format("this model has:{0} Nodes", model.Nodes.Count.ToString());
                string info2 = string.Format("this model has:{0} Elements", model.Elements.Count.ToString());
                string info3 = string.Format("     includes1:{0} Beams", model.BeamElements.Count.ToString());
                string info4 = string.Format("     includes1:{0} Shells", model.ShellElements.Count.ToString());
                infoList.Add(info1);
                infoList.Add(info2);
                infoList.Add(info3);
                infoList.Add(info4);
                DA.SetDataList(2, infoList);
            }
            if (boolBL)//如果有梁单元荷载的话
            {
                model.BeamLoads = BeamLoadList;
                for (int i = 0; i < BeamLoadList.Count; i++)
                {
                    BeamLoadCls bloadi = BeamLoadList[i];
                    if(bloadi!=null)
                    {
                        //Point3d midPtChk = bloadi.LoadLine.PointAt(0.5);
                        SetBeamLoadEleNo(beamMidPts,bloadi,err);//设置梁单元荷载对象中的单元编号属性，此属性在单元未合并之前，并不知道每个单元的编号，因此只能放在创建模型中实现
                    }
                   
                }
            }
            if (boolSL)//如果有壳单元荷载信息的话
            {
                model.ShellLoads = ShellLoadList;
            }
            if (boolSPT)//如果有支座信息的话，附加支座信息
            {
                model.Supports = SupportList;
            }
            DA.SetData(1, model);//输出模型数据对象
        }
        /// <summary>
        /// 设置荷载对象的单元编号
        /// </summary>
        /// <param name="rt">所有梁单元中点，RTree存储</param>
        /// <param name="beamLoad">被检查的梁单元线荷载</param>
        /// <param name="err">检测误差</param>
        /// 
        private void SetBeamLoadEleNo(RTree rt, BeamLoadCls beamLoad,double err)
        {
            Point3d chkPt = beamLoad.LoadLine.PointAt(0.5);
            EventHandler<RTreeEventArgs> rTreeCallback = (object sender, RTreeEventArgs args) =>
                 {
                     beamLoad.EleNo=args.Id+1;
                 };
            rt.Search(new Sphere(chkPt, err), rTreeCallback);
        }
        /// <summary>
        /// 当被检测点与某个点很近时，记录该点的索引。
        /// </summary>
        /// <param name="pointsCloud"></param>
        /// <param name="chkPt"></param>
        /// <param name="err"></param>
        /// <param name="vertices"></param>
        private void RtreeSearch(RTree pointsCloud, Point3d chkPt, double err, List<int> vertices)
        {

            EventHandler<RTreeEventArgs> rTreeCallback =
                (object sender, RTreeEventArgs args) =>
                {
                    vertices.Add(args.Id);//点云中找到某个点跟chkPt比较近的时候，将该点的索引赋予vertices
                };
            pointsCloud.Search(new Sphere(chkPt, err), rTreeCallback);//在空间点云pointsCloud中搜索点chkPt,如果距离小于err，则调用rTreeCallback
        }
        /// <summary>
        /// 设置壳单元的顶点索引
        /// </summary>
        /// <param name="pointsCloud">模型中所有的节点，RTree存储</param>
        /// <param name="ShellElem">壳单元列表</param>
        /// <param name="err">检测误差</param>
        private void SetMeshVerInd(RTree pointsCloud, List<ShellElementCls> ShellElem, double err)
        {
            for (int i = 0; i < ShellElem.Count; i++)
            {
                List<int> verticesIndex = new List<int>();//用于存储壳单元顶点编号
                List<Point3d> ShellNodes = ShellElem[i].Nodes;//获得壳单元的顶点
                foreach (Point3d ptChk in ShellNodes)//对每个网格的每个面的每个顶点搜索顶点的索引，并将结果存入verticesIndex
                {
                    RtreeSearch(pointsCloud, ptChk, err, verticesIndex);
                }
                ShellElem[i].Nodes_no = verticesIndex;
            }
        }
        /// <summary>
        /// 设置梁单元的顶点索引
        /// </summary>
        /// <param name="pointsCloud"></param>
        /// <param name="lineobjs"></param>
        /// <param name="err"></param>
        private void SetLineVerInd(RTree pointsCloud, List<BeamElementCls> lineobjs, double err)
        {
            for (int i = 0; i < lineobjs.Count; i++)
            {
                List<int> verticesIndex = new List<int>();
                for (int j = 0; j < 2; j++)
                {
                    Point3d chkPt = lineobjs[i].Line.PointAt((double)j);
                    RtreeSearch(pointsCloud, chkPt, err, verticesIndex);
                }
                lineobjs[i].Nodes_no = verticesIndex;
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
            get { return new Guid("cc1b4071-f302-4c6c-995b-8d7cc9d44b7e"); }
        }
        public override GH_Exposure Exposure
        {
            get { return GH_Exposure.secondary; }
        }
    }
}