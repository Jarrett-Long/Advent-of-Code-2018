void Main()
{
	// Part 1
	var scores = new LinkedList<int>();
	var elf1 = scores.AddFirst(3);
	var elf2 = scores.AddLast(7);
	var arr = new List<int>() { 3, 7 };
	var numberOfRecipes = 110201;
	var count = 2;
	var limit = numberOfRecipes + 10;
	var next10 = string.Empty;
	var startAt = 0;
	//var match = new[] { 5, 1, 5, 8, 9 };
	//var match = new[] { 0, 1, 2, 4, 5 };
	//var match = new[] { 9, 2, 5, 1, 0 };
	//var match = new[] { 5, 9, 4, 1, 4 };
	var match = new[] { 1, 1, 0, 2, 0, 1 };
	var isMatch = false;
	while (isMatch == false)
	{
		var sum = elf1.Value + elf2.Value;
		foreach (var i in sum.ToString().Select(c => int.Parse(c.ToString())))
		{
			count++;
			scores.AddLast(i);
			arr.Add(i);
			if (count > numberOfRecipes && next10.Length < 10) next10 += i.ToString();
		}

		var movesForElf1 = elf1.Value + 1;
		for (var i = 0; i < movesForElf1; i++)
		{
			if (elf1.Next != null) elf1 = elf1.Next;
			else elf1 = scores.First;
		}

		var movesForElf2 = elf2.Value + 1;
		for (var i = 0; i < movesForElf2; i++)
		{
			if (elf2.Next != null) elf2 = elf2.Next;
			else elf2 = scores.First;
		}
		for (var i = startAt; i < arr.Count(); i++)
		{
			if (arr[i] == match[0])
			{
				isMatch = true;
				for (var j = 0; j < match.Length; j++)
				{
					if ((i + j) < arr.Count)
					{
						if ((arr[i + j] != match[j] && isMatch))
						{
							startAt = arr.Count > 10 ? arr.Count - 10 : 0;
							isMatch = false;
							break;
						}
					}
					else {
						startAt = arr.Count > 10 ? arr.Count - 10 : 0;
						isMatch = false;
						break;
					}

				}
				if (isMatch)
				{
					$"Next 10 recipes after the puzzle input: {next10}".Dump();
					$"Number of recipes before puzzle input: {i}".Dump();
					break;
				}
			}
		}
	}
}