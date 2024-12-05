using Microsoft.AspNetCore.SignalR;

namespace app.Services;

public class EncodingProgressHub : Hub
{

    public async Task SendProgressUpdate(int progress, int total)
    {
        await Clients.Caller.SendAsync("ReceiveProgress", progress, total);
    }
    
    
}