//using System;
//using System.Collections.Generic;
//using System.Drawing;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;

//namespace Game.ConsoleDrawingEngine.Types {
//    public abstract class ConsoleContainer : ConsoleControl {
//        public IReadOnlyList<ConsoleControl> Controls => controls.AsReadOnly();
//        protected List<ConsoleControl> controls;



//        public void AddControl(ConsoleControl control) {
//            controls.Add(control);
//        }
//        public bool RemoveControl(ConsoleControl control) {
//            return controls.Remove(control);
//        }


//        /// <summary>
//        /// False, если заданный <see cref="ConsoleControl"/> не имеет пересечения с внутренними <see cref="ConsoleControl"/>.
//        /// </summary>
//        public bool IntersectsWintControls(ConsoleControl control) {
//            foreach (var internalControl in Controls) {
//                if (control.IntersectsWith(internalControl)) {
//                    return true;
//                }
//            }
//            return false;
//        }

//    }
//}
