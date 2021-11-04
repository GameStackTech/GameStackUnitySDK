using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using GameStack.Model.Leaderboard.Enums;

namespace GameStack.Model.Leaderboard.Entity
{
    /// <summary>
    /// User is a players user in the context of an application.
    /// </summary>
    [Serializable]
    public class User
    {
        /// <summary>
        /// alias is the users public name in the application.
        /// </summary>
        public string alias;
        /// <summary>
        /// status is the applcation users activity state.
        /// </summary>
        public string status;

        public User()
        {
            alias = null;
            status = null;
        }

        public User(string alias, string status)
        {
            this.alias = alias;
            this.status = status;
        }

        public override string ToString()
        {
            return typeof(User).FullName + ":: alias: " +
                alias + " status: " + status;
        }
    }

    /// <summary>
    /// Value contains a type (string, int, or float) and value pair.
    /// </summary>
    [Serializable]
    public class Value
    {
        private static readonly string[] _fieldKeys = new string[2] {
            "type", "val" };

        /// <summary>
        /// val is the data value.
        /// </summary>
        public string val;

        /// <summary>
        /// type is the data type of the value.
        /// </summary>
        public string type;

        public Value()
        {
            val = null;
            type = null;
        }

        public Value(string type)
        {
            this.type = type;
        }

        public Value(string val, string type)
        {
            this.val = val;
            this.type = type;
        }

        public override bool Equals(object obj)
        {
            if ((obj == null) || !GetType().Equals(obj.GetType()))
            {
                return false;
            }

            Value v = (Value)obj;
            return (v.type == type) && (v.val == val);

        }

        public override int GetHashCode()
        {
            return (val?.GetHashCode() ?? 0) + (type?.GetHashCode() ?? 0);
        }

        public override string ToString()
        {
            return typeof(Value).FullName + ":: val: " +
                val + " type: " + type;
        }

        /// <summary>
        /// Int produces a new integer Value from a long.
        /// </summary>
        public static Value Int(long value)
        {
            return new Value(value.ToString(), DataType.Int);
        }

        /// <summary>
        /// Int produces a new integer Value.
        /// </summary>
        public static Value Int(int value)
        {
            return new Value(value.ToString(), DataType.Int);
        }

        /// <summary>
        /// Float produces a new floating point Value from a double.
        /// </summary>
        public static Value Float(double value)
        {
            return new Value(value.ToString(), DataType.Float);
        }

        /// <summary>
        /// Float produces a new floating point Value.
        /// </summary>
        public static Value Float(float value)
        {
            return new Value(value.ToString(), DataType.Float);
        }

        /// <summary>
        /// String produces a new string Value.
        /// </summary>
        public static Value String(string value)
        {
            return new Value(value, DataType.String);
        }

        /// <summary>
        /// FromJSON produces a new Value from a JSON string representation.
        /// </summary>
        /// <param name="json">The JSON representation of a Dimension</param>
        /// <remarks>
        /// Example inputs:
        /// {"type":"FLOAT","val":"3.4"}
        /// {"val":"3.4","type":"FLOAT"}
        /// <remarks>
        public static Value FromJSON(string json)
        {
            if (string.IsNullOrEmpty(json))
            {
                return null;
            }

            var value = new Value();
            var parts = json.Split(',');
            if (parts.Length == 0)
            {
                return value;
            }

            foreach (string part in parts)
            {
                var matches = Regex.Matches(part, "\\\"([^\\\"]*)\\\"");
                string key = null;
                var fieldLookup = new Dictionary<string, string>();
                foreach (Match match in matches)
                {
                    if (key == null)
                    {
                        key = match.Groups[0].Value.Trim('"');
                    }
                    else
                    {
                        fieldLookup.Add(key, match.Groups[0].Value.Trim('"'));
                        switch (key)
                        {
                            case "val":
                                value.val = fieldLookup["val"];
                                break;

                            case "type":
                                value.type = fieldLookup["type"];
                                break;
                        }
                        key = null;
                    }
                }
            }

            return value;
        }
    }

