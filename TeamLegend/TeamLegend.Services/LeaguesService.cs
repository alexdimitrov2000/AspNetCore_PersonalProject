﻿namespace TeamLegend.Services
{
    using Data;
    using Models;
    using Contracts;

    using Microsoft.EntityFrameworkCore;

    using System.Threading.Tasks;
    using System.Collections.Generic;

    public class LeaguesService : ILeaguesService
    {
        private readonly ApplicationDbContext context;

        public LeaguesService(ApplicationDbContext context)
        {
            this.context = context;
        }

        public async Task<League> CreateAsync(League league)
        {
            if (league == null)
                return null;

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
            if (string.IsNullOrEmpty(id) || string.IsNullOrWhiteSpace(id))
                return null;

            return await this.context.Leagues.SingleOrDefaultAsync(l => l.Id == id);
        }

        public async Task<bool> DeleteAsync(League league)
        {
            if (league == null)
                return false;

            this.context.Leagues.Remove(league);
            await this.context.SaveChangesAsync();

            return true;
        }

        public async Task<League> AddTeamsAsync(League league, List<Team> teams)
        {
            if (league == null || teams == null)
                return null;

            teams.ForEach(t => league.Teams.Add(t));
            this.context.Leagues.Update(league);

            await this.context.SaveChangesAsync();

            return league;
        }
    }
}
