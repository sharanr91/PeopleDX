using System;
using System.Collections.Generic;
using System.Text;

namespace RecruitmentStep1
{
    public class Attribute
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Type { get; set; }
        public string Minvalue { get; set; }
        public string MaxValue { get; set; }
        public bool Optional { get; set; }
        public Dictionary<Guid, string> PossibleValues { get; set; }
    }
}
