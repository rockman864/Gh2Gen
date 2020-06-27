using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Grasshopper.Kernel;
using Grasshopper.Kernel.Types;
using Rhino.Geometry;

namespace Gh2Gen._00_BaseObj
{
    /// <summary>
    /// 类似GH_Circle与Circle的关系，GH_Circle提供了其他方法：内化数据、预览数据、烘焙数据、转换数据
    /// </summary>
    public class GH_BeamLoadCls : GH_GeometricGoo<BeamLoadCls>
    {
        public GH_BeamLoadCls()
        {

        }
        public GH_BeamLoadCls(BeamLoadCls load):base(load)
        {
        }
        public override BoundingBox Boundingbox
        {
            get
            {
                return Boundingbox;
            }
        }
        public override bool CastTo<Q>(out Q target)
        {
            Type c = new BeamLoadCls().GetType();
            object obj2 = base.m_value;
            target = (Q)obj2;
            return typeof(Q).IsAssignableFrom(c);
        }
        /// <summary>
        /// 返回类型的名字
        /// </summary>
        public override string TypeName
        {
            get
            {
                return "GH_BeamLoad";
            }
        }
        /// <summary>
        /// 描述这个类是干啥的
        /// </summary>
        public override string TypeDescription
        {
            get
            {
                return "创建梁单元荷载对象：包含以下信息-工况，单元，坐标系，方向，大小";
            }
        }


        public override IGH_GeometricGoo DuplicateGeometry()
        {
            return null;
        }

        public override BoundingBox GetBoundingBox(Transform xform)
        {
            return this.Boundingbox;
        }

        public override IGH_GeometricGoo Morph(SpaceMorph xmorph)
        {
            return null;
        }
        /// <summary>
        /// 描述这个对象的状态（数据）
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {

            return string.Concat(string.Format("load value is:{0} KN/m", Value.Value));
        }

        public override IGH_GeometricGoo Transform(Transform xform)
        {
            return null;
        }
    }
}
