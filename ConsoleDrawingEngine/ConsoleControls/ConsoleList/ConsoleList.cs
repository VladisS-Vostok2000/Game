//using System;
//using System.Collections.Generic;
//using System.Drawing;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;
//using ConsoleDrawingEngine;

//namespace ConsoleDrawingEngine {
//    [Obsolete]
//    public sealed class ConsoleList : ConsoleControl {
//        public bool Empty => Items.Count == 0;
//        private List<ConsoleListColoredString> items;
//        internal IReadOnlyList<ColoredString> Items => items.AsReadOnly();

//        private int selectedIndex = -1;
//        public int SelectedIndex {
//            get => selectedIndex;
//            set {
//                if (Items.Count < value) throw new IndexOutOfRangeException("Cannot select this index.");
//                selectedIndex = value;
//                if (!Empty) Items[SelectedIndex].Checked = true;
//            }
//        }


//        internal ColoredString SelectedString {
//            get => (!Empty) ? Items[SelectedIndex].ColoredString : null;
//            set {
//                if (Empty) { throw new InvalidOperationException("Листбокс не содержит элементов для выделения."); }
                
//                int valueIndex = items.In
//                if (!Items.Contains(value)) {
//                    throw new ArgumentException("value", "Заданный элемент не содержится.");
//                }

//                if (!Empty) { SelectedString.Checked = false; }


//                value.Checked = true;
//            }
//        }



//        public ConsoleList(int width, int height) {
//            Location = new Point(0, 0);
//            Width = width;
//            Height = height;
//            items = new List<ConsoleListColoredString>();
//        }



//        public Picture ColoredCharPicture {
//            get {
//                int Width = 0;
//                for (int i = 0; i < Items.Count; i++)
//                    if (Width < Items[i].ColoredString.Length)
//                        Width = Items[i].ColoredString.Length;

//                ColoredChar[,] Pixels = new ColoredChar[Items.Count, Width];

//                for (int i = 0; i < Items.Count; i++) {
//                    ColoredString cs = new ColoredString(Items[i].ColoredString.Text, Items[i].ColoredString.Color);
//                    for (int j = 0; j < cs.Length; j++)
//                        Pixels[i, j] = cs[j];
//                }

//                return new ConsolePicture(Pixels);

//            }
//        }

//        public override Picture ConsolePicture { get => throw new NotImplementedException(); protected set => throw new NotImplementedException(); }
//    }
//}
