using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Gh2Gen._00_BaseObj;

namespace Gh2Gen._02_UtilityFunction
{
    public class ExportAbaqus:ExportModel
    {
        public ExportAbaqus(ModelCls model,string path0):base(model,path0,".inp")
        {
            OutPutFile();
        }
        public void OutPutFile()
        {
            Sw.WriteLine("Hello this is abaqus inp ,under construction");
            Sw.Dispose();

        }
    }
}
