using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Linq;
using System.Web;

namespace Core.Salesforce
{
    public class SalesforceClient : IDisposable
    {
        private readonly HttpClient _httpClient;
        private readonly SalesforceConfig _config;
        private readonly JsonSerializerOptions _jsonOptions;
        private string? _accessToken;
        private string? _instanceUrl;
        private DateTime _tokenExpiry;

        public SalesforceClient(SalesforceConfig config)
        {
            _config = config ?? throw new ArgumentNullException(nameof(config));
            _httpClient = new HttpClient
            {
                Timeout = TimeSpan.FromSeconds(30)
            };

            // Configure JSON serialization options
            _jsonOptions = new JsonSerializerOptions
            {
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
                DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull,
                WriteIndented = false
            };
        }

        #region Authentication

        /// <summary>
        /// Authenticate using OAuth 2.0 Username-Password flow
        /// </summary>
        public async Task<bool> AuthenticateAsync()
        {
            try
            {
                var loginUrl = $"{_config.LoginUrl}/services/oauth2/token";

                var parameters = new Dictionary<string, string>
                {
                    {"grant_type", "password"},
                    {"client_id", _config.ClientId!},
                    {"client_secret", _config.ClientSecret!},
                    {"username", _config.Username!},
                    {"password", _config.Password + _config.SecurityToken}
                };

                var content = new FormUrlEncodedContent(parameters);
                var response = await _httpClient.PostAsync(loginUrl, content);

                if (response.IsSuccessStatusCode)
                {
                    var responseContent = await response.Content.ReadAsStringAsync();
                    var authResponse = JsonSerializer.Deserialize<AuthResponse>(responseContent, _jsonOptions);

                    _accessToken = authResponse!.AccessToken;
                    _instanceUrl = authResponse.InstanceUrl;
                    _tokenExpiry = DateTime.UtcNow.AddSeconds(3600); // Default 1 hour

                    // Set default authorization header
                    _httpClient.DefaultRequestHeaders.Authorization =
                        new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", _accessToken);

                    return true;
                }
                else
                {
                    var errorContent = await response.Content.ReadAsStringAsync();
                    throw new SalesforceException($"Authentication failed: {response.StatusCode} - {errorContent}");
                }
            }
            catch (Exception ex)
            {
                throw new SalesforceException($"Authentication error: {ex.Message}", ex);
            }
        }

        /// <summary>
        /// Check if the current token is still valid
        /// </summary>
        private async Task EnsureAuthenticatedAsync()
        {
            if (string.IsNullOrEmpty(_accessToken) || DateTime.UtcNow >= _tokenExpiry)
            {
                await AuthenticateAsync();
            }
        }

        #endregion

        #region CRUD Operations

        /// <summary>
        /// Create a new record
        /// </summary>
        public async Task<string?> CreateRecordAsync(string sobjectType, object record)
        {
            await EnsureAuthenticatedAsync();

            var url = $"{_instanceUrl}/services/data/v{_config.ApiVersion}/sobjects/{sobjectType}/";
            var json = JsonSerializer.Serialize(record, _jsonOptions);

            var content = new StringContent(json, Encoding.UTF8, "application/json");
            var response = await _httpClient.PostAsync(url, content);

            var responseContent = await response.Content.ReadAsStringAsync();

            if (response.IsSuccessStatusCode)
            {
                var result = JsonSerializer.Deserialize<CreateResponse>(responseContent, _jsonOptions);
                return result!.Id;
            }
            else
            {
                HandleErrorResponse(response, responseContent);
                return null;
            }
        }

        /// <summary>
        /// Retrieve a record by ID
        /// </summary>
        public async Task<T?> GetRecordAsync<T>(string sobjectType, string recordId, string? fields = null)
        {
            await EnsureAuthenticatedAsync();

            var url = $"{_instanceUrl}/services/data/v{_config.ApiVersion}/sobjects/{sobjectType}/{recordId}";

            if (!string.IsNullOrEmpty(fields))
            {
                url += $"?fields={HttpUtility.UrlEncode(fields)}";
            }

            var response = await _httpClient.GetAsync(url);
            var responseContent = await response.Content.ReadAsStringAsync();

            if (response.IsSuccessStatusCode)
            {
                return JsonSerializer.Deserialize<T>(responseContent, _jsonOptions);
            }
            else
            {
                HandleErrorResponse(response, responseContent);
                return default;
            }
        }

