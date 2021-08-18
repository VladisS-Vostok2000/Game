//using System;
//using System.Collections.Generic;
//using System.Drawing;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;
//using static Game.ExtensionMethods.ConsoleExtensionMethods;
//using static Game.ExtensionMethods.BasicTypesExtensionsMethods;
//using static Game.ConsoleDrawingEngine.Geometry;
//using static Game.ConsoleDrawingEngine.SystemDrawingExtensionMethods;
//using System.Runtime.CompilerServices;

//namespace Game.ConsoleDrawingEngine.Controls {
//    public sealed class ConsolePanel : ConsoleContainer {
//        private List<ConsoleControl> controls;
//        public override IReadOnlyList<ConsoleControl> Controls => controls.AsReadOnly();


//        private ColoredChar[,] coloredChars;
//        public override Picture ConsolePicture { protected set; get; }



//        public ConsolePanel(Point location, Size size) : base(location, size) {
//            controls = new List<ConsoleControl>();
//            coloredChars = new ColoredChar[Width, Height];
//            ConsolePicture = new ColoredCharsPicture(coloredChars);
//        }



//        public override void AddControl(ConsoleControl control) {
//            bool valid = ContainsAreaOf(control) && !IntersectsWintControls(control);
//            if (!valid) {
//                throw new ColoredCharPanelInvalidArgumentException("Заданный контрол находится за пределами дочернего или пересекает его границу.", control);
//            }

//            controls.Add(control);
//        }
//        public override bool RemoveControl(ConsoleControl control) => controls.Remove(control);

//    }
//}
