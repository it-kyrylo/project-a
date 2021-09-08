using System.Text;

namespace ProjectA.Models.PlayersModels
{
    public class PlayerSpecificStatsModel
    {
        public int Id { get; init; }

        public string FirstName { get; init; }

        public string LastName { get; init; }

        public double Price { get; init; }

        public double Form { get; init; }

        public double PointsPerGame { get; init; }

        public int TotalPoints { get; init; }

        public string Position { get; init; }

        public int InfluenceCreativityThreatRank { get; init; }

        public override string ToString()
        {
            var sb = new StringBuilder();

            sb
                .AppendLine($"Name: {FirstName} {LastName}")
                .AppendLine($"Price: {Price}")
                .AppendLine($"Form: {Form}")
                .AppendLine($"Points Per Game: {PointsPerGame}")
                .AppendLine($"Total Points: {TotalPoints}")
                .AppendLine($"Position: {Position.ToUpper()}");

            return sb.ToString();
        }

    }
}
