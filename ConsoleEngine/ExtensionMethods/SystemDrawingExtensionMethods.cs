using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Game.ConsoleEngine {
	public static class SystemDrawingExtensionMethods {
		/// <summary>
		/// True, если заданный <see cref="Rectangle"/> полностью лежит в прямоугольнике.
		/// </summary>
		/// <param name="rectangle"></param>
		/// <param name="target"></param>
		/// <returns></returns>
		public static bool ContainsRectangle(this Rectangle rectangle, Rectangle target) {
			return target.X >= 0 && target.Y >= 0 &&
				target.Right < rectangle.Width && target.Bottom < rectangle.Height;
        }

	}
}
