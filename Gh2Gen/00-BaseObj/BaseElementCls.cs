using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Gh2Gen._00_BaseObj
{
    /// <summary>
    /// 基单元类，具备以下属性：单元编号、单元节点编号、材料编号、属性编号、所属组名称
    /// </summary>
    public abstract class BaseElementCls
    {
        public int Ele_no { get; set; }
        public List<int> Nodes_no { get; set; }
        public int Mat { get; set; }
        public int Prop { get; set; }
        public List<string> Groupname { get; set; }
    }
}
