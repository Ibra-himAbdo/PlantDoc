using Microsoft.JSInterop;

namespace PlantDoc.UI.Services;

public class HybridScrollService(IJSRuntime jsRuntime)
{
    public async Task ScrollToBottom(string elementId)
    {
        try
        {
            await jsRuntime.InvokeVoidAsync("scrollHelpers.scrollToBottom", elementId);
        }
        catch (Exception ex)
        {
            // Fallback to C# scrolling if JS fails
            Console.WriteLine($"JS scroll failed: {ex.Message}");
        }
    }
}