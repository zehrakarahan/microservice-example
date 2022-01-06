using System;
using System.Collections.Generic;

namespace DataStructureApp
{
    class Program
    {
        static void Main(string[] args)
        {
            var testData = new int[] { 8, 6, 1, 5, 9 };
            PrintList(testData);
            TPrintList(testData);
            //QuickSort(testData,0, testData.Length-1);
            ShellSort(testData);
            Console.WriteLine("******");
            PrintList(testData);
            Console.Read();
        }

        public static void TPrintList(int[] list)
        {
            var k = 0;
            for (int i = k; i < list.Length; i++)
            {
                k=100;
                Console.Write(list[i] + ",");
            }
        }

        public static void PrintList(int[] list)
        {
            for (int i = 0; i < list.Length; i++)
            {
                Console.Write(list[i]+",");
            }
        }

        //插入排序
        private static void InsertSort(int[] arr)
        {
            //插入排序是把无序列的数一个一个插入到有序的数
            //先默认下标为0这个数已经是有序
            for (int i = 1; i < arr.Length; i++)
            {
                int insertVal = arr[i];  //首先记住这个预备要插入的数
                int insertIndex = i - 1; //找出它前一个数的下标（等下 准备插入的数 要跟这个数做比较）

                //如果这个条件满足，说明，我们还没有找到适当的位置
                while (insertIndex >= 0 && insertVal < arr[insertIndex])   //这里小于是升序，大于是降序
                {
                    arr[insertIndex + 1] = arr[insertIndex];   //同时把比插入数要大的数往后移
                    insertIndex--;      //指针继续往后移，等下插入的数也要跟这个指针指向的数做比较         
                }
                //插入(这时候给insertVal找到适当位置)
                arr[insertIndex + 1] = insertVal;
            }
        }

        //希尔排序
        public static void ShellSort(int[] array)
        {
            int gap = array.Length / 2;

            while (1 <= gap)
            {
                // 把距离为 gap 的元素编为一个组，扫描所有组
                for (int i = gap; i < array.Length; i++)
                {
                    int j = 0;
                    int temp = array[i];

                    // 对距离为 gap 的元素组进行排序
                    for (j = i - gap; j >= 0 && temp < array[j]; j = j - gap)
                    {
                        array[j + gap] = array[j];
                    }
                    array[j + gap] = temp;
                }

                Console.WriteLine("gap={0}", gap);
                foreach (int n in array)
                {
                    Console.Write("{0} ", n);
                }
                Console.WriteLine();

                gap = gap / 2; // 减小增量
            }
        }


        //选择排序
        public static void SelectSort(List<int> arr)
        {
            int temp = 0;
            for (int i = 0; i < arr.Count - 1; i++)
            {
                int minVal = arr[i]; //假设 i 下标就是最小的数
                int minIndex = i;  //记录我认为最小的数的下标

                for (int j = i + 1; j < arr.Count; j++)   //这里只是找出这一趟最小的数值并记录下它的下标
                {
                    //说明我们认为的最小值，不是最小
                    if (minVal > arr[j])    //这里大于号是升序(大于是找出最小值) 小于是降序(小于是找出最大值)
                    {
                        minVal = arr[j];  //更新这趟最小(或最大)的值 (上面要拿这个数来跟后面的数继续做比较)
                        minIndex = j;    //记下它的下标
                    }
                }
                //最后把最小的数与第一的位置交换
                temp = arr[i];    //把第一个原先认为是最小值的数,临时保存起来
                arr[i] = arr[minIndex];   //把最终我们找到的最小值赋给这一趟的比较的第一个位置
                arr[minIndex] = temp;  //把原先保存好临时数值放回这个数组的空地方，  保证数组的完整性
            }
            //控制台输出
            foreach (int item in arr)
            {
                Console.WriteLine("C#遍历：{0}", item);
            }
        }

        //快速排序
        private static void QuickSort(int[] arr, int begin, int end)
        {
            if (begin >= end) return;   //两个指针重合就返回，结束调用
            int pivotIndex = QuickSort_Once(arr, begin, end);  //会得到一个基准值下标

            QuickSort(arr, begin, pivotIndex - 1);  //对基准的左端进行排序  递归
            QuickSort(arr, pivotIndex + 1, end);   //对基准的右端进行排序  递归
        }

        private static int QuickSort_Once(int[] arr, int left, int right)
        {
            var pivot = left;                    // 设定基准值（pivot）
            var index = pivot + 1;

            for (var i = index; i <= right; i++)
            {
                if (arr[i] < arr[pivot])
                {
                    swap(arr, i, index);
                    index++;
                }
            }
            swap(arr, pivot, index - 1);
            return index - 1;
        }


        private static void swap(int[] arr, int i, int j)
        {
            var temp = arr[i];
            arr[i] = arr[j];
            arr[j] = temp;
        }



    }
}
