using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Game {
    public abstract class PartCondition {
        protected Part Part { get; set; }


        public int CurrentHP { get; set; }



        public PartCondition(Part part) {
            Part = part;
        }

    }
}