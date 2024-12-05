using Microsoft.AspNetCore.SignalR;

namespace app.Services;

public class DecodingProgressHub : Hub
{
    public async Task SendProgressUpdate(int progress, int total)
    {
        await Clients.Caller.SendAsync("ReceiveDecodeProgress", progress, total);
    }
    
    
}