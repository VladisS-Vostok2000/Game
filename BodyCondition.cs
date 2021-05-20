using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Game {
    public class BodyCondition : PartCondition {
        public Body Body => (Body)Part;



        public BodyCondition(Body body) : base(body) { }

    }
}
