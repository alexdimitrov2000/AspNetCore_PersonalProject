﻿namespace TeamLegend.Services.Contracts
{
    using Models;

    using System.Threading.Tasks;

    public interface ITeamsService
    {
        Task<Team> CreateAsync(Team team);

        Task<Team> GetByIdAsync(string id);

        Task<Team> UpdateAsync(Team team);

        Task<bool> DeleteAsync(Team team);
    }
}
