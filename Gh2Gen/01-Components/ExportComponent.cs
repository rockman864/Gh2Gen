using System;
using System.Collections.Generic;
using System.Diagnostics;

using Grasshopper.Kernel;
using Rhino.Geometry;
using Gh2Gen._00_BaseObj;
using Gh2Gen._02_UtilityFunction;
using System.Windows.Forms;

namespace Gh2Gen._01_Components
{
    public class ExportComponent : GH_Component
    {
        /// <summary>
        /// Initializes a new instance of the ExportModel class.
        /// </summary>
        public ExportComponent()
            : base("ExportModel", "ExportModel",
              "将模型数据导出为结构分析软件的模型文件",
              "Midas", "03 Export")
        {
        }

        /// <summary>
        /// Registers all the input parameters for this component.
        /// </summary>
        protected override void RegisterInputParams(GH_Component.GH_InputParamManager pManager)
        {
            pManager.AddGenericParameter("ModelData", "ModelData", "输入来自CreatModel的模型数据", GH_ParamAccess.item);
            pManager.AddTextParameter("PathOfFile", "path", "结构模型文件路径", GH_ParamAccess.item);
            pManager.AddIntegerParameter("TypeOfFile", "FileType", "1-midas MGT,2-Abaqus INP", GH_ParamAccess.item, (int)1);
            pManager.AddBooleanParameter("WriteCmd", "WriteAndOpen", "设为true则输出文件", GH_ParamAccess.item);

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
            ModelCls mymodel = new ModelCls();//新建模型对象，存储建立的模型
            string path0 = null;//路径
            int typeFile = 1;//文件类型，1为midas mgt，2为abaqus inp
            bool cmd = false;//文件输出指令，True为输出
            DA.GetData(3, ref cmd);
            if(DA.GetData(0,ref mymodel)&&DA.GetData(1,ref path0)&&DA.GetData(2,ref typeFile)&&cmd)
            {
                ExportModel myExport =null;
                switch (typeFile)
                {
                    case 1:
                        myExport = new ExportGen(mymodel,path0);
                        break;
                    case 2:
                        myExport= new ExportAbaqus(mymodel, path0);
                        break;
                }
                Process process = Process.Start(myExport.Pathfile);//打开输出的文件，便于直接复制
                process.Dispose();
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
            get { return new Guid("67ea6e68-1136-4589-bf50-0314ec00b93d"); }
        }
    }
}