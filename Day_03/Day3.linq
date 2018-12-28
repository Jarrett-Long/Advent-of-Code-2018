<Query Kind="Program" />

void Main()
{
	var ids = new List<string>();
	var line = string.Empty;
	var file = new System.IO.StreamReader(@"C:\AoC2018\Day_03\input.txt");
	while((line = file.ReadLine()) != null) ids.Add(line);
	
	$"Overlaps: {PartOne.SolveFor(ids)}".Dump();
	// $"Unique Claim (2 1/2 minutes): {PartTwo.SolveFor(ids)}".Dump(); 
	$"Unique Claim (0.663 seconds): {PartTwo.SolveFaster(ids)}".Dump();
}

public static class PartOne {

	public static int SolveFor(List<string> ids) => new Fabric(ids.Select(c => new Claim(c)), 1000).FindNumberOfOverlaps();

}

public static class PartTwo {

	public static Claim SolveFor(List<string> ids) => new Fabric(ids.Select(c=> new Claim(c)), 1000).FindUniqueClaim();
	
	public static int SolveFaster(List<string> ids) => new Fabric(ids.Select(c=> new Claim(c)), 1000).ClaimWithNoOverlaps();

}

public class Fabric {
	
	private int SquareSize;
	
	private List<Claim>[,] Square { get; set; }
	
	private Dictionary<int, HashSet<int>> TouchingClaims = new Dictionary<int, HashSet<int>>();
	
	public Fabric(IEnumerable<Claim> claims, int squareSize) {
	
		// initialize 2d array
		this.SquareSize = squareSize;
		
		this.Square = new List<Claim>[SquareSize, SquareSize];
		for (var i = 0; i < SquareSize; i++) { 
			for (var j = 0; j < SquareSize; j++) {
				this.Square[i,j] = new List<Claim>();
			}
		}
		
		foreach (var claim in claims) this.TouchingClaims[claim.ID] = new HashSet<int>();
		
		//this.Claims = claims.ToList();
		
		foreach (var claim in claims) PlaceClaim(claim);

	}
	
	private void PlaceClaim(Claim claim) { 
		
		var xStart = claim.LeftToLeft;
		var xEnd = claim.LeftToLeft + claim.Width;
		
		var yStart = SquareSize - claim.TopToTop;
		var yEnd = SquareSize - (claim.TopToTop + claim.Height);
		
		for (var x = xStart; x < xEnd; x++) {
			for (var y = yStart; y > yEnd; y--) {
				this.Square[x, y].Add(claim);
				foreach (var c in this.Square[x, y]) {
					this.TouchingClaims[c.ID].Add(claim.ID);
					this.TouchingClaims[claim.ID].Add(c.ID);
				}
			}
		}
	}
	
	public int FindNumberOfOverlaps() {
		var overlaps = 0;
		for (var x = 0; x < this.SquareSize; x++){
			for (var y = 0; y < this.SquareSize; y++){ 
				if (this.Square[x,y].Count() > 1) overlaps++;
			}
		}
		return overlaps;
	}
	
	public Claim FindUniqueClaim() { 
	
		 var claims = new List<Claim>();
		 for (var x = 0; x < this.SquareSize; x++){
		 	for (var y = 0; y < this.SquareSize; y++){ 
				if (this.Square[x,y].Count() == 1) claims.Add(this.Square[x,y].First());
			}
		}
		claims.GroupBy(c => c.ID);
		claims.Count.Dump();
		return claims.First(claim => claims.Count(c => c.ID == claim.ID) == claim.Width * claim.Height).Dump();
	}
	
	public int ClaimWithNoOverlaps() {
		return this.TouchingClaims.First(kvp => kvp.Value.Count() == 1).Key;
	}
	

}

public class Claim {
	
	public int ID { get; set; }
	
	// number of inches between the left of the fabric and left of the rectangle
	public int LeftToLeft { get; set; }
	
	// number of inches between the topof the fabric and the top of the rectangle
	public int TopToTop { get; set; }
	
	public int Width { get; set; }
	
	public int Height { get; set; }
	
	public Claim(string input) {

		this.ID = int.Parse(input.Slice(input.IndexOf('#') + 1, input.IndexOf('@')));
		this.LeftToLeft = int.Parse(input.Slice(input.IndexOf('@') + 2, input.IndexOf(',')));
		this.TopToTop = int.Parse(input.Slice(input.IndexOf(',') + 1, input.IndexOf(':')));
		this.Width = int.Parse(input.Slice(input.IndexOf(':') + 2, input.IndexOf('x')));
		this.Height = int.Parse(input.Slice(input.IndexOf('x') + 1, input.Length));
	
	}
	
	public override string ToString() {
		return string.Format("#{0} @ {1},{2}: {3}x{4}", this.ID, this.LeftToLeft, this.TopToTop, this.Width, this.Height);
	}

}



public static class Extensions
{
    public static string Slice(this string source, int start, int end)
    {
        if (end < 0) end = source.Length + end;
        int len = end - start;               
        return source.Substring(start, len);
    }
}