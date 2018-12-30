<Query Kind="Program" />

void Main()
{
	var instructions = new List<string>();
	var line = string.Empty;
	var file = new System.IO.StreamReader(@"C:\AoC2018\Day_07\input.txt");
	while ((line = file.ReadLine()) != null) instructions.Add(line);
	
	$"Order of steps: {Solutions.PartOne(instructions)}".Dump();
	$"Time it takes for 5 workers: {Solutions.PartTwo(instructions)}".Dump();
}

public static class Solutions
{
	public static string PartOne(List<string> instructions)
	{
		var stepDependencies = GetDependencyDictionary(instructions);
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

	public static int PartTwo(List<string> instructions)
	{
		var stepDependencies = GetDependencyDictionary(instructions);
		var steps = stepDependencies.Keys.OrderBy(k => k).ToList();
		var workers = new Worker[] { new Worker(), new Worker(), new Worker(), new Worker(), new Worker() };
		var order = new List<string>();
		var stepCount = steps.Count();
		var timer = -1;
		while (order.Count() != stepCount) 
		{
			foreach (var w in workers)
			{
				w.Work();
				if (!w.ReadyForNextStep) continue;
				if (w.CurrentStep != null) { order.Add(w.CurrentStep); w.CurrentStep = null; }
				var nextStep = steps.FirstOrDefault(s => stepDependencies[s].All(d => order.Contains(d)));
				if (nextStep != null) { w.WorkOnStep(nextStep); steps.Remove(nextStep); }
			}
			timer++;
		}
		
		return timer;
	}

	public static Dictionary<string, List<string>> GetDependencyDictionary(List<string> instructions)
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
		return stepDependencies;
	}
}

public class Worker 
{
	public string CurrentStep { get; set; }
	
	public int TimeRemaining { get; set; }
	
	public bool ReadyForNextStep => TimeRemaining <= 0;
	
	public void WorkOnStep(string step) {
		this.CurrentStep = step;
		this.TimeRemaining = char.ToUpper(step[0]) - 4;
	}
	
	public void Work() {
		if (TimeRemaining > 0) TimeRemaining--;
	}
	
}
