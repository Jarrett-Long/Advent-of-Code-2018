<Query Kind="Program" />

void Main()
{
	var instructions = new List<string>();
	var line = string.Empty;
	var file = new System.IO.StreamReader(@"C:\AoC2018\Day_07\input.txt");
	while ((line = file.ReadLine()) != null) instructions.Add(line);
	
	var test = new List<string>() 
	{
		"Step C must be finished before step A can begin.",
		"Step C must be finished before step F can begin.",
		"Step A must be finished before step B can begin.",
		"Step A must be finished before step D can begin.",
		"Step B must be finished before step E can begin.",
		"Step D must be finished before step E can begin.",
		"Step F must be finished before step E can begin."	
	};

	$"Test: {Solutions.PartOne(test)}".Dump();
	$"Order of steps: {Solutions.PartOne(instructions)}".Dump();
}

public static class Solutions 
{
	public static string PartOne(List<string> instructions)
	{
		var stepDependencies = new Dictionary<string, List<string>>();
		foreach (var instruction in instructions)
		{
			var dependency = instruction[5].ToString();
			var step = instruction[36].ToString();
			if (!stepDependencies.ContainsKey(step)) stepDependencies[step] = new List<string>();
			if (!stepDependencies.ContainsKey(dependency)) stepDependencies[dependency] = new List<string>();
			stepDependencies[step].Add(dependency);
		}

		var steps = stepDependencies.Keys.OrderBy(k => k).ToList();
		var order = new List<string>();
		while (steps.Count() > 0)
		{
			var step = steps.First(s => stepDependencies[s].All(d => order.Contains(d)));
			order.Add(step);
			steps.Remove(step);
		}
		return string.Join(string.Empty, order);
	}
}
