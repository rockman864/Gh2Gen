using System;
using System.Drawing;
using Grasshopper.Kernel;

namespace Gh2Gen
{
    public class Gh2GenInfo : GH_AssemblyInfo
    {
        public override string Name
        {
            get
            {
                return "Gh2Gen";
            }
        }
        public override Bitmap Icon
        {
            get
            {
                //Return a 24x24 pixel bitmap to represent this GHA library.
                return null;
            }
        }
        public override string Description
        {
            get
            {
                //Return a short string describing the purpose of this GHA library.
                return "";
            }
        }
        public override Guid Id
        {
            get
            {
                return new Guid("97fc73d9-616b-47c8-a313-5934624d3505");
            }
        }

        public override string AuthorName
        {
            get
            {
                //Return a string identifying you or your company.
                return "Microsoft";
            }
        }
        public override string AuthorContact
        {
            get
            {
                //Return a string representing your preferred contact details.
                return "";
            }
        }
    }
}
