<Query Kind="Program" />

void Main()
{
	var file = new System.IO.StreamReader(@"C:\AoC2018\Day_13\input.txt");
	var tracks = new Dictionary<Coordinate, Track>(new CoordinateEqualityComparer());
	var carts = new List<Cart>();
	
	string line;
	int y = 0;
	while ((line = file.ReadLine()) != null) {		
		for (var x = 0; x < line.Length; x++)
		{
			var c = line[x];
			
			if (c == ' ' || c == '\n') continue;

			var coord = new Coordinate(x, y);
						
			if (Cart.IsCart(c))
			{
				var cart = new Cart(c, coord);
				carts.Add(cart);
				bool vertical = cart.Direction == Direction.Up || cart.Direction == Direction.Down;
				tracks[coord] = new Track(vertical ? '|' : '-', true);
			}
			else
			{
				tracks[coord] = new Track(c, false);
			}
		} 
		y++;
	}
		
	var tick = 0;
	var keepOnTruckin = true;
	while (keepOnTruckin)
	{
		carts = carts.OrderBy(c => c.Position.Y).ThenBy(c => c.Position.X).ToList();
		tick++;
		foreach (var cart in carts)
		{
			if (cart.TryMove(out var crashCoord, ref tracks)) continue;
			$"Cart {cart} collided with another cart at {crashCoord} at tick {tick}".Dump();
			keepOnTruckin = false; break;
		}
	}
}

public class Cart
{
	private const char _u = '^';
	private const char _d = 'v';
	private const char _l = '<';
	private const char _r = '>';
	
	public char Icon { get; set; }

	public Direction Direction { get; set; }
	
	public NextDirection NextDirection { get; set; }
	
	public Coordinate Position { get; set; }
	
	public Coordinate OriginalPosition { get; set;}

	public Cart(char icon, Coordinate position)
	{
		this.Position = position;
		this.OriginalPosition = position;
		this.Icon = icon;
		switch (icon)
		{
			case _u: this.Direction = Direction.Up; break;
			case _d: this.Direction = Direction.Down; break;
			case _l: this.Direction = Direction.Left; break;
			case _r: this.Direction = Direction.Right; break;
		}
		this.NextDirection = NextDirection.Left;
	}
	
	public bool TryMove(out Coordinate nextPosition, ref Dictionary<Coordinate, Track> track) {

		switch (this.Direction)
		{
			case Direction.Up: nextPosition = this.Position.Up(1); break;
			case Direction.Down: nextPosition = this.Position.Down(1); break;
			case Direction.Left: nextPosition = this.Position.ToTheLeft(1); break;
			case Direction.Right: nextPosition = this.Position.ToTheRight(1); break;
			default: nextPosition = null; break;
		}

		if (!track.ContainsKey(nextPosition))
		{
			throw new Exception($"You fucked up. {nextPosition}");
		}
		
		if (track[nextPosition].HasACart) return false;
		
		if (this.Direction == Direction.Up || this.Direction == Direction.Down)
		{
			switch (track[nextPosition].Type)
			{
				case TrackType.NEorSW: this.TurnLeft(); break;
				case TrackType.NWorSE: this.TurnRight(); break;
				case TrackType.Intersection: this.MakeIntersectionDecision(); break;
			}
		}
		
		else if (this.Direction == Direction.Left || this.Direction == Direction.Right)
		{
			switch (track[nextPosition].Type)
			{
				case TrackType.NEorSW: this.TurnRight(); break;
				case TrackType.NWorSE: this.TurnLeft(); break;
				case TrackType.Intersection: this.MakeIntersectionDecision(); break;
			}
		}
		
		track[this.Position].HasACart = false;
		track[nextPosition].HasACart = true;
		this.Position = nextPosition;
		return true;
	}
	
	public void MakeIntersectionDecision()
	{
		switch (this.NextDirection)
		{
			case NextDirection.Left: this.TurnLeft(); this.NextDirection = NextDirection.Straight; break;
			case NextDirection.Right: this.TurnRight(); this.NextDirection = NextDirection.Left; break;
			case NextDirection.Straight: this.NextDirection = NextDirection.Right; break;
		}
	}

	public void TurnLeft()
	{
		switch (this.Icon)
		{
			case _u: this.Icon = _l; this.Direction = Direction.Left; break;
			case _d: this.Icon = _r; this.Direction = Direction.Right; break;
			case _l: this.Icon = _d; this.Direction = Direction.Down; break;
			case _r: this.Icon = _u; this.Direction = Direction.Up; break;
		}
	}
	
	public void TurnRight()
	{
		switch (this.Icon)
		{
			case _u: this.Icon = _r; this.Direction = Direction.Right; break;
			case _d: this.Icon = _l; this.Direction = Direction.Left; break;
			case _l: this.Icon = _u; this.Direction = Direction.Up; break;
			case _r: this.Icon = _d; this.Direction = Direction.Down; break;
		}
	}
	
	public static bool IsCart(char c) {
		return (c == _u
			||  c == _d
			||  c == _l
			||  c == _r);;
	}

	public bool Equals(Cart other) => this.Position == other.Position;

	public override string ToString() => $"OGID: {this.OriginalPosition.ToString()}";

}

public class Track
{
	public TrackType Type { get; set; }
	
	public bool HasACart { get; set; }

	public Track(char line, bool hasACart)
	{
		this.HasACart = HasACart;
		switch (line) 
		{
			case '|': this.Type = TrackType.Vertical; break;
			case '-': this.Type = TrackType.Horizontal; break;
			case '/': this.Type = TrackType.NWorSE; break;
			case '\\': this.Type = TrackType.NEorSW; break;
			case '+': this.Type = TrackType.Intersection; break;
		}
	}
}

public class Coordinate
{
	public int X { get; }
	public int Y { get; }

	public Coordinate(int x, int y)
	{
		this.X = x;
		this.Y = y;
	}

	public Coordinate ToTheRight(int number) => new Coordinate(this.X + number, this.Y);

	public Coordinate ToTheLeft(int number) => new Coordinate(this.X - number, this.Y);

	public Coordinate Up(int number) => new Coordinate(this.X, this.Y - number);

	public Coordinate Down(int number) => new Coordinate(this.X, this.Y + number);

	public bool Equals(Coordinate other) => this.X == other.X && this.Y == other.Y;

	public override int GetHashCode() => this.ToString().GetHashCode();

	public override string ToString() => $"({this.X},{this.Y})";

}

public enum TrackType {
	Vertical, Horizontal, NWorSE, NEorSW, Intersection
}

public enum NextDirection {
	Left, Straight, Right 
}

public enum Direction {
	Up, Down, Left, Right
}

// for some reason, linq isn't using the cusom Equals & GetHashCode methods I wrote when comparing equality in Dictionary.ContainsKey
// so, I'm passing a custom equality comparer to the dictionary. Where did I go wrong? 
private class CoordinateEqualityComparer : IEqualityComparer<Coordinate>
{
	public bool Equals(Coordinate one, Coordinate two) => one.X == two.X && one.Y == two.Y;
	
	public int GetHashCode(Coordinate obj) => obj.ToString().GetHashCode();
}