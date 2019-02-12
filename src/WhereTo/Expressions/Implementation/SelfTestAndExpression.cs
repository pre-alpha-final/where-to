namespace WhereTo.Expressions.Implementation
{
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
			throw new System.NotImplementedException();
		}
	}
}
