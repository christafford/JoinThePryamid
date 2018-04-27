using System;
using System.Collections.Generic;

namespace Pryamid
{
    public class Player
    {
        private static Random _random = new Random();

        private static int Count;

        public Player(int day, string name = null)
        {
            JoinDay = day;
            Name = name ?? $"#{++Count}";
            this.MaxSignups = _random.Next(0, 4);
        }

        public void Associate(Player referrer)
        {
            referrer.Signups.Add(this);
            Referrer = referrer;
        }

        public bool CanSignupAnother => Signups.Count < MaxSignups;

        public string Name;

        private int MaxSignups;

        public List<Player> Signups = new List<Player>();

        public Player Referrer;

        public int JoinDay;

        public decimal Balance;
    }
}