using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Rhino.Geometry;
using Grasshopper.Kernel;

namespace Gh2Gen._00_BaseObj
{
/// <summary>
/// 三维模型类，具有节点属性、梁单元属性、壳单元属性、单元属性、组属性、梁荷载属性、壳压力荷载属性、支承属性
/// </summary>
    public class ModelCls
    {

        public List<Point3d> Nodes { get; set; }
        public List<BeamElementCls> BeamElements { get; set; }
        public List<ShellElementCls> ShellElements { get; set; }
        public List<BaseElementCls> Elements { get; set; }
        public List<Group> Groups { get; set; }
        public List<BeamLoadCls> BeamLoads { get; set; }
        public List<ShellLoadCls> ShellLoads { get; set; }
        public List<SupportCls> Supports { get; set; }



    }
}
