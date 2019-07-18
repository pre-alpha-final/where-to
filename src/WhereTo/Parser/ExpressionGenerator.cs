using System;
using System.Collections.Generic;
using WhereTo.Expressions;
using WhereTo.Parser.Enums;

namespace WhereTo.Parser
{
	public class ExpressionGenerator
	{
		private readonly IExpressionFactory _expressionFactory;

		public ExpressionGenerator(IExpressionFactory expressionFactory)
		{
			_expressionFactory = expressionFactory;
		}

		public IExpression Generate(IList<MetaExpression> metaExpressions)
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
					throw new ArgumentException($"Cannot minimize WhereTo query");
				}

				return metaExpressions[0].ConcreteExpression;
			}
			catch (Exception e)
			{
				throw new ArgumentException($"Cannot minimize WhereTo query");
			}
		}
	}
}
