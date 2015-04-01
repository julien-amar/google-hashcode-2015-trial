namespace PizzaCutter.Model
{
	public class InputData
	{
		public int Rows { get; set; }
		public int Columns { get; set; }
		public int MaximumHamOnSlice { get; set; }
		public int MaximumSizeOfSlice { get; set; }
		public PizzaCell[,] Pizza { get; set; }
	}
}