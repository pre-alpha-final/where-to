namespace WhereTo.Expressions.Implementation
{
	public class SelfTestOrExpression : IExpression
	{
		private readonly IExpression _leftSide;
		private readonly IExpression _rightSide;

		public SelfTestOrExpression(IExpression leftSide, IExpression rightSide)
		{
			_leftSide = leftSide;
			_rightSide = rightSide;
		}

		public string Evaluate()
		{
			throw new System.NotImplementedException();
		}
	}
}
