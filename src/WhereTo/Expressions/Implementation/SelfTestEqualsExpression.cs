namespace WhereTo.Expressions.Implementation
{
	/*
	 * Demo expression for testing purposes
	 * It wraps the keyword it is responsible for in hashes
	 */
	public class SelfTestEqualsExpression : IExpression
	{
		private readonly string _leftSide;
		private readonly string _rightSide;

		public SelfTestEqualsExpression(string leftSide, string rightSide)
		{
			_leftSide = leftSide;
			_rightSide = rightSide;
		}

		public string Evaluate()
		{
			return $"{_leftSide} #=# {_rightSide}";
		}
	}
}
