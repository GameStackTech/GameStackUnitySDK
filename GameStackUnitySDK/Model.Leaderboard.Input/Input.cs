using System;
using GameStackUnitySDK.Model.Leaderboard.Entity;

namespace GameStackUnitySDK.Model.Leaderboard.Input
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
    }

    [Serializable]
    public class GetLeaderboardStatsInput
    {
        /// <summary>
        /// application_id is the ID of the application that contains the
        /// leaderboard to get statistics from.
        /// </summary>
        public string application_id;
        /// <summary>
        /// leaderboard_id is the ID of the leaderboard to get data from.
        /// </summary>
        public string leaderboard_id;
        /// <summary>
        /// dimensions is an optional list of dimension names to return. If not
        /// specified all dimensions will be returned.
        /// </summary>
        public string[] dimensions;
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
            application_id = null;
            leaderboard_id = null;
            dimensions = null;
            filters = null;
            sort = null;
            pagination = null;
        }

        public GetLeaderboardStatsInput(string applicationID,
            string leaderboardID, string[] dimensions, Filter[] filters,
            Sort sort, Pagination pagination)
        {
            application_id = applicationID;
            leaderboard_id = leaderboardID;
            this.dimensions = dimensions;
            this.filters = filters;
            this.sort = sort;
            this.pagination = pagination;
        }
    }

    [Serializable]
    public class PutLeaderboardStatsInput
    {
        /// <summary>
        /// application_id is the ID of the application that the leaderboard
        /// belongs to.
        /// </summary>
        public string application_id;
        /// <summary>
        /// leaderboard_id is the ID of the leaderboard this entry will be added
        /// to.
        /// </summary>
        public string leaderboard_id;
        /// <summary>
        /// instance_id is the ID of the play instance these stats were derived
        /// from.
        /// </summary>
        public string instance_id;
        /// <summary>
        /// dimensions are the dimesions that will be applied to the leaderboard
        /// entry.
        /// </summary>
        public Dimension[] dimensions;

        public PutLeaderboardStatsInput()
        {
            application_id = null;
            leaderboard_id = null;
            instance_id = null;
            dimensions = null;
        }

        public PutLeaderboardStatsInput(string applicationID,
            string leaderboardID, string instanceID, Dimension[] dimensions)
        {
            application_id = applicationID;
            leaderboard_id = leaderboardID;
            instance_id = instanceID;
            this.dimensions = dimensions;
        }
    }
}
