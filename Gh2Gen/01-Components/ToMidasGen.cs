using System;
using System.Collections.Generic;
using System.IO;
using Grasshopper.Kernel;
using Rhino.Geometry;
using System.Collections;
using Gh2Gen._02_UtilityFunction;
using Gh2Gen._00_BaseObj;

namespace Gh2Gen._01_Components
{
    public class ToMidasGen : GH_Component
    {
        /// <summary>
        /// Initializes a new instance of the MGTComponent1 class.
        /// </summary>
        public ToMidasGen()
          : base("OutputToGen", "ToGen",
              "导出模型至mgt文件",
              "Midas", "03 Export")
        {
        }

        /// <summary>
        /// Registers all the input parameters for this component.
        /// </summary>
        protected override void RegisterInputParams(GH_Component.GH_InputParamManager pManager)
        {
            pManager.AddGenericParameter("Nodes", "Nodes", "模型节点", GH_ParamAccess.list);
            pManager.AddGenericParameter("BeamEle", "Beams", "梁单元", GH_ParamAccess.list);
            pManager.AddGenericParameter("ShellEle", "Shells", "壳单元", GH_ParamAccess.list);
            pManager[2].Optional = true;
            pManager.AddTextParameter("PathOfFile", "path", "mgt文件路径", GH_ParamAccess.item);
            pManager.AddBooleanParameter("StartWrite", "write", "设置为true输出mgt文件", GH_ParamAccess.item);

        }

        /// <summary>
        /// Registers all the output parameters for this component.
        /// </summary>
        protected override void RegisterOutputParams(GH_Component.GH_OutputParamManager pManager)
        {

        }

        /// <summary>
        /// This is the method that actually does the work.
        /// </summary>
        /// <param name="DA">The DA object is used to retrieve from inputs and store in outputs.</param>
        protected override void SolveInstance(IGH_DataAccess DA)
        {
            List<Point3d> nodes = new List<Point3d>();
            List<BeamElementCls> beamObjs = new List<BeamElementCls>();
            List<ShellElementCls> shellObjs = new List<ShellElementCls>();
            List<BaseElementCls> eleObjs = new List<BaseElementCls>();


            string path0 = "";
            bool writeCmd = false;

            //输入单数赋予局部变量；
            bool b1 = DA.GetDataList(0, nodes);
            bool b2 = DA.GetDataList(1, beamObjs);
            bool b3 = DA.GetDataList(2, shellObjs);
            bool b4 = DA.GetData(3, ref path0);
            bool b5 = DA.GetData(4, ref writeCmd);
            //将各种单元类型对象转化为单元基类，便于调用同一种提取组对象的方法；
            foreach (BeamElementCls beami in beamObjs)
            {
                BaseElementCls beamEleobji = beami;
                eleObjs.Add(beamEleobji);
            }
            foreach (ShellElementCls shelli in shellObjs)
            {
                BaseElementCls shellEleobji = shelli;
                eleObjs.Add(shellEleobji);
            }
            //从单元对象获得组信息，并创建组对象
            List<Group> GroupObj = FunctionClass.getGroupObjects0(eleObjs);

            if (b1 && b2 && b4 && b5)
            {
                string pathfile = Path.Combine(@"", path0);

                //输出节点命令流
                FileStream file = new FileStream(pathfile, FileMode.Create, FileAccess.Write);
                StreamWriter sw = new StreamWriter(file);
                sw.WriteLine("*UNIT");
                sw.WriteLine("KN,M,KJ,C");
                sw.WriteLine("*NODE");
                for (int i = 0; i < nodes.Count; i++)
                {
                    sw.WriteLine("    {0},{1},{2},{3}", i + 1, nodes[i].X, nodes[i].Y, nodes[i].Z);
                }
                sw.Dispose();

                //输出梁单元命令流
                FileStream file2 = new FileStream(pathfile, FileMode.Append, FileAccess.Write);
                StreamWriter sw2 = new StreamWriter(file2);
                sw2.WriteLine("*ELEMENT");
                for (int j = 0; j < beamObjs.Count; j++)
                {
                    BeamElementCls beami = beamObjs[j];
                    List<int> beamiNodesInd = beami.Nodes_no;
                    if (beamiNodesInd.Count == 2)
                    {
                        sw2.WriteLine("    {0},BEAM,{1},{2},{3},{4},{5},{6}", beami.Ele_no, beami.Mat, beami.Prop, beamiNodesInd[0] + 1, beamiNodesInd[1] + 1, beami.Beta, 0);
                    }
                }
                sw2.Dispose();

                //输出壳元命令流
                if (b3)
                {
                    FileStream file3 = new FileStream(pathfile, FileMode.Append, FileAccess.Write);
                    StreamWriter sw3 = new StreamWriter(file3);
                    for (int k = 0; k < shellObjs.Count; k++)
                    {
                        ShellElementCls shellk = shellObjs[k];
                        List<int> shellK_nodesInd = shellk.Nodes_no;
                        if (shellK_nodesInd.Count == 3)
                        {
                            sw3.WriteLine("    {0},PLATE,{1},{2},{3},{4},{5},0,3,0", shellk.Ele_no, shellk.Mat, shellk.Prop, shellK_nodesInd[0] + 1, shellK_nodesInd[1] + 1, shellK_nodesInd[2] + 1);
                        }
                        if (shellK_nodesInd.Count == 4)
                        {
                            sw3.WriteLine("    {0},PLATE,{1},{2},{3},{4},{5},{6},3,0", shellk.Ele_no, shellk.Mat, shellk.Prop, shellK_nodesInd[0] + 1, shellK_nodesInd[1] + 1, shellK_nodesInd[2] + 1, shellK_nodesInd[3] + 1);
                        }

                    }
                    sw3.Dispose();
                }
                //输出组信息命令流
                FileStream file4 = new FileStream(pathfile, FileMode.Append, FileAccess.Write);
                StreamWriter sw4 = new StreamWriter(file4);
                sw4.WriteLine("*GROUP");
                for (int i = 0; i < GroupObj.Count; i++)
                {
                    string groupi_name = GroupObj[i].Gname;
                    List<int> groupi_no = GroupObj[i].Ele_no;
                    groupi_no.Sort();
                    List<string> groupi_DomList = FunctionClass.getDomain(groupi_no);
                    List<string> groupi_DomList2 = FunctionClass.mergeList(groupi_DomList, 60);
                    string cmd = null;
                    for (int j = 0; j < groupi_DomList2.Count; j++)
                    {
                        if (j == 0)
                        {
                            cmd = groupi_name + "," + " " + "," + groupi_DomList2[0];
                        }
                        else
                        {
                            cmd = groupi_DomList2[j];
                        }
                        sw4.WriteLine(cmd);
                    }

                }
                sw4.Dispose();


            }
            //int numGB = GroupBeam.Count;
            //int numGS = GroupShell.Count;
            //DA.SetData(0, numGB);
            //DA.SetData(1, numGS);
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
            get { return new Guid("eea6d422-4f5b-4422-88b2-fb4dda0e93a3"); }
        }
    }
}