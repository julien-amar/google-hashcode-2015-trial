using System.Collections.Generic;

namespace PizzaCutter.Model
{
	public class PizzaSlice
	{
		public int X { get; private set; }
		public int Y { get; private set; }
		public int Width { get; private set; }
		public int Height { get; private set; }

		public ICollection<PizzaCell> ImpactedCells { get; private set; }

		public PizzaSlice(int x, int y, int width, int height)
		{
			X = x;
			Y = y;

			Width = width;
			Height = height;

			ImpactedCells = new List<PizzaCell>();
		}
	}
}
