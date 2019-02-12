namespace WhereTo.Expressions.Implementation
{
	/*
	 * Demo expression for testing purposes
	 * It wraps the keyword it is responsible for in hashes
	 */
	public class SelfTestAndExpression : IExpression
	{
		private readonly IExpression _leftSide;
		private readonly IExpression _rightSide;

		public SelfTestAndExpression(IExpression leftSide, IExpression rightSide)
		{
			_leftSide = leftSide;
			_rightSide = rightSide;
		}

		public string Evaluate()
		{
			return $"{_leftSide.Evaluate()} #and# {_rightSide.Evaluate()}";
		}
	}
}
