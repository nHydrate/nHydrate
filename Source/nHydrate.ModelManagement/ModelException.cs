namespace nHydrate.ModelManagement
{
    public class ModelException : System.Exception
    {
        public ModelException(string message)
            : base(message)
        {
        }
    }

    public class ModelFileLoadException : ModelException
    {
        public ModelFileLoadException(string message)
            : base(message)
        {
        }
    }
}
