using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Game.Core {
    public sealed class Team {
        public string Name { get; set; }
        public string DisplayedName { get; set; }
        public ConsoleColor Color { get; set; }



        public Team(string name) => Name = name;

    }
}
