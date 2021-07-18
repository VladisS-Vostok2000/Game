using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleEngine{
    public sealed class ColoredCharPicture : ConsoleControl {
        public override Picture ConsolePicture { get; protected set; }


        // REFACTORING: Picture жёстко связан с контролом.
        public ColoredCharPicture(Picture picture) : base(picture.Width, picture.Height) => ConsolePicture = picture;

    }
}
