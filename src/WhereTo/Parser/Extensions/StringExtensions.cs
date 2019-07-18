using System;
using System.Text.RegularExpressions;

namespace WhereTo.Parser.Extensions
{
	public static class StringExtensions
	{
		public static int FindFirstRegExGroup(this string input, string pattern)
		{
			try
			{
				var result = new Regex(pattern).Match(input).Groups[1];
				return result.Success ? result.Index : -1;
			}
			catch (Exception e)
			{
				return -1;
			}
		}
	}
}
