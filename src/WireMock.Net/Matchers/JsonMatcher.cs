﻿using JetBrains.Annotations;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using WireMock.Validation;

namespace WireMock.Matchers
{
    /// <summary>
    /// JsonMatcher
    /// </summary>
    public class JsonMatcher : IValueMatcher
    {
        /// <inheritdoc cref="IValueMatcher.Value"/>
        public object Value { get; }

        /// <inheritdoc cref="IMatcher.Name"/>
        public string Name => "JsonMatcher";

        /// <inheritdoc cref="IMatcher.MatchBehaviour"/>
        public MatchBehaviour MatchBehaviour { get; }

        /// <summary>
        /// Initializes a new instance of the <see cref="JsonMatcher"/> class.
        /// </summary>
        /// <param name="value">The string value to check for equality.</param>
        public JsonMatcher([NotNull] string value) : this(MatchBehaviour.AcceptOnMatch, value)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="JsonMatcher"/> class.
        /// </summary>
        /// <param name="value">The object value to check for equality.</param>
        public JsonMatcher([NotNull] object value) : this(MatchBehaviour.AcceptOnMatch, value)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="JsonMatcher"/> class.
        /// </summary>
        /// <param name="matchBehaviour">The match behaviour.</param>
        /// <param name="value">The string value to check for equality.</param>
        public JsonMatcher(MatchBehaviour matchBehaviour, [NotNull] string value)
        {
            Check.NotNull(value, nameof(value));

            MatchBehaviour = matchBehaviour;
            Value = value;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="JsonMatcher"/> class.
        /// </summary>
        /// <param name="matchBehaviour">The match behaviour.</param>
        /// <param name="value">The object value to check for equality.</param>
        public JsonMatcher(MatchBehaviour matchBehaviour, [NotNull] object value)
        {
            Check.NotNull(value, nameof(value));

            MatchBehaviour = matchBehaviour;
            Value = value;
        }

        /// <inheritdoc cref="IObjectMatcher.IsMatch"/>
        public double IsMatch(object input)
        {
            bool match = false;
            if (input != null)
            {
                try
                {
                    // Check if JToken or object
                    JToken jtokenInput = input is JToken tokenInput ? tokenInput : JObject.FromObject(input);

                    // Check if JToken or string or object
                    JToken jtokenValue =
                        Value is JToken tokenValue ? tokenValue :
                        Value is string stringValue ? JToken.Parse(stringValue) :
                        JObject.FromObject(input);

                    match = JToken.DeepEquals(jtokenValue, jtokenInput);
                }
                catch (JsonException)
                {
                    // just ignore JsonException
                }
            }

            return MatchBehaviourHelper.Convert(MatchBehaviour, MatchScores.ToScore(match));
        }
    }
}