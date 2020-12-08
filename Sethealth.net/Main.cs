﻿using System;
using System.Text;
using System.Net.Http;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;



namespace Sethealth
{
    /// <summary>
    /// <c>Client</c> exposes the public api for sethealth.
    /// </summary>
    public class Client
    {
        private const string HOST = "https://api.set.health";

        /// <summary>
        /// Creates a new instance of Client for the server sethealth API
        /// It will automatically get the Sethealth credentials from the
        /// <c>"SETHEALTH_KEY"</c> and <c>"SETHEALTH_SECRET"</c> environment variables.
        /// </summary>
        /// <exception cref="InputException">Thrown when SETHEALTH_KEY or SETHEALTH_SECRET
        /// is missing.</exception>

        public Client()
        {
            var key = Environment.GetEnvironmentVariable("SETHEALTH_KEY");
            if (key == "" || key == null)
            {
                throw new InputException("Service Account Key is missing (check SETHEALTH_KEY)");
            }
            var secret = Environment.GetEnvironmentVariable("SETHEALTH_SECRET");
            if (secret == "" || secret == null)
            {
                throw new InputException("Service Account Secret is missing (check SETHEALTH_SECRET)");
            }
            this.key = key;
            this.secret = secret;
        }
        /// <summary>
        /// Creates a new client for the server sethealth API with the key and secret specified.
        /// </summary>
        /// <param name="key"> Sethealth's API key.</param>
        /// <param name="secret"> Sethealth's API secret.</param>
        /// <exception cref="InputException">Thrown when key or secret
        /// is missing or empty string.</exception>

        public Client(string key, string secret)
        {
            if (key == "" || key == null)
            {
                throw new InputException("Service Account Key is missing (check SETHEALTH_KEY)");
            }
            if (secret == "" || secret == null)
            {
                throw new InputException("Service Account Secret is missing (check SETHEALTH_SECRET)");
            }
            this.key = key;
            this.secret = secret;
        }
        /// <summary>
        /// Sethealth's API key.
        /// </summary>
        public string key { get; }
        /// <summary>
        /// Sethealth's API secret.
        /// </summary>
        public string secret { get; }

        /// <summary>
        /// This method creates a new short-living token to be used by client side.
        /// </summary>
        /// <returns>A <c>GetTokenResponse</c> instance that contains the new token created.</returns>
        /// <exception cref="AuthException">Thrown when SETHEALTH_KEY or SETHEALTH_SECRET
        /// is/are wrong and the authentification fails.</exception>

        public async Task<GetTokenResponse> GetToken()
        {
            GetTokenOptions opts = new GetTokenOptions("", 0, false);
            return await GetToken(opts);
        }

        /// <summary>
        /// This method creates a new short-living token to be used by client side with the options specified.
        /// </summary>
        /// <param name="opts"> Token options.</param>
        /// <returns>A <c>GetTokenResponse</c> instance that contains the new token created.</returns>
        /// <exception cref="AuthException">Thrown when SETHEALTH_KEY or SETHEALTH_SECRET
        /// is/are wrong and the authentification fails.</exception>

        public async Task<GetTokenResponse> GetToken(GetTokenOptions opts)
        {
            try
            {
                var json = JsonConvert.SerializeObject(new GetTokenRequestOptions
                {
                    id = this.key,
                    secret = this.secret,
                    expiresIn = opts.expiresIn,
                    testMode = opts.testMode,
                    userId = opts.userId
                });
                var body = new StringContent(json, Encoding.UTF8, "application/json");
                var client = new HttpClient();

                var response = await client.PostAsync(HOST + "/token", body);
                string result = response.Content.ReadAsStringAsync().Result;
                // if (!response.IsSuccessStatusCode)
                // {
                //     throw new AuthException("Bad auth");
                // }
                var tokenResponse = JsonConvert.DeserializeObject<GetTokenResponse>(result);
                if (tokenResponse.token == null)
                {
                    throw new AuthException("Bad response");
                }
                return tokenResponse;
            }
            catch (Exception e)
            {
                throw new AuthException("Bad request", e);
            }
        }
    }
    /// <summary>
    /// <c>GetTokenOptions</c> contains options to create the token.
    /// </summary>
    public struct GetTokenOptions
    {
        /// <summary>
        /// User's ID.
        /// </summary>
        public string userId { get; }
        /// <summary>
        /// Token duration in nanoseconds.
        /// </summary>
        public long expiresIn { get; }
        /// <summary>
        /// If true, It would access to test server.
        /// </summary>
        public bool testMode { get; }

