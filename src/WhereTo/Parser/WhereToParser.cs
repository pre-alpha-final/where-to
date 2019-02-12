using WhereTo.Expressions;
using WhereTo.Expressions.Implementation;

namespace WhereTo.Parser
{
	public class WhereToParser
	{
		public IExpression Parse(string input)
		{
			var metaExpressions = new MetaExpressionGenerator().Generate(input);
			return new ExpressionGenerator(new SelfTestExpressionFactory()).Generate(metaExpressions);
		}
	}
}
