using System;
using System.IO;
using System.Text.RegularExpressions;

int part = 2;

string filePath = "input";

string input = File.ReadAllText(filePath);

if (part == 1)
{
	string pat = @"mul\(([0-9]{1,3}),([0-9]{1,3})\)";

	Regex r = new Regex(pat, RegexOptions.IgnoreCase);
	Match m = r.Match(input);
	int matchCount = 0;
	int sum = 0;
	while (m.Success)
	{
		Console.WriteLine((++matchCount) + ". Match");
		Console.WriteLine("Groups Count: " + m.Groups.Count);
		Console.WriteLine("Groups: " + m.Groups[0].Value);
		sum += Int32.Parse(m.Groups[1].Value) * Int32.Parse(m.Groups[2].Value);
		m = m.NextMatch();
	}
	Console.WriteLine($"Sum of products: {sum}");

}
else if (part == 2)
{

	string pat = @"do\(\)|don\'t\(\)|mul\(([0-9]{1,3}),([0-9]{1,3})\)";

	Regex r = new Regex(pat, RegexOptions.IgnoreCase);
	Match m = r.Match(input);
	int matchCount = 0;
	int sum = 0;
	bool enable = true;
	while (m.Success)
	{
		Console.WriteLine((++matchCount) + ". Match");
		Console.WriteLine("Groups Count: " + m.Groups.Count);
		Console.WriteLine("Groups: " + m.Groups[0].Value);
		if (m.Groups[0].Value.Contains("do()"))
		{
			enable = true;
		}
		else if (m.Groups[0].Value.Contains("don't()"))
		{
			enable = false;
		}
		else if (enable && m.Groups[0].Value.Contains("mul"))
		{
			sum += Int32.Parse(m.Groups[1].Value) * Int32.Parse(m.Groups[2].Value);
		}
		m = m.NextMatch();
	}
	Console.WriteLine($"Sum of products: {sum}");

}
