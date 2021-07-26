using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleEngine{
    public sealed class ColoredCharPicture : ConsoleControl {
        public override Picture ConsolePicture { get; protected set; }



        public ColoredCharPicture(int x, int y, Picture picture) : base(x, y, picture.Width, picture.Height) => ConsolePicture = picture;

    }
}
