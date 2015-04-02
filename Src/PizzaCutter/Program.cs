using PizzaCutter.Model;
using System;
using System.Configuration;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace PizzaCutter
{
	public static class Program
	{
		private static readonly object Lock = new object();

		public static int BestScore { get; set; }

		public static void Main(string[] args)
		{
			var inputFile = ConfigurationManager.AppSettings["Input.Path"];
			var nbRun = int.Parse(ConfigurationManager.AppSettings["Iteration.Count"]);

			Console.ForegroundColor = ConsoleColor.White;

			Parallel.For(0, nbRun, new ParallelOptions() { MaxDegreeOfParallelism = Environment.ProcessorCount }, w => {
				var pizza = new Pizza();

				var inputData = pizza.ParseInput(inputFile);

				var possibleSlices = pizza.FindPizzaSlices(inputData);
				var slices = pizza.SlicePizza(possibleSlices);

				var score = pizza.CalculteScore(slices);

				if (BestScore < score)
				{
					BestScore = score;

					pizza.Save(slices, BestScore);

					DisplayScore(w, BestScore, ConsoleColor.Green);
				}
				else
				{
					DisplayScore(w, score, ConsoleColor.Red);
				}
			});
		}

		private static void DisplayScore(int worker, int score, ConsoleColor color)
		{
			Monitor.Enter(Lock);

			Console.Write("[Worker #{0}] Finished properly with score ", worker);

			Console.ForegroundColor = color;
			Console.WriteLine(score);
			Console.ForegroundColor = ConsoleColor.White;

			Monitor.Exit(Lock);
		} 
	}
}
