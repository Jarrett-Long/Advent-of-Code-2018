<Query Kind="Program" />

void Main()
{
	$"10 players; last marble is worth 1618 points: high score is {Solutions.PlayGame(10, 1618)}".Dump();
	$"13 players; last marble is worth 7999 points: high score is: {Solutions.PlayGame(13, 7999)}".Dump();
	$"17 players; last marble is worth 1104 points: high score is: {Solutions.PlayGame(17, 1104)}".Dump(); // this is the only one that doesn't match??! whyy!
	$"21 players; last marble is worth 6111 points: high score is: {Solutions.PlayGame(21, 6111)}".Dump();
	$"30 players; last marble is worth 5807 points: high score is: {Solutions.PlayGame(30, 5807)}".Dump();
	$"465 players; last marble is worth 71940 points high score is: {Solutions.PlayGame(465, 71940)}".Dump();
	$"465 players; last marble is worth 7194000 points high score is: {Solutions.PlayGame(465, 7194000)}".Dump();
}

public static class Solutions
{
	public static ulong PlayGame(int playerCount, ulong lastMarblePoints, int bonusMultiplier = 1)
	{
		var players = new List<Player>();
		for (var p = 1; p <= playerCount; p++) players.Add(new Player(p));

		var circle = new LinkedList<ulong>();
		circle.AddFirst(0);
		
		var current = circle.First;
		ulong nextMarble = 1;

		while (nextMarble <= lastMarblePoints)
		{
			foreach (var player in players)
			{
				if (nextMarble % 23 == 0)
				{
					player.Score += nextMarble;
					var pointer = current;
					var count = 7;
					while (count > 0)
					{
						if (current.Previous != null) current = current.Previous; 
						else current = circle.Last;
						count--;
					}
					player.Score += current.Value;
					if (current.Next != null) { current = current.Next; circle.Remove(current.Previous); }
					else { current = circle.First; circle.RemoveLast(); }
				}
				else
				{
					if (current.Next != null) { circle.AddAfter(current.Next, nextMarble); current = current.Next.Next; }
					else { circle.AddAfter(circle.First, nextMarble); current = circle.First.Next; }
				}
				nextMarble++;
				if (nextMarble >= lastMarblePoints) break;
			}
		}

		return players.Max(p => p.Score);
	}
}

public class Player
{
	public int ID { get; set; }
	public ulong Score { get; set; }

	public Player(int id)
	{
		this.ID = id;
	}
	public override string ToString()
	{
		return $"Player #{ID} | Score: {Score}";
	}
}