﻿namespace TeamLegend.Services
{
    using Data;
    using Models;
    using Contracts;

    using Microsoft.EntityFrameworkCore;

    using System.Collections.Generic;
    using System.Threading.Tasks;

    public class LeaguesService : ILeaguesService
    {
        private readonly ApplicationDbContext context;

        public LeaguesService(ApplicationDbContext context)
        {
            this.context = context;
        }

        public async Task<League> CreateAsync(League league)
        {
            await this.context.Leagues.AddAsync(league);
            await this.context.SaveChangesAsync();

            return league;
        }

        public async Task<ICollection<League>> GetAllAsync()
        {
            return await this.context.Leagues.ToListAsync();
        }

        public async Task<League> GetByIdAsync(string id)
        {
            if (string.IsNullOrEmpty(id))
            {
                return null;
            }
            return await this.context.Leagues.SingleOrDefaultAsync(l => l.Id == id);
        }
    }
}
