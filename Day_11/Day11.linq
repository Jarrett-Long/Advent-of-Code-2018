<Query Kind="Program" />

void Main()
{
		$"{Solutions.Solve(3613, 300)}".Dump();
		//$"{Solutions.Solve(18, 300)}".Dump();
		//real solution: https://en.wikipedia.org/wiki/Summed-area_table	
}

public static class Solutions
{
	public static string Solve(int serialNumber, int squareSize)
	{
		var grid = new int[squareSize,squareSize];
		for (var x = 0; x < squareSize; x++){
			for (var y = 0; y < squareSize; y++){
				grid[x,y] = CalculatePowerLevel(x, y, serialNumber);
			}
		}
		int largestTotalPower = GetTotalPowerLevel(0, 0, 3, grid);
		var coordinate = "none";
		for (var x = 0; x < squareSize; x++){
			for (var y = 0; y < squareSize; y++){
				var xToEdge = squareSize - x;
				var yToEdge = squareSize - y;
				var maxSize = xToEdge > yToEdge ? yToEdge : xToEdge;
				for (var size = 1; size < maxSize; size++)
				{
					var sum = GetTotalPowerLevel(x, y, size, grid);
					if (sum > largestTotalPower)
					{
						largestTotalPower = sum;
						coordinate = $"{x},{y},{size}";
					}
				}
			}
		}
		return coordinate;
	}
	private static int GetTotalPowerLevel(int x, int y, int size, int[,] grid)
	{
		int sum = 0;
		for (var i = 0; i < size; i++)
		{
			for (var j = 0; j < size; j++)
			{
				sum += grid[x + i, y + j];
			}
		}
		return sum;
	}

	private static int CalculatePowerLevel(int x, int y, int serialNumber)
	{
		var powerLevel = 0;
		var rackId = x + 10;
		powerLevel = rackId * y;
		powerLevel = powerLevel + serialNumber;
		powerLevel = powerLevel * rackId;
		powerLevel = (int)Math.Abs(powerLevel/100%10);
		powerLevel = powerLevel - 5;
		return powerLevel;
	}
}

// Define other methods and classes here
