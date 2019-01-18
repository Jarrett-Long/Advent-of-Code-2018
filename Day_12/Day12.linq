<Query Kind="Program" />

void Main()
{
	var initialState = "##.####..####...#.####..##.#..##..#####.##.#..#...#.###.###....####.###...##..#...##.#.#...##.##..";
	var state = new Dictionary<long, char>();
	for (var i = 0; i < initialState.Length; i++) state[i] = initialState[i];

	var notes = new List<string>() { "##.## => #", "....# => .", ".#.#. => #", "..### => .", "##... => #", "##### => .", "###.# => #", ".##.. => .", "..##. => .", "...## => #", "####. => .", "###.. => .", ".#### => #", "#...# => #", "..... => .", "..#.. => .", "#..## => .", "#.#.# => #", ".#.## => #", ".###. => .", "##..# => .", ".#... => #", ".#..# => #", "...#. => .", "#.#.. => .", "#.... => .", "##.#. => .", "#.### => .", ".##.# => .", "#..#. => #", "..#.# => .", "#.##. => #" };
	
	var min = state.First().Key - 10;
	var max = state.Last().Key + 10;
	long lastSum = 0;
	long thisSum = 0;

	for (long gen = 0; gen < 150; gen++)  // 50000000000 would take roughly 52 years for this algorithm to complete.
	{
		var potsToUpdate = new List<Tuple<long, char, string>>();
		
		var start = min;
		var end = max;

		for (var i = start; i < end; i++)
		{			
			if (!state.TryGetValue(i - 1, out char leftOne)) { state[i - 1] = '.'; leftOne = '.'; }
			if (!state.TryGetValue(i - 2, out char leftTwo)) { state[i - 2] = '.'; leftTwo = '.'; min = i - 2; }
			if (!state.TryGetValue(i, out char center)) { state[i] = '.'; center = '.'; }
			if (!state.TryGetValue(i + 1, out char rightOne)) { state[i + 1] = '.'; rightOne = '.'; }
			if (!state.TryGetValue(i + 2, out char rightTwo)) { state[i + 2] = '.'; rightTwo = '.'; max = i + 2; }
									
			foreach(var note in notes)
			{
				if (leftTwo == note[0]
				 && leftOne == note[1]
				 && center == note[2]
				 && rightOne == note[3]
				 && rightTwo == note[4]) 
				 potsToUpdate.Add(new Tuple<long, char, string>(i, note[9], note));
			}
		}
		thisSum = 0;
		foreach (var kvp in state) if (kvp.Value == '#') thisSum += kvp.Key;
		$"Gen: {gen}   Sum: {thisSum}  Difference: {thisSum - lastSum}".Dump();
		lastSum = thisSum;
		
		var newState = new Dictionary<long, char>();
		foreach (var potToUpdate in potsToUpdate) newState[potToUpdate.Item1] = potToUpdate.Item2;
		state = newState;
	}

	$"Sum after 50 billion gens: {4977 + (32 * (50000000000 - 143))}".Dump();
}