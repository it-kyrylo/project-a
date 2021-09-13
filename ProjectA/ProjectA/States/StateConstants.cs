namespace ProjectA.States
{
    public class StateConstants
    {
        public class Suggestions
        {
            public const string PlayersSuggestions = "/PlayersSuggestion";
            public const string PointsPerGameCriteria = "Points Per Game";
            public const string CurrentFormCriteria = "Current Form";
            //ITC stans for Influence, Threat, Creativity
            public const string ITCRankCriteria = "ITC Rank";
            public const string PointsPerPriceCriteria = "Points Per Price";
            public const string OverallStatsCriteria = "Overall Stats";
            public const string BackToPreviousMenu = "Back";
            public const string PlayersByOverallStats = "Here's your result ordered by Overall stats";
            public const string PlayersByForm = "Here's your result ordered by Form";
            public const string PlayersByITC = "Here's your result ordered by ITC Rank";
            public const string PlayersByPointsPerGame = "Here's your result ordered by Points Per Game";
            public const string PlayersByPointsPerPrice = "Here's your result ordered by Points Per Price";
        }
        public class TeamStatistics
        {
            public const string Statistics = "/TeamsStatistics";
            public const string TopThreeTeams = "Top 3 Teams";
            public const string AllTeams = "All Teams";
            public const string StrongestTeamHome = "Strongest Team Home";
            public const string StrongestTeamAway = "Strongest Team Away";
            public const string MostWinsTeam = "Most Wins Team";
            public const string MostLossesTeam = "Most Losses Team";
            public const string SearchTeam = "Search Team";
            public const string BackToPreviousMenu = "Back";
        }
        public class Statistics
        {
            public const string PlayersStatistics = "/PlayersStatistics";
            public const string TeamStatistics = "/TeamsStatistics";
            public const string PlayersData = "Player's data";
            public const string TopScorersLeague = "Top scorers of the championship";
            public const string TopScorersTeam = "Top scorers in a team";
            public const string PlayersInTeamFromPosition = "Player in a team of position";
            public const string PlayerInDreamtem = "Times player has been in dream team";
            public const string PlayersFromTeamInDreamteam = "Team's players in dream teams";
        }  
        

        public class StateMessages
        {
            public const string ChooseOptionMainState = "Please choose on of the options:";
            public const string ChooseCategory = "Pick a category you want to get data for.";
            public const string InsertPlayersSuggestionsPreferences = "Please insert your preferences:";
            public const string PlayersSuggestionPreferencesFormat = "(position/min Price/max Price)";

            public const string WrongInputFormat = "Wrong preferences format.";
            public const string WrongPlayersPosition = "Sorry, there's no such position in football.";
            public const string InvalidPrices = "Sorry, invalid prices.";
            public const string MinPriceSmallerThanMaxPrice = "The minimum price must be lower than the maximum price.";

            public const string GetPlayersSuggestionMessage = "To get 5 suggested players, please choose a criteria:";
        }
    }
}