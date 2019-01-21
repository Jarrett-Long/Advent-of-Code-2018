void Main()
{
	var file = new System.IO.StreamReader(@"C:\AoC2018\Day_02\input.txt");
	var ids = new List<string>();
	var line = string.Empty;
	while((line = file.ReadLine()) != null) ids.Add(line);
	
	PartOne.SolveFor(ids).Dump();
	PartTwo.SolveFor(ids).Dump();
}

public class PartOne 
{
	public static int SolveFor(List<string> ids)
	{
		var idsWithTwo = 0;
		var idsWithThree = 0;
		foreach (var id in ids) {
			var hasTwo = false;
			var hasThree = false;
			foreach (var c in id) {
				if (id.Count(x => x == c) == 2) hasTwo = true;
				if (id.Count(x => x == c) == 3) hasThree = true;
			}
			if (hasTwo) idsWithTwo += 1;
			if (hasThree) idsWithThree += 1;
		}
		return idsWithTwo * idsWithThree;
	}
}

public class PartTwo
{
	public static object SolveFor(List<string> ids)
	{
		var similarId1 = string.Empty;
		var differAtIndex = 0;
		foreach (var id1 in ids)
		{
			foreach (var id2 in ids.Skip(1))
			{
				var differAtIndexes = new List<int>();
				for (var i = 0; i < id2.Length; i++)
				{
					if (id1[i] != id2[i]) differAtIndexes.Add(i);
				}
				if (differAtIndexes.Count() == 1)
				{
					similarId1 = id1;
					differAtIndex = differAtIndexes.First();
					break;
				}
			}
		}
		return similarId1.Remove(differAtIndex, 1);
	}
}