using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GenericCloudant.Models
{
    public class Student : CloudantModel
    {
        public Student(string name, string @class)
        {
            Name = name;
            Class = @class;
            ClassName = GetType().Name;
        }

        public string Name { get; set; }
        public string Class { get; set; }

        public Student()
        {
                
        }
    }
}
