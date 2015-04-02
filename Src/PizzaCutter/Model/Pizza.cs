using PizzaCutter.Extensions;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;

namespace PizzaCutter.Model
{
	public class Pizza
	{
		public InputData ParseInput(string inputFile)
		{
			var lines = File.ReadAllLines(inputFile);

			var infos = lines.First()
				.Split(' ')
				.Select(int.Parse)
				.ToArray();

			var nbRow = infos[0];
			var nbCol = infos[1];
			var maximumHamOnSlice = infos[2];
			var maximumSizeOfSlice = infos[3];

			var pizza = new PizzaCell[nbRow, nbCol];

			var y = 0;
			foreach (var line in lines.Skip(1))
			{
				var x = 0;
				foreach (var cell in line)
				{
					pizza[y, x] = new PizzaCell(cell == 'H');

					x++;
				}
				y++;
			}

			return new InputData
			{
				Rows = nbRow,
				Columns = nbCol,
				MaximumHamOnSlice = maximumHamOnSlice,
				MaximumSizeOfSlice = maximumSizeOfSlice,
				Pizza = pizza
			};
		}

		public IEnumerable<PizzaSlice> FindPizzaSlices(InputData inputData)
		{
			var parts = new List<PizzaSlice>();

			for (int y = 0; y < inputData.Rows; ++y)
				for (int x = 0; x < inputData.Columns; ++x)
				{
					for (int height = 1; height <= inputData.MaximumSizeOfSlice; ++height)
						for (int width = inputData.MaximumSizeOfSlice / height; width > 0; --width)
						{
							var surface = width * height;

							if (surface < inputData.MaximumHamOnSlice)
								continue;

							if (surface > inputData.MaximumSizeOfSlice)
								continue;

							if (y + height > inputData.Rows)
								continue;

							if (x + width > inputData.Columns)
								continue;

							var part = new PizzaSlice(x, y, width, height);

							for (int yPart = y; yPart < y + height; ++yPart)
								for (int xPart = x; xPart < x + width; ++xPart)
									part.ImpactedCells.Add(inputData.Pizza[yPart, xPart]);

							if (part.ImpactedCells.Where(c => c.IsHam).Count() == 3)
								parts.Add(part);
						}
				}

			return parts
				.OrderByDescending(x => x.Surface)
				.ToList();
		}

		public IEnumerable<PizzaSlice> SlicePizza(IEnumerable<PizzaSlice> slices)
		{
			var randomCounter = int.Parse(ConfigurationManager.AppSettings["Random.Counter"]);
			var slicedParts = new List<PizzaSlice>();

			while (slices.Any())
			{
				PizzaSlice slice;

				if (randomCounter > 0)
				{
					var biggestSliceSize = slices.Max(s => s.Surface);

                    slice = slices.ShuffleFirst();

					randomCounter--;
				}
				else
				{
					slice = slices.FirstOrDefault();
				}

				foreach (var impactedCell in slice.ImpactedCells)
				{
					impactedCell.State = PizzaCellState.Analyzed;
				}

				slices = slices
					.Where(x => !x.ImpactedCells.Any(c => c.State == PizzaCellState.Analyzed))
					.ToList();

				slicedParts.Add(slice);
			}

			return slicedParts;
		}

		public int CalculteScore(IEnumerable<PizzaSlice> slices)
		{
			var score = slices.Sum(x => x.Width * x.Height);

			return score;
		}

		public void Save(IEnumerable<PizzaSlice> slices, int bestScore)
		{
			var filename = string.Format("{0}.txt", bestScore);

			var output = from l in slices
						 select string.Format(
							 "{0} {1} {2} {3}",
							 l.Y,
							 l.X,
							 l.Y + l.Height - 1,
							 l.X + l.Width - 1
						 );

			File.AppendAllLines(filename, new[] { slices.Count().ToString() });
			File.AppendAllLines(filename, output);
		}
	}
}
