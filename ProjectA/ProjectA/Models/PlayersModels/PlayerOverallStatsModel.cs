using System.Text;

namespace ProjectA.Models.PlayersModels
{
    public class PlayerOverallStatsModel
    {
        public int Id { get; init; }

        public string FirstName { get; init; }

        public string LastName { get; init; }

        public double Price { get; init; }

        public double OverallStats { get; init; }

        public string Position { get; init; }

        public override string ToString()
        {
            var sb = new StringBuilder();

            sb
                .AppendLine($"Name: {FirstName} {LastName}")
                .AppendLine($"Price: {Price}")
                .AppendLine($"Overall Stats: {OverallStats:f2}")
                .AppendLine($"Position: {Position.ToUpper()}");

            return sb.ToString();
        }
    }
}
