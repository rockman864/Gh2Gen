using System;
using System.Collections.Generic;

using Grasshopper.Kernel;
using Rhino.Geometry;
using Gh2Gen._00_BaseObj;
using Grasshopper.Kernel.Types;
namespace Gh2Gen._01_Componentsh2Gen
{
    public class ShellElement : GH_Component
    {
        /// <summary>
        /// Initializes a new instance of the AreaObjComponent class.
        /// </summary>
        public ShellElement()
          : base("ShellEle", "SElE",
              "对mesh赋予物理属性建立壳单元",
              "MIDAS", "01 Elements")
        {
        }

        /// <summary>
        /// Registers all the input parameters for this component.
        /// </summary>
        protected override void RegisterInputParams(GH_Component.GH_InputParamManager pManager)
        {
            pManager.AddMeshParameter("List of meshs", "Mesh", "网格组", GH_ParamAccess.item);
            pManager.AddIntegerParameter("Material Number", "Material", "材料编号", GH_ParamAccess.item, (int)1);
            pManager.AddIntegerParameter("Thickness Number", "Thickness", "截面编号", GH_ParamAccess.item, (int)2);
            pManager.AddTextParameter("GroupNames", "GName", "所属组别", GH_ParamAccess.item);
            pManager[3].Optional = true;
        }

        /// <summary>
        /// Registers all the output parameters for this component.
        /// </summary>
        protected override void RegisterOutputParams(GH_Component.GH_OutputParamManager pManager)
        {
            pManager.AddGenericParameter( "ShellElements", "Shells", "具有物理属性的网格", GH_ParamAccess.item);
        }

        /// <summary>
        /// This is the method that actually does the work.
        /// </summary>
        /// <param name="DA">The DA object is used to retrieve from inputs and store in outputs.</param>
        protected override void SolveInstance(IGH_DataAccess DA)
        {
            Mesh meshi = new Mesh();
            int material = 0;
            int Thickness = 0;
            string gnames = null;
            List<string> gnames2 = null;

            DA.GetData(0, ref meshi);
            DA.GetData(1, ref material);
            DA.GetData(2, ref Thickness);
            bool bool_gname = DA.GetData(3, ref gnames);
            if (bool_gname)
            {
                string[] namesArray = gnames.Split(',');
                gnames2 = new List<string>(namesArray);
            }


            int meshfaces = meshi.Faces.Count;
            for (int j = 0; j < meshfaces; j++)
            {
                MeshFace fj = meshi.Faces[j];
                //List<int> faceVind = new List<int>();
                List<Point3d> faceNodes = new List<Point3d>();
                faceNodes.Add(meshi.Vertices[fj.A]);
                faceNodes.Add(meshi.Vertices[fj.B]);
                faceNodes.Add(meshi.Vertices[fj.C]);

                if (fj.IsQuad)
                {
                    faceNodes.Add(meshi.Vertices[fj.D]);
                }
                ShellElementCls shelli = new ShellElementCls(fj, material, Thickness);//对网格的每个面建立面单元对象
                shelli.Meshi = meshi;                                                                      //shelli.Nodes_no = faceVind;//对每个面单元对象赋予顶点索引
                shelli.Nodes = faceNodes;//对每个面单元对象赋予顶点
                shelli.Groupname = gnames2;//对每个面单元赋予组名
                DA.SetData(0, new GH_ShellElement(shelli));
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
            get { return new Guid("639ccb88-a5ff-486c-805c-24e7d1223c28"); }
        }
    }
}