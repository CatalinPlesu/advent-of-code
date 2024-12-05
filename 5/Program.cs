using System;
using System.IO;
using System.Text.RegularExpressions;
using System.Collections;

namespace AdventOfCode
{
	public class Rules
	{
		public Dictionary<int, List<int>> rulesX = new Dictionary<int, List<int>>();
		public Dictionary<int, HashSet<int>> rulesY = new Dictionary<int, HashSet<int>>();
		public Rules(string input)
		{
			string pat = @"(\d*)\|(\d*)";

			Regex r = new Regex(pat, RegexOptions.IgnoreCase);
			Match m = r.Match(input);
			int sum = 0;
			while (m.Success)
			{
				if (rulesX.ContainsKey(Int32.Parse(m.Groups[1].Value)))
				{
					rulesX[Int32.Parse(m.Groups[1].Value)].Add(Int32.Parse(m.Groups[2].Value));
				}
				else
				{
					rulesX.Add(Int32.Parse(m.Groups[1].Value), [Int32.Parse(m.Groups[2].Value)]);
				}
				if (rulesY.ContainsKey(Int32.Parse(m.Groups[2].Value)))
				{
					rulesY[Int32.Parse(m.Groups[2].Value)].Add(Int32.Parse(m.Groups[1].Value));
				}
				else
				{
					rulesY.Add(Int32.Parse(m.Groups[2].Value), [Int32.Parse(m.Groups[1].Value)]);
				}
				m = m.NextMatch(); // Uncomment if you'd like to process more matches
			}
		}

		public void Print()
		{
			Console.WriteLine($"Dictionary of rules X");
			foreach (var item in rulesX)
			{
				Console.Write("{0} > ", item.Key);
				foreach (var Y in item.Value)
				{
					Console.Write($"{Y}; ");
				}
				Console.WriteLine();
			}

			Console.WriteLine($"Dictionary of rules Y");
			foreach (var item in rulesY)
			{
				Console.Write("{0} < ", item.Key);
				foreach (var X in item.Value)
				{
					Console.Write($"{X}; ");
				}
				Console.WriteLine();
			}
		}

		public bool validateBatch(List<int> batch)
		{
			List<int> redFlags = new List<int>();
			foreach (var page in batch)
			{
				if (redFlags.Contains(page))
				{
					return false;
				}
				else
				{
					if (rulesY.ContainsKey(page))
					{
						redFlags.AddRange(rulesY[page].ToList());
					}
				}
			}
			return true;
		}

		public void orderIncorectBatch(ref List<int> batch)
		{
			List<int> redFlags = new List<int>();
			foreach (var page in batch)
			{
				if (redFlags.Contains(page))
				{
				}
				else
				{
					if (rulesY.ContainsKey(page))
					{
						redFlags.AddRange(rulesY[page].ToList());
					}
				}
			}
			int n = batch.Count();
			for (int i = 0; i < n - 1; i++)
				for (int j = 0; j < n - i - 1; j++)
					if (rulesY.ContainsKey(batch[j]))
					{
						if (rulesY[batch[j]].Contains(batch[j + 1]))
						{
							var tempVar = batch[j];
							batch[j] = batch[j + 1];
							batch[j + 1] = tempVar;
						}
					}
		}
	}

	public class Updates
	{
		public List<List<int>> updates = new List<List<int>>();
		private Rules rules;

		public Updates(string input, Rules rules)
		{
			this.rules = rules;

			string[] lines = input.Split(new string[] { "\n" },
							   StringSplitOptions.RemoveEmptyEntries);

			foreach (var line in lines)
			{
				string[] updatesStr = line.Split(new string[] { "," },
								   StringSplitOptions.RemoveEmptyEntries);
				List<int> updateBatch = new List<int>();
				foreach (var update in updatesStr)
				{
					updateBatch.Add(Int32.Parse(update));
				}
				updates.Add(updateBatch);
			}

		}
		public void Print()
		{
			Console.WriteLine("Update batches:");
			foreach (var batch in updates)
			{
				foreach (var page in batch)
				{
					Console.Write($"{page}, ");
				}
				Console.WriteLine();
			}
		}
		public int middlePageSum()
		{
			int sum = 0;
			foreach (var batch in updates)
			{
				if (rules.validateBatch(batch))
				{
					sum += batch[batch.Count() / 2];
				}
			}
			return sum;
		}

		public int correctedMiddlePageSum()
		{
			int sum = 0;
			foreach (var batch in updates)
			{
				if (!rules.validateBatch(batch))
				{
					var correctedBatch = batch;
					rules.orderIncorectBatch(ref correctedBatch);
					sum += correctedBatch[correctedBatch.Count() / 2];
				}
			}
			return sum;
		}
	}

	class Program
	{
		static void Main()
		{
			string filePath = "input";

			string input = File.ReadAllText(filePath);

			string[] lines = input.Split(new string[] { "\n\n" }, // on windows might be \r\n\r\n
							   StringSplitOptions.RemoveEmptyEntries);

			Rules rules = new Rules(lines[0]);
			// rules.Print();

			Updates updates = new Updates(lines[1], rules);
			// updates.Print();
			Console.WriteLine($"Sum: {updates.correctedMiddlePageSum()}");
		}
	}
}
