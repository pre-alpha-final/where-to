namespace WhereTo.Expressions.Implementation
{
	public class SelfTestExpressionFactory : IExpressionFactory
	{
		public IExpression CreateEqualsExpression(string leftSide, string rightSide)
		{
			return new SelfTestEqualsExpression(leftSide, rightSide);
		}

		public IExpression CreateNotEqualsExpression(string leftSide, string rightSide)
		{
			return new SelfTestNotEqualsExpression(leftSide, rightSide);
		}

		public IExpression CreateLessThanExpression(string leftSide, string rightSide)
		{
			return new SelfTestLessThanExpression(leftSide, rightSide);
		}

		public IExpression CreateLessThanOrEqualToExpression(string leftSide, string rightSide)
		{
			return new SelfTestLessThanOrEqualToExpression(leftSide, rightSide);
		}

		public IExpression CreateMoreThanExpression(string leftSide, string rightSide)
		{
			return new SelfTestMoreThanExpression(leftSide, rightSide);
		}

		public IExpression CreateMoreThanOrEqualToExpression(string leftSide, string rightSide)
		{
			return new SelfTestMoreThanOrEqualToExpression(leftSide, rightSide);
		}

		public IExpression CreateAndExpression()
		{
			return new SelfTestAndExpression();
		}

		public IExpression CreateOrExpression()
		{
			return new SelfTestOrExpression();
		}

		public IExpression CreateGroupExpression()
		{
			return new SelfTestGroupExpression();
		}
	}
}
