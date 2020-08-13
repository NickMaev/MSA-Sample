using System.Text.RegularExpressions;

namespace Shared.Validation
{
    public class IataCodeValidator
    {
		public static bool IsValid(string iataCode)
		{
			return new Regex("^[A-Z]{3}$").IsMatch(iataCode);
		}
	}
}