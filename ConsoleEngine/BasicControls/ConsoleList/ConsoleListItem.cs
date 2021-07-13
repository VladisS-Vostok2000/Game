using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ConsoleEngine;

namespace Game.ConsoleEngine.BasicControls.ConsoleList
{
    class ConsoleListItem
    {
        private bool _Checked;
        public bool Checked {
            get
            {
                return _Checked;
            }
            set
            {
                _Checked = value;
                Text.Color = (_Checked) ? CheckedColor : UncheckedColor;
            } 
        }
        public ColoredString Text { get; set; }
        public ConsoleColor CheckedColor { get; set; } = ConsoleColor.Red;
        public ConsoleColor UncheckedColor { get; set; } = ConsoleColor.White;

        public ConsoleListItem(string text, ConsoleColor color = ConsoleColor.White)
        {
            Text = new ColoredString(text, color);
            Checked = false;
        }

    }
}
