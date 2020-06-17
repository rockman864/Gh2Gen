using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Rhino.Geometry;
using Grasshopper.Kernel;
using Grasshopper.Kernel.Types;

namespace Gh2Gen._00_BaseObj
{
    /// <summary>
    /// 梁单元线荷载类
    /// 属性：
    /// </summary>
    public class BeamLoadCls : ICloneable
    {
        private string coord;
        private string dir;
        private Point3d midpt;
        private Point3d midpt2;
        public string CoordDir { get; set; }
        public int LoadCase { get; set; }
        public int EleNo { get; set; }
        public int Coordinate { get; set; }
        public int Direction { get; set; }
        public double Value { get; set; }
        public Line LoadLine { get; set; }
        public bool Hidden { get; set; }

        public BeamLoadCls()
        {

        }
        public BeamLoadCls(int loadcase, Line loadline, int coord, int dir, double value)
        {
            LoadCase = loadcase;
            LoadLine = loadline;//用于荷载可视化定位
            Coordinate = coord;
            Direction = dir;
            Value = value;
            midpt = LoadLine.PointAt(0.5);//用于显示荷载箭头起点
            midpt2 = new Point3d(midpt.X, midpt.Y, midpt.Z + 500);//用于显示荷载箭头终点
            switch (coord)
            {
                case 1:
                    this.coord = "G";
                    break;
                case 2:
                    this.coord = "L";
                    break;
            }
            switch (dir)
            {
                case 1:
                    this.dir = "X";
                    break;
                case 2:
                    this.dir = "Y";
                    break;
                case 3:
                    this.dir = "Z";
                    break;
            }
            CoordDir = string.Concat(this.coord, this.dir);
        }
        public override string ToString()
        {
            return string.Concat("this is a beam load,value is :", Value);

        }

        public object Clone()
        {
            return this;
        }
        public void preview(IGH_PreviewArgs args)
        {
            Line l = new Line(midpt2, midpt);
            args.Display.DrawArrow(l, System.Drawing.Color.Red);
        }
    }
}
