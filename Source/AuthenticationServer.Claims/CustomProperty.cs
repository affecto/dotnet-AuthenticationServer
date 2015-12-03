using System;

namespace Affecto.AuthenticationServer.Claims
{
    public class CustomProperty
    {
        public string Name { get; private set; }
        public string Value { get; private set; }

        public CustomProperty(string name, string value)
        {
            if (string.IsNullOrWhiteSpace(name))
            {
                throw new ArgumentException("Name must be provided.", "name");
            }
            if (string.IsNullOrWhiteSpace(value))
            {
                throw new ArgumentException("Value must be provided.", "value");
            }

            Name = name;
            Value = value;
        }
    }
}