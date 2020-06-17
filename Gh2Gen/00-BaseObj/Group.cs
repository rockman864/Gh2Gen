using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Gh2Gen._00_BaseObj
{
    public class Group
    {
        public string Gname { get; set; }
        public List<int> Ele_no { get; set; }
        public Group(string gname, List<int> ele_no)
        {
            Gname = gname;
            Ele_no = ele_no;
        }
    }
}
