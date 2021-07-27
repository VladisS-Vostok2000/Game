using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Game.ConsoleEngine.ConsoleControls {
    public sealed class ColoredCharPicture : ConsoleControl {
        public override Picture ConsolePicture { get; protected set; }



        public ColoredCharPicture(int x, int y, Picture picture) : base(new Point(x, y), new Size(picture.Width, picture.Height)) => ConsolePicture = picture;

    }
}
