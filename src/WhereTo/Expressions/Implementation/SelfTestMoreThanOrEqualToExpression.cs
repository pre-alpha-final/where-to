namespace WhereTo.Expressions.Implementation
{
	/*
	 * Demo expression for testing purposes
	 * It wraps the keyword it is responsible for in hashes
	 * and the values in ampersands
	 */
	public class SelfTestMoreThanOrEqualToExpression : IExpression
	{
		private readonly string _leftSide;
		private readonly string _rightSide;

		public SelfTestMoreThanOrEqualToExpression(string leftSide, string rightSide)
		{
			_leftSide = leftSide;
			_rightSide = rightSide;
		}

		public string Evaluate()
		{
			return $"&{_leftSide}& #>=# &{_rightSide}&";
		}
	}
}
