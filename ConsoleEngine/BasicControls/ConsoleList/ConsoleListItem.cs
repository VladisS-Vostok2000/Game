using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ConsoleEngine;

namespace Game.ConsoleEngine.BasicControls.ConsoleList
{
    internal class ConsoleListItem
    {
        private bool @checked;
        public bool Checked {
            get
            {
                return @checked;
            }
            set
            {
                @checked = value;
                ColoredString.Color = (@checked) ? CheckedColor : UncheckedColor;
            } 
        }
        public ColoredString ColoredString { get; set; }
        public ConsoleColor CheckedColor { get; set; } = ConsoleColor.Red;
        public ConsoleColor UncheckedColor { get; set; } = ConsoleColor.White;

        public ConsoleListItem(string text, ConsoleColor color = ConsoleColor.White)
        {
            ColoredString = new ColoredString(text, color);
            Checked = false;
        }

    }
}
