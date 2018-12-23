<Query Kind="Program" />

void Main()
{
	string line;
	List<string> ids = new List<string>();
	System.IO.StreamReader file = new System.IO.StreamReader(@"C:\Advent-of-Code -2018\Day 2\input.txt");
	while((line = file.ReadLine()) != null) 
	{
		ids.Add(line);
	}
	ids.Dump();
}

public class PartOne 
{
	public static int NumberOfIdsWithExactlyTwoLetters { get; set; }
	
	public static int NumberOfIdsWithExactlyThreeLetters { get; set; }
	
	public static int CheckSum()
	{
		return NumberOfIdsWithExactlyTwoLetters * NumberOfIdsWithExactlyThreeLetters; 
	}
}

public class PartTwo 
{
	
}