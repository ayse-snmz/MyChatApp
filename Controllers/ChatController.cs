using Lets2Chat.Data;
using Lets2Chat.Hubs;
using Lets2Chat.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Lets2Chat.Controllers
{
    [Authorize]
    public class ChatController : Controller
    {
        public readonly UserManager<AppUser> _userManager;
        public readonly Context _context;

        public ChatController(UserManager<AppUser> userManager, Context context)
        {
            _userManager = userManager;
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            var nowUser = await _userManager.GetUserAsync(User);
            ViewBag.NowUserName = nowUser.UserName;
            ViewBag.Messages = _context.Message.Where(m => m.TargetName == null).ToList();
            return View();
        }

        public async Task<IActionResult> SendMessage(string Descricao, [FromServices] IHubContext<ChatHub> chat)
        {
            var postman = await _userManager.GetUserAsync(User);
            Message message = new Message
            {
                Descricao = Descricao,
                UserName = User.Identity.Name,
                UserId = postman.Id,
                Datetime = DateTime.Now
            };

            _context.Message.Add(message);
            _context.SaveChanges();
            await chat.Clients.All.SendAsync("ReceiveMessage", message);

            return Ok();
            //return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Private()
        {
            var currentUser = await _userManager.GetUserAsync(User);
            List<IdentityUser> allUsers = _context.Users.Where(u => u.UserName != currentUser.UserName).ToList();
            return View(allUsers);
        }

        public async Task<IActionResult> PrivateMessage(string id) 
        {
            var current = await _userManager.GetUserAsync(User);
            var target = await _userManager.FindByEmailAsync(id);


            var sentMessages = _context.Message.Where(m => m.UserName == current.UserName && m.TargetName == target.UserName).ToList();
            var receivedMessages = _context.Message.Where(m => m.UserName == target.UserName && m.TargetName == current.UserName).ToList();
            var messages = sentMessages.Concat(receivedMessages).ToList();

            ViewBag.Messages = messages;
            ViewBag.CurrentUser = current;
            ViewBag.TargetUser = target;
            return View();
        }
    }
}
