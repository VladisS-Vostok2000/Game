using System.Text;

namespace MyParsers {
    public static class StringExtensionsMethods {
        /// <summary>
        /// Отчистит строку от символов, относящихся к категории пробелов.
        /// </summary>
        public static string ClearEmptySpaces(this string target) {
            StringBuilder sb = new StringBuilder();
            for (int i = 0; i < target.Length; i++) {
                if (!char.IsWhiteSpace(target[i])) {
                    sb.Append(target[i]);
                }
            }

            return sb.ToString();
        }

        /// <summary>
        /// Извлекает подстроку заданной длинны с заданной позиции.
        /// </summary>
        public static string Extract(this string target, int startIndex, int length) {
            // REFACTORING: добавить exception.
            StringBuilder sb = new StringBuilder();
            for (int i = startIndex; length-- > 0; i++) {
                sb.Append(target[i]);
            }

            return sb.ToString();
        }

        /// <summary>
        /// Извлекает подстроку с заданной позиции, пока не будет встречен заданный символ.
        /// Возвращается строка содержащая символ со стартовой индексацией, но без заданного.
        /// </summary>
        public static string Extract(this string target, int startIndex, char chr) {
            int lastIndex = target.IndexOf(chr, startIndex);
            int length = lastIndex - startIndex;
            return Extract(target, startIndex, length);
        }

    }
}
