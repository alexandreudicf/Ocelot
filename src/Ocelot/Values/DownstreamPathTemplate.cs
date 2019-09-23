namespace Ocelot.Values
{
    public class DownstreamPathTemplate
    {
        private readonly string _value;
        public DownstreamPathTemplate(string value)
        {
            _value = value;
        }

        public string Value { get { return !string.IsNullOrEmpty(Path) ? Path + _value : _value;} }
        public string Path { get; set; }
    }
}
