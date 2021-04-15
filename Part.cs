using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Game {
    public abstract class Part : ICloneable {
        public string Name { get; set; }
        public string DisplayedName { get; set; }
        public int MaxHP { get; set; }
        public int CurrentHP { get; set; }
        public int Masse { get; set; }



        // TODO: выбрать тип конструктора.
        protected Part(string name) => Name = name;
        protected Part(string name, string displayedName, int maxHP, int masse) : this(name) {
            DisplayedName = DisplayedName;
            MaxHP = maxHP;
            Masse = masse;
        }



        public object Clone() => MemberwiseClone();

    }
}
