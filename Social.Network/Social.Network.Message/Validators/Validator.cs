using System.Text.RegularExpressions;


namespace Social.Network.Message.Validators
{
    public static class Validator
    {
        public static bool IsValidEmail(this string input)
        {
            if (string.IsNullOrWhiteSpace(input))
                return false;

            input = input.Trim();
            Regex pattern = new Regex(@"\w+([-+.']\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*", RegexOptions.Compiled);
            return pattern.IsMatch(input);
        }
    }
}
