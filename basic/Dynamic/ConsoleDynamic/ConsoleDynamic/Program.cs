using Newtonsoft.Json;
using System;

namespace ConsoleDynamic
{
    class Program
    {
        static void Main(string[] args)
        {
            var info = "{\"Account\":\"张三\",\"Pwd\":\"12323\"}";  //json字符串

            var infos = JsonConvert.DeserializeObject<dynamic>(info);   //反序列化为 dynamic 对象

            var AccountOne = infos.Account.Value;   // 结果为  "张三"

            var AccountTwo = infos["Account"].Value;  // 结果为  "张三"






            string jsonstr = "{ \"listData\": [{\"Account\":\"张三\",\"Pwd\":\"12323\"},{\"Account\":\"李四\",\"Pwd\":\"9999\"}]  }";

            dynamic infosstr = JsonConvert.DeserializeObject(jsonstr);

            var listobj = infosstr.listData;

            for (int i = 0; i < listobj.Count; i++)
            {
                string Accountv = listobj[i].Account.Value;   //获取到了 Account 的值！
            }


            Console.WriteLine("Hello World!");
        }
    }
}
