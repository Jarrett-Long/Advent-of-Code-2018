<Query Kind="Program" />

void Main()
{
	var records = new List<string>();
	var line = string.Empty;
	var file = new System.IO.StreamReader(@"C:\AoC2018\Day_05\input.txt");
	var input = file.ReadToEnd().Replace("\n", string.Empty);

	$"Number of remaining units: {Solutions.PartOne(input)}".Dump();
	$"Smallest polymer: {Solutions.PartTwo(input)}".Dump();
	
}

public static class Solutions 
{
	public static int PartOne(string input) {
		var output = FullyReact(input);
		return output.Length;
	}
	
	public static int PartTwo(string input) {
		int smallestPolymer = FullyReact(input).Length;
		foreach (var unit in new List<string>() {"a", "b", "c" , "d", "e", "f", "g", "h", "i", "j", "k", "l", "m", "n", "o", "p", "q", "r", "s", "t", "u", "v", "w", "x", "y", "z" }) 
		{
			var polymer = FullyReact(input.Replace(unit, string.Empty).Replace(unit.ToUpper(), string.Empty));
			if (polymer.Length < smallestPolymer) smallestPolymer = polymer.Length;
		}
		return smallestPolymer;
	}

	public static string FullyReact(string output)
	{
		var loopAgain = true;
		while (loopAgain)
		{
			var swaps = 0;
			for (var i = 0; i < output.Length - 1; i++)
			{
				var sameType = Char.ToUpper(output[i]) == Char.ToUpper(output[i + 1]);
				var polar = (Char.IsUpper(output[i]) && Char.IsLower(output[i + 1])) || (Char.IsLower(output[i]) && Char.IsUpper(output[i + 1]));
				if (sameType && polar)
				{
					output = output.Remove(i, 2);
					swaps++;
				}
			}
			if (swaps == 0) loopAgain = false;
			else swaps = 0; // else reset the counter and loop again.
		}
		return output;
	}

	// recursive version. this causes a stack overflow unfortunately
	public static string Destroy(string output)
	{
		for (var i = 0; i < output.Length - 1; i++)
		{
			var sameType = Char.ToUpper(output[i]) == Char.ToUpper(output[i + 1]);
			var polar = (Char.IsUpper(output[i]) && Char.IsLower(output[i + 1])) || (Char.IsLower(output[i]) && Char.IsUpper(output[i + 1]));
			if (sameType && polar) return Destroy(output.Remove(i, 2));
		}
		return output;
	}


} 
