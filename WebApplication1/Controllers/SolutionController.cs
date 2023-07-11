using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using ClassLibrary1;
using WebApplication1.DTO;

namespace WebApplication1.Controllers
{
    public class SolutionController : ApiController
    {
        // GET: api/Solution/Q1/{gameNumber}
        [HttpGet]
        [Route("api/Solution/Q1/{gameNumber}")]
        public List<GameSummaryDTO> ReturnGameSummaries(int gameNumber)
        {
            KillEmAllDbContext db = new KillEmAllDbContext();            

            List<GameSummaryDTO> gameSummaries = db.Players.Where(p => p.Shots.Any(s => s.Game.Id == gameNumber)).Select(p => new GameSummaryDTO
            {
                PlayerId = p.Id,
                PlayerName = p.Name,
                TotalScore = p.Shots.Where(s => s.GameId == gameNumber).Sum(s => s.Score),
                TotalShots = p.Shots.Where(s => s.GameId == gameNumber).Count()
            }).ToList();

            return gameSummaries;
        }

        // POST: api/Solution/Q2/{gameNumber}
        [HttpPost]
        [Route("api/Solution/Q2/{gameNumber}")]
        public string AddShotToPlayerGame(int gameNumber, PlayerShotDTO playerShot)
        {
            KillEmAllDbContext db = new KillEmAllDbContext();

            // assume valid game number
            Game game = db.Games.SingleOrDefault(g => g.Id == gameNumber);

            // check if given shot score is valid
            if (playerShot.ShotScore < 0 || playerShot.ShotScore > game.MaxScorePerShot)
                return $"Score must be between 0 and {game.MaxScorePerShot}";

            int curPlayerGameShots = game.Shots.Where(s => s.PlayerId == playerShot.PlayerId).Count();

            // check if player has already reached the max allowed shots in the game
            if (curPlayerGameShots >= game.MaxShots)
                return $"Max number of shots for this game is {game.MaxShots}";

            // create new shot with the given parameters
            Shot newShot = new Shot
            {
                // ID will be handled by DB
                
                PlayerId = playerShot.PlayerId,
                GameId = gameNumber,
                Score = playerShot.ShotScore,
                DateCreated = DateTime.Now,                
            };

            // set the connections
            newShot.Game = game;
            newShot.Player = db.Players.SingleOrDefault(p => p.Id == playerShot.PlayerId);

            // add the newly created shot to the shots collection
            db.Shots.Add(newShot);

            // attempt to save changes into the database
            try
            {
                db.SaveChanges();
            }
            catch(Exception ex)
            {
                return "Error saving changes into database";
            }

            // successfully added the newly created shot into the database
            return "Updated Successfully";
        }

        // GET: api/Solution
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }

        // GET: api/Solution/5
        public string Get(int id)
        {
            return "value";
        }

        // POST: api/Solution
        public void Post([FromBody]string value)
        {
        }

        // PUT: api/Solution/5
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE: api/Solution/5
        public void Delete(int id)
        {
        }
    }
}
