using Rhino.Geometry;
using System.Collections.Generic;
using Grasshopper.Kernel;

namespace Gh2Gen._00_BaseObj
{
    public class SupportCls
    {
        public List<Point3d> Nodes { get; set; }
        public string ConsDOF { get; set; }
    }
}