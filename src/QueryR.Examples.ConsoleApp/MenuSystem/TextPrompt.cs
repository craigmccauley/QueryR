namespace QueryR.Examples.ConsoleApp.MenuSystem
{
    public class TextPrompt : PromptBase<string>
    {
        public override string GetValue() => Response!;
        public override bool IsValid() => !string.IsNullOrWhiteSpace(Response);
    }
}
