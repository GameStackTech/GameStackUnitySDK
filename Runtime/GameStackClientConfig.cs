namespace GameStack
{
    static internal class GameStackClientConfig
    {
        private const string _gameStackAuthAPIURL = "http://localhost:8070";
        private const string _gameStackLeaderboardAPIURL = "http://localhost:8080";

        static internal string GameStackAuthAPIURL
        {
            get
            {
                return _gameStackAuthAPIURL;
            }
        }

        static internal string GameStackLeaderboardAPIURL
        {
            get
            {
                return _gameStackLeaderboardAPIURL;
            }
        }

        static internal string GetGameStackAuthURL(string path)
        {
            return $"{_gameStackAuthAPIURL}/{path}";
        }

        static internal string GetGameStackLeaderboardURL(string path)
        {
            return $"{_gameStackLeaderboardAPIURL}/{path}";
        }
    }
}
