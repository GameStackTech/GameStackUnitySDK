using System;
using GameStack.Model.Leaderboard.Entity;

namespace GameStack.Model.Leaderboard.Input
{
    [Serializable]
    public class CreateApplicationUserInput
    {
        /// <summary>
        /// application_id is the ID of the application the user will be
        /// associated with.
        /// </summary>
        public string application_id;
        /// <summary>
        /// alias is the users alias.
        /// </summary>
        public string alias;

        public CreateApplicationUserInput()
        {
            application_id = null;
            alias = null;
        }

        public CreateApplicationUserInput(string applicationID, string alias)
        {
            application_id = applicationID;
            this.alias = alias;
        }

        public override string ToString()
        {
            return typeof(CreateApplicationUserInput).FullName +
                ":: application_id: " + application_id + " alias: " + alias;
        }
    }

    [Serializable]
    public class GetLeaderboardStatsInput
    {
        /// <summary>
        /// stats is a list of statistics to be derived from dimensions.
        /// </summary>
        public Statistic[] stats;
        /// <summary>
        /// filters are optional filtering options to apply to the leaderboard
        /// data.
        /// </summary>
        public Filter[] filters;
        /// <summary>
        /// sort is the sorting that will be applied to the data.
        /// </summary>
        public Sort sort;
        /// <summary>
        /// pagination is the optional pagination configuration to apply.
        /// </summary>
        public Pagination pagination;

        public GetLeaderboardStatsInput()
        {
            stats = null;
            filters = null;
            sort = null;
            pagination = null;
        }

        public GetLeaderboardStatsInput(string applicationID,
            string leaderboardID, Statistic[] stats, Filter[] filters,
            Sort sort, Pagination pagination)
        {
            this.stats = stats;
            this.filters = filters;
            this.sort = sort;
            this.pagination = pagination;
        }

        public override string ToString()
        {
            return typeof(GetLeaderboardStatsInput).FullName +
                ":: stats: " + stats + " filters: " + filters +
                " sort: " + sort.ToString() + " pagination: " +
                pagination.ToString();
        }
    }

    [Serializable]
    public class PutLeaderboardStatsInput
    {
        /// <summary>
        /// dimensions are the dimesions that will be applied to the leaderboard
        /// entry.
        /// </summary>
        public Dimension[] dimensions;

        public PutLeaderboardStatsInput()
        {
            dimensions = null;
        }

        public PutLeaderboardStatsInput(Dimension[] dimensions)
        {
            this.dimensions = dimensions;
        }

        public override string ToString()
        {
            return typeof(PutLeaderboardStatsInput).FullName +
                ":: dimensions: " + dimensions;
        }
    }
}
