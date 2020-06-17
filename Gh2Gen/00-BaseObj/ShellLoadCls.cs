using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Gh2Gen._00_BaseObj
{
    /// <summary>
    /// 壳单元荷载对象
    /// 属性：荷载工况编号、单元编号、坐标系编号、方向、荷载大小
    /// </summary>
    public class ShellLoadCls
    {
        public int LoadCase { get; set; }
        public int EleNo { get; set; }
        public int Coordinate { get; set; }
        public int Direction { get; set; }
        public double Value { get; set; }

        public ShellLoadCls(int loadcase, int coord, int dir, double value)
        {
            LoadCase = loadcase;
            Coordinate = coord;
            Direction = dir;
            Value = value;
        }
    }
}
