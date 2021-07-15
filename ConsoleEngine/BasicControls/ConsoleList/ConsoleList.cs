using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ConsoleEngine;

namespace Game.ConsoleEngine.BasicControls.ConsoleList
{
    class ConsoleList : IConsoleControl
    {        
        public Point Location {
            get => new Point(X, Y);
            set
            {
                X = value.X;
                Y = value.Y;
            }
        }
        public int X { get; set; }
        public bool Empty 
        {
            get
            {
                return Items.Count == 0;
            } 
        }
        public int Y { get; set; }
        public int Width { get; set; }
        public int Height { get; set; }
        private List<ConsoleListItem> items;
        public List<ConsoleListItem> Items 
        { 
            get 
            {
                return items.AsReadOnly().ToList();
            } 
            private set 
            {
                items = value;
            } 
        }
        
        private int selectedIndex = -1;
        public int SelectedIndex {
            get
            {
                return selectedIndex;
                
                
            }
            set
            {
                if (Items.Count < value) throw new IndexOutOfRangeException("Cannot select this index");
                selectedIndex = value;
                if (!this.Empty) Items[SelectedIndex].Checked = true;
            }
        }

        
        public ConsoleListItem SelectedItem 
        { 
            get
            {
                return (!this.Empty) ? Items[SelectedIndex] : null;
            }
            set
            {

                if (!this.Empty) 
                {
                    SelectedItem.Checked = false;
                }

                if (!Items.Contains(value)) throw new ArgumentException("value", "Attempt to set invalid item as SelectedItem"); 
                value.Checked = true;
                
                

            }
        }
       
        public ConsoleList()
        {
            Location = new Point(0, 0);
            Height = 1;
            Width = 3;
            items = new List<ConsoleListItem>();            
        }
        ConsolePicture IConsoleDrawable.ColoredCharPicture
        {
            get
            {
                int Width = 0;
                for (int i = 0; i < Items.Count; i++)
                    if (Width < Items[i].ColoredString.Length)
                        Width = Items[i].ColoredString.Length;

                ColoredChar[,] Pixels = new ColoredChar[Items.Count, Width];

                for (int i = 0; i < Items.Count; i++)
                {
                    ColoredString cs = new ColoredString(Items[i].ColoredString.Text, Items[i].ColoredString.Color);
                    for (int j = 0; j < cs.Length; j++)
                        Pixels[i, j] = cs[j];
                }

                return new ConsolePicture(Pixels);

            }
        }

        
    }
}
