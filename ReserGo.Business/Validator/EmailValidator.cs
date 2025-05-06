using System.Text.RegularExpressions;

namespace ReserGo.Business.Validator;

public static class EmailValidator {
    public static string GetError(string email) {
        if (!Regex.IsMatch(email, @"^[a-zA-Z0-9._%+-]+@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,}$"))
            return "Invalid E-mail.";

        return string.Empty;
    }
}