        /// <summary>
        /// Update a record
        /// </summary>
        public async Task<bool> UpdateRecordAsync(string sobjectType, string recordId, object record)
        {
            await EnsureAuthenticatedAsync();

            var url = $"{_instanceUrl}/services/data/v{_config.ApiVersion}/sobjects/{sobjectType}/{recordId}";
            var json = JsonSerializer.Serialize(record, _jsonOptions);

            var content = new StringContent(json, Encoding.UTF8, "application/json");
            var request = new HttpRequestMessage(new HttpMethod("PATCH"), url) { Content = content };

            var response = await _httpClient.SendAsync(request);

            if (response.IsSuccessStatusCode)
            {
                return true;
            }
            else
            {
                var responseContent = await response.Content.ReadAsStringAsync();
                HandleErrorResponse(response, responseContent);
                return false;
            }
        }

        /// <summary>
        /// Delete a record
        /// </summary>
        public async Task<bool> DeleteRecordAsync(string sobjectType, string recordId)
        {
            await EnsureAuthenticatedAsync();

            var url = $"{_instanceUrl}/services/data/v{_config.ApiVersion}/sobjects/{sobjectType}/{recordId}";
            var response = await _httpClient.DeleteAsync(url);

            if (response.IsSuccessStatusCode)
            {
                return true;
            }
            else
            {
                var responseContent = await response.Content.ReadAsStringAsync();
                HandleErrorResponse(response, responseContent);
                return false;
            }
        }

        #endregion

        #region Query Operations

        /// <summary>
        /// Execute SOQL query
        /// </summary>
        public async Task<QueryResult<T>?> QueryAsync<T>(string soql)
        {
            await EnsureAuthenticatedAsync();

            var encodedQuery = HttpUtility.UrlEncode(soql);
            var url = $"{_instanceUrl}/services/data/v{_config.ApiVersion}/query?q={encodedQuery}";

            var response = await _httpClient.GetAsync(url);
            var responseContent = await response.Content.ReadAsStringAsync();

            if (response.IsSuccessStatusCode)
            {
                return JsonSerializer.Deserialize<QueryResult<T>>(responseContent, _jsonOptions);
            }
            else
            {
                HandleErrorResponse(response, responseContent);
                return null;
            }
        }

        /// <summary>
        /// Get next batch of query results
        /// </summary>
        public async Task<QueryResult<T>?> QueryNextAsync<T>(string nextRecordsUrl)
        {
            await EnsureAuthenticatedAsync();

            var url = $"{_instanceUrl}{nextRecordsUrl}";
            var response = await _httpClient.GetAsync(url);
            var responseContent = await response.Content.ReadAsStringAsync();

            if (response.IsSuccessStatusCode)
            {
                return JsonSerializer.Deserialize<QueryResult<T>>(responseContent, _jsonOptions);
            }
            else
            {
                HandleErrorResponse(response, responseContent);
                return null;
            }
        }

        /// <summary>
        /// Execute SOQL query and return all records (handles pagination automatically)
        /// </summary>
        public async Task<List<T>> QueryAllAsync<T>(string soql)
        {
            var allRecords = new List<T>();
            var result = await QueryAsync<T>(soql);

            while (result != null)
            {
                allRecords.AddRange(result.Records!);

                if (result.Done || string.IsNullOrEmpty(result.NextRecordsUrl))
                    break;

                result = await QueryNextAsync<T>(result.NextRecordsUrl);
            }

            return allRecords;
        }

        #endregion

        #region Bulk Operations

        /// <summary>
        /// Create multiple records using Composite API
        /// </summary>
        public async Task<List<CompositeResult>?> CreateRecordsAsync(string sobjectType, List<object> records)
        {
            await EnsureAuthenticatedAsync();

            var compositeRequest = new
            {
                allOrNone = false,
                compositeRequest = records.Select((record, index) => new
                {
                    method = "POST",
                    url = $"/services/data/v{_config.ApiVersion}/sobjects/{sobjectType}/",
                    referenceId = $"ref{index}",
                    body = record
                }).ToArray()
            };

            var url = $"{_instanceUrl}/services/data/v{_config.ApiVersion}/composite";
            var json = JsonSerializer.Serialize(compositeRequest, _jsonOptions);
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            var response = await _httpClient.PostAsync(url, content);
            var responseContent = await response.Content.ReadAsStringAsync();

            if (response.IsSuccessStatusCode)
            {
                var result = JsonSerializer.Deserialize<CompositeResponse>(responseContent, _jsonOptions);
                return result!.CompositeResponses;
            }
            else
            {
                HandleErrorResponse(response, responseContent);
                return null;
            }
        }

