using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Rhino.Display;
using Grasshopper.Kernel;
using Rhino.Geometry;
using System.Drawing;

namespace Gh2Gen._00_BaseObj
{
    public class ShellElementCls:BaseElementCls
    {
        //fields

        private List<Point3d> nodes;
        private MeshFace face;
        

        //method
        public ShellElementCls()
        { }
        public ShellElementCls(MeshFace face,int mat,int thick )
        {
            this.face = face;
            this.Mat = mat;
            this.Prop = thick;
        }
        public Mesh Meshi { get; set; }
        public List<Point3d> Nodes
        {
            get { return nodes; }
            set { nodes = value; }
        }
        public MeshFace Face
        {
            get { return this.face;}
        }
        public void preview(IGH_PreviewArgs args)
        {
            DisplayMaterial m = new DisplayMaterial(Color.Red, 0.8);
            args.Display.DrawMeshShaded(Meshi,m);
        }
    }
}