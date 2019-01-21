<Query Kind="Program" />

void Main()
{
	
}

public class Cart
{
	private const string _u = "^";
	private const string _d = "v";
	private const string _l = "<";
	private const string _r = ">";
	
	public string Icon { get; set; }

	public Direction Direction { get; set; }
	
	public NextDirection NextDirection { get; set; }

	public Cart(string icon)
	{
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
	
	public NextDirection MakeIntersectionDecision()
	{
		var decision = this.NextDirection;
		switch (this.NextDirection)
		{
			case NextDirection.Left: this.TurnLeft(); this.NextDirection = NextDirection.Straight; break;
			case NextDirection.Straight: this.NextDirection = NextDirection.Right; break;
			case NextDirection.Right: this.TurnRight(); this.NextDirection = NextDirection.Left; break;
		}
		return decision;
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
}

public class Coordinate 
{
	public int X { get; }
	public int Y { get; }
	
	public Coordinate(int x, int y) {
		this.X = x;
		this.Y = y;
	}
	
	
}

public enum NextDirection {
	Left, Straight, Right 
}

public enum Direction {
	Up, Down, Left, Right
}

public enum TrackType {
	Vertical, Horizontal, LeftCurve, RightCurve
}