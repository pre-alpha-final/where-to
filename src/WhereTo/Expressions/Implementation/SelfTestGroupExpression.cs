namespace WhereTo.Expressions.Implementation
{
	public class SelfTestGroupExpression : IExpression
	{
		private readonly IExpression _content;

		public SelfTestGroupExpression(IExpression content)
		{
			_content = content;
		}

		public string Evaluate()
		{
			throw new System.NotImplementedException();
		}
	}
}
