using System.Collections.Generic;
using System.Linq;

namespace QueryR.Examples.Data
{
    internal static class CraftNames
    {
        public static readonly IReadOnlyList<string> OtherNames = new List<string>
        {
            "Bebop",
            "Discovery One",
            "Lewis & Clark",
            "Event Horizon",
            "USS Enterprise D",
            "Stargazer",
            "Yamato",
            "Artemis",
            "USG Ishimura",
            "Evangelion Unit-01",
        };
        public static readonly IReadOnlyList<string> HeinleinShips = new List<string>
        {
            "Gay Deceiver",
            "Rocinante",
            "Little Prince",
            "Goliath",
            "Jason Smith",
            "Lying Bastard",
            "Mayflower II",
            "New Frontiers",
            "Vanguard",
            "Winston",
            "Penguin",
            "Betsy",
            "Mothership",
            "Envoy",
            "Swiftsure",
            "Betsy Ross",
            "H.M.S. Rodney",
            "Bonny Sandy",
            "Dora",
            "Sisu",
            "Queen of Sheba",
            "Challenger",
            "New Frontiers",
            "Lunar Queen",
            "Arachne",
            "Dolphin",
            "Belle of Boskone",
            "Lazy Eight III",
        };
        public static readonly IReadOnlyList<string> Names = HeinleinShips.Concat(OtherNames).ToList();
    }
}
