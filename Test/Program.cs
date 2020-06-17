using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Test
{
    public class PermutationAndCombination<T>
    {
        /// <summary>
        /// 交换两个变量
        /// </summary>
        /// <param name="a">变量1</param>
        /// <param name="b">变量2</param>
        public static void Swap(ref T a, ref T b)
        {
            T temp = a;
            a = b;
            b = temp;
        }

        /// <summary>
        /// 递归算法求数组的组合,长度为n的t数组里取m个元素组合(私有成员)
        /// </summary>
        /// <param name="list">返回的范型</param>
        /// <param name="t">所求数组</param>
        /// <param name="n">辅助变量,t的长度</param>
        /// <param name="m">辅助变量,组合的个数</param>
        /// <param name="b">辅助数组,空数组，长度为m</param>
        /// <param name="M">辅助变量，所求组合的个数，不变量</param>
        private static void GetCombination(ref List<T[]> list, T[] t, int n, int m, int[] b, int M)
        {
            for (int i = n; i >= m; i--)
            {
                b[m - 1] = i - 1;//存放所求组合的每个元素的索引
                if (m > 1)
                {
                    GetCombination(ref list, t, i - 1, m - 1, b, M);
                }
                else
                {
                    if (list == null)
                    {
                        list = new List<T[]>();
                    }
                    T[] temp = new T[M];//每个组合的数组
                    for (int j = 0; j < b.Length; j++)
                    {
                        temp[j] = t[b[j]];
                    }
                    list.Add(temp);
                }
            }
        }
        private static List<T[]> GetCombination2(T[] t, int m)
        {
            List<T[]> res = new List<T[]>();
            T[] temp = new T[m];

            for (int i = 0; i < t.Length - 1; i++)
            {
                if (m == 1)
                {
                    temp[0] = t[i];
                }
                else
                {
                    T[] t2 = new T[t.Length - 1];
                    Array.Copy(t, i + 1, t2, 0, t.Length - 1);
                    GetCombination2(t2, m - 1);
                }

            }

        }
        /// <summary>
        /// 递归算法求排列(私有成员)
        /// </summary>
        /// <param name="list">返回的列表</param>
        /// <param name="t">所求数组</param>
        /// <param name="startIndex">起始标号</param>
        /// <param name="endIndex">结束标号</param>
        private static void GetPermutation(ref List<T[]> list, T[] t, int startIndex, int endIndex)
        {
            if (startIndex == endIndex)
            {
                if (list == null)
                {
                    list = new List<T[]>();
                }
                T[] temp = new T[t.Length];
                t.CopyTo(temp, 0);
                list.Add(temp);
            }
            else
            {
                for (int i = startIndex; i <= endIndex; i++)
                {
                    Swap(ref t[startIndex], ref t[i]);
                    GetPermutation(ref list, t, startIndex + 1, endIndex);
                    Swap(ref t[startIndex], ref t[i]);
                }
            }
        }

        /// <summary>
        /// 求从起始标号到结束标号的排列，其余元素不变
        /// </summary>
        /// <param name="t">所求数组</param>
        /// <param name="startIndex">起始标号</param>
        /// <param name="endIndex">结束标号</param>
        /// <returns>从起始标号到结束标号排列的范型</returns>
        public static List<T[]> GetPermutation(T[] t, int startIndex, int endIndex)
        {
            if (startIndex < 0 || endIndex > t.Length - 1)
            {
                return null;
            }
            List<T[]> list = new List<T[]>();
            GetPermutation(ref list, t, startIndex, endIndex);
            return list;
        }

        /// <summary>
        /// 返回数组所有元素的全排列
        /// </summary>
        /// <param name="t">所求数组</param>
        /// <returns>全排列的范型</returns>
        public static List<T[]> GetPermutation(T[] t)
        {
            return GetPermutation(t, 0, t.Length - 1);
        }

        /// <summary>
        /// 求数组中n个元素的排列
        /// </summary>
        /// <param name="t">所求数组</param>
        /// <param name="n">元素个数</param>
        /// <returns>数组中n个元素的排列</returns>
        public static List<T[]> GetPermutation(T[] t, int n)
        {
            if (n > t.Length)
            {
                return null;
            }
            List<T[]> list = new List<T[]>();
            List<T[]> c = GetCombination(t, n);
            for (int i = 0; i < c.Count; i++)
            {
                List<T[]> l = new List<T[]>();
                GetPermutation(ref l, c[i], 0, n - 1);
                list.AddRange(l);
            }
            return list;
        }


        /// <summary>
        /// 求数组中n个元素的组合
        /// </summary>
        /// <param name="t">所求数组</param>
        /// <param name="n">元素个数</param>
        /// <returns>数组中n个元素的组合的范型</returns>
        public static List<T[]> GetCombination(T[] t, int n)
        {
            if (t.Length < n)
            {
                return null;
            }
            int[] temp = new int[n];
            List<T[]> list = new List<T[]>();
            GetCombination(ref list, t, t.Length, n, temp, n);
            return list;
        }
    }

    enum LoadTypes { D, L, W, T };
    class baseType
    {
        public LoadTypes Type { get; set; }

        public baseType(LoadTypes type)
        {
            this.Type = type;
        }
    }
    /// <summary>
    /// 可变荷载工况，属性：类型、分项系数、组合系数
    /// </summary>
    class VariableLoadType : baseType
    {
        public double Fxxs { get; set; }
        public double Zhxs { get; set; }

        public VariableLoadType(LoadTypes type, double fxxs, double zhxs) : base(type)
        {
            this.Type = type;
            this.Fxxs = fxxs;
            this.Zhxs = zhxs;
        }
    }
    /// <summary>
    /// 恒载工况，属性：类型、分项系数列表（1.3,1.0）
    /// </summary>
    class ConstantLoadType : baseType
    {
        public List<double> Fxxs { get; set; }
        public ConstantLoadType(LoadTypes type, List<double> fxxs) : base(type)
        {
            this.Type = type;
            this.Fxxs = fxxs;
        }
    }
    class LoadCase
    {
        public baseType Type { get; set; }
        public string Name { get; set; }
        public string Descr { get; set; }
        public LoadCase(baseType type, string name, string descr)
        {
            this.Type = type;
            this.Name = name;
            this.Descr = descr;

        }
    }
    class Program
    {
        static void lambdaTest()
        {
            //1.实名函数Func委托
            int add(int x, int y)
            {
                return x + y;
            }
            Func<int, int, int> fc = add;
            //2.匿名函数Func委托，lambda应用
            Func<int, int, int> fc2 = (x, y) => x + y;
            //3.验证等效性
            Console.WriteLine(fc(2, 3));
            Console.WriteLine(fc2(2, 3));
            //***************************************
            //1.显示实名函数Action委托
            List<int> oList = new List<int> { 2, 3, 4 };
            List<int> nList = new List<int>();
            List<int> nList2 = new List<int>();

            Action<int> act = actFun;
            void actFun(int x)
            {
                nList.Add(x * 3);
            }
            oList.ForEach(act);
            //2.隐式匿名函数Action委托
            oList.ForEach(x => nList2.Add(x * 4));
            Console.WriteLine(String.Join(",", nList));
            Console.WriteLine(String.Join(",", nList2));

            Console.ReadLine();

            Console.ReadLine();
        }
        static void GetCombinationTest()
        {
            int[] arr = new int[6];
            for (int i = 0; i < arr.Length; i++)
            {
                arr[i] = i + 1;
            }
            //求排列
            List<int[]> lst_Permutation = PermutationAndCombination<int>.GetPermutation(arr, 3);
            //求组合
            List<int[]> lst_Combination = PermutationAndCombination<int>.GetCombination(arr, 3);
            foreach (int[] item in lst_Permutation)
            {
                Console.WriteLine(String.Join(",", item));
            }
            Console.WriteLine("一共{0}种排列", lst_Permutation.Count());

            Action<int[]> act = actFun;
            void actFun(int[] ar)
            {
                Console.WriteLine(String.Join(",", ar));
            }
            lst_Combination.ForEach(x => actFun(x));
            Console.WriteLine("一共{0}种组合", lst_Combination.Count());

            Console.ReadLine();
        }
        static void LoadCombinationTest()
        {

            List<double> factors = new List<double> { 1.3, 1.0 };
            ConstantLoadType typeD = new ConstantLoadType(LoadTypes.D, factors);
            VariableLoadType typeL = new VariableLoadType(LoadTypes.L, 1.5, 0.7);
            VariableLoadType typeW = new VariableLoadType(LoadTypes.W, 1.5, 0.6);
            VariableLoadType typeT = new VariableLoadType(LoadTypes.T, 1.5, 0.6);
            VariableLoadType[] loadTypesL = { typeL, typeW, typeT };//可变荷载种类


            LoadCase CaseD = new LoadCase(typeD, "DeadLoad", "这是恒载");
            LoadCase CaseL = new LoadCase(typeL, "LiveLoad", "满跨活荷载");
            LoadCase CaseLl = new LoadCase(typeL, "leftLiveLoad", "左跨活荷载");
            LoadCase CaseLr = new LoadCase(typeL, "rightLiveLoad", "右跨活荷载");
            LoadCase CaseWx1 = new LoadCase(typeW, "Wx1", "x正向风荷载");
            LoadCase CaseWx2 = new LoadCase(typeW, "Wx2", "x负向风荷载");
            LoadCase CaseWy1 = new LoadCase(typeW, "Wy1", "y正向风荷载");
            LoadCase CaseWy2 = new LoadCase(typeW, "Wy2", "y负向风荷载");
            LoadCase CaseT1 = new LoadCase(typeT, "T1", "升温");
            LoadCase CaseT2 = new LoadCase(typeT, "T2", "降温");

            LoadCase[] cases = { CaseL, CaseLl, CaseLr, CaseWx1, CaseWx2, CaseWy1, CaseWy2 };//荷载单工况种类
            List<VariableLoadType[]> combinations = PermutationAndCombination<VariableLoadType>.GetCombination(loadTypesL, 1);
            int num = 0;
            foreach (VariableLoadType[] item in combinations)
            {
                Console.WriteLine("num = {0} ", num.ToString());
                num++;
                List<string> combnames = new List<string>();
                foreach (VariableLoadType casei in item)
                {
                    combnames.Add(casei.Type.ToString());
                }
                combnames.ForEach(x => Console.WriteLine(String.Join(",", x)));
            }
            Console.ReadLine();




        }
        static void Main(string[] args)
        {
            //lambdaTest();

            GetCombinationTest();
            //LoadCombinationTest();

        }
    }
}
