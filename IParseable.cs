//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;

//namespace Game {
//    public interface IParseable<T> {
//        bool TryParse(string str, out T result);
//    }
//    public static class ExistingMethods {
//        public static bool TryToGetAndParse<T1, T2>(this Dictionary<T1, string> dictionary, T1 key, out T2 value) where T2 : IParseable<T2> {
//            value = default;
//            return dictionary.ContainsKey(key) && value.TryParse(dictionary[key], out value);
//        }

//    }

//}
