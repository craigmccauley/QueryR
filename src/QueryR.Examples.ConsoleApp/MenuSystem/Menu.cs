namespace QueryR.Examples.ConsoleApp.MenuSystem
{
    public class Menu : IMenuItem
    {
        private string title = string.Empty;
        public required string Title
        {
            get => GetTitle?.Invoke() ?? title;
            set => title = value;
        }
        public Func<string>? GetTitle { get; set; }
        public string? Description { get; set; }
        public string? Response { get; set; }

        public Func<IEnumerable<IMenuItem>> GetItems { get; set; } = () => new List<IMenuItem>();

        private bool isRunning = false;
        public Func<bool> Run => ShowMenu;


        private bool ShowMenu()
        {
            isRunning = true;
            Console.Clear();
            while (isRunning)
            {
                Console.WriteLine(Title);
                Console.WriteLine($"Choose one of the following options");
                var items = GetItems().ToList();
                for (int i = 0; i < items.Count; i++)
                {
                    Console.WriteLine($"{i + 1} - {items[i].Description}");
                }
                Response = Console.ReadLine();

                if (int.TryParse(Response, out var index)
                    && index > 0
                    && index <= items.Count)
                {
                    if (!items[index - 1].Run())
                    {
                        return true;
                    }
                }
                else
                {
                    Console.Clear();
                    Console.WriteLine("Invalid Entry");
                }
            }
            return true;
        }
    }
}
