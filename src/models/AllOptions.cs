using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LuSiBack.src.models
{
    public class AllOptions
    {
        public AllOptions(int id, string name, bool complete)
        {
            Id = id;
            Name = name;
            Complete = complete;
        }

        public int Id { get; set; }
        public string Name { get; set; }
        public bool Complete { get; set; }
    }
}