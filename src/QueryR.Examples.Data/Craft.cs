using System.Collections.Generic;

namespace QueryR.Examples.Data
{
    public class Craft
    {
        public int Id { get; set; }
        public string CraftName { get; set; }
        public List<Kerbal> AssignedKerbals { get; set; } = new List<Kerbal>();

        public override string ToString() => $"{Id} - {CraftName} - Complement: {(AssignedKerbals == null ? "unknown" : AssignedKerbals.Count.ToString())}";
    }
}
