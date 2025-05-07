namespace ReserGo.Business.Validator;

public static class StayIdValidator {
    public static string Check(long stayId, int type) {
        var stayIdRef = stayId.ToString();
        if (stayIdRef.Length != 9) return "StayId must be 10 characters long.";
        var prefix = type switch {
            1 => "961",
            2 => "861",
            3 => "761",
            _ => ""
        };
        if (string.IsNullOrEmpty(prefix)) return "Product unknown";
        if (!stayIdRef.StartsWith(prefix)) return $"StayId must start with {prefix}.";
        return "";
    }
}