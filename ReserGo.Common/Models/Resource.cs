namespace ReserGo.Common.Models;

public class Resource<T> {
    public T Data;
    public List<Link> Links = new();
}

public class Link {
    public string Href { get; set; } = string.Empty;
    public string Rel { get; set; } = string.Empty; // Relation (e.g., "self", "update", "delete")
    public string Method { get; set; } = string.Empty; // HTTP method (e.g., "GET", "POST")
}