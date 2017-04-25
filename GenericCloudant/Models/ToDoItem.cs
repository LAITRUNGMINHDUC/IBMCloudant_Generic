using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GenericCloudant.Models
{
    public class ToDoItem : CloudantModel
    {
        public ToDoItem()
        {

        }

        public ToDoItem(string _text)
        {
            text = _text;
            ClassName = GetType().Name;
        }

        public string text { get; set; }
    }
}
