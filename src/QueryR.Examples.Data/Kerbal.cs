using System;
using System.Collections.Generic;
using System.Linq;

namespace QueryR.Examples.Data
{
    public class Kerbal
    {
        public int Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public int AssignedSpaceCraftId { get; set; }
        public Craft AssignedSpaceCraft { get; set; }

        public List<PlanetaryBody> PlanetaryBodiesVisited { get; set; } = new List<PlanetaryBody>();
        public Dictionary<string, int> SnacksOnHand { get; set; } = new Dictionary<string, int>();

        public override string ToString() => $@"{FirstName} {LastName}
    ID:             {Id}
    Craft ID:       {AssignedSpaceCraftId}
    Craft Details:  {(AssignedSpaceCraft == null ? "unknown" : AssignedSpaceCraft.ToString())}
    Visited Planetary Bodies
        {(PlanetaryBodiesVisited == null ? "unknown" : !PlanetaryBodiesVisited.Any() ? "none" : string.Join($"{Environment.NewLine}\t", PlanetaryBodiesVisited.Select(pb => pb.Id + ") " + pb.Name)))}
    Snacks on Hand
        {(SnacksOnHand == null ? "unknown" : !SnacksOnHand.Any() ? "none" : string.Join($"{Environment.NewLine}\t", SnacksOnHand.Select(snack => $"{snack.Value} x {snack.Key}")))}
";
    }
}
