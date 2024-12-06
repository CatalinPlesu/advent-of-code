using System;
using System.IO;
using System.Text.RegularExpressions;
using System.Collections;
using System.Threading;

namespace AdventOfCode
{
	public class Point
	{
		public int x;
		public int y;

		public Point(int x, int y)
		{
			this.x = x;
			this.y = y;
		}

		public override string ToString()
		{
			return $"({x}, {y})";
		}

		public override bool Equals(object obj)
		{
			if (obj is Point p)
			{
				return x == p.x && y == p.y;
			}
			return false;
		}
	}


	public enum Direction
	{
		Up,
		Down,
		Left,
		Right,
		None
	}

	public class Guard
	{
		public Point position;
		public Direction facing;

		public static Direction checkFacingDirectoin(char direction)
		{
			Direction facing;
			switch (direction)
			{
				case '^':
					facing = Direction.Up;
					break;
				case '>':
					facing = Direction.Right;
					break;
				case '<':
					facing = Direction.Left;
					break;
				case 'v':
					facing = Direction.Down;
					break;
				default:
					facing = Direction.None;
					break;
			}
			return facing;
		}

		public Guard(Point position, Direction direction)
		{
			this.position = position;
			this.facing = direction;
		}

		public Point Move()
		{
			Point p = new Point(this.position.x, this.position.y);
			switch (facing)
			{
				case Direction.Up:
					p.y -= 1;
					break;
				case Direction.Down:
					p.y += 1;
					break;
				case Direction.Left:
					p.x -= 1;
					break;
				case Direction.Right:
					p.x += 1;
					break;
				default:
					break;
			}
			return p;
		}

		public void TurnRight()
		{
			switch (facing)
			{
				case Direction.Up:
					facing = Direction.Right;
					break;
				case Direction.Down:
					facing = Direction.Left;
					break;
				case Direction.Left:
					facing = Direction.Up;
					break;
				case Direction.Right:
					facing = Direction.Down;
					break;
				default:
					break;
			}
		}

		public void Print()
		{
			Console.WriteLine($"Guard{position.ToString()}; Facing: {facing}");
		}
	}

	public class Map
	{
		private List<Point> obstructions = new List<Point>();
		private List<Point> check_ed = new List<Point>();
		private Guard guard;
		private int width;
		private int height;
		public List<List<char>> map = new List<List<char>>();

		public Map(string input)
		{
			string[] lines = input.Split(new string[] { "\n" },
							   StringSplitOptions.RemoveEmptyEntries);
			width = lines[0].Length;
			height = lines.Count();
			for (int i = 0; i < height; i++)
			{
				List<char> line = new List<char>();
				for (int j = 0; j < width; j++)
				{
					line.Add(lines[i][j]);
					if (lines[i][j] == '#')
					{
						obstructions.Add(new Point(j, i));
					}
					else
					{
						Direction direction = Guard.checkFacingDirectoin(lines[i][j]);
						if (direction != Direction.None)
						{
							guard = new Guard(new Point(j, i), direction);
							if (!check_ed.Contains(guard.position))
							{
								check_ed.Add(guard.position);
							}
						}
					}
				}
				map.Add(line);
			}

		}

		public void Simulate()
		{
			while (guard.position.x >= 0 && guard.position.x < width && guard.position.y >= 0 && guard.position.y < height)
			{
				Point p = guard.Move();
				if (obstructions.Contains(p))
				{
					guard.TurnRight();
					p = guard.Move();
				}
				guard.position = p;
				if (!check_ed.Contains(guard.position))
				{
					check_ed.Add(guard.position);
				}
				if (p.y >= 0 && p.y < map.Count())
				{
					if (p.x >= 0 && p.x < map[p.y].Count())
					{
						map[p.y][p.x] = 'O';
					}
				}
				// this.Print();
				if (p.y >= 0 && p.y < map.Count())
				{
					if (p.x >= 0 && p.x < map[p.y].Count())
					{
						map[p.y][p.x] = 'X';
					}
				}
			}

			Console.WriteLine($"Total distinct items: {check_ed.Count() - 1}");
		}

		public void Print()
		{
			guard.Print();
			Console.WriteLine($"Obstacles: {obstructions.Count()}");
			Console.WriteLine($"Checked: {check_ed.Count()}");
			foreach (var line in map)
			{
				foreach (var p in line)
				{
					Console.Write($"{p}");
				}
				Console.WriteLine();
			}
		}

	}

	class Program
	{
		static void Main()
		{
			// string filePath = "input0";
			string filePath = "input";

			string input = File.ReadAllText(filePath);

			Map map = new Map(input);
			// map.Print();
			map.Simulate();
		}
	}
}
