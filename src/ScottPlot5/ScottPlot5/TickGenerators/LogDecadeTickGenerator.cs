using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ScottPlot.TickGenerators;
public class LogDecadeTickGenerator(int decades = 3) : ITickGenerator {
	public double Decades { get; set; } = decades;

	public Tick[] Ticks { get; set; } = [];

	public int MaxTickCount { get; set; } = 25;

	public void Regenerate(CoordinateRange range, Edge edge, PixelLength size, SKPaint paint, Label labelStyle) {
		List<Tick> ticks = [];
		var nearestDecade = Math.Round(Math.Log10(range.Min));
		var decades = Math.Ceiling(Math.Log10(range.Max / range.Min));
		//var decade2px = size.Length / decades;

		for (int j = 0; j < decades; j++) {
			var decadeStart = (int)(range.Min * Math.Pow(10, j));
			var decadeEnd = (int)(range.Min * Math.Pow(10, j + 1));
			var decadeMid = (int)Math.Floor((decadeStart + decadeEnd) / 2f / decadeStart) * decadeStart;
			var increment = decadeStart > 0 ? decadeStart : 1;
			for (int i = decadeStart; i < decadeEnd; i += increment) {
				var showLabel = (i == decadeStart * 2) || i == decadeStart || i == decadeMid;
				var label = string.Empty;
				if (showLabel) {
					label += i;
					var tenThousandsDigit = (int)Math.Abs(i / 10_000 % 10);
					var thousandsDigit = (int)Math.Abs(i / 1000 % 10);
					if (thousandsDigit > 0 || tenThousandsDigit > 0) {
						var abbrValue = ushort.Parse($"{tenThousandsDigit}{thousandsDigit}", CultureInfo.InvariantCulture);
						label = $"{abbrValue}k";
					}
				}
				if (i == 100)
					Debug.WriteLine("100 Hz check");
				ticks.Add(new Tick(i, label, true));
				if (i > range.Max)
					break;
			}
		}

		Ticks = ticks.Where(x => range.Contains(x.Position)).ToArray();
	}
}