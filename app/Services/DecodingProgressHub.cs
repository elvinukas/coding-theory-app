using Microsoft.AspNetCore.SignalR;

namespace app.Services;

/// <summary>
/// Class to send progress updates of decoding processes to the front end. Child class of <see cref="Hub"/>.
/// </summary>

public class DecodingProgressHub : Hub
{
    public async Task SendProgressUpdate(int progress, int total)
    {
        await Clients.Caller.SendAsync("ReceiveDecodeProgress", progress, total);
    }
    
    
}