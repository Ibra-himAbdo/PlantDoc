namespace PlantDoc.UI.Services.ChatServices;

public class ChatService : IChatService
{
    private List<Chat> _chats = [];
    private string? _currentChatId;

    public event Action? OnChange;
    public bool IsBotTyping { get; private set; }
    public Chat? CurrentChat => _chats.FirstOrDefault(c => c.Id == _currentChatId);

    public List<Chat> ChatHistory => _chats.OrderByDescending(c => c.CreatedAt)
        .ToList();

    // Sample responses for the bot
    private readonly List<string> _botResponses = new()
    {
        "Based on your description, your plant might be suffering from overwatering.",
        "The yellow leaves could indicate a nutrient deficiency.",
        "That sounds like spider mite damage. Check the undersides of leaves.",
        "Your plant needs more sunlight but avoid direct afternoon sun.",
        "This is normal for this type of plant during its dormancy period.",
        "The brown spots suggest a fungal infection.",
        "Wilting despite moist soil could indicate root rot.",
        "Curling leaves often mean the plant is too dry.",
        "The plant appears to be a Monstera deliciosa.",
        "This looks like a Peace Lily (Spathiphyllum)."
    };

    public ChatService()
    {
        InitializeSampleChatsAsync()
            .ConfigureAwait(false);
    }

    public async Task CreateNewChatAsync()
    {
        var newChat = new Chat
        {
            Messages = new List<ChatMessage>
            {
                new()
                {
                    Sender = SenderType.Bot,
                    Text = "Hello! I'm PlantDoc, your AI plant care assistant. How can I help you today?",
                    Timestamp = DateTime.UtcNow
                }
            }
        };

        _chats.Add(newChat);
        _currentChatId = newChat.Id;
        await NotifyStateChangedAsync();
    }

    public async Task SendMessageAsync(string message)
    {
        if (CurrentChat == null || string.IsNullOrWhiteSpace(message)) return;

        // Add user message
        CurrentChat.Messages.Add(new ChatMessage
        {
            Sender = SenderType.User,
            Text = message,
            Timestamp = DateTime.UtcNow
        });

        // Update title if first user message
        if (CurrentChat.Messages.Count == 2)
        {
            CurrentChat.Title = message.Length > 30
                ? message[..30] + "..."
                : message;
        }

        await NotifyStateChangedAsync();

        // Simulate bot typing
        IsBotTyping = true;
        await NotifyStateChangedAsync();

        // Simulate processing delay
        await Task.Delay(1500);

        // Add bot response
        CurrentChat.Messages.Add(new ChatMessage
        {
            Sender = SenderType.Bot,
            Text = GetBotResponse(),
            Timestamp = DateTime.UtcNow
        });

        IsBotTyping = false;
        await NotifyStateChangedAsync();
    }

    public async Task LoadChatAsync(string chatId)
    {
        _currentChatId = chatId;
        await NotifyStateChangedAsync();
    }

    public async Task DeleteChatAsync(string chatId)
    {
        _chats.RemoveAll(c => c.Id == chatId);
        if (_currentChatId == chatId)
        {
            await CreateNewChatAsync();
        }

        await NotifyStateChangedAsync();
    }

    private string GetBotResponse()
    {
        return _botResponses[new Random().Next(_botResponses.Count)];
    }

    private async Task InitializeSampleChatsAsync()
    {
        var sampleChats = new List<Chat>
        {
            new()
            {
                Id = Guid.NewGuid()
                    .ToString(),
                Title = "Yellow leaves on my Monstera",
                Messages = new List<ChatMessage>
                {
                    new()
                    {
                        Sender = SenderType.Bot,
                        Text = "Hello! I'm PlantDoc. How can I help you today?",
                        Timestamp = DateTime.UtcNow.AddHours(-1)
                    },
                    new()
                    {
                        Sender = SenderType.User,
                        Text = "The leaves on my Monstera are turning yellow",
                        Timestamp = DateTime.UtcNow.AddHours(-1)
                    },
                    new()
                    {
                        Sender = SenderType.Bot,
                        Text = "Yellow leaves often indicate overwatering",
                        Timestamp = DateTime.UtcNow.AddHours(-1)
                    }
                },
                CreatedAt = DateTime.UtcNow.AddHours(-1)
            }
        };

        _chats.AddRange(sampleChats);
        await CreateNewChatAsync();
    }

    private async Task NotifyStateChangedAsync()
    {
        await Task.Run(() => OnChange?.Invoke());
    }

    public void Dispose()
    {
        // Clean up any resources if needed
    }
}