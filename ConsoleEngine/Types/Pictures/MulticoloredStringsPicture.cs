using ConsoleEngine;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleEngine {
    class MulticoloredStringsPicture : Picture {
        private MulticoloredStringBuilder[] picture;



        public MulticoloredStringsPicture(MulticoloredStringBuilder[] picture) {
            // TODO: проверка на "квадратность"
            this.picture = picture;
            // Width = 
            // Height =
        }


        // TODO: реализовать
        public IEnumerable<MulticoloredStringBuilder> ToMulticoloredStrings() {
            foreach (var multicoloredString in picture) {
                yield return multicoloredString;
            }
        }

    }
}
