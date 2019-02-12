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
		private readonly Dictionary<Keywords, string> _joinKeywordRegExes = new Dictionary<Keywords, string>
		{
			{ Keywords.And, "(and )" },
			{ Keywords.Or, "(or )" },
		};

		private readonly IExpressionFactory _expressionFactory;
		private string _context;
		private string _originalInput;
		private bool _allowJoinKeywords;

		public WhereToParser(IExpressionFactory expressionFactory)
		{
			_expressionFactory = expressionFactory;
		}

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
						_allowJoinKeywords = true;
						metaExpressions.Add(HandleOperators(keyword));
						break;

					case Keywords.LeftBracket:
						_allowJoinKeywords = false;
						_context = _context.Remove(0, "(".Length);
						metaExpressions.Add(new MetaExpression(Keywords.LeftBracket, "", ""));
						break;

					case Keywords.RightBracket:
						_allowJoinKeywords = true;
						_context = _context.Remove(0, ")".Length);
						metaExpressions.Add(new MetaExpression(Keywords.RightBracket, "", ""));
						break;

					case Keywords.And:
						_allowJoinKeywords = false;
						_context = _context.Remove(0, "and ".Length);
						metaExpressions.Add(new MetaExpression(Keywords.And, "", ""));
						break;

					case Keywords.Or:
						_allowJoinKeywords = false;
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

		private IExpression GenerateExpressions(IList<MetaExpression> metaExpressions)
		{
			try
			{
				bool minimizing;
				do
				{
					minimizing = false;
					for (var i = 0; i < metaExpressions.Count; i++)
					{
						if (metaExpressions[i].ConcreteExpression != null)
						{
							continue;
						}

						if (metaExpressions[i].Keyword == Keywords.Equals)
						{
							metaExpressions[i].ConcreteExpression =
								_expressionFactory.CreateEqualsExpression(
									metaExpressions[i].Argument1, metaExpressions[i].Argument2);
							minimizing = true;
						}

						if (metaExpressions[i].Keyword == Keywords.NotEquals)
						{
							metaExpressions[i].ConcreteExpression =
								_expressionFactory.CreateNotEqualsExpression(
									metaExpressions[i].Argument1, metaExpressions[i].Argument2);
							minimizing = true;
						}

						if (metaExpressions[i].Keyword == Keywords.LessThan)
						{
							metaExpressions[i].ConcreteExpression =
								_expressionFactory.CreateLessThanExpression(
									metaExpressions[i].Argument1, metaExpressions[i].Argument2);
							minimizing = true;
						}

						if (metaExpressions[i].Keyword == Keywords.LessThanOrEqualTo)
						{
							metaExpressions[i].ConcreteExpression =
								_expressionFactory.CreateLessThanOrEqualToExpression(
									metaExpressions[i].Argument1, metaExpressions[i].Argument2);
							minimizing = true;
						}

						if (metaExpressions[i].Keyword == Keywords.MoreThan)
						{
							metaExpressions[i].ConcreteExpression =
								_expressionFactory.CreateMoreThanExpression(
									metaExpressions[i].Argument1, metaExpressions[i].Argument2);
							minimizing = true;
						}

						if (metaExpressions[i].Keyword == Keywords.MoreThanOrEqualTo)
						{
							metaExpressions[i].ConcreteExpression =
								_expressionFactory.CreateMoreThanOrEqualToExpression(
									metaExpressions[i].Argument1, metaExpressions[i].Argument2);
							minimizing = true;
						}

						if (metaExpressions[i].Keyword == Keywords.And &&
							metaExpressions[i - 1].ConcreteExpression != null &&
							metaExpressions[i + 1].ConcreteExpression != null)
						{
							metaExpressions[i].ConcreteExpression =
								_expressionFactory.CreateAndExpression(
									metaExpressions[i - 1].ConcreteExpression,
									metaExpressions[i + 1].ConcreteExpression);
							metaExpressions.RemoveAt(i + 1);
							metaExpressions.RemoveAt(i - 1);
							minimizing = true;
							break;
						}

						if (metaExpressions[i].Keyword == Keywords.Or &&
							metaExpressions[i - 1].ConcreteExpression != null &&
							metaExpressions[i + 1].ConcreteExpression != null)
						{
							metaExpressions[i].ConcreteExpression =
								_expressionFactory.CreateOrExpression(
									metaExpressions[i - 1].ConcreteExpression,
									metaExpressions[i + 1].ConcreteExpression);
							metaExpressions.RemoveAt(i + 1);
							metaExpressions.RemoveAt(i - 1);
							minimizing = true;
							break;
						}

						if (metaExpressions[i].Keyword == Keywords.LeftBracket &&
							metaExpressions[i + 1].ConcreteExpression != null &&
							metaExpressions[i + 2].Keyword == Keywords.RightBracket)
						{
							metaExpressions[i].ConcreteExpression =
								_expressionFactory.CreateGroupExpression(
									metaExpressions[i + 1].ConcreteExpression);
							metaExpressions.RemoveAt(i + 2);
							metaExpressions.RemoveAt(i + 1);
							minimizing = true;
							break;
						}
					}
				} while (minimizing);

				if (metaExpressions.Count > 1)
				{
					throw new ArgumentException($"Cannot minimize WhereTo query: '{_originalInput}'");
				}

				return metaExpressions[0].ConcreteExpression;
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
				// Left side of operator
				var leftSide = _context.Substring(0, keyword.index).Trim();
				if (leftSide.Contains(" "))
				{
					throw new ArgumentException($"Cannot parse WhereTo query: '{_originalInput}'");
				}

				// Remove operator
				var operatorSize = keyword.keyword == Keywords.Equals || keyword.keyword == Keywords.LessThan || keyword.keyword == Keywords.MoreThan ? 1 : 2;
				_context = _context.Remove(0, keyword.index + operatorSize).Trim();

				// Right side of operator
				var nextKeyword = FindNextKeyword();
				int endIndex;
				if (nextKeyword.keyword == Keywords.SingleQuote)
				{
					if (nextKeyword.index != 0)
					{
						throw new ArgumentException($"Cannot parse WhereTo query: '{_originalInput}'");
					}
					endIndex = _context
						.Substring(1, _context.Length - 1)
						.FindFirstRegExGroup(_keywordRegExes[Keywords.SingleQuote]) + 1;
				}
				else
				{
					endIndex = nextKeyword.index != -1
						? nextKeyword.index
						: _context.Length;
				}
				var rightSide = _context.Substring(0, endIndex).Trim();
				_context = _context.Remove(0, endIndex);

				return new MetaExpression(keyword.keyword, leftSide, rightSide);
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

			if (_allowJoinKeywords == false)
			{
				return (keyword, foundAt);
			}

			foreach (var joinKeywordRegEx in _joinKeywordRegExes)
			{
				var index = _context.FindFirstRegExGroup(joinKeywordRegEx.Value);
				if (index >= 0 && (foundAt == -1 || index < foundAt))
				{
					foundAt = index;
					keyword = joinKeywordRegEx.Key;
				}
			}

			return (keyword, foundAt);
		}
	}
}
