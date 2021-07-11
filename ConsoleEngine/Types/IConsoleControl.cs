using ConsoleEngine;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Game.ConsoleEngine.Types {
    public interface IConsoleControl : IConsoleDrawable {
        IReadOnlyList<IConsoleControl> Controls { get; }

    }
}
