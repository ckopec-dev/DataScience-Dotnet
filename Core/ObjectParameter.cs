
namespace Core
{
    public class ObjectParameter
    {
        public string Name;
        public object Value;

        public ObjectParameter(string name, object value)
        {
            Name = name;
            Value = value;
        }
    }
}
