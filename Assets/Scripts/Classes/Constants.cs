using System.Collections.Generic;

namespace Assets.Scripts.Classes
{
    public static class Constants
    {
        public static class GameObjects
        {
            public static string Player = "PlayerObj";
            public static string PlayerBody = "PlayerBodyObj";
            public static string PlayerTail = "PlayerTailObj";
        }

        public static IReadOnlyDictionary<string, string> FunFacts { get; set; } = new Dictionary<string, string>
        {
            { "FunFact1", "This version of the game is a demo." },
            { "FunFact2", "The largest type of fish in the world is a whale shark." },
            { "FunFact3", "Fish breathe oxygen by filtering it from water via their gills." },
            { "FunFact4", "Some types of fish are 'sequential hermaphrodites', meaning they can change sex permanently." },
            { "FunFact5", "Some fish can grow in an indeterminate way, which means they will keep on growing until they die." },
        };
    }
}
