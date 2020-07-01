using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Grasshopper.Kernel;
using Rhino.Geometry;
using Rhino.Geometry.Collections;
using Gh2Gen;
using Gh2Gen._00_BaseObj;

namespace Gh2Gen._02_UtilityFunction
{
    public class FunctionClass
    {
        public static List<Point3d> deleteDuplicatPts(List<Point3d> pointsOrigin,double err)
        {
            List<Point3d> pointsAfter = new List<Point3d>();
            for(int i=0;i<pointsOrigin.Count;i++)
            {
                if(TestDistance(pointsAfter,pointsOrigin[i],err))
                {
                    pointsAfter.Add(pointsOrigin[i]);
                }
            }
            return pointsAfter;

        }
        public static bool TestDistance(List<Point3d> points,Point3d chkpt,double err)
        {
            bool bo = true;
            for(int i = 0;i<points.Count;i++)
            {
                if(Math.Abs(points[i].X-chkpt.X)<err&& Math.Abs((points[i].Y - chkpt.Y)) < err&& Math.Abs((points[i].Z - chkpt.Z)) < err)
                {
                    bo = false;
                    break;
                }
            }
            return bo;
        }
        public static void BigThanErr(List<Point3d> points, Point3d chkPt, double err, out bool bo, out int ind)
        {//返回重合点的索引号
            bo = true;
            ind = 0;
            for (int i = 0; i < points.Count; i++)
            {
                if (chkPt.DistanceTo(points[i]) < err)//效率没有Math.Abs(x1-x2)<err&&Math.Abs(y1-y2)<err&&Math.Abs(z1-z2)<err高
                {
                    bo = false;
                    ind = i;
                }
            }
        }
        /// <summary>
        /// 提取所有梁单元顶点列表
        /// </summary>
        /// <param name="LineObj"></param>
        /// <returns></returns>
        public static List<Point3d> getPoints(List<BeamElementCls> LineObj)
        {
            int numL = LineObj.Count;
            List<Point3d> pts = new List<Point3d>();
            for (int i = 0; i < numL; i++)
            {
                pts.Add(LineObj[i].Line.PointAt(0));
                pts.Add(LineObj[i].Line.PointAt(1));
            }
            return pts;
        }
        /// <summary>
        /// 提取所有壳单元的顶点列表
        /// </summary>
        /// <param name="AreaObj">壳单元列表</param>
        /// <returns></returns>
        public static List<Point3d> getPoints(List<ShellElementCls> AreaObj)
        {
            int numM = AreaObj.Count;
            List<Point3d> pts = new List<Point3d>();
            for (int i = 0; i < numM; i++)//提取每个网格的面的顶点
            {
                pts.AddRange(AreaObj[i].Nodes);

            }
            return pts;
        }
        /// <summary>
        /// 从模型所有单元信息中提取组对象列表，每个组有组名属性和单元列表属性
        /// </summary>
        /// <param name="EleObjList"></param>
        /// <returns></returns>
        public static List<Group> ExtractGroups(List<BaseElementCls> EleObjList)
        {
            List<Group> GroupList = new List<Group>();
            for (int i = 0; i < EleObjList.Count; i++)//遍历每个单元（包括梁单元、壳单元）
            {
                int gnum = EleObjList[i].Groupname.Count;
                for (int j = 0; j < gnum; j++)
                {
                    string gn = EleObjList[i].Groupname[j];
                    if (GroupList.Find(a => a.Gname == gn) == null)//组对象列表中没有组的名字=gn，可以新建组对象
                    {
                        List<int> nodesij = new List<int> { EleObjList[i].Ele_no };
                        GroupList.Add(new Group(gn, nodesij));
                    }
                    else
                    {
                        Group groupFind = GroupList.Find(b => b.Gname == gn);//找到组对象中哪个组的名字=gn，然后给该组增加新的单元索引
                        groupFind.Ele_no.Add(EleObjList[i].Ele_no);
                        groupFind.Ele_no.Sort();
                    }
                }
            }
            return GroupList;
        }
        /// <summary>
        /// 输入一整数列表，返回列表中的数字范围，比如输入{1，2，3，7，8，9，11，12，13，15，17，19}，返回{1to3,7to9,11to13，15，17，19}
        /// </summary>
        /// <param name="numList">输入的整数列表</param>
        /// <returns>数字范围列表</returns>
        public static List<string> GetDomain(List<int> numList)
        {
            List<int> flagindex = new List<int> { 0 };
            List<string> domainOutput = new List<string>();
            //在间断的地方打标记，得到标记列表{0,3,6,9,10,11}
            for (int i = 0; i < numList.Count - 1; i++)
            {
                if (numList[i + 1] - numList[i] == 1) continue;
                else flagindex.Add(i + 1);
            }
            //根据标记列表获得数字范围起始点
            for (int j = 0; j < flagindex.Count; j++)
            {
                int domainS = numList[flagindex[j]];
                int domainE;
                if (j < flagindex.Count - 1)
                {
                    domainE = numList[flagindex[j + 1] - 1];
                }
                else { domainE = numList[numList.Count - 1]; }
                domainOutput.Add(domainS.ToString() + "to" + domainE.ToString());
            }
            return domainOutput;
        }
        /// <summary>
        /// 将单元编号范围列表domainList按照指定长度截断成多行，一行最长lengthTol。比如{"1to10","10to100","5","1","4","1000000to2000000","100","200"}=>{"1to10","10to100 5","1 4","1000000to2000000","100 200"}
        /// </summary>
        /// <param name="domainList">单元编号范围列表</param>
        /// <param name="lengthTol">字符串允许长度</param>
        /// <returns></returns>
        public static List<string> mergeList(List<string> domainList, int lengthTol)
        {
            //将单元范围列表按照一定长度合并，然后输出为要打印的信息
            //
            List<int> length_index = new List<int>();
            List<string> resultStringList = new List<string>();
            string temp = null;
            if (domainList.Count == 1)
            {
                resultStringList = domainList;
            }
            for (int i = 0; i < domainList.Count - 1; i++)
            {
                temp += domainList[i] + " ";//截止指针i处的字符串1
                string temp2 = temp + domainList[i + 1];//截止指针i+1处的字符串2
                if (temp2.Length < lengthTol && i < domainList.Count - 2)//字符串2长度小于允许长度,且指针i没到倒数第二个，继续给字符串1添加数据
                {
                    continue;
                }
                else if (temp2.Length < lengthTol && i == domainList.Count - 2)//字符串2长度小于允许长度,且指针i为倒数第二个,那么temp2字符串已经到头了，存储字符串2
                {
                    resultStringList.Add(temp2);
                }
                else if (temp2.Length >= lengthTol && i < domainList.Count - 2)//字符串2长度大于等于允许长度,且指针i没到倒数第二个，存储字符串1,然后清空变量，重新记录
                {
                    resultStringList.Add(string.Concat(temp, @"\"));
                    temp = null;
                }
                else if (temp2.Length >= lengthTol && i == domainList.Count - 2)//字符串2长度大于等于允许长度,且指针i为倒数第二个，存储字符串1,然后存储倒数第一个数据
                {
                    resultStringList.Add(string.Concat(temp, @"\"));
                    resultStringList.Add(domainList[i + 1]);
                }

            }
            return resultStringList;
        }
        public static void RtreeSearch(RTree pointsCloud, List<int> PointsIndex, Point3d chkPt, double err, List<int> vertices)
        {

            EventHandler<RTreeEventArgs> rTreeCallback =
                (object sender, RTreeEventArgs args) =>
                {
                    vertices.Add(PointsIndex[args.Id]);//点云中找到某个点跟chkPt比较近的时候，将该点的索引赋予vertices
                };
            pointsCloud.Search(new Sphere(chkPt, err), rTreeCallback);//在空间点云pointsCloud中搜索点chkPt,如果距离小于err，则调用rTreeCallback
        }
    }
}

