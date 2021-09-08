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
        }

        public class Statistics
        {
            public const string PlayersStatistics = "/PlayersStatistics";
            public const string TeamStatistics = "/TeamsStatistics";
        }  
        

        public class StateMessages
        {
            public const string ChooseOptionMainState = "Please choose on of the options:";
            public const string InsertPlayersSuggestionsPreferences = "Please insert your preferences:";
            public const string PlayersSuggestionPreferencesFormat = "(position/min Price/max Price)";

            public const string WrongInputFormat = "Wrong preferences format";

            public const string GetPlayersSuggestionMessage = "To get 5 suggested players, please choose a criteria:";
        }
    }
}
