using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace hangfireApp
{


    public class Appsetting
    {
        public RedisConfig RedisConfig { get; set; }

        public List<string> TaskQueues { get; set; }
    }

    public class RedisConfig
    {
        public string HostName { get; set; }
        public int Port { get; set; }
        public string Password { get; set; }
        public int Defaultdatabase { get; set; }
    }

   
    
  
}
