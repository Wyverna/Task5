using Microsoft.AspNetCore.SignalR;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using Task4.Controllers;
using Microsoft.AspNetCore.Mvc;
using Task4.Models;

namespace Task4
{
    public class GameHub : Hub
    {
        public async Task CheckOpp(string groupName)
        {
            await Clients.Group(groupName).SendAsync("CheckOpp");
        }

        public async Task RemoveFromGame(string Name)
        {
            await Groups.RemoveFromGroupAsync(Context.ConnectionId, Name);
        }

        public async Task Move(int id_cell, string groupName)
        {
            await Clients.OthersInGroup(groupName).SendAsync("Move", id_cell);
        }       
        
        public async Task AddToGame(string Name)
        {
            await Groups.AddToGroupAsync(Context.ConnectionId, Name);
        }
    }
}

