using System;
using System.Collections.Generic;
using GameStack.Model.Leaderboard.Entity;

namespace GameStack.Model.Leaderboard.Props
{
    [Serializable]
    public class CreateApplicationUserProps
    {
        /// <summary>
        /// ApplicationID is the GameStack application ID to create the user
        /// under.
        /// </summary>
        public string ApplicationID;
        /// <summary>
        /// Alias is the alias that will be assigned to the user.
        /// </summary>
        public string Alias;

        public CreateApplicationUserProps()
        {
            ApplicationID = null;
            Alias = null;
        }

        public CreateApplicationUserProps(string applicationID, string alias)
        {
            ApplicationID = applicationID;
            Alias = alias;
        }

        public override string ToString()
        {
            return typeof(CreateApplicationUserProps).FullName + ":: ApplicationID: "
                + ApplicationID + " Alias: " + Alias;
        }
    }

    [Serializable]
    public class GetLeaderboardStatsProps
    {
        /// <summary>
        /// ApplicationID is the GameStack application ID the leaderboard to get
        /// stats from.
        /// </summary>
        public string ApplicationID;
        /// <summary>
        /// LeaderboardID is the GameStack leaderboard ID to get stats from.
        /// </summary>
        public string LeaderboardID;
        /// <summary>
        /// Stats is an optional list of dimension names to get stats for. If
        /// not specified all the leaderboards stat dimensions will be returned.
        /// </summary>
        public Statistic[] Stats;
        /// <summary>
        /// Filters is an optional list of filters that control how a result set
        /// is filtered.
        /// </summary>
        public Filter[] Filters;
        /// <summary>
        /// Sort is an optional directive that defines how a result set is
        /// ordered.
        /// </summary>
        public Sort Sort;
        /// <summary>
        /// Pagination is an optional pagination configuration that controls how
        /// result sets are paginated.
        /// </summary>
        public Pagination Pagination;

        public GetLeaderboardStatsProps()
        {
            ApplicationID = null;
            LeaderboardID = null;
            Stats = null;
            Filters = null;
            Sort = null;
            Pagination = null;
        }

        public GetLeaderboardStatsProps(string applicationID,
            string leaderboardID, Statistic[] stats, Filter[] filters,
            Sort sort, Pagination pagination)
        {
            ApplicationID = applicationID;
            LeaderboardID = leaderboardID;
            Stats = stats;
            Filters = filters;
            Sort = sort;
            Pagination = pagination;
        }

        public GetLeaderboardStatsProps(string applicationID,
            string leaderboardID, List<Statistic> stats, List<Filter> filters,
            Sort sort, Pagination pagination) : this(applicationID,
                leaderboardID, (stats == null) ? null : stats.ToArray(),
                (filters == null) ? null : filters.ToArray(), sort, pagination)
        {
        }

        public GetLeaderboardStatsProps(string applicationID,
            string leaderboardID, Statistic[] stats, Filter[] filters,
            Pagination pagination)
        {
            ApplicationID = applicationID;
            LeaderboardID = leaderboardID;
            Stats = stats;
            Filters = filters;
            Sort = null;
            Pagination = pagination;
        }

        public GetLeaderboardStatsProps(string applicationID,
            string leaderboardID, List<Statistic> stats, List<Filter> filters,
            Pagination pagination) : this(applicationID, leaderboardID,
                (stats == null) ? null : stats.ToArray(),
                (filters == null) ? null : filters.ToArray(), pagination)
        {
        }

        public GetLeaderboardStatsProps(string applicationID,
            string leaderboardID, Statistic[] stats, Filter[] filters)
        {
            ApplicationID = applicationID;
            LeaderboardID = leaderboardID;
            Stats = stats;
            Filters = filters;
            Sort = null;
            Pagination = null;
        }

        public GetLeaderboardStatsProps(string applicationID,
            string leaderboardID, List<Statistic> stats,
            List<Filter> filters) : this(applicationID, leaderboardID,
                (stats == null) ? null : stats.ToArray(),
                (filters == null) ? null : filters.ToArray())
        {
        }

        public override string ToString()
        {
            return typeof(GetLeaderboardStatsProps).FullName +
                ":: ApplicationID: " + ApplicationID + " LeaderboardID: " +
                LeaderboardID + " Stats: " + Stats + " Filters: " + Filters +
                " Sort: " + Sort.ToString() + " Pagination: " +
                Pagination.ToString();
        }
    }

    [Serializable]
    public class GetUserForApplicationProps
    {
        /// <summary>
        /// ApplicationID is the GameStack application ID to get a players
        /// application user from.
        /// </summary>
        public string ApplicationID;

        public GetUserForApplicationProps()
        {
            ApplicationID = null;
        }

        public GetUserForApplicationProps(string applicationID)
        {
            ApplicationID = applicationID;
        }

        public override string ToString()
        {
            return typeof(GetUserForApplicationProps).FullName +
                ":: ApplicationID: " + ApplicationID;
        }
    }

    [Serializable]
    public class LoginPlayerProps
    {
        /// <summary>
        /// Username is the players username.
        /// </summary>
        public string Username;
        /// <summary>
        /// Password is the players password.
        /// </summary>
        public string Password;
        /// <summary>
        /// ApplicationID is the ID of the application the user will be
        /// logged in to.
        /// </summary>
        public string ApplicationID;
        /// <summary>
        /// AutoCreateApplicationUser is a flag that determines if
        /// a players application user is automaticcally create for them
        /// using their GameStack player username as their application
        /// user alias.
        /// </summary>
        public bool AutoCreateApplicationUser;

        public LoginPlayerProps()
        {
            Username = null;
            Password = null;
            ApplicationID = null;
            AutoCreateApplicationUser = false;
        }

        public LoginPlayerProps(string username, string password,
            string applicationID, bool autoCreateApplicationUser)
        {
            Username = username;
            Password = password;
            ApplicationID = applicationID;
            AutoCreateApplicationUser = autoCreateApplicationUser;
        }

        public override string ToString()
        {
            return typeof(LoginPlayerProps).FullName + ":: Username: " +
                Username + " Password: " + Password + " ApplicationID: " +
                ApplicationID + " AutoCreateApplicationUser: " +
                AutoCreateApplicationUser;
        }
    }

    [Serializable]
    public class PutLeaderboardStatsProps
    {
        /// <summary>
        /// ApplicationID is the GameStack application ID the leaderboard to add
        /// stats to.
        /// </summary>
        public string ApplicationID;
        /// <summary>
        /// LeaderboardID is the GameStack leaderboard ID to add stats to.
        /// </summary>
        public string LeaderboardID;
        /// <summary>
        /// Dimensions is a list of stat dimensions that will be added to the
        /// leaderboard.
        /// </summary>
        public Dimension[] Dimensions;

        public PutLeaderboardStatsProps()
        {
            ApplicationID = null;
            LeaderboardID = null;
            Dimensions = null;
        }

        public PutLeaderboardStatsProps(string applicationID,
            string leaderboardID, Dimension[] dimensions)
        {
            ApplicationID = applicationID;
            LeaderboardID = leaderboardID;
            Dimensions = dimensions;
        }

        public PutLeaderboardStatsProps(string applicationID,
            string leaderboardID, List<Dimension> dimensions) : this(
                applicationID, leaderboardID,
                (dimensions == null) ? null : dimensions.ToArray())
        {
        }

        public override string ToString()
        {
            return typeof(PutLeaderboardStatsProps).FullName +
                ":: ApplicationID: " + ApplicationID + " LeaderboardID: " +
                LeaderboardID + " Dimensions: " + Dimensions;
        }
    }
}
