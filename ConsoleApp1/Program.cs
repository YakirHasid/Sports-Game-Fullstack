using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ClassLibrary1;
using ClassLibrary1.DTO;

namespace ConsoleApp1
{
    internal class Program
    {
        static void Main(string[] args)
        {
            KillEmAllDbContext db = new KillEmAllDbContext();

            #region Part A

            #region Q1

            Console.WriteLine("PartA_Q1 - All players:\n");

            List<Player> players = db.Players.OrderBy(p => p.Name).ToList();            

            foreach (Player player in players)
            {
                Console.WriteLine($"Id: {player.Id}, Name: {player.Name}");
            }

            #endregion

            #region Q2

            Console.WriteLine("PartA_Q2 - Info of specific player:\n");

            Console.WriteLine("Please Insert Game Number to see game players:");

            int gameNumber = Convert.ToInt32(Console.ReadLine());

            List<GameSummaryDTO> gameSummaries = db.Players.Where(p => p.Shots.Any(s => s.Game.Id == gameNumber)).Select(p => new GameSummaryDTO
            {
                PlayerId = p.Id,
                PlayerName = p.Name,
                TotalScore = p.Shots.Where(s => s.GameId == gameNumber).Sum(s => s.Score),
                TotalShots = p.Shots.Where(s => s.GameId == gameNumber).Count()
            }).ToList();

            foreach(GameSummaryDTO gameSummary in gameSummaries)
            {
                Console.WriteLine($"Id: {gameSummary.PlayerId}, Name: {gameSummary.PlayerName}, TotalScore: {gameSummary.TotalScore}, TotalShots: {gameSummary.TotalShots}");
            }

            #endregion

            #endregion
        }
    }
}
