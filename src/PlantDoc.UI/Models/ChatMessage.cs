namespace PlantDoc.UI.Models;

public class ChatMessage
{
    public string Id { get; set; } = Guid.NewGuid().ToString();
    public SenderType Sender { get; set; }
    public string? Text { get; set; }
    public string? ImageUrl { get; set; }
    public DateTime Timestamp { get; set; } = DateTime.UtcNow;
}