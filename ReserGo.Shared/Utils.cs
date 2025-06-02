using System.Net.Mail;
using Microsoft.Extensions.Logging;
using ReserGo.Common.Security;

namespace ReserGo.Shared;

public static class Utils {
    public static bool CheckMail(string? mail) {
        if (string.IsNullOrEmpty(mail)) return false;

        try {
            var mailAddress = new MailAddress(mail);
            return mailAddress.Address == mail;
        }
        catch (FormatException) {
            return false;
        }
    }

    public static void IsAuthorized(ConnectedUser connectedUser, ILogger logger) {
        if (connectedUser == null) {
            logger.LogWarning("User not connected.");
            throw new UnauthorizedAccessException("User not connected.");
        }
    }
}