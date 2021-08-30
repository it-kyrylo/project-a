namespace ProjectA.Infrastructure
{
    public static class KeyBuilder
    {
        public static string Build(string firstName, string lastName)
        {
            return firstName + " " + lastName;
        }
    }
}
