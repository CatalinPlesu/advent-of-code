using System;
using System.IO;
using System.Text.RegularExpressions;

string filePath = "input";

string input = File.ReadAllText(filePath);

string[] lines = input.Split(
	new string[] { Environment.NewLine },
	StringSplitOptions.None
);

// Aproach:
// Have a function that tries to discover if is a valid XMAS word in any direction
// Detect only if the letter is X

Console.WriteLine($"Lines: {lines.Count()}; Chars per line: {lines[0].Length}");

string word = "XMAS";
int detectXmasWord(int xRow, int xColumn)
{
	int detectedWords = 0;
	//   i  j   i  j   i  j
	// (-1 -1) (0 -1) (1 -1)
	// (-1  0) XXXXXX (1  0)
	// (-1  1) (0  1) (1  1)
	for (int i = -1; i <= 1; i++)
	{
		for (int j = -1; j <= 1; j++)
		{
			if (!(i == 0 && j == 0))
			{
				for (int l = 1; l <= 3; l++)
				{
					int checkColumn = xColumn + (i * l);
					int checkRow = xRow + (j * l);
					if (checkRow >= 0 && checkRow < lines.Count())
					{
						if (checkColumn >= 0 && checkColumn < lines[checkRow].Length)
						{
							if (lines[checkRow][checkColumn] == word[l])
								if (lines[checkRow][checkColumn] == word[l])
								{
									// so far so good
									if (l == 3)
									{
										detectedWords += 1;
									}
								} else { break; }
						}
						else { break; }
					}
					else { break; }
				}
			}
		}
	}
	return detectedWords;
}

int totalWords = 0;
for (int i = 0; i < lines.Count(); i++)
{
	for (int j = 0; j < lines[i].Length; j++)
	{
		if (lines[i][j] == 'X')
		{
			totalWords += detectXmasWord(i, j);
		}
	}
}
Console.WriteLine($"Total words detected: {totalWords}");
