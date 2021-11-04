using System;
using System.Collections.Generic;
using GameStack.JWT.Algorithms;
using UnityEngine;

using static GameStack.JWT.Internal.EncodingHelper;

namespace GameStack.JWT
{
    /// <summary>
    /// Encodes Jwt.
    /// </summary>
    public sealed class JwtEncoder : IJwtEncoder
    {
        private readonly IJwtAlgorithm _algorithm;
        private readonly IBase64UrlEncoder _urlEncoder;

        /// <summary>
        /// Creates an instance of <see cref="JwtEncoder" />
        /// </summary>
        /// <remarks>
        /// Modified from the original version since the IJsonSerializer is
        /// replaced by the Unity JSON utility.
        /// </remarks>
        /// <param name="algorithm">The Jwt Algorithm</param>
        /// <param name="urlEncoder">The Base64 URL Encoder</param>
        public JwtEncoder(IJwtAlgorithm algorithm, IBase64UrlEncoder urlEncoder)
        {
            _algorithm = algorithm;
            _urlEncoder = urlEncoder;
        }

        /// <inheritdoc />
        /// <exception cref="ArgumentNullException" />
        public string Encode(IDictionary<string, object> extraHeaders,
            object payload, byte[] key)
        {
            if (payload is null)
                throw new ArgumentNullException(nameof(payload));
            if (!_algorithm.IsAsymmetric() && key is null)
                throw new ArgumentNullException(nameof(key));

            var header = extraHeaders is null ?
                new Dictionary<string, object>(2, StringComparer.OrdinalIgnoreCase) :
                new Dictionary<string, object>(extraHeaders, StringComparer.OrdinalIgnoreCase);

            if (!header.ContainsKey("typ"))
                header.Add("typ", "JWT");
            header.Add("alg", _algorithm.Name);


            var headerBytes = GetBytes(JsonUtility.ToJson(header));
            var payloadBytes = GetBytes(JsonUtility.ToJson(payload));

            var headerSegment = _urlEncoder.Encode(headerBytes);
            var payloadSegment = _urlEncoder.Encode(payloadBytes);

            var stringToSign = headerSegment + "." + payloadSegment;
            var bytesToSign = GetBytes(stringToSign);

            var signature = _algorithm.Sign(key, bytesToSign);
            var signatureSegment = _urlEncoder.Encode(signature);

            return stringToSign + "." + signatureSegment;
        }
    }
}
