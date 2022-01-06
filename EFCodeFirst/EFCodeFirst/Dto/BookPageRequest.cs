using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EFCodeFirst.Dto
{
    public class BookPageRequest: PageRequest
    {
        public string Name { get; set; }
    }
}
