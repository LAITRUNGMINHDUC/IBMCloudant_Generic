using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GenericCloudant.Models
{
    public class CloudantModel
    {
        public string id { get; set; }
        public string rev { get; set; }
        public string ClassName { get; set; }
    }
}
