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
    class FunctionClass
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
        public static List<Group> getGroupObjects0(List<BaseElementCls> EleObjList)
        {
            List<Group> GroupList = new List<Group>();
            for (int i = 0; i < EleObjList.Count; i++)
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
                        Group groupFind = GroupList.Find(b => b.Gname == gn);//找到组对象中哪个组的名字=gn，然后增加新的索引
                        groupFind.Ele_no.Add(EleObjList[i].Ele_no);
                        groupFind.Ele_no.Sort();
                    }
                }
            }
            return GroupList;
        }
        public static List<string> getDomain(List<int> numList)
        {
            List<int> flagindex = new List<int> { 0 };
            List<string> domainOutput = new List<string>();

            for (int i = 0; i < numList.Count - 1; i++)
            {
                if (numList[i + 1] - numList[i] == 1) continue;
                else flagindex.Add(i + 1);
            }
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
        public static List<string> mergeList(List<string> domainList, int bitsNum)
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
                temp += domainList[i] + " ";
                string temp2 = temp + domainList[i + 1];
                if (temp.Length < bitsNum && temp2.Length < bitsNum && i < domainList.Count - 2)
                {
                    continue;
                }
                else if (temp.Length <= bitsNum && temp2.Length >= bitsNum)
                {
                    resultStringList.Add(string.Concat(temp, @"\"));
                    temp = null;
                }
                else if (temp.Length < bitsNum && i == domainList.Count - 2)
                {
                    resultStringList.Add(temp2);
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

