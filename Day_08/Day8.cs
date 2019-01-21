void Main()
{
	var line = string.Empty;
	var file = new System.IO.StreamReader(@"C:\AoC2018\Day_08\input.txt");
	var licence = file.ReadToEnd().Replace("\n", string.Empty);

	// $"Test: {Solutions.PartTwo("2 3 0 3 10 11 12 1 1 0 1 99 2 1 1 2")}".Dump();
	$"Sum of all metadata entries: {Solutions.PartOne(licence)}".Dump();
	$"Root node value: {Solutions.PartTwo(licence)}".Dump();
}

public static class Solutions
{
	public static int PartOne(string licence)
	{
		var tree = new Tree(licence.Split(' ').Select(i => int.Parse(i)).ToList());
		return tree.MetaDataSum;
	}
	
	public static int PartTwo(string licence)
	{
		var tree = new Tree(licence.Split(' ').Select(i => int.Parse(i)).ToList());
		return GetNodeValue(tree.Root);
	}
	
	public static int GetNodeValue(Node node)
	{
		if (node.ChildNodes.Count() == 0)
		{
			return node.MetadataEntries.Sum();
		}
		else
		{
			int value = 0;
			foreach (var index in node.MetadataEntries)
			{
				if (index <= node.ChildNodes.Count())
				{
					value += GetNodeValue(node.ChildNodes[index - 1]);
				}
			}
			return value;
		}
	}
}

public class Tree 
{
	public Node Root { get; set; }
	
	public List<int> License { get; set; }
	
	public int MetaDataSum = 0;

	public Tree(List<int> license) 
	{
		this.License = license;
		this.Root = InitializeNode();
		this.License = license;
	}
	
	public Node InitializeNode()
	{
		var node = new Node();
		var childNodes = License[0];
		var metadataEntries = License[1];
		this.License.RemoveRange(0, 2);
		for (var i = 0; i < childNodes; i++)
		{
			node.ChildNodes.Add(InitializeNode());
		}
		for (var i = 0; i < metadataEntries; i++)
		{
			var metadata = this.License.First();
			License.RemoveAt(0);
			node.MetadataEntries.Add(metadata);
			MetaDataSum += metadata;
		}
		return node;
	}
}

public class Node 
{
	public List<Node> ChildNodes = new List<Node>();
	
	public List<int> MetadataEntries = new List<int>();
}
