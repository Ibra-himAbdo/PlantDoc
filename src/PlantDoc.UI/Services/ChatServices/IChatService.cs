global using PlantDoc.UI.Models;

namespace PlantDoc.UI.Services.ChatServices;

public interface IChatService : IDisposable
{
    event Action OnChange;
    bool IsBotTyping { get; }
    Chat? CurrentChat { get; }
    List<Chat> ChatHistory { get; }

    Task CreateNewChatAsync();
    Task SendMessageAsync(string message);
    Task LoadChatAsync(string chatId);
    Task DeleteChatAsync(string chatId);
}