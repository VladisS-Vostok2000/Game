////using System;
////using System.Collections.Generic;
////using System.Linq;
////using System.Text;
////using System.Threading.Tasks;

////namespace Dumb {
////    public abstract class Parser1 { }
////    public class IntParser : Parser1 {
////        public bool TryParse(string str, out int result) => int.TryParse(str, out result);
////    }
////    public interface IParser<T> {
////        bool TryParse(string str, out T result);
////    }

////    public class Example {
////        public void Main() {
////            Dictionary<string, string> a = new Dictionary<string, string>();
////            {
////                if (a.TryGetValue("section name", out string key) && int.TryParse(key, out int result)) {
////                    var g = result;
////                }
////            }
////            {
////                if (a.TryGetValueAndParse<int>(a, "section name", out int result)) {

////                }
////            }

////        }
////        public bool TryParse<T>(IParser<T> parser, out T result) {
////            return parser.TryParse()
////        }
////        private enum ParsableTypes {
////            Int32,
////            Float
////        }
////        public bool TryGetValueAndParse<T1, T2>(Dictionary<T1, string> pairs, T1 key, out T2 result, ParsableTypes type) {
////            // Попытка 1
////            result = Method(pairs[key]);
////            // Попытка 2
////            switch (type) {
////                case ParsableTypes.Int32:
////                    result = int.Parse(pairs[key]);
////                    break;
////                case ParsableTypes.Float:
////                    break;
////                default: throw new NotImplementedException();
////            }
////            // Попытка 3
////        }
////        public T Method<T>(string str) where T : IParseable<T> {
////            Type type = typeof(T);
////            if (type == typeof(int)) {
////                return (T)int.Parse(str);
////            }
////            else
////            if (type == typeof(long)) {

////            }
////            else {
////                throw new InvalidOperationException();
////            }
////        }
////        public interface IParseable<T> {
////            T Parse(string str);
////        }
////        public void Method(string str, ) { }

////    }


////}
//using System;
//using System.Collections.Generic;

//public class Example {
//    public void Main() {
//        var a = new Dictionary<string, string>();
//        if (a.TryGetValue("some key", out string value1) && int.TryParse(value1, out int result1)) {
//            int b = result1;
//        }
//        if (a.TryGetValue("some key", out string value2) && int.TryParse(value2, out int result2)) {
//            int c = result2;
//        }
//        if (a.TryGetValue("some key", out string value3)) {
//            string d = value3;
//        }
//        if (a.TryGetValue("some key", out string value4)) {
//            string e = value4;
//        }
//        if (a.TryGetValue("some key", out string value5) && long.TryParse(value2, out long result5)) {
//            long f = result5;
//        }
//        if (a.TryGetValue("some key", out string value6) && int.TryParse(value2, out int result6)) {
//            long g = result6;
//        }
//    }


//}