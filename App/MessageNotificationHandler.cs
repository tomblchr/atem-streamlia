using MediatR;
using Microsoft.Extensions.Logging;
using SwitcherServer.Atem;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace SwitcherServer
{
    public class MessageNotificationHandler : INotificationHandler<SwitcherMessageNotify>
    {
        private readonly ILogger<MessageNotificationHandler> _logger;

        public MessageNotificationHandler(ILogger<MessageNotificationHandler> logger)
        {
            _logger = logger;
        }

        public Task Handle(SwitcherMessageNotify notification, CancellationToken cancellationToken)
        {
            _logger.LogDebug(notification.Message);

            return Task.CompletedTask;
        }
    }
}
