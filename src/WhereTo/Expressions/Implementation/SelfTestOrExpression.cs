namespace WhereTo.Expressions.Implementation
{
	/*
	 * Demo expression for testing purposes
	 * It wraps the keyword it is responsible for in hashes
	 */
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
			return $"{_leftSide.Evaluate()} #or# {_rightSide.Evaluate()}";
		}
	}
}