        #endregion

        #region Metadata Operations

        /// <summary>
        /// Describe an SObject
        /// </summary>
        public async Task<SObjectDescribe?> DescribeSObjectAsync(string sobjectType)
        {
            await EnsureAuthenticatedAsync();

            var url = $"{_instanceUrl}/services/data/v{_config.ApiVersion}/sobjects/{sobjectType}/describe";
            var response = await _httpClient.GetAsync(url);
            var responseContent = await response.Content.ReadAsStringAsync();

            if (response.IsSuccessStatusCode)
            {
                return JsonSerializer.Deserialize<SObjectDescribe>(responseContent, _jsonOptions);
            }
            else
            {
                HandleErrorResponse(response, responseContent);
                return null;
            }
        }

        /// <summary>
        /// List all available SObjects
        /// </summary>
        public async Task<List<SObjectInfo>?> ListSObjectsAsync()
        {
            await EnsureAuthenticatedAsync();

            var url = $"{_instanceUrl}/services/data/v{_config.ApiVersion}/sobjects";
            var response = await _httpClient.GetAsync(url);
            var responseContent = await response.Content.ReadAsStringAsync();

            if (response.IsSuccessStatusCode)
            {
                var result = JsonSerializer.Deserialize<SObjectsResponse>(responseContent, _jsonOptions);
                return result!.SObjects;
            }
            else
            {
                HandleErrorResponse(response, responseContent);
                return null;
            }
        }

        #endregion

        #region Utility Methods

        /// <summary>
        /// Execute a custom REST API call
        /// </summary>
        public async Task<string?> ExecuteRestCallAsync(HttpMethod method, string endpoint, object? body = null)
        {
            await EnsureAuthenticatedAsync();

            var url = endpoint.StartsWith("http") ? endpoint : $"{_instanceUrl}{endpoint}";
            var request = new HttpRequestMessage(method, url);

            if (body != null && (method == HttpMethod.Post || method == HttpMethod.Put || method.Method == "PATCH"))
            {
                var json = JsonSerializer.Serialize(body, _jsonOptions);
                request.Content = new StringContent(json, Encoding.UTF8, "application/json");
            }

            var response = await _httpClient.SendAsync(request);
            var responseContent = await response.Content.ReadAsStringAsync();

            if (response.IsSuccessStatusCode)
            {
                return responseContent;
            }
            else
            {
                HandleErrorResponse(response, responseContent);
                return null;
            }
        }

        private void HandleErrorResponse(HttpResponseMessage response, string content)
        {
            try
            {
                var errors = JsonSerializer.Deserialize<List<SalesforceError>>(content, _jsonOptions);
                var errorMessage = string.Join(", ", errors!.Select(e => $"{e.ErrorCode}: {e.Message}"));
                throw new SalesforceException($"Salesforce API error: {errorMessage}");
            }
            catch (JsonException)
            {
                throw new SalesforceException($"HTTP {(int)response.StatusCode} {response.ReasonPhrase}: {content}");
            }
        }

        #endregion

        #region IDisposable

        public void Dispose()
        {
            GC.SuppressFinalize(this);
        }

        #endregion
    }

    #region Configuration and Models

    public class SalesforceConfig
    {
        public string LoginUrl { get; set; } = "https://login.salesforce.com";
        public string? ClientId { get; set; }
        public string? ClientSecret { get; set; }
        public string? Username { get; set; }
        public string? Password { get; set; }
        public string? SecurityToken { get; set; }
        public string ApiVersion { get; set; } = "58.0";
    }

    public class AuthResponse
    {
        [JsonPropertyName("access_token")]
        public string? AccessToken { get; set; }

        [JsonPropertyName("instance_url")]
        public string? InstanceUrl { get; set; }

        [JsonPropertyName("id")]
        public string? Id { get; set; }

        [JsonPropertyName("token_type")]
        public string? TokenType { get; set; }

        [JsonPropertyName("issued_at")]
        public string? IssuedAt { get; set; }

        [JsonPropertyName("signature")]
        public string? Signature { get; set; }
    }

    public class CreateResponse
    {
        [JsonPropertyName("id")]
        public string? Id { get; set; }

        [JsonPropertyName("success")]
        public bool Success { get; set; }

        [JsonPropertyName("errors")]
        public List<SalesforceError>? Errors { get; set; }
    }