    /// <summary>
    /// Dimension contains a field name and the data that applies to that field.
    /// </summary>
    [Serializable]
    public class Dimension
    {
        /// <summary>
        /// name is the name of the Dimension to apply values to.
        /// </summary>
        public string name;
        /// <summary>
        /// data is the value that applies to this dimension.
        /// </summary>
        public Value data;

        public Dimension()
        {
            name = null;
        }

        public Dimension(string name)
        {
            this.name = name;
            data = null;
        }

        public Dimension(string name, Value data)
        {
            this.name = name;
            this.data = data;
        }

        public override bool Equals(object obj)
        {
            if ((obj == null) || !GetType().Equals(obj.GetType()))
            {
                return false;
            }

            Dimension d = (Dimension)obj;
            return (d.name == name) && d.data.Equals(data);

        }

        public override int GetHashCode()
        {
            return (name?.GetHashCode() ?? 0) + (data?.GetHashCode() ?? 0);
        }

        public override string ToString()
        {
            return typeof(Dimension).FullName + ":: name: " +
                name + " data: " + data?.ToString();
        }

        /// <summary>
        /// Int produces a new named Dimension with an integer value from
        /// a long.
        /// </summary>
        public static Dimension Int(string name, long value)
        {
            return new Dimension(name, Value.Int(value));
        }

        /// <summary>
        /// Int produces a new named Dimension with an integer value.
        /// </summary>
        public static Dimension Int(string name, int value)
        {
            return new Dimension(name, Value.Int(value));
        }

        /// <summary>
        /// Float produces a new named Dimension with a floating point value
        /// taking a double as an input.
        /// </summary>
        public static Dimension Float(string name, double value)
        {
            return new Dimension(name, Value.Float(value));
        }

        /// <summary>
        /// Float produces a new named Dimension with a floating point value.
        /// </summary>
        public static Dimension Float(string name, float value)
        {
            return new Dimension(name, Value.Float(value));
        }

        /// <summary>
        /// String produces a new named Dimension with a string value.
        /// </summary>
        public static Dimension String(string name, string value)
        {
            return new Dimension(name, Value.String(value));
        }

        /// <summary>
        /// FromJSON produces a new Dimension from a JSON string representation.
        /// </summary>
        /// <param name="json">The JSON representation of a Dimension</param>
        /// <remarks>
        /// Example inputs:
        /// {"data":{"type":"FLOAT","val":"0.8"},"name":"wins"}
        /// {"data":{"val":"0.8","type":"FLOAT"},"name":"wins"}
        /// {"name":"wins","data":{"type":"FLOAT","val":"0.8"}}
        /// {"name":"wins","data":{"val":"0.8","type":"FLOAT"}}
        /// </remarks>
        public static Dimension FromJSON(string json)
        {
            if (string.IsNullOrEmpty(json))
            {
                return null;
            }

            var dimension = new Dimension();
            dimension.name = Regex.Match(json,
                ",?\\\"name\\\":\"([^\\\"]*)\\\",?")?.Groups[1]?.Value;
            var dataOnlyJSON = Regex.Replace(json,
                ",?\\\"name\\\":\"([^\\\"]*)\\\",?", "");
            var valJSON = dataOnlyJSON.Replace("{\"data\":", "");
            dimension.data = Value.FromJSON(valJSON.Remove(valJSON.Length - 1));

            return dimension;
        }
    }

    /// <summary>
    /// Statistic represents a statistical value derived from a dimension.
    /// </summary>
    [Serializable]
    public class Statistic
    {
        /// <summary>
        /// dimension is the name of the dimension this statistic is derived
        /// from.
        /// </summary>
        public string dimension;
        /// <summary>
        /// op is the operation that will be applied to the statistic.
        /// </summary>
        public string op;

        public Statistic()
        {
            dimension = null;
            op = null;
        }

        public Statistic(string dimension, string op)
        {
            this.dimension = dimension;
            this.op = op;
        }

