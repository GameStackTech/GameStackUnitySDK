using System;
using System.Text.RegularExpressions;
using GameStack.Model.Leaderboard.Entity;
using UnityEngine;

namespace GameStack.Model.Leaderboard.Output
{
    [Serializable]
    public class CreateApplicationUserOutput
    {
        /// <summary>
        /// id is the ID of the created user.
        /// </summary>
        public string id;

        public CreateApplicationUserOutput()
        {
            id = null;
        }

        public CreateApplicationUserOutput(string id)
        {
            this.id = id;
        }

        public override string ToString()
        {
            return typeof(CreateApplicationUserOutput).FullName + ":: id: " + id;
        }
    }


    [Serializable]
    public class GetUserForApplicationOutput
    {
        /// <summary>
        /// user is a players user for an application.
        /// associated with.
        /// </summary>
        public User user;

        public GetUserForApplicationOutput()
        {
            user = null;
        }

        public GetUserForApplicationOutput(User user)
        {
            this.user = user;
        }

        public override string ToString()
        {
            return typeof(GetUserForApplicationOutput).FullName + ":: user: " +
                user.ToString();
        }
    }

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

        public GetLeaderboardStatsOutput()
        {
            stats = null;
            pagination = null;
        }

        public GetLeaderboardStatsOutput(Dimension[][] stats,
            Pagination pagination)
        {
            this.stats = stats;
            this.pagination = pagination;
        }

        public override string ToString()
        {
            string statsOut = "no stats";
            if (stats != null && stats.Length > 0)
            {
                statsOut = "";
                foreach (var statRow in stats)
                {
                    var statRowOut = "empty stats";
                    if (statRow != null && statRow.Length > 0)
                    {
                        statRowOut = "";
                        foreach (var stat in statRow)
                        {
                            statRowOut += stat.ToString() + "\n";
                        }
                    }
                    statsOut += statRowOut + "\n";
                }
            }

            return typeof(GetLeaderboardStatsOutput).FullName + ":: stats: " +
                statsOut + " pagination: " + pagination?.ToString();
        }

        /// <summary>
        /// FromJSON produces a new GetLeaderboardStatsOutput from a JSON string
        /// representation.
        /// </summary>
        ///
        /// Example inputs:
        /// {"pagination":{"next":"I5w293ZMYLj5qaoY3B5bur3sjkkRolT6ogzHtleTOkZUMTLf4VdydyqSLsu8Qft6WvW8SdI/aUCUVCSv0wSLOyY3BpOImLdwNu3XkOcwzTybl1LcZM5z5g=="},"stats":[[{"data":{"type":"FLOAT","val":"0.8"},"name":"wins"}]]}
        /// {"stats":[[{"data":{"type":"FLOAT","val":"0.8"},"name":"wins"}]],"pagination":{"next":"I5w293ZMYLj5qaoY3B5bur3sjkkRolT6ogzHtleTOkZUMTLf4VdydyqSLsu8Qft6WvW8SdI/aUCUVCSv0wSLOyY3BpOImLdwNu3XkOcwzTybl1LcZM5z5g=="}}
        public static GetLeaderboardStatsOutput FromJSON(string json)
        {
            if (string.IsNullOrEmpty(json))
            {
                return null;
            }

            // First we deserialize the field like pagination that will work
            // with the Unity Json utility.
            var output = JsonUtility.FromJson<GetLeaderboardStatsOutput>(json);
            // Next we have to convert the stats from JSON to objects manually
            // since the Unity Json utility cannot handle lists of lists.
            output.stats = StatsFromJSON(json);

            return output;
        }

        /// <summary>
        /// FromJSON produces a new list of Dimension lists from a JSON string
        /// representation.
        /// </summary>
        public static Dimension[][] StatsFromJSON(string json)
        {
            var match = Regex.Match(json, "\\\"stats\\\":\\[(.*!?)\\]");
            var statsJSON = match.Groups[1].Value;
            if (statsJSON == null || statsJSON.Trim().Length == 0)
            {
                return new Dimension[0][];
            }

            var dimensionLists = statsJSON.Split(new string[] { ",[" },
                StringSplitOptions.None);
            Dimension[][] stats = new Dimension[dimensionLists.Length][];
            for (var i = 0; i < stats.Length; ++i)
            {
                var dimensionList = dimensionLists[i].Trim('[', ']');
                var dimensions = dimensionList.Split(new string[] { "},{" },
                    StringSplitOptions.None);
                Dimension[] statRow = new Dimension[dimensions.Length];
                for (var j = 0; j < dimensions.Length; ++j)
                {
                    var dimension = dimensions[j];
                    if (!dimension.StartsWith("{"))
                    {
                        dimension = "{" + dimension;
                    }
                    if (!dimension.EndsWith("}"))
                    {
                        dimension += "}";
                    }

                    statRow[j] = Dimension.FromJSON(dimension);
                }

                stats[i] = statRow;
            }

            return stats;
        }
    }

    [Serializable]
    public class PutLeaderboardStatsOutput
    {

        public PutLeaderboardStatsOutput()
        {
        }

        public override string ToString()
        {
            return typeof(PutLeaderboardStatsOutput).FullName;
        }
    }
}

