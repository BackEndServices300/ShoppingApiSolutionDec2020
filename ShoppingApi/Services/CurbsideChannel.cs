using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Channels;
using System.Threading.Tasks;

namespace ShoppingApi.Services
{
    public class CurbsideChannel
    {
        private const int MaxMessagesInChannel = 100;
        private readonly ILogger<CurbsideChannel> _logger;

        private Channel<CurbsideChannelRequest> _theChannel;

        public CurbsideChannel(ILogger<CurbsideChannel> logger)
        {
            _logger = logger;
            var options = new BoundedChannelOptions(MaxMessagesInChannel)
            {
                SingleReader = true,
                SingleWriter = false
            };
            _theChannel = Channel.CreateBounded<CurbsideChannelRequest>(options);
        }

        // From the controller - tell the channel to write this to the queu.
        public async Task<bool> AddCurbside(CurbsideChannelRequest order, CancellationToken ct = default)
        {
            while (await _theChannel.Writer.WaitToWriteAsync(ct) && !ct.IsCancellationRequested)
            {
                if (_theChannel.Writer.TryWrite(order))
                {
                    return true;
                }
            }
            return false;
        }

        // Processor async foreach's on this - over time.
        public IAsyncEnumerable<CurbsideChannelRequest> ReadAllAsync(CancellationToken ct = default) => _theChannel.Reader.ReadAllAsync(ct);
    }

}
