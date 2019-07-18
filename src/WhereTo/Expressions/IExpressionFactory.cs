namespace WhereTo.Expressions
{
	public interface IExpressionFactory
	{
		IExpression CreateEqualsExpression(string leftSide, string rightSide);
		IExpression CreateNotEqualsExpression(string leftSide, string rightSide);
		IExpression CreateLessThanExpression(string leftSide, string rightSide);
		IExpression CreateLessThanOrEqualToExpression(string leftSide, string rightSide);
		IExpression CreateMoreThanExpression(string leftSide, string rightSide);
		IExpression CreateMoreThanOrEqualToExpression(string leftSide, string rightSide);
		IExpression CreateAndExpression(IExpression leftSide, IExpression rightSide);
		IExpression CreateOrExpression(IExpression leftSide, IExpression rightSide);
		IExpression CreateGroupExpression(IExpression content);
	}
}
