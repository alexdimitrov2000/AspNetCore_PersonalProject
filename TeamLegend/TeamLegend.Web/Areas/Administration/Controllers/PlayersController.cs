﻿namespace TeamLegend.Web.Areas.Administration.Controllers
{
    using Common;
    using Models.Players;
    using TeamLegend.Models;
    using Services.Contracts;

    using AutoMapper;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.Extensions.Logging;
    using Microsoft.EntityFrameworkCore;

    using System.Threading.Tasks;

    public class PlayersController : AdministrationController
    {
        private readonly ILogger<PlayersController> logger;
        private readonly IPlayersService playersService;
        private readonly IMapper mapper;
        private readonly ICloudinaryService cloudinaryService;

        public PlayersController(ILogger<PlayersController> logger, IPlayersService playersService, IMapper mapper, ICloudinaryService cloudinaryService)
        {
            this.logger = logger;
            this.playersService = playersService;
            this.mapper = mapper;
            this.cloudinaryService = cloudinaryService;
        }

        public IActionResult Create()
        {
            return this.View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(PlayerCreateInputModel model)
        {
            if (!this.ModelState.IsValid)
            {
                return this.View(model);
            }

            Player player;
            try
            {
                player = this.mapper.Map<Player>(model);

                var file = model.PlayerPicture;
                if (file != null)
                {
                    var playerPictureId = string.Format(GlobalConstants.PlayerPicture, model.Name);
                    var fileStream = file.OpenReadStream();

                    var imageUploadResult = this.cloudinaryService.UploadPicture(player.GetType(), playerPictureId, fileStream);
                    player.PlayerPictureVersion = imageUploadResult.Version;
                }
                await this.playersService.CreateAsync(player);
            }
            catch (DbUpdateException e)
            {
                this.logger.LogError(e.Message);
                return this.View(model);
            }

            return this.RedirectToAction("Details", "Players", new { area = "", id = player.Id });
        }

        public async Task<IActionResult> Delete(string id)
        {
            var player = await this.playersService.GetByIdAsync(id);

            if (player == null)
                return this.NotFound();

            var playerDeleteViewModel = this.mapper.Map<PlayerDeleteViewModel>(player);
            playerDeleteViewModel.PlayerPictureUrl = this.cloudinaryService.BuildPlayerPictureUrl(player.Name, player.PlayerPictureVersion);

            return this.View(playerDeleteViewModel);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(string id)
        {
            var player = await this.playersService.GetByIdAsync(id);

            if (player == null)
                return this.NotFound();

            try
            {
                await this.playersService.DeleteAsync(player);
            }
            catch (DbUpdateException e)
            {
                this.logger.LogError(e.Message);
                this.ModelState.AddModelError("Error", "Could not delete player.");
                return this.BadRequest(this.ModelState);
            }

            return this.RedirectToAction("Index", "Home", new { area = "" });
        }
    }
}
