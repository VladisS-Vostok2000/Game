//using System;
//using System.Collections.Generic;
//using System.Drawing;
//using System.Linq;
//using System.Runtime.CompilerServices;
//using System.Security.Cryptography.X509Certificates;
//using System.Text;
//using System.Threading.Tasks;
//using System.Windows.Forms;

//namespace Dumb {
//    public interface A {
//        static Method();
//    }
//    public class B : A {
//        public static void Method();
//    }
//    public class C : B {
//        public override void Method() {
//            var c = new C();
//            var b = (B)c;
//            var ac = (A)c;
//            var ab = (A)b;
//            c.Method();
//            b.Method();
//            ac.Method();
//            ab.Method();
//        }
//    }
//}
//namespace Core {
//    public interface Chair { }
//    public class VictorianChair : Chair { }
//    public class ModernChair : Chair { }

//    public interface Sofa { }
//    public class VictorianSofa : Sofa { }
//    public class ModernSofa : Sofa { }

//    public interface FurnitureFactory {
//        Chair CreateChair();
//        Sofa CreateSofa();
//    }

//    public class VictorianFurnitureFactory : FurnitureFactory {
//        public Chair CreateChair() => new VictorianChair();
//        public Sofa CreateSofa() => new VictorianSofa();
//    }

//    public class ModernFurnitureFactory : FurnitureFactory {
//        public Chair CreateChair() => new ModernChair();
//        public Sofa CreateSofa() => new ModernSofa();
//    }

//    public class Programm {
//        public static void Main() {
//            ICollection<Point> s;
//            FurnitureFactory factory;
//            string input = "modern";
//            if (input == "modern") {
//                factory = new ModernFurnitureFactory();
//            }
//            else {
//                factory = new VictorianFurnitureFactory();
//            }

//            Sofa sofa = factory.CreateSofa();
//            Chair chair = factory.CreateChair();

//            // Без паттерна:
//            Sofa sofa1;
//            Chair chair1;
//            if (input == "modern") {
//                sofa1 = new ModernSofa();
//                chair1 = new ModernChair();
//            }
//            else {
//                sofa1 = new VictorianSofa();
//                chair1 = new VictorianChair();
//            }


//        }

//    }
//    public class dumb {
//        protected static int Field { get; set; }
//    }
//    public class dumb1 : dumb {
//        public void Method() {
//            //dumb1.Field
//            List<int> s = new List<int>();
//            s[0] = default;
//            s.Fin
//        }
//    }
//    public interface IList {
//        int this[] {
//            get {

//            }
//        }
//    }
//    public static class Extensions {
//        public static int AExtension(this A sourse) { return 5; }
//        public static int BExtension(this B sourse) { return 10; }
//    }
//    public class A {

//    }
//    public class B : A {

//    }
//    public class Programm1 {
//        public static void Main() {
//            var b = new B();
//            var a = (A)b;
//            //a.

//            var form = new Form();
//            form.Controls[0] = null;

//        }
//    }

//}
//using System;
//namespace Main {
//    public class A {
//        public static A operator +(A v1, B v2) { return v1; }
//        public static B operator +(B v1, A v2) { return v1; }
//        public static void Method() {
//            A a;
//            B b;
//            A s = a + b;
//            B ss = b + a;
//        } 
//    }
//    public class B { }

//}
