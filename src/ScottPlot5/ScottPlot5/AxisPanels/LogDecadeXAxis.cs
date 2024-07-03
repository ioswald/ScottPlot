using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using ScottPlot.TickGenerators;

namespace ScottPlot.AxisPanels;
public class LogDecadeXAxis : XAxisBase, IXAxis {
	private static Type mType = typeof(LogDecadeXAxis);

	public override Edge Edge { get; } = Edge.Bottom;

	//public Range DisplayRange { get; set; }
	//	get => TickGenerator.DisplayRange;
	//	set => TickGenerator.DisplayRange = value;
	//}

	private LogDecadeTickGenerator _tickGenerator = new();

	public new LogDecadeTickGenerator TickGenerator {
		get => _tickGenerator;
		set {
			if (value is not LogDecadeTickGenerator)
				throw new ArgumentException($"LogDecade axis must have a {nameof(ITickGenerator)} generator");

			_tickGenerator = value;
			base.TickGenerator = _tickGenerator;
		}
	}

	public LogDecadeXAxis() : base() {
		base.TickGenerator = _tickGenerator;
	}

	public new float GetPixel(double position, PixelRect dataArea) {
		var decades = Math.Log10(Range.Max / Range.Min);
		var decade2px = dataArea.Width / decades;
		var pixel = dataArea.Left + (float)(Math.Log10(position / Range.Min) * decade2px);
		return pixel;
	}

	public new double GetCoordinate(float pixel, PixelRect dataArea) {
		var decades = Math.Log10(Range.Max / Range.Min);
		var decade2px = dataArea.Width / decades;
		//float pxFromLeftEdge = dataArea.Left + (float)(Math.Log10(position / Range.Min) * decade2px);
		//double pxPerUnit = dataArea.Width / Width;
		float pxFromLeftEdge = pixel - dataArea.Left;
		var position = (Math.Pow(10.0, pxFromLeftEdge / decade2px) * Range.Min);
		//double unitsFromEdge = pxFromLeftEdge / pxPerUnit;
		return position;
	}

	public new double GetPixelDistance(double distance, PixelRect dataArea) {
		var decades = Math.Log10(Range.Max / Range.Min);
		return distance * dataArea.Width / decades;
	}
}