void Main()
{
	var coordinates = new List<string>();
	var line = string.Empty;
	var file = new System.IO.StreamReader(@"C:\AoC2018\Day_06\input.txt");
	while ((line = file.ReadLine()) != null) coordinates.Add(line);

	$"Test: {Solutions.PartOne(new List<string>() { "1, 1", "1, 6", "8, 3", "3, 4", "5, 5", "8, 9" })}".Dump();
	$"(5 seconds) Largest area that isn't infinite: {Solutions.PartOne(coordinates)}".Dump();
	$"(0.2 seconds) Area of the region with locations closest to all coordinates: {Solutions.PartTwo(coordinates)}".Dump();

}

public static class Solutions 
{
	private static List<Coordinate> _coordinates;
	
	public static int PartOne(List<string> input) 
	{
		_coordinates = input.Select(c => new Coordinate(c)).ToList();
		var smallPlaneAreas = CalculateCoordinateAreas(0, _coordinates.Max(c => c.X), 0, _coordinates.Max(c => c.Y));
		var largePlaneAreas = CalculateCoordinateAreas(-50, _coordinates.Max(c => c.X) + 50, -50, _coordinates.Max(c => c.Y) + 50);
		var containedAreas = smallPlaneAreas.Where(kvp => largePlaneAreas[kvp.Key] == kvp.Value);
		return containedAreas.Max(kvp => kvp.Value);
	}
	
	public static int PartTwo(List<string> input) 
	{
		if (_coordinates == null) _coordinates = input.Select(c => new Coordinate(c)).ToList();
		var maxDistance = 10000;
		var region = new List<Coordinate>();
		var width = _coordinates.Max(c => c.X);
		var height = _coordinates.Max(c => c.Y);
		for (var x = 0; x < width; x++) {
			for (var y = 0; y < height; y++) {
				var totalDistance = 0;
				var pointer = new Coordinate(x, y);
				foreach (var c in _coordinates) {
					totalDistance += ManhattanDistance(pointer, c);
				}
				if (totalDistance < maxDistance) region.Add(pointer);
			}
		}
		return region.Count();
	}
	
	private static Dictionary<string, int> CalculateCoordinateAreas(int xStart, int xEnd, int yStart, int yEnd)
	{
		var areas = new Dictionary<string, int>();
		foreach (var c in _coordinates) areas[c.ToString()] = 0;
		for (var x = xStart; x < xEnd; x++)
		{
			for (var y = yStart; y < yEnd; y++)
			{
				var closest = ClosestCoordinateTo(new Coordinate(x, y));
				if (closest != null && _coordinates.Contains(closest))
				{
					areas[closest.ToString()]++;
				}
			}
		}
		return areas;
	}

	private static Coordinate ClosestCoordinateTo(Coordinate pointer) {
		var sortedList = _coordinates.OrderBy(c => ManhattanDistance(pointer, c));
		if (ManhattanDistance(pointer, sortedList.ElementAt(0)) == ManhattanDistance(pointer, sortedList.ElementAt(1))) return null;
		return sortedList.First();
	}
			
	private static int ManhattanDistance(Coordinate c1, Coordinate c2) {
		return Math.Abs(c1.X - c2.X) + Math.Abs(c1.Y - c2.Y);
	}
}

public class Coordinate : IEquatable<Coordinate>
{
	public int X { get; set; }
	
	public int Y { get; set; }
	
	public Coordinate(string coordinate) 
	{
		this.X = int.Parse(coordinate.Slice(0, coordinate.IndexOf(',')));
		this.Y = int.Parse(coordinate.Slice(coordinate.IndexOf(',') + 2, coordinate.Length));
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