        public override string ToString()
        {
            return typeof(Statistic).FullName + ":: dimension: " +
                dimension + " op: " + op;
        }

        /// <summary>
        /// Avg produces a new named Statistic that will eb the average value of
        /// the dimensions values.
        /// </summary>
        public static Statistic Avg(string dimension)
        {
            return new Statistic(dimension, StatOperation.Avg);
        }

        /// <summary>
        /// Max produces a new named Statistic that will be the maximum of the
        /// dimensions values.
        /// </summary>
        public static Statistic Max(string dimension)
        {
            return new Statistic(dimension, StatOperation.Max);
        }

        /// <summary>
        /// Min produces a new named Statistic that will be the minimum of the
        /// dimensions values.
        /// </summary>
        public static Statistic Min(string dimension)
        {
            return new Statistic(dimension, StatOperation.Min);
        }

        /// <summary>
        /// Min produces a new named Statistic that will be the total of the
        /// dimensions values.
        /// </summary>
        public static Statistic Sum(string dimension)
        {
            return new Statistic(dimension, StatOperation.Sum);
        }
    }

    /// <summary>
    /// Sort defines what field to sort on and how to sort the results.
    /// </summary>
    [Serializable]
    public class Sort
    {
        /// <summary>
        /// op is the SortOperation that will be applied to data.
        /// </summary>
        public string op;
        /// <summary>
        /// target is the aspect that will be sorted on.
        /// </summary>
        public string target;

        public Sort()
        {
            op = null;
            target = null;
        }

        public Sort(string op, string target)
        {
            this.op = op;
            this.target = target;
        }

        public override string ToString()
        {
            return typeof(Sort).FullName + ":: op: " +
                op + " target: " + target;
        }
    }

    /// <summary>
    /// Filter defines how to filter a data set.
    /// </summary>
    [Serializable]
    public class Filter
    {
        /// <summary>
        /// op is the FilterOperation that will be applied to data.
        /// </summary>
        public string op;
        /// <summary>
        /// target is the aspect that this filter will be applied to.
        /// </summary>
        public string target;
        /// <summary>
        /// val is the value a filter will use when comparing to the target as
        /// specified by the operation.
        /// </summary>
        public Value val;
        /// <summary>
        /// not is an optional flag that dictates this filter applies to matches
        /// that do not meet the criteria.
        /// </summary>
        public bool not;

        public Filter()
        {
            op = null;
            target = null;
            val = null;
            not = false;
        }

        public Filter(string op, string target, Value val, bool not)
        {
            this.op = op;
            this.target = target;
            this.val = val;
            this.not = not;
        }

        public Filter(string op, string target, Value val)
        {
            this.op = op;
            this.target = target;
            this.val = val;
        }

        public override string ToString()
        {
            return typeof(Filter).FullName + ":: op: " +
                op + " target: " + target + " val: " + val.ToString() + " not: "
                + not;
        }
    }

    /// <summary>
    /// Pagination defines how a request should be paginated or contain
    /// information on how to paginate a request.
    /// </summary>
    [System.Serializable]
    public class Pagination
    {
        /// <summary>
        /// count is the FilterOperation that will be applied to data.
        /// </summary>
        public int count;
        /// <summary>
        /// token is the optional pagination token to use for a pagination
        /// request.
        /// </summary>
        public string token;
        /// <summary>
        /// prev is a token that will provide the previous page of data when
        /// used as the token for a paginated request.
        /// </summary>
        public string prev;
        /// <summary>
        /// next is a token that will provide the next page of data when used
        /// as the token for a paginated request.
        /// </summary>
        public string next;

        public Pagination()
        {
            count = 0;
            token = null;
            prev = null;
            next = null;
        }

        public Pagination(int count, string token, string prev, string next)
        {
            this.count = count;
            this.token = token;
            this.prev = prev;
            this.next = next;
        }

        public override string ToString()
        {
            return typeof(Pagination).FullName + ":: count: " +
                count + " token: " + token + " prev: " + prev + " next: "
                + next;
        }
    }
}

