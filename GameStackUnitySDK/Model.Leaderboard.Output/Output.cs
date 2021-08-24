using System;
using GameStackUnitySDK.Model.Leaderboard.Entity;

namespace GameStackUnitySDK.Model.Leaderboard.Output
{
    [Serializable]
    public class GetLeaderboardStatsOutput
    {
        /// <summary>
        /// stats are a list of statistic dimesions from the leaderboard that
        /// met the filtering criteria.
        /// </summary>
        public Dimension[][] stats;
        /// <summary>
        /// pagination are a list of statistic dimesions from the leaderboard that
        /// met the filtering criteria.
        /// </summary>
        public Pagination pagination;
    }

    [Serializable]
    public class PutLeaderboardStatsOutput
    {
        /// <summary>
        /// entry_id is the ID of the entry that was created.
        /// </summary>
        public string entry_id;
    }

    [Serializable]
    public class CreateInstanceOutput
    {
        /// <summary>
        /// instance_id is the ID of the instance that was created.
        /// </summary>
        public string instance_id;
    }
}

