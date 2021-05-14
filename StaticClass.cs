//////using System;
//////using System.Collections.Generic;
//////using System.Linq;
//////using System.Text;
//////using System.Threading.Tasks;
//using System;
//using System.Collections.Generic;
//using System.Drawing;
//public static class StaticClass {
//    public static Struct ExtensionMethod<T>(this Interface @interface, string s) { }
//}
//public struct Struct {
//    public static Class<Struct> Field;
//}
//    public enum Enum { }
//    public delegate Struct Delegate(Enum param);
//public interface Interface {
//    int Field { get; set; }
//}
//public class Class<T> : Interface {
//    public static Class<T> Field { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
//    public event Delegate PropertyEvent;


//    /// <summary>
//    public Delegate Method() {
//        // Comment
//        PropertyEvent +=
//        (Enum local) => ((Interface)Struct.Field).ExtensionMethod<T>("string\"");
//        throw new Exception();
//    }


//}

//////namespace Dumb {
//////    class Dumb {
//////        public void Do1() { }
//////        public void Do2() { }
//////        public void Do3() { }
//////        public void Do4() { }
//////        public void Main() {
//////            List<bool> sourse = new List<bool>();
//////            for (int i = 0; i < sourse.Count - 3; i++) {
//////                // Необходимое условие для создания объекта из списка 
//////                // параметров потенциальных объектов
//////                bool condition1 = sourse[i];
//////                if (condition1) {
//////                    Do1();
//////                }
//////                else {
//////                    continue;
//////                }
//////                bool condition2 = sourse[i + 1];
//////                if (condition2) {
//////                    Do2();
//////                }
//////                else {
//////                    continue;
//////                }
//////                bool condition3 = sourse[i + 2];
//////                if (condition3) {
//////                    Do3();
//////                }
//////                else {
//////                    continue;
//////                }
//////                bool condition4 = sourse[i + 3];
//////                if (condition4) {
//////                    Do2();
//////                }
//////                else {
//////                    continue;
//////                }
//////            }
//////        }
//////    }
//////}
//////namespace Dumb1 {
//////    class Dumb {
//////        public void Do1() { }
//////        public void Do2() { }
//////        public void Do3() { }
//////        public void Do4() { }
//////        public void Main() {
//////            List<bool> sourse = new List<bool>();
//////            for (int i = 0; i < sourse.Count - 3; i++) {
//////                // Необходимое условие для создания объекта из списка 
//////                // параметров потенциальных объектов
//////                bool condition1 = sourse[i];
//////                bool susefully = sourse[i] ? Do1() : continue {
//////                    Do1();
//////                }
//////                else {
//////                    continue;
//////                }
//////                bool condition2 = sourse[i + 1];
//////                if (condition2) {
//////                    Do2();
//////                }
//////                else {
//////                    continue;
//////                }
//////                bool condition3 = sourse[i + 2];
//////                if (condition3) {
//////                    Do3();
//////                }
//////                else {
//////                    continue;
//////                }
//////                bool condition4 = sourse[i + 3];
//////                if (condition4) {
//////                    Do2();
//////                }
//////                else {
//////                    continue;
//////                }
//////            }
//////        }
//////    }
//////}
////using System.Collections.Generic;

////namespace Dumb {
////    class Dumb {
////        Dictionary<MyClass, MyClass> dictionary = new Dictionary<MyClass, MyClass>();
////        void Method() {
////            foreach (var pair in dictionary) {
////                pair.Key.Field = 5;
////            };
////        }
////    }
////    class MyClass {
////        public int Field;
////    }
////}

