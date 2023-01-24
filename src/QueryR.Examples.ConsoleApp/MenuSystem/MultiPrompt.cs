namespace QueryR.Examples.ConsoleApp.MenuSystem
{
    public class MultiPrompt : IMenuItem
    {
        public required string Description { get; set; }
        public string? Response => null;
        public List<IMenuItem> Prompts { get; set; } = new();
        public Func<bool> Run => ShowPrompts;

        public required Action<List<IMenuItem>> OnSuccess { get; set; }

        public bool ShowPrompts()
        {
            var isNext = false;
            var index = 0;
            while (index < Prompts.Count)
            {
                if(index < 0)
                {
                    return true;
                }
                if (Prompts[index] is IPrompt p)
                {
                    p.Run();
                    isNext = p.IsValid();
                }
                else
                {
                    isNext = Prompts[index].Run();
                }

                if (isNext)
                {
                    index++;
                }
                else
                {
                    index--;
                }
            }

            OnSuccess?.Invoke(Prompts);
            return true;
        }
    }
}
