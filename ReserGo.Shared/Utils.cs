namespace ReserGo.Shared;

public class Utils {
    public static bool CheckMail(string? mail) {
        if (string.IsNullOrEmpty(mail)) {
            return false;
        }

        try {
            new System.Net.Mail.MailAddress(mail);
            return true;
        }
        catch (FormatException) {
            return false;
        }
    }
}