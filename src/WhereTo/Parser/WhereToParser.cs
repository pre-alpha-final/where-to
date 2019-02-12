using System;
using System.Collections.Generic;
using WhereTo.Expressions;
using WhereTo.Parser.Enums;
using WhereTo.Parser.Extensions;

namespace WhereTo.Parser
{
	public class WhereToParser
	{
		private readonly Dictionary<Keywords, string> _keywordRegExes = new Dictionary<Keywords, string>
		{
			{ Keywords.Equals, "(?:[^!])(=)" },
			{ Keywords.NotEquals, "(!=)" },
			{ Keywords.LessThan, "(<)" },
			{ Keywords.LessThanOrEqualTo, "(<=)" },
			{ Keywords.MoreThan, "(>)" },
			{ Keywords.MoreThanOrEqualTo, "(>=)" },
			{ Keywords.LeftBracket, "(\\()" },
			{ Keywords.RightBracket, "(\\))" },
			{ Keywords.SingleQuote, "(?:[^\\\\]|^)(')" },
		};
		private readonly Dictionary<Keywords, string> _joiningKeywordRegExes = new Dictionary<Keywords, string>
		{
			{ Keywords.And, "(and )" },
			{ Keywords.Or, "(or )" },
		};

		private string _context;
		private string _originalInput;
		private bool _allowJoinKeyword;

		public IExpression Parse(string input)
		{
			_originalInput = input;
			_context = input;
			var metaExpressions = GenerateMetaExpressions();
			return GenerateExpressions(metaExpressions);
		}

		private List<MetaExpression> GenerateMetaExpressions()
		{
			var metaExpressions = new List<MetaExpression>();
			while (true)
			{
				_context = _context.Trim();
				var keyword = FindNextKeyword();
				if (keyword.keyword == Keywords.None)
				{
					if (string.IsNullOrWhiteSpace(_context) == false)
					{
						throw new ArgumentException($"Cannot parse WhereTo query: '{_originalInput}'");
					}
					break;
				}

				switch (keyword.keyword)
				{
					case Keywords.None:
						break;

					case Keywords.Equals:
					case Keywords.NotEquals:
					case Keywords.LessThan:
					case Keywords.LessThanOrEqualTo:
					case Keywords.MoreThan:
					case Keywords.MoreThanOrEqualTo:
						_allowJoinKeyword = true;
						metaExpressions.Add(HandleOperators(keyword));
						break;

					case Keywords.LeftBracket:
						_allowJoinKeyword = false;
						_context = _context.Remove(0, "(".Length);
						metaExpressions.Add(new MetaExpression(Keywords.LeftBracket, "", ""));
						break;

					case Keywords.RightBracket:
						_allowJoinKeyword = true;
						_context = _context.Remove(0, ")".Length);
						metaExpressions.Add(new MetaExpression(Keywords.RightBracket, "", ""));
						break;

					case Keywords.And:
						_allowJoinKeyword = false;
						_context = _context.Remove(0, "and ".Length);
						metaExpressions.Add(new MetaExpression(Keywords.And, "", ""));
						break;

					case Keywords.Or:
						_allowJoinKeyword = false;
						_context = _context.Remove(0, "or ".Length);
						metaExpressions.Add(new MetaExpression(Keywords.Or, "", ""));
						break;

					case Keywords.SingleQuote:
						_context = _context.Remove(keyword.index, 1);
						break;
				}
			}

			return metaExpressions;
		}

		private IExpression GenerateExpressions(List<MetaExpression> metaExpressions)
		{
			var minimizing = false;
			var expressions = new List<IExpression>();
			try
			{
				do
				{
					foreach (var metaExpression in metaExpressions)
					{
						if (metaExpression.Keyword == Keywords.Equals)
						{

						}
					}
				} while (minimizing);
			}
			catch (Exception e)
			{
				throw new ArgumentException($"Cannot minimize WhereTo query: '{_originalInput}'");
			}
		}

		private MetaExpression HandleOperators((Keywords keyword, int index) keyword)
		{
			try
			{
				var firstPart = _context.Substring(0, keyword.index).Trim();
				if (firstPart.Contains(" "))
				{
					throw new ArgumentException($"Cannot parse WhereTo query: '{_originalInput}'");
				}
				var operatorSize = keyword.keyword == Keywords.Equals || keyword.keyword == Keywords.LessThan || keyword.keyword == Keywords.MoreThan ? 1 : 2;
				_context = _context.Remove(0, keyword.index + operatorSize).Trim();
				var nextKeyword = FindNextKeyword();
				int endIndex;
				if (nextKeyword.keyword == Keywords.SingleQuote)
				{
					if (nextKeyword.index != 0)
					{
						throw new ArgumentException($"Cannot parse WhereTo query: '{_originalInput}'");
					}
					endIndex = _context.IndexOf('\'', 1) + 1;
				}
				else
				{
					endIndex = nextKeyword.index != -1
						? nextKeyword.index
						: _context.Length;
				}
				var secondPart = _context.Substring(0, endIndex).Trim();
				_context = _context.Remove(0, endIndex);

				return new MetaExpression(keyword.keyword, firstPart, secondPart);
			}
			catch (Exception e)
			{
				throw new ArgumentException($"Cannot parse WhereTo query: '{_originalInput}'");
			}
		}

		private (Keywords keyword, int index) FindNextKeyword()
		{
			var foundAt = -1;
			var keyword = Keywords.None;

			foreach (var keywordRegEx in _keywordRegExes)
			{
				var index = _context.FindFirstRegExGroup(keywordRegEx.Value);
				if (index >= 0 && (foundAt == -1 || index < foundAt))
				{
					foundAt = index;
					keyword = keywordRegEx.Key;
				}
			}

			if (_allowJoinKeyword == false)
			{
				return (keyword, foundAt);
			}

			foreach (var keywordJoiningRegEx in _joiningKeywordRegExes)
			{
				var index = _context.FindFirstRegExGroup(keywordJoiningRegEx.Value);
				if (index >= 0 && (foundAt == -1 || index < foundAt))
				{
					foundAt = index;
					keyword = keywordJoiningRegEx.Key;
				}
			}

			return (keyword, foundAt);
		}
	}
}
