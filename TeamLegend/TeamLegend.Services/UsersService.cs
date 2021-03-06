﻿namespace TeamLegend.Services
{
    using Data;
    using Models;
    using Contracts;

    using System.Threading.Tasks;

    public class UsersService : IUsersService
    {
        private readonly ApplicationDbContext context;

        public UsersService(ApplicationDbContext context)
        {
            this.context = context;
        }

        public async Task<ApplicationUser> SetProfilePictureVersionAsync(ApplicationUser user, string version)
        {
            if (user == null || string.IsNullOrEmpty(version) || string.IsNullOrWhiteSpace(version))
                return null;

            user.ProfilePictureVersion = version;

            this.context.Users.Update(user);
            await this.context.SaveChangesAsync();

            return user;
        }
    }
}
