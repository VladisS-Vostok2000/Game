using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Game {
    public sealed class EngineCondition : PartCondition {
        public Engine Engine => (Engine)Part;
         


        public EngineCondition(Engine engine) : base(engine) {
        
        }

    }
}
