namespace QueryR.Examples.ConsoleApp.MenuSystem
{
    internal class IntPrompt : PromptBase<int>
    {
        public override int GetValue() => int.Parse(Response);
        public override bool IsValid() => int.TryParse(Response, out _);
    }
}
