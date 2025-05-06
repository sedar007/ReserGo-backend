using System.Text.RegularExpressions;

namespace ReserGo.Business.Validator;

public static class PasswordValidator {
    public static string GetError(string password) {
        if (string.IsNullOrWhiteSpace(password))
            return "Password cannot be empty.";

        if (password.Length < 8)
            return "Password must be at least 8 characters long.";

        if (!Regex.IsMatch(password, @"[A-Z]"))
            return "Password must contain at least one uppercase letter.";

        if (!Regex.IsMatch(password, @"[a-z]"))
            return "Password must contain at least one lowercase letter.";

        if (!Regex.IsMatch(password, @"\d"))
            return "Password must contain at least one digit.";

        if (!Regex.IsMatch(password, @"[\W_]"))
            return "Password must contain at least one special character.";

        return string.Empty;
    }
}