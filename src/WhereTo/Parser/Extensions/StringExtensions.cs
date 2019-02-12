using System;
using System.Linq;
using System.Text.RegularExpressions;

namespace WhereTo.Parser.Extensions
{
	public static class StringExtensions
	{
		public static int RegExFind(this string input, string pattern)
		{
			try
			{
				return new Regex(pattern).Match(input).Groups.Skip(1).First().Index;
			}
			catch (Exception e)
			{
				return -1;
			}
		}
	}
}
