using System.Collections.Generic;

namespace PizzaCutter.Model
{
	public enum PizzaCellState
	{
		NotAnalyzed,
		Analyzed
	}

	public class PizzaCell
	{
		public bool IsHam { get; private set; }
		public ICollection<PizzaSlice> Slices { get; private set; }
		public PizzaCellState State { get; set; }

		public PizzaCell(bool isHam)
		{
			IsHam = isHam;

			State = PizzaCellState.NotAnalyzed;

			Slices = new List<PizzaSlice>();
		}
	}
}
