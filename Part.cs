using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Game {
    // REFACTORING: вычленить отсюда изменяющиеся, но не инициализированные данные? Таким образом все объекты Unit
    // будут ссылаться на один объект, и можно будет, например, изменить MaxHP всех кузовов
    // без прохождения коллекции юнитов. Либо же скомпозировать Part в новом классе, агрегированном в Unit.
    public abstract class Part {
        public string Name { get; set; }
        public string DisplayedName { get; set; }
        public int MaxHP { get; set; }
        public int Masse { get; set; }



        protected Part(string name) => Name = name;
        protected Part(string name, string displayedName, int maxHP, int masse) : this(name) {
            DisplayedName = DisplayedName;
            MaxHP = maxHP;
            Masse = masse;
        }

    }
}
