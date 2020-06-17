using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Grasshopper.Kernel;
using Rhino.Geometry;
using Rhino;
namespace Gh2Gen._00_BaseObj
{
    /// <summary>
    /// 梁单元类，扩展基单元类
    /// 扩展属性：线段
    /// </summary>
    public class BeamElementCls:BaseElementCls
    {
        //fields
        private Line _line;
        private List<Point3d> nodes;
        private double beta;
 

        //methods
        public BeamElementCls()
        {

        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="line"></param>
        /// <param name="material"></param>
        /// <param name="section"></param>
        public BeamElementCls(Line line, int material, int section)
        {
            this._line = line;
            this.Mat = material;
            this.Prop = section;
        }

        public Line Line
        {
            set { this._line = value; }
            get {return this._line;}
        }
        public List<Point3d> Nodes
        {
            get { return Nodes; }
            set { nodes = value; }
        }
        public double Beta
        {
            get { return beta; }
            set { beta = value; }
        }
        public void preview(IGH_PreviewArgs args)
        {
            args.Display.DrawLine(Line, System.Drawing.Color.Blue, 2);
        }
    }
}
