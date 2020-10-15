using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.Json;

namespace Assignment
{
	class Program
	{
		static void Main(string[] args)
		{
			//string filePath = @"..\..\..\input\";//visual studio
			string filePath = @".\input\";//visual studio code
			string fileName = "test3.txt";

			int n, m;
			int[][] data, cal, assignment;
			Pair[][] trace;

			StreamReader file = new StreamReader(Path.Combine(filePath, fileName));
			string[] temp = file.ReadLine().Split(' ');
			n = int.Parse(temp[0]);
			m = int.Parse(temp[1]);

			data = new int[m+1][];
			cal = new int[m+1][];
			assignment = new int[m+1][];
			trace = new Pair[m+1][];
			trace[0] = Enumerable.Repeat(new Pair(0, 0), n+1).ToArray();

			data[0] = Enumerable.Repeat(0, n + 1).ToArray();
			cal[0] = Enumerable.Repeat(int.MaxValue, n + 1).ToArray();
			cal[0][0] = 0;

			for(int i = 1; i <= m; i++)
			{
				string row = file.ReadLine();
				data[i] = Array.ConvertAll<string, int>(row.Split(' '), s => int.Parse(s));
				data[i] = data[i].Reverse().ToArray();
				Array.Resize(ref data[i], n + 1);
				data[i] = data[i].Reverse().ToArray();

				cal[i] = new int[n + 1];
				trace[i] = new Pair[n+1];
				trace[i][0] = new Pair(0,0);
				assignment[i] = new int[n+1];

				for(int j = 1; j <= n; j++)
				{
					//int t1 = Math.Min(Math.Max(cal[i-1][j-1], data[i][j]), cal[i-1][j]);
					int t1 = Math.Max(cal[i-1][j-1], data[i][j]);
					int assignment1Temp;
					Pair trace1Temp = new Pair(i-1, j-1);
					
					if(cal[i-1][j] < t1)
					{
						t1 = cal[i-1][j];
						assignment1Temp = i - 1;
					}
					else assignment1Temp = i;
					
					int t2 = cal[i][j-1] + data[i][j];
					int assignment2Temp = i;
					Pair trace2Temp = new Pair(i, j-1);
					for(int z = 1; z < i; z++)
					{
						//t2 = Math.Min(t2, cal[i][j-1] + data[z][j]);
						int t2Temp = cal[i][j-1] + data[z][j];
						if(t2Temp < t2)
						{
							t2 = t2Temp;
							assignment2Temp = z;
						}
					}

					//cal[i][j] = Math.Min(t1, t2);
					if(t1 < t2)
					{
						cal[i][j] = t1;
						assignment[i][j] = assignment1Temp;
						trace[i][j] = trace1Temp;
					}
					else
					{
						cal[i][j] = t2;
						assignment[i][j] = assignment2Temp;
						trace[i][j] = trace2Temp;
					}
				}
			}
			file.Close();
			
			Stack<int> traceBack = new Stack<int>();
			Pair position = new Pair(m, n);
			int[] time = Enumerable.Repeat(0, m+1).ToArray();
			while(position.X != 0 && position.Y != 0)
			{
				traceBack.Push(assignment[position.X][position.Y]);
				time[assignment[position.X][position.Y]] += data[assignment[position.X][position.Y]][position.Y];
				position = trace[position.X][position.Y];
			}
			while(traceBack.Any())
			{
				Console.Write($"{traceBack.Pop()} ");
			}
			Console.WriteLine();
			Console.WriteLine($"Time to finish: {time.Max()}");
		}
	}
}
//t1 = min( max( cal[i-1][j-1] , data[i][j] ) , cal[i-1][j] )
//t2 = min( cal[i][j-1] + data[i][j] , cal[i][j-1] + data[i-1][j] )
//cal[i][j] = min( t1 , t2 )