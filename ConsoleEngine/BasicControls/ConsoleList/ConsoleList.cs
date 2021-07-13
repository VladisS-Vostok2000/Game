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
        public int Y { get; set; }
        public int Width { get; set; }
        public int Height { get; set; }
        public List<ConsoleListItem> Items { get; private set; }
        
        private int _SelectedIndex = -1;
        public int SelectedIndex {
            get
            {
                return _SelectedIndex;
                
                
            }
            set
            {
                if (Items.Count < value)
                {
                    if (SelectedIndex != -1) Items[SelectedIndex].Checked = false;
                    _SelectedIndex = -1;
                }
                else
                {
                    _SelectedIndex = value;
                    if (SelectedIndex != -1) Items[SelectedIndex].Checked = true;
                }
            }
        }

        
        public ConsoleListItem SelectedItem 
        { 
            get
            {
                return (SelectedIndex != -1) ? Items[SelectedIndex] : null;
            }
            set
            {

                if (SelectedIndex != -1) 
                {
                    SelectedItem.Checked = false;
                }

                ConsoleListItem ItemToCheck = Items.Find(i => i == value);
                if (ItemToCheck != null) 
                {
                    ItemToCheck.Checked = true;
                }
                

            }
        }

        public ConsoleList()
        {
            Location = new Point(0, 0);
            Height = 1;
            Width = 3;
           
            
        }
        ConsolePicture IConsoleDrawable.ColoredCharPicture => throw new NotImplementedException();

        //Not so optimized
        public ConsolePicture ColoredCharPicture() {

            string Decorator = "[*] - ";
            //Calculation of Width of ConsolePicture 
            int Width = 0;
            for (int i = 0; i < Items.Count; i++)
                if (Width < Items[i].Text.Length)
                    Width = Items[i].Text.Length;

            Width += Decorator.Length;
            ColoredChar[,] Pixels = new ColoredChar[Items.Count, Width];

            for (int i = 0; i < Items.Count; i++)
            {
                ColoredString cs = new ColoredString(Decorator + Items[i].Text, Items[i].Text.Color);
                for (int j = 0; j < cs.Length; j++)
                    Pixels[i, j] = cs[j];
            }

            return new ConsolePicture(Pixels);
        }
    }
}
