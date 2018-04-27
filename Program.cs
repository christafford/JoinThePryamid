using System;
using System.Collections.Generic;
using System.Linq;

namespace Pryamid
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine();
            Console.WriteLine();
            Console.WriteLine("-------------------------");
            Console.WriteLine("Welcome to The Pyramid!!!");
            Console.WriteLine("-------------------------");
            Console.WriteLine();
            Console.WriteLine("Each participant has a 1 in 4 change of signing somebody up each day");
            Console.WriteLine("Also, each participant will have a maximum number they can sign up, from 0 to 4");
            Console.WriteLine();
            Console.WriteLine();
            Console.WriteLine("Press [enter] to begin");
            Console.WriteLine();
            Console.ReadLine();

            var random = new Random();
            var bigDaddy = new Player(0, "Big Daddy");
            var dayToPlayers = new Dictionary<int, List<Player>>();

            dayToPlayers[0] = new List<Player>();
            dayToPlayers[0].Add(bigDaddy);

            var allPlayers = new List<Player>();
            allPlayers.Add(bigDaddy);

            for (var day = 1; true; day++)
            {
                Console.Write($"Executing Day {day}....");

                dayToPlayers[day] = new List<Player>();

                // each day, each player has a 1 in 4 change of signing up someone, up to the max
                foreach (var player in allPlayers.Where(x => x.CanSignupAnother).ToList())
                {
                    if (random.Next(1, 4) == 1)
                    {
                        var newPlayer = new Player(day);
                        newPlayer.Associate(player);

                        allPlayers.Add(newPlayer);
                        dayToPlayers[day].Add(newPlayer);
                    }
                }

                // give each player their current balance
                Func<decimal, Player, decimal> candyMan = null;
                candyMan = (depth, player) =>
                {
                    var halfSingups = player.Signups.Count * 10m;
                    var fromCandyMan = player.Signups.Sum(x => candyMan(depth + 1, x));
                    var myShareCandy = (1 / depth) * fromCandyMan;
                    player.Balance = -20 + halfSingups + myShareCandy;
                    return 10 + fromCandyMan - myShareCandy;
                };

                candyMan(1, bigDaddy);
                Console.WriteLine("Done");
                Console.WriteLine("-------------");
                Console.WriteLine("Report: ");

                // report
                for (var reportDay = 0; reportDay <= day; reportDay++)
                {
                    if (!dayToPlayers[reportDay].Any())
                    {
                        continue;
                    }

                    Console.WriteLine($"Day {reportDay} Signups: {dayToPlayers[reportDay].Count}, Average Balance: {dayToPlayers[reportDay].Average(x => x.Balance).ToString("c")}");
                }

                var input = Console.ReadLine();
                while (input != "")
                {
                    if (input == "p")
                    {
                        Console.WriteLine("");
                        Console.WriteLine("-------------------------------------------");
                        Action<int, Player> printer = null;
                        printer = (depth, player) =>
                        {
                            for (var i = 0; i < depth; i++)
                            {
                                Console.Write(" ");
                            }
                            Console.WriteLine($"{player.Name}{(player.Referrer == null ? string.Empty : " - Referred by " + player.Referrer.Name)} - Balance: {player.Balance.ToString("c")} - Signed up {player.Signups.Count}");
                            player.Signups.ForEach(x => printer(depth + 1, x));
                        };
                        printer(1, bigDaddy);
                        Console.WriteLine("-------------------------------------------");
                    }
                    input = Console.ReadLine();
                }

                Console.WriteLine("-------------------------");
                Console.WriteLine();
            }
        }
    }
}