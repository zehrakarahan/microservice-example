using System;

namespace ClassLibrary1
{
    public class Department
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Owner { get; set; }
        public string SecretProperty { get; set; }
    }

    //目标
    public class DepartmentDTO
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Owner { get; set; }
    }

}
