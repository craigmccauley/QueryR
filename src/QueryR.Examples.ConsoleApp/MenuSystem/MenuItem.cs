namespace QueryR.Examples.ConsoleApp.MenuSystem
{
    public class MenuItem : IMenuItem
    {
        public required string Description { get; set; }
        public required virtual Func<bool> Run { get; set; }
        public string? Response { get; set; }
    }
}
