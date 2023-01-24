namespace QueryR.Examples.ConsoleApp.MenuSystem
{
    public abstract class PromptBase<T>: IPrompt
    {
        public required string Description { get; set; }
        public required string PromptText { get; set; }
        public string? Response { get; set; }
        public required Action<T?> OnSuccess { get; set; }
        public Func<bool> Run => () =>
        {
            Console.Clear();
            Console.WriteLine(PromptText);
            Response = Console.ReadLine();
            if (IsValid())
            {
                OnSuccess?.Invoke(GetValue());
            }
            Console.Clear();
            return true;
        };

        public abstract bool IsValid();
        public abstract T GetValue();
    }
}