    public class QueryResult<T>
    {
        [JsonPropertyName("totalSize")]
        public int TotalSize { get; set; }

        [JsonPropertyName("done")]
        public bool Done { get; set; }

        [JsonPropertyName("nextRecordsUrl")]
        public string? NextRecordsUrl { get; set; }

        [JsonPropertyName("records")]
        public List<T>? Records { get; set; }
    }

    public class CompositeResponse
    {
        [JsonPropertyName("compositeResponse")]
        public List<CompositeResult>? CompositeResponses { get; set; }
    }

    public class CompositeResult
    {
        [JsonPropertyName("body")]
        public JsonElement Body { get; set; }

        [JsonPropertyName("httpHeaders")]
        public Dictionary<string, string>? HttpHeaders { get; set; }

        [JsonPropertyName("httpStatusCode")]
        public int HttpStatusCode { get; set; }

        [JsonPropertyName("referenceId")]
        public string? ReferenceId { get; set; }
    }

    public class SObjectDescribe
    {
        [JsonPropertyName("name")]
        public string? Name { get; set; }

        [JsonPropertyName("label")]
        public string? Label { get; set; }

        [JsonPropertyName("fields")]
        public List<FieldDescribe>? Fields { get; set; }

        [JsonPropertyName("createable")]
        public bool Createable { get; set; }

        [JsonPropertyName("updateable")]
        public bool Updateable { get; set; }

        [JsonPropertyName("deletable")]
        public bool Deletable { get; set; }
    }

    public class FieldDescribe
    {
        [JsonPropertyName("name")]
        public string? Name { get; set; }

        [JsonPropertyName("label")]
        public string? Label { get; set; }

        [JsonPropertyName("type")]
        public string? Type { get; set; }

        [JsonPropertyName("length")]
        public int Length { get; set; }

        [JsonPropertyName("createable")]
        public bool Createable { get; set; }

        [JsonPropertyName("updateable")]
        public bool Updateable { get; set; }
    }

    public class SObjectsResponse
    {
        [JsonPropertyName("sobjects")]
        public List<SObjectInfo>? SObjects { get; set; }
    }

    public class SObjectInfo
    {
        [JsonPropertyName("name")]
        public string? Name { get; set; }

        [JsonPropertyName("label")]
        public string? Label { get; set; }

        [JsonPropertyName("createable")]
        public bool Createable { get; set; }

        [JsonPropertyName("updateable")]
        public bool Updateable { get; set; }
    }

    public class SalesforceError
    {
        [JsonPropertyName("message")]
        public string? Message { get; set; }

        [JsonPropertyName("errorCode")]
        public string? ErrorCode { get; set; }

        [JsonPropertyName("fields")]
        public List<string>? Fields { get; set; }
    }

    public class SalesforceException : Exception
    {
        public SalesforceException(string message) : base(message) { }
        public SalesforceException(string message, Exception innerException) : base(message, innerException) { }
    }

    #endregion

    #region Usage Example

    /*
    // Example usage:
    var config = new SalesforceConfig
    {
        ClientId = "your_client_id",
        ClientSecret = "your_client_secret", 
        Username = "your_username",
        Password = "your_password",
        SecurityToken = "your_security_token"
    };

    using var client = new SalesforceClient(config);
    
    // Authenticate
    await client.AuthenticateAsync();
    
    // Create a record
    var account = new { Name = "Test Account", Type = "Customer" };
    var accountId = await client.CreateRecordAsync("Account", account);
    
    // Query records
    var accounts = await client.QueryAsync<JsonElement>("SELECT Id, Name FROM Account LIMIT 10");
    
    // Get a specific record
    var specificAccount = await client.GetRecordAsync<JsonElement>("Account", accountId, "Id,Name,Type");
    
    // Update a record
    var updateData = new { Name = "Updated Account Name" };
    await client.UpdateRecordAsync("Account", accountId, updateData);
    
    // Delete a record
    await client.DeleteRecordAsync("Account", accountId);
    
    // Working with strongly-typed objects
    public class Account
    {
        [JsonPropertyName("Id")]
        public string Id { get; set; }
        
        [JsonPropertyName("Name")]
        public string Name { get; set; }
        
        [JsonPropertyName("Type")]
        public string Type { get; set; }
    }
    
    var typedAccounts = await client.QueryAsync<Account>("SELECT Id, Name, Type FROM Account LIMIT 10");
    */

    #endregion
}
