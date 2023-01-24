using System;
using System.Collections.Generic;
using System.Linq;

namespace QueryR.Examples.Data
{
    public static class Instances
    {
        private static List<Kerbal> kerbals = null;
        private static List<Craft> crafts = null;
        private static List<PlanetaryBody> planetaryBodies = null;

        public static List<Kerbal> Kerbals
        {
            get
            {
                Initialize();
                return kerbals;
            }
        }

        private static Random random = new Random(0);

        private static void Initialize()
        {
            if (kerbals != null)
            {
                return;
            }

            planetaryBodies = new List<PlanetaryBody>(PlanetaryBodyNames.Names.OrderBy(_ => random.Next()).Select((name, index) => new PlanetaryBody
            {
                Id = index + 1,
                Name = name,
            }));

            kerbals = new List<Kerbal>(KerbalNames.Names.OrderBy(_ => random.Next()).Select((name, index) => new Kerbal
            {
                Id = index + 1,
                FirstName = name,
                LastName = "Kerman",
                PlanetaryBodiesVisited = planetaryBodies.OrderBy(_ => random.Next()).Take(random.Next(0, 5)).ToList(),
                SnacksOnHand = SnackNames.Names.OrderBy(_ => random.Next()).Take(random.Next(0, 5)).ToDictionary(snackName => snackName, _ => random.Next(1, 3)),
            }));

            crafts = new List<Craft>(CraftNames.Names.OrderBy(_ => random.Next()).Select((name, index) => new Craft
            {
                Id = index + 1,
                CraftName = name,
            }));

            foreach (var kerbal in kerbals.OrderBy(_ => random.Next()).Take(kerbals.Count * 4 / 5))
            {
                var craft = crafts.OrderBy(_ => random.Next()).Take(1).First();
                kerbal.AssignedSpaceCraft = craft;
                kerbal.AssignedSpaceCraftId = craft.Id;
                craft.AssignedKerbals.Add(kerbal);
            }
        }
    }
}
