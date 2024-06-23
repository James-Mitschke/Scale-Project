using System;

namespace Assets.Scripts.Classes
{
    public class LeaderboardScore
    {
        public LeaderboardScore(int score, int timePlayed, DateTime scoreDate)
        {
            Score = score;
            TimePlayed = timePlayed;
            ScoreDate = ScoreDate;
        }

        public int Score { get; set; }
        public int TimePlayed { get; set; }
        public DateTime ScoreDate { get; set; }
    }
}
