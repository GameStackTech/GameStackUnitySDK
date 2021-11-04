using System;

namespace GameStack.JWT.Builder
{
    /// <summary>
    /// JSON header model with predefined parameter names specified by RFC
    /// 7515. See https://tools.ietf.org/html/rfc7515
    /// </summary>
    [Serializable]
    public class JwtHeader
    {
        public string typ;

        public string cty;

        public string alg;

        public string kid;

        public string x5u;

        public string[] x5c;

        public string x5t;
    }
}
