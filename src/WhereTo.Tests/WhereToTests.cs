using System;
using WhereTo.Expressions.Implementation;
using WhereTo.Parser;
using Xunit;

namespace WhereTo.Tests
{
	public class WhereToTests
	{
		[Theory]
		[InlineData(@"a=1", @"&a& #=# &1&")]
		[InlineData(@"a!=1", @"&a& #!=# &1&")]
		[InlineData(@"a<1", @"&a& #<# &1&")]
		[InlineData(@"a<=1", @"&a& #<=# &1&")]
		[InlineData(@"a>1", @"&a& #># &1&")]
		[InlineData(@"a>=1", @"&a& #>=# &1&")]
		[InlineData(@"a=1.1 or a='foo\'bar'", @"&a& #=# &1.1& #or# &a& #=# &'foo\'bar'&")]
		[InlineData(@"a='b' or (a<1 and c!=true)", @"&a& #=# &'b'& #or# #(#&a& #<# &1& #and# &c& #!=# &true&#)#")]
		[InlineData(@"and=1", @"&and& #=# &1&")]
		[InlineData(@"and='foo'", @"&and& #=# &'foo'&")]
		[InlineData(@"and=true", @"&and& #=# &true&")]
		[InlineData(@"'and'>1", @"&and& #># &1&")]
		[InlineData(@"'and'<'1'", @"&and& #<# &'1'&")]
		[InlineData(@"'and' = '1'", @"&and& #=# &'1'&")]
		[InlineData(@" 'and' = 1", @"&and& #=# &1&")]
		[InlineData(@" 'and' = '1'", @"&and& #=# &'1'&")]
		[InlineData(@" and <= 1", @"&and& #<=# &1&")]
		[InlineData(@"a=1.1 or b=1.1", @"&a& #=# &1.1& #or# &b& #=# &1.1&")]
		[InlineData(@"(a=1 and b=1)", @"#(#&a& #=# &1& #and# &b& #=# &1&#)#")]
		[InlineData(@"and = 'and' or and != 'and'", @"&and& #=# &'and'& #or# &and& #!=# &'and'&")]
		[InlineData(@"a = true", @"&a& #=# &true&")]
		[InlineData(@"true = false", @"&true& #=# &false&")]
		[InlineData(@"true = false or false = true", @"&true& #=# &false& #or# &false& #=# &true&")]
		[InlineData(@"a = 1 or and = 1", @"&a& #=# &1& #or# &and& #=# &1&")]
		[InlineData(@"a = 1 or  and = 1", @"&a& #=# &1& #or# &and& #=# &1&")]
		[InlineData(@"a='foo\'bar'", @"&a& #=# &'foo\'bar'&")]
		[InlineData(@"a='foo\""bar'", @"&a& #=# &'foo\""bar'&")]
		[InlineData(
			@"a = 'b' or ( a > 1 and c != true ) and ( d = 1 )",
			@"&a& #=# &'b'& #or# #(#&a& #># &1& #and# &c& #!=# &true&#)# #and# #(#&d& #=# &1&#)#")]
		[InlineData(
			@"a  =  'b\'b'  or  (  a  >  1  and  c  !=  true  )  and  (  d  >=  1  )",
			@"&a& #=# &'b\'b'& #or# #(#&a& #># &1& #and# &c& #!=# &true&#)# #and# #(#&d& #>=# &1&#)#")]
		[InlineData(
			@"a='a=\'b\' or (a>1 and c!=true)' or (a>1 and c!=true)",
			@"&a& #=# &'a=\'b\' or (a>1 and c!=true)'& #or# #(#&a& #># &1& #and# &c& #!=# &true&#)#")]
		[InlineData(
			@"and='and=\'and\' and (and>1 and and!=true)' and (and>1 and and!=true)",
			@"&and& #=# &'and=\'and\' and (and>1 and and!=true)'& #and# #(#&and& #># &1& #and# &and& #!=# &true&#)#")]
		public void WhenInputCorrect_ShouldParse(string input, string expectedResult)
		{
			var whereToParser = new WhereToParser();
			var expression = whereToParser.Parse(input);
			var result = expression.Evaluate();

			Assert.Equal(expectedResult, result);
		}

		[Theory]
		[InlineData(@"")]
		[InlineData(@"'")]
		[InlineData(@"a='foo'\bar'")]
		[InlineData(@"a and")]
		[InlineData(@" or a=1")]
		[InlineData(@"a=1 or or b=1")]
		[InlineData(@"a = and")]
		[InlineData(@"a='foo'bar'")]
		[InlineData(@"a=""foo""")]
		public void WhenInputIncorrect_ShouldThrowException(string input)
		{
			var whereToParser = new WhereToParser();
			Assert.Throws<ArgumentException>(() => whereToParser.Parse(input));
		}
	}
}
