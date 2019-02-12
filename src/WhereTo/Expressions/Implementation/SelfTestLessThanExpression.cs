namespace WhereTo.Expressions.Implementation
{
	public class SelfTestLessThanExpression : IExpression
	{
		private readonly string _leftSide;
		private readonly string _rightSide;

		public SelfTestLessThanExpression(string leftSide, string rightSide)
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
