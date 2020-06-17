using System;
using System.Collections.Generic;
using System.IO;
using Grasshopper.Kernel;
using Rhino.Geometry;
using System.Collections;
using Gh2Gen._02_UtilityFunction;
using Gh2Gen._00_BaseObj;

namespace Gh2Gen._02_UtilityFunction
{
    public abstract class PrepareModel//准备好模型数据、路径和文件流
    {
        //private StreamWriter sw;
        public ModelCls Model { get; set; }
        public string Pathfile { get; set; }
        public StreamWriter Sw { get; set; }

        public PrepareModel(ModelCls model, string path0,string type)
        {
            Model = model;
            string path = string.Concat(path0, type);
            Pathfile = Path.Combine(@"", path);
            Sw = new StreamWriter(new FileStream(Pathfile, FileMode.Create, FileAccess.Write));
        }
    }
    interface Ioutput
    {
        string Pathfile { get; set; }
        void OutPutFile();
    }
    public class ExportGen:PrepareModel,Ioutput
    {
        public ExportGen(ModelCls model,string path0):base(model,path0,".mgt")
        {
            OutPutFile();
        }
        public void OutPutFile()
        {
            ExptNodesBeams();
            if (Model.ShellElements is null) { }
            else { ExptShells(); }
            if (Model.Groups is null) { }
            else { ExptGroups(); }
            if (Model.BeamLoads is null) { }
            else { ExptBLoad(); }
            ExptEnd();

        }
        void ExptBLoad()
        {
            Sw.WriteLine("*STLDCASE");
            Sw.WriteLine("    DeadLoad,D,");
            Sw.WriteLine("    LiveLoad,L,");
            Sw.WriteLine("*USE-STLD,DeadLoad");
            Sw.WriteLine("*SELFWEIGHT");
            Sw.WriteLine("0,0,-1");
            Sw.WriteLine("*BEAMLOAD");
            for(int i=0;i<Model.BeamLoads.Count;i++)
            {
                BeamLoadCls loadi = Model.BeamLoads[i];
                Sw.WriteLine("   {0},Beam,UNILOAD,{1},NO,NO,aDir[1], , , , 0,{2},1,{2},0,0,0,0,,NO,0,0,NO,", loadi.EleNo, loadi.CoordDir, loadi.Value);
            }


        }
        void ExptNodesBeams()
        {
            //输出节点信息
            Sw.WriteLine("*UNIT");
            Sw.WriteLine("KN,M,KJ,C");
            Sw.WriteLine("*NODE");
            for (int i = 0; i < Model.Nodes.Count; i++)
            {
                Sw.WriteLine("    {0},{1},{2},{3}", i + 1, Model.Nodes[i].X, Model.Nodes[i].Y, Model.Nodes[i].Z);
            }
            //输出梁单元信息
            Sw.WriteLine("*ELEMENT");
            for (int j = 0; j < Model.BeamElements.Count; j++)
            {
                BeamElementCls beami = Model.BeamElements[j];
                List<int> beamiNodesInd = beami.Nodes_no;
                if (beamiNodesInd.Count == 2)
                {
                    Sw.WriteLine("    {0},BEAM,{1},{2},{3},{4},{5},{6}", beami.Ele_no, beami.Mat, beami.Prop, beamiNodesInd[0] + 1, beamiNodesInd[1] + 1, beami.Beta, 0);
                }
            }
        }
        //输出壳元命令流
        void ExptShells()
        {
            for (int k = 0; k < Model.ShellElements.Count; k++)
            {
                ShellElementCls shellk = Model.ShellElements[k];
                List<int> shellK_nodesInd = shellk.Nodes_no;
                if (shellK_nodesInd.Count == 3)
                {
                    Sw.WriteLine("    {0},PLATE,{1},{2},{3},{4},{5},0,3,0", shellk.Ele_no, shellk.Mat, shellk.Prop, shellK_nodesInd[0] + 1, shellK_nodesInd[1] + 1, shellK_nodesInd[2] + 1);
                }
                if (shellK_nodesInd.Count == 4)
                {
                    Sw.WriteLine("    {0},PLATE,{1},{2},{3},{4},{5},{6},3,0", shellk.Ele_no, shellk.Mat, shellk.Prop, shellK_nodesInd[0] + 1, shellK_nodesInd[1] + 1, shellK_nodesInd[2] + 1, shellK_nodesInd[3] + 1);
                }

            }
        }
        //输出组信息命令流
        void ExptGroups()
        {
            Sw.WriteLine("*GROUP");
            for (int i = 0; i < Model.Groups.Count; i++)
            {
                string groupi_name = Model.Groups[i].Gname;
                List<int> groupi_no = Model.Groups[i].Ele_no;
                groupi_no.Sort();
                List<string> groupi_DomList = FunctionClass.getDomain(groupi_no);
                List<string> groupi_DomList2 = FunctionClass.mergeList(groupi_DomList, 60);//单元列表改为每一行60个字符
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
                    Sw.WriteLine(cmd);
                }

            }

        }
        void ExptEnd()
        {
            Sw.WriteLine("*ENDDATA");
            Sw.Dispose();
        }

        //输出组信息命令流

    }

}
