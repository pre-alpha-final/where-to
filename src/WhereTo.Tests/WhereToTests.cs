using WhereTo.Parser;
using Xunit;

namespace WhereTo.Tests
{
	public class WhereToTests
	{
		[Theory]
		[InlineData("a=1.1 or b=1.1")]
		public void ShouldValidateCorrectInputs(string input)
		{
			var whereToParser = new WhereToParser();
			whereToParser.Parse(input);
		}
	}
}
