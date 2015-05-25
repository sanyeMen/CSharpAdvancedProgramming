using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CSharpAdvancedProgramming.Linq
{
    [Serializable]
    public class Team
    {

        public Team(string name, params int[] years)
        {
            this.Name = name;
            this.Years = years;
        }

        public string Name { get; set; }

        public int[] Years { get; set; }
    }
}
