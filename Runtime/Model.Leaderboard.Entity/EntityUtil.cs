using System;

namespace GameStack.Model.Leaderboard.Entity
{
    public sealed class Dimensions
    {
        public static string[] Names(Dimension[] dimensions)
        {
            Func<string[]> getNames = () =>
            {
                string[] names = new string[dimensions.Length];
                for (var i = 0; i < dimensions.Length; i++)
                {
                    names[i] = dimensions[i].name;
                }
                return names;
            };
            return (dimensions == null) ? new string[0] : getNames();
        }
    }
}