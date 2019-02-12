using System;
using System.Collections.Generic;
using WhereTo.Expressions;
using WhereTo.Parser.Extensions;

namespace WhereTo.Parser
{
	public class WhereToParser
	{
		private readonly Dictionary<Keywords, string> _keywordRegExes = new Dictionary<Keywords, string>
		{
			{ Keywords.Equals, "([=]?)" }
		};

		public IExpression Parse(string input)
		{
			bool didParsing;
			var processedInput = input.Trim();
			var keyword = FindNextKeyword(processedInput);
			switch (keyword)
			{
				// take operator

				// take expression
			}

			throw new NotImplementedException();
		}

		private Keywords FindNextKeyword(string processedInput)
		{
			var foundAt = -1;
			var keyword = Keywords.None;

			foreach (var keywordRegEx in _keywordRegExes)
			{
				var index = processedInput.RegExFind(keywordRegEx.Value);
				if (index >= 0 && index < foundAt)
				{
					foundAt = index;
					keyword = keywordRegEx.Key;
				}
			}

			return keyword;
		}
	}
}
