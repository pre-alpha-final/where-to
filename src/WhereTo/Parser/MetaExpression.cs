using WhereTo.Expressions;
using WhereTo.Parser.Enums;

namespace WhereTo.Parser
{
	public class MetaExpression
	{
		public Keywords Keyword { get; set; }
		public string Argument1 { get; set; }
		public string Argument2 { get; set; }
		public IExpression ConcreteExpression { get; set; }

		public MetaExpression(Keywords keyword, string argument1, string argument2)
		{
			Keyword = keyword;
			Argument1 = argument1;
			Argument2 = argument2;
		}
	}
}
