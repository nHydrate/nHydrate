namespace nHydrate.Generator
{
    public class Message
    {
        public Message(string text, INHydrateModelObjectController controller)
        {
            Text = text;
            Controller = controller;
        }

        public string Text { get; } = string.Empty;

        public INHydrateModelObjectController Controller { get; } = null;

    }
}
