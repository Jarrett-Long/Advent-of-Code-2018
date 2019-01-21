void Main()
{
	var points = new List<string>();
	var line = string.Empty;
	var file = new System.IO.StreamReader(@"C:\AoC2018\Day_10\input.txt");
	while ((line = file.ReadLine()) != null) points.Add(line);
	
	var test = new List<string>() {
		"position=< 9,  1> velocity=< 0,  2>",
		"position=< 7,  0> velocity=<-1,  0>",
		"position=< 3, -2> velocity=<-1,  1>",
		"position=< 6, 10> velocity=<-2, -1>",
		"position=< 2, -4> velocity=< 2,  2>",
		"position=<-6, 10> velocity=< 2, -2>",
		"position=< 1,  8> velocity=< 1, -1>",
		"position=< 1,  7> velocity=< 1,  0>",
		"position=<-3, 11> velocity=< 1, -2>",
		"position=< 7,  6> velocity=<-1, -1>",
		"position=<-2,  3> velocity=< 1,  0>",
		"position=<-4,  3> velocity=< 2,  0>",
		"position=<10, -3> velocity=<-1,  1>",
		"position=< 5, 11> velocity=< 1, -2>",
		"position=< 4,  7> velocity=< 0, -1>",
		"position=< 8, -2> velocity=< 0,  1>",
		"position=<15,  0> velocity=<-2,  0>",
		"position=< 1,  6> velocity=< 1,  0>",
		"position=< 8,  9> velocity=< 0, -1>",
		"position=< 3,  3> velocity=<-1,  1>",
		"position=< 0,  5> velocity=< 0, -1>",
		"position=<-2,  2> velocity=< 2,  0>",
		"position=< 5, -2> velocity=< 1,  2>",
		"position=< 1,  4> velocity=< 2,  1>",
		"position=<-2,  7> velocity=< 2, -2>",
		"position=< 3,  6> velocity=<-1, -1>",
		"position=< 5,  0> velocity=< 1,  0>",
		"position=<-6,  0> velocity=< 2,  0>",
		"position=< 5,  9> velocity=< 1, -2>",
		"position=<14,  7> velocity=<-2,  0>",
		"position=<-3,  6> velocity=< 2, -1>"
	};

	$"{Solutions.PartOne(points)}".Dump();
	
}

public static class Solutions
{
	private static List<Point> _points;
	
	public static string PartOne(List<string> input)
	{
		_points = input.Select(p => new Point(p)).ToList();
		
		long xMin, xMax, yMin, yMax;
		
		long smallestArea = long.MaxValue;
		
		int seconds = 0;
		
		while (true)
		{
			xMin = (long)_points.Min(p => p.Position.X);
			xMax = (long)_points.Max(p => p.Position.X);
			yMin = (long)_points.Min(p => p.Position.Y);
			yMax = (long)_points.Max(p => p.Position.Y);
			
			var width = xMax - xMin + 1;
			var height = yMax - yMin + 1;
			var area = (width * height);

			if (area < smallestArea) smallestArea = area;
			else break;

			foreach (var p in _points) p.Position += p.Velocity;

			seconds++;
		}
		
		(--seconds).Dump();
		foreach (var p in _points) p.Position -= p.Velocity;
		
		var coordinatesOfPoints = _points.Select(p => p.Position).ToList();
		
		var grid = new StringBuilder();

		xMin = (long)_points.Min(p => p.Position.X) - 10;
		xMax = (long)_points.Max(p => p.Position.X) + 10;
		yMin = (long)_points.Min(p => p.Position.Y) - 5;
		yMax = (long)_points.Max(p => p.Position.Y) + 5;

		for (var y = yMin; y < yMax; y++)
		{
			var row = string.Empty;
			for (var x = xMin; x < xMax; x++)
			{
				var pointer = new Coordinate((int)x, (int)y);
				if (coordinatesOfPoints.Contains(pointer)) row += "#";
				else row += ".";
			}
			row += "\n";
			grid.AppendLine(row);
		}
		return grid.ToString();
	}
}

public class Point
{
	public Coordinate Position { get; set; }
	
	public Coordinate Velocity { get; set; }
	
	public Point(string p) 
	{
		this.Position = new Coordinate(p.Slice(p.IndexOf("position=<") + "position=<".Length, p.IndexOf(">")));
		this.Velocity = new Coordinate(p.Slice(p.IndexOf("velocity=<") + "velocity=<".Length, p.LastIndexOf(">")));
	}
}

public class Coordinate : IEquatable<Coordinate>
{
	public int X { get; set; }

	public int Y { get; set; }

	public Coordinate(string coordinate)
	{
		this.X = int.Parse(coordinate.Slice(0, coordinate.IndexOf(',')).Trim());
		this.Y = int.Parse(coordinate.Slice(coordinate.IndexOf(',') + 1, coordinate.Length).Trim());
	}

	public Coordinate(int x, int y)
	{
		this.X = x;
		this.Y = y;
	}

	public bool Equals(Coordinate other)
	{
		return this.X == other.X && this.Y == other.Y;
	}
	
	public static Coordinate operator +(Coordinate one, Coordinate two)
	{
		return new Coordinate(one.X + two.X, one.Y + two.Y);
	}

	public static Coordinate operator -(Coordinate one, Coordinate two)
	{
		return new Coordinate(one.X - two.X, one.Y - two.Y);
	}

	public override string ToString()
	{
		return $"({this.X}, {this.Y})";
	}

}

public static class Extensions
{
	public static string Slice(this string source, int start, int end)
	{
		if (end < 0) end = source.Length + end;
		int len = end - start;
		return source.Substring(start, len);
	}
}