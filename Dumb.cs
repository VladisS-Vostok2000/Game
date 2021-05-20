//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;

//namespace Game {
//    public interface Chair { }
//    public class VictorianChair : Chair { }
//    public class ModernChair : Chair { }

//    public interface Sofa { }
//    public class VictorianSofa : Sofa{ }
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

//}
