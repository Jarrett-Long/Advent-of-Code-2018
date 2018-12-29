<Query Kind="Program" />

void Main()
{
	var coordinates = new List<string>();
	var line = string.Empty;
	var file = new System.IO.StreamReader(@"C:\AoC2018\Day_06\input.txt");
	while ((line = file.ReadLine()) != null) coordinates.Add(line);

	$"Test: {Solutions.PartOne(new List<string>() { "1, 1", "1, 6", "8, 3", "3, 4", "5, 5", "8, 9" })}".Dump();
	$"Largest area that isn't infinite: {Solutions.PartOne(coordinates)}".Dump();

}

public static class Solutions 
{
	private static List<Coordinate> coordinates;
	
	public static int PartOne(List<string> input) 
	{
		coordinates = input.Select(c => new Coordinate(c)).ToList();
		var width = coordinates.Max(c => c.X);
		var height = coordinates.Max(c => c.Y);

		// get coordinate areas on a small plane
		var coordinateAreasSmall = new Dictionary<Coordinate, int>();
		foreach (var c in coordinates) coordinateAreasSmall[c] = 0;

		for (var x = 0; x < width; x++) {
			for (var y = 0; y < height; y++) {
				var closest =  ClosestCoordinateTo(new Coordinate(x,y));
				if (closest != null && coordinates.Contains(closest)) {
					coordinateAreasSmall[closest]++;
				}
			}
		}
		
		// get coordinate areas on a larger plane
		var coordinateAreasLarge = new Dictionary<Coordinate, int>();
		foreach (var c in coordinates) coordinateAreasLarge[c] = 0;

		for (var x = -50; x < width + 50; x++){
			for (var y = -50; y < height + 50; y++){
				var closest = ClosestCoordinateTo(new Coordinate(x, y));
				if (closest != null && coordinates.Contains(closest)){
					coordinateAreasLarge[closest]++;
				}
			}
		}
		
		// compare coordinate areas between small and large planes to determine which ones grow infinitely
		var coordinateAreasFinal = new Dictionary<Coordinate, int>();
		
		foreach (var c in coordinates) {
			if (coordinateAreasSmall[c] == coordinateAreasLarge[c]) {
				coordinateAreasFinal[c] = coordinateAreasSmall[c];
			}
		}	
		
		return coordinateAreasFinal.Max(kvp => kvp.Value);
	}
	
	private static Coordinate ClosestCoordinateTo(Coordinate pointer) {
		var sortedList = coordinates.OrderBy(c => ManhattanDistance(pointer, c));
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