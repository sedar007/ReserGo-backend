namespace ReserGo.Shared;

public static class Utils {
    
    public static bool CheckMail(string? mail) {
        if (string.IsNullOrEmpty(mail)) {
            return false;
        }

        try {
            var mailAddress = new System.Net.Mail.MailAddress(mail);
            return mailAddress.Address == mail ;
        }
        catch (FormatException) {
            return false;
        }
    }
}