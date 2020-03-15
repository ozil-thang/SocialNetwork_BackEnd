using System.Collections.Generic;
using System.Diagnostics;
using Api.Utils;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using Persistence;
using StackExchange.Redis;
using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;

namespace Api.Hubs
{
    [Authorize]
    public class OnlineUserHub : Hub
    {
        private readonly SocialNetworkContext _context;
        private readonly RedisDatabaseProvider _redisDatabaseProvider;
        private IHubContext<OnlineUserHub> _onlineUserHubContext;

        private static LinkedList<String> onlineUser = new LinkedList<string>();
        private static SortedList<String, int> onlineUserHash = new SortedList<string, int>();
        public OnlineUserHub(SocialNetworkContext context, RedisDatabaseProvider redisDatabaseProvider, IHubContext<OnlineUserHub> onlineUserHubContext)
        {
            _context = context;
            _redisDatabaseProvider = redisDatabaseProvider;
            _onlineUserHubContext = onlineUserHubContext;
        }

        public override async Task OnConnectedAsync()
        {
            Debug.WriteLine("Connected Method\n");

            var UserId = Context.UserIdentifier;

            var profile = await _context.Profiles.FirstOrDefaultAsync(p => p.UserId == UserId);

            if (onlineUserHash.ContainsKey(profile.DisplayName))
            {
                onlineUserHash[profile.DisplayName] += 1;
            }
            else
            {
                onlineUserHash[profile.DisplayName] = 1;
            }

            if (!onlineUser.Contains(profile.DisplayName))
                onlineUser.AddFirst(profile.DisplayName);

            await _onlineUserHubContext.Clients.All.SendAsync("onlineUser", onlineUser);

            await base.OnConnectedAsync();
        }
        public override async Task OnDisconnectedAsync(Exception exception)
        {
            Debug.WriteLine("User Off \n");
            var UserId = Context.UserIdentifier;

            var user = await _context.Users.Include(u => u.Profile).FirstOrDefaultAsync(u => u.Id == UserId);
            var profile = await _context.Profiles.FirstOrDefaultAsync(p => p.UserId == UserId);

            onlineUserHash[profile.DisplayName] -= 1;

            if (onlineUserHash[profile.DisplayName] == 0)
            {
                onlineUser.Remove(profile.DisplayName);
            }

            await _onlineUserHubContext.Clients.All.SendAsync("onlineUser", onlineUser);
            await base.OnDisconnectedAsync(exception);
        }

    }
}