        /// <summary>
        /// Creates new instance with the options needed in order to get a token.
        /// </summary>
        /// <param name="userId"> User's ID.</param>
        /// <param name="expiresIn"> Token duration in nanoseconds.</param>
        /// <param name="testMode"> If true, It would access to test server.</param>

        public GetTokenOptions(string userId, long expiresIn, bool testMode)
        {
            this.userId = userId;
            this.expiresIn = expiresIn;
            this.testMode = testMode;
        }
    }

    /// <summary>
    /// <c>GetTokenOptions</c> contains options sent in the request to create the token.
    /// </summary>
    struct GetTokenRequestOptions
    {
        /// <summary>
        /// Sethealth's API key.
        /// </summary>
        [JsonProperty("id")]
        public string id { get; set; }

        /// <summary>
        /// Sethealth's API secret.
        /// </summary>
        [JsonProperty("secret")]
        public string secret { get; set; }

        /// <summary>
        /// User's ID.
        /// </summary>
        [JsonProperty("user-id")]
        public string userId { get; set; }

        /// <summary>
        /// Token duration in nanoseconds.
        /// </summary>
        [JsonProperty("expires-in")]
        public long expiresIn { get; set; }

        /// <summary>
        /// If true, It would access to test server.
        /// </summary>

        [JsonProperty("test-mode")]
        public bool testMode { get; set; }

    }

    /// <summary>
    /// <c>GetTokenResponse</c> contains the result values of calling the <c>GetToken()</c> API.
    /// </summary>
    public struct GetTokenResponse
    {
        /// <summary>
        /// Sethealth's token string
        /// </summary>
        public string token;
    }

    /// <summary>
    /// Thrown when SETHEALTH_KEY or SETHEALTH_SECRET
    /// is/are wrong and the authentification fails.
    /// </summary>
    public class AuthException : Exception
    {
        /// <summary>
        /// Creates a new instance of AuthException. Used 
        /// when SETHEALTH_KEY or SETHEALTH_SECRET
        /// is/are wrong and the authentification fails
        /// </summary>
        public AuthException()
        {
        }
        /// <summary>
        /// Creates a new instance of AuthException. Used 
        /// when SETHEALTH_KEY or SETHEALTH_SECRET
        /// is/are wrong and the authentification fails
        /// </summary>
        /// <param name="message"> Exceptions message.</param>

        public AuthException(string message) : base(message)
        {
        }
        /// <summary>
        /// Creates a new instance of AuthException. Used 
        /// when SETHEALTH_KEY or SETHEALTH_SECRET
        /// is/are wrong and the authentification fails
        /// </summary>
        /// <param name="message"> Exceptions message.</param>
        /// <param name="inner"> Inner exception.</param>


        public AuthException(string message, Exception inner)
        : base(message, inner)
        {
        }
    }

    /// <summary>
    /// This exception should be thrown when the input is
    /// empty or badly-formatted.
    /// </summary>
    public class InputException : Exception
    {
        /// <summary>
        /// Creates a new instance of InputException. Used 
        /// when the input is empty or badly-formatted.
        /// </summary>
        public InputException()
        {
        }
        /// <summary>
        /// Creates a new instance of InputException. Used 
        /// when the input is empty or badly-formatted.
        /// </summary>
        /// <param name="message"> Exceptions message.</param>

        public InputException(string message) : base(message)
        {
        }
         /// <summary>
        /// Creates a new instance of InputException. Used 
        /// when the input is empty or badly-formatted.
        /// </summary>
        /// <param name="message"> Exceptions message.</param>
        /// <param name="inner"> Inner exception.</param>

        public InputException(string message, Exception inner)
        : base(message, inner)
        {
        }
    }

}