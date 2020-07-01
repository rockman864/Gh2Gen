using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Rhino.Geometry;
using Grasshopper.Kernel;
using Gh2Gen._02_UtilityFunction;

namespace Gh2Gen._00_BaseObj
{
    /// <summary>
    /// 三维模型类，具有节点属性、梁单元属性、壳单元属性、单元属性、组属性、梁荷载属性、壳压力荷载属性、支承属性
    /// </summary>
    public class ModelCls
    {
        List<BeamElementCls> beams = new List<BeamElementCls>();
        List<ShellElementCls> shells = new List<ShellElementCls>();

        public List<Point3d> Nodes { get; set; }//节点            
        public List<BeamElementCls> BeamElements//梁单元 
        {
            get { return beams; }
            set
            { 
                beams = value;
                Elements.AddRange(beams);//添加梁单元列表时，往总单元列表属性里添加数据
            }
        }
        public List<ShellElementCls> ShellElements //壳单元
        {
            get { return shells; }
            set
            {
                shells = value;
                Elements.AddRange(shells);//添加壳单元列表时，往总单元列表属性里添加数据
            }
        }
        public List<BaseElementCls> Elements { get; } = new List<BaseElementCls>();
        public List<Group> Groups
        {
            get
            {
                return FunctionClass.ExtractGroups(Elements);
            }
        }

        public List<BeamLoadCls> BeamLoads { get; set; }//梁荷载
        public List<ShellLoadCls> ShellLoads { get; set; }//壳荷载
        public List<SupportCls> Supports { get; set; }//支座
        public List<String> ModelInfo//模型信息
        {
            get
            {
                List<String> modelInfo = new List<string>();
                string info1 = string.Format("this model has:{0} Nodes", Nodes.Count.ToString());
                string info2 = string.Format("this model has:{0} Elements", Elements.Count.ToString());
                string info3 = string.Format("     includes1:{0} Beams", BeamElements.Count.ToString());
                string info4 = string.Format("     includes1:{0} Shells", ShellElements.Count.ToString());
                modelInfo.Add(info1);
                modelInfo.Add(info2);
                modelInfo.Add(info3);
                modelInfo.Add(info4);
                return modelInfo;
            }

        }



    }
}
