namespace WhereTo.Expressions.Implementation
{
	/*
	 * Demo expression for testing purposes
	 * It wraps the keyword it is responsible for in hashes
	 */
	public class SelfTestGroupExpression : IExpression
	{
		private readonly IExpression _content;

		public SelfTestGroupExpression(IExpression content)
		{
			_content = content;
		}

		public string Evaluate()
		{
			return $"#(#{_content.Evaluate()}#)#";
		}
	}
}
