using Lets2Chat.Data;
using Lets2Chat.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.SignalR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Lets2Chat.Hubs
{
    public class ChatHub : Hub
    {
        Context _context;
        IHttpContextAccessor _httpContextAccessor;

        public ChatHub(Context context, IHttpContextAccessor httpContextAccessor)
        {
            _context = context;
            _httpContextAccessor = httpContextAccessor;
        }

        public async Task SendMessage(Message m)
        {
            m.Datetime = DateTime.Now;
            if (m.UserName != Context.User.Identity.Name) return; // Serve pra identificar se o usuário logado é o usuário que está enviando a mensagem

            var user = _context.Users.FirstOrDefault(u => u.UserName == m.UserName);
            if (user == null) return;

            _context.Message.Add(m);
            _context.SaveChanges();

            Message returnMessage = new Message // Estamos simplificandoa mensagem
            {
                UserName = m.UserName,
                Descricao = m.Descricao,
                Datetime = m.Datetime
            };
            await Clients.All.SendAsync("ReceiveMessage", returnMessage);
        }

        public async Task SendPrivateMessage(Message m)
        {

            if (m.UserName != Context.User.Identity.Name) return;
            var user = _context.Users.FirstOrDefault(u => u.UserName == m.UserName);
            var target = _context.Users.FirstOrDefault(u => u.UserName == m.TargetName);

            if (target == null) return;

            m.UserId = user.Id;
            m.TargetId = target.Id;
            m.Datetime = DateTime.Now;
            _context.Message.Add(m);
            _context.SaveChanges();

            Message returnMessage = new Message
            {
                UserName = m.UserName,
                Descricao = m.Descricao,
                Datetime = m.Datetime
            };

            var currentName = Context.User.Identity.Name;
            var targetName = m.TargetName;

            var group = currentName.Length >= targetName.Length ? $"{targetName}{currentName}" : $"{currentName}{targetName}";
            await Clients.Group(group).SendAsync("PrivateMessage", returnMessage);
        }

        public string GetConnectionId() => Context.ConnectionId;

        public Task JoinPrivate(string targetName) // método criado para ingressar no grupo
        {
            var currentName = Context.User.Identity.Name;
            var group = String.Compare(currentName.ToUpper(), targetName.ToUpper()) > 0 ? $"{targetName}{currentName}" : $"{currentName}{targetName}";
            //var group = currentName.Length >= targetName.Length ? $"{targetName}{currentName}" : $"{currentName}{targetName}"; // nome do grupo
            return Groups.AddToGroupAsync(Context.ConnectionId, group);
        }

        public Task LeavePrivate(string targetName) // método criado para deixar o grupo
        {
            var currentName = Context.User.Identity.Name;
            var group = String.Compare(currentName.ToUpper(), targetName.ToUpper()) > 0 ? $"{targetName}{currentName}" : $"{currentName}{targetName}";
            //var group = currentName.Length >= targetName.Length ? $"{targetName}{currentName}" : $"{currentName}{targetName}";
            return Groups.RemoveFromGroupAsync(Context.ConnectionId, group);
        }
    }
}
