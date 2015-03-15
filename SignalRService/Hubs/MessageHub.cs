using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Microsoft.AspNet.SignalR;

namespace SignalRService.Hubs
{
    public class MessageHub : Hub
    {
        public void Send(int pin, int value)
        {
            Clients.All.messageArrived(new { Pin = pin, Value = value });
        }
    }
}