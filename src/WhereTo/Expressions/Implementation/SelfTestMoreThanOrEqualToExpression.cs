namespace WhereTo.Expressions.Implementation
{
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
			throw new System.NotImplementedException();
		}
	}
}
