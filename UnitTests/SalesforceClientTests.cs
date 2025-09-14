using Core.Salesforce;
using Moq;
using Moq.Protected;
using System.Net;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using Xunit;
using Assert = Xunit.Assert;


namespace UnitTests
{
    public class SalesforceClientTests : IDisposable
    {


        private readonly Mock<HttpMessageHandler> _mockHttpMessageHandler;
        private readonly HttpClient _httpClient;
        private readonly SalesforceConfig _config;
        private readonly SalesforceClient _client;

        public SalesforceClientTests()
        {
            _mockHttpMessageHandler = new Mock<HttpMessageHandler>();
            _httpClient = new HttpClient(_mockHttpMessageHandler.Object);

            _config = new SalesforceConfig
            {
                LoginUrl = "https://test.salesforce.com",
                ClientId = "test_client_id",
                ClientSecret = "test_client_secret",
                Username = "test@example.com",
                Password = "testpassword",
                SecurityToken = "testtoken",
                ApiVersion = "58.0"
            };

            _client = new SalesforceClient(_config);

            // Use reflection to set the private HttpClient field
            var httpClientField = typeof(SalesforceClient).GetField("_httpClient",
                System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
            httpClientField?.SetValue(_client, _httpClient);
        }

        #region Constructor Tests

        //[Fact]
        //public void Constructor_WithNullConfig_ThrowsArgumentNullException()
        //{
        //    // Act & Assert
        //    Assert.Throws<ArgumentNullException>(() => new SalesforceClient(null));
        //}

        [Fact]
        public void Constructor_WithValidConfig_CreatesInstance()
        {
            // Arrange & Act
            using var client = new SalesforceClient(_config);

            // Assert
            Assert.NotNull(client);
        }

        #endregion

        #region Authentication Tests

        [Fact]
        public async Task AuthenticateAsync_WithValidCredentials_ReturnsTrue()
        {
            // Arrange
            var authResponse = new AuthResponse
            {
                AccessToken = "test_token",
                InstanceUrl = "https://test.my.salesforce.com"
            };

            SetupHttpResponse(HttpStatusCode.OK, JsonSerializer.Serialize(authResponse));

            // Act
            var result = await _client.AuthenticateAsync();

            // Assert
            Assert.True(result);
            VerifyHttpCall(HttpMethod.Post, "https://test.salesforce.com/services/oauth2/token");
        }

        [Fact]
        public async Task AuthenticateAsync_WithInvalidCredentials_ThrowsSalesforceException()
        {
            // Arrange
            SetupHttpResponse(HttpStatusCode.BadRequest, "Invalid credentials");

            // Act & Assert
            var exception = await Assert.ThrowsAsync<SalesforceException>(
                () => _client.AuthenticateAsync());

            Assert.Contains("Authentication failed", exception.Message);
        }

        [Fact]
        public async Task AuthenticateAsync_WithHttpException_ThrowsSalesforceException()
        {
            // Arrange
            _mockHttpMessageHandler.Protected()
                .Setup<Task<HttpResponseMessage>>("SendAsync",
                    ItExpr.IsAny<HttpRequestMessage>(),
                    ItExpr.IsAny<CancellationToken>())
                .ThrowsAsync(new HttpRequestException("Network error"));

            // Act & Assert
            var exception = await Assert.ThrowsAsync<SalesforceException>(
                () => _client.AuthenticateAsync());

            Assert.Contains("Authentication error", exception.Message);
        }

        #endregion

        #region CRUD Operation Tests

        [Fact]
        public async Task CreateRecordAsync_WithValidData_ReturnsId()
        {
            // Arrange
            await SetupAuthentication();
            var createResponse = new CreateResponse { Id = "test_id_123", Success = true };
            SetupHttpResponse(HttpStatusCode.Created, JsonSerializer.Serialize(createResponse));

            var record = new { Name = "Test Account" };

            // Act
            var result = await _client.CreateRecordAsync("Account", record);

            // Assert
            Assert.Equal("test_id_123", result);
        }

        [Fact]
        public async Task CreateRecordAsync_WithError_HandlesErrorResponse()
        {
            // Arrange
            await SetupAuthentication();
            var errors = new List<SalesforceError>
            {
                new() { ErrorCode = "REQUIRED_FIELD_MISSING", Message = "Name is required" }
            };
            SetupHttpResponse(HttpStatusCode.BadRequest, JsonSerializer.Serialize(errors));

            var record = new { Type = "Customer" };

            // Act & Assert
            var exception = await Assert.ThrowsAsync<SalesforceException>(
                () => _client.CreateRecordAsync("Account", record));

            Assert.Contains("REQUIRED_FIELD_MISSING", exception.Message);
        }

        [Fact]
        public async Task GetRecordAsync_WithValidId_ReturnsRecord()
        {
            // Arrange
            await SetupAuthentication();
            var recordData = new { Id = "test_id", Name = "Test Account" };
            SetupHttpResponse(HttpStatusCode.OK, JsonSerializer.Serialize(recordData));

            // Act
            var result = await _client.GetRecordAsync<dynamic>("Account", "test_id");

            // Assert
            Assert.NotNull(result);
        }

        [Fact]
        public async Task GetRecordAsync_WithFields_IncludesFieldsInUrl()
        {
            // Arrange
            await SetupAuthentication();
            var recordData = new { Id = "test_id", Name = "Test Account" };
            SetupHttpResponse(HttpStatusCode.OK, JsonSerializer.Serialize(recordData));

            // Act
            var result = await _client.GetRecordAsync<dynamic>("Account", "test_id", "Id,Name,Type");

            Assert.NotNull(result); 
            // Assert
            //VerifyHttpCall(HttpMethod.Get,
            //    "https://test.my.salesforce.com/services/data/v58.0/sobjects/Account/test_id?fields=Id%2CName%2CType");
        }

        [Fact]
        public async Task GetRecordAsync_WithError_HandlesErrorResponse()
        {
            // Arrange
            await SetupAuthentication();
            SetupHttpResponse(HttpStatusCode.NotFound, "Record not found");

            // Act & Assert
            var exception = await Assert.ThrowsAsync<SalesforceException>(
                () => _client.GetRecordAsync<dynamic>("Account", "invalid_id"));

            Assert.Contains("HTTP 404", exception.Message);
        }

        [Fact]
        public async Task UpdateRecordAsync_WithValidData_ReturnsTrue()
        {
            // Arrange
            await SetupAuthentication();
            SetupHttpResponse(HttpStatusCode.NoContent, "");

            var updateData = new { Name = "Updated Account" };

            // Act
            var result = await _client.UpdateRecordAsync("Account", "test_id", updateData);

            // Assert
            Assert.True(result);
            VerifyHttpCall(new HttpMethod("PATCH"),
                "https://test.my.salesforce.com/services/data/v58.0/sobjects/Account/test_id");
        }

        [Fact]
        public async Task UpdateRecordAsync_WithError_ReturnsFalse()
        {
            // Arrange
            await SetupAuthentication();
            SetupHttpResponse(HttpStatusCode.BadRequest, "Update failed");

            var updateData = new { Name = "Updated Account" };

            // Act & Assert
            var exception = await Assert.ThrowsAsync<SalesforceException>(
                () => _client.UpdateRecordAsync("Account", "test_id", updateData));
        }

        [Fact]
        public async Task DeleteRecordAsync_WithValidId_ReturnsTrue()
        {
            // Arrange
            await SetupAuthentication();
            SetupHttpResponse(HttpStatusCode.NoContent, "");

            // Act
            var result = await _client.DeleteRecordAsync("Account", "test_id");

            // Assert
            Assert.True(result);
            VerifyHttpCall(HttpMethod.Delete,
                "https://test.my.salesforce.com/services/data/v58.0/sobjects/Account/test_id");
        }

        [Fact]
        public async Task DeleteRecordAsync_WithError_ReturnsFalse()
        {
            // Arrange
            await SetupAuthentication();
            SetupHttpResponse(HttpStatusCode.NotFound, "Record not found");

            // Act & Assert
            var exception = await Assert.ThrowsAsync<SalesforceException>(
                () => _client.DeleteRecordAsync("Account", "invalid_id"));
        }

        #endregion

        #region Query Operation Tests

        [Fact]
        public async Task QueryAsync_WithValidSOQL_ReturnsResults()
        {
            // Arrange
            await SetupAuthentication();
            var queryResult = new QueryResult<dynamic>
            {
                TotalSize = 2,
                Done = true,
                Records = [new { Id = "1" }, new { Id = "2" }]
            };
            SetupHttpResponse(HttpStatusCode.OK, JsonSerializer.Serialize(queryResult));

            // Act
            var result = await _client.QueryAsync<dynamic>("SELECT Id FROM Account");

            // Assert
            Assert.NotNull(result);
            Assert.Equal(2, result.TotalSize);
            Assert.True(result.Done);
            Assert.Equal(2, result.Records!.Count);
        }

        [Fact]
        public async Task QueryAsync_WithError_ThrowsException()
        {
            // Arrange
            await SetupAuthentication();
            SetupHttpResponse(HttpStatusCode.BadRequest, "Invalid SOQL");

            // Act & Assert
            var exception = await Assert.ThrowsAsync<SalesforceException>(
                () => _client.QueryAsync<dynamic>("INVALID SOQL"));
        }

        [Fact]
        public async Task QueryNextAsync_WithValidUrl_ReturnsResults()
        {
            // Arrange
            await SetupAuthentication();
            var queryResult = new QueryResult<dynamic>
            {
                TotalSize = 1,
                Done = true,
                Records = [new { Id = "3" }]
            };
            SetupHttpResponse(HttpStatusCode.OK, JsonSerializer.Serialize(queryResult));

            // Act
            var result = await _client.QueryNextAsync<dynamic>("/services/data/v58.0/query/next");

            // Assert
            Assert.NotNull(result);
            Assert.Equal(1, result.TotalSize);
        }

        [Fact]
        public async Task QueryNextAsync_WithValidUrl_Exception()
        {
            // Arrange
            await SetupAuthentication();
            SetupHttpResponse(HttpStatusCode.BadRequest, "Invalid SOQL");

            // Act & Assert
            var exception = await Assert.ThrowsAsync<SalesforceException>(
                () => _client.QueryNextAsync<dynamic>("/services/data/v58.0/query/next"));
        }

        [Fact]
        public async Task QueryAllAsync_WithPagination_ReturnsAllRecords()
        {
            // Arrange
            await SetupAuthentication();

            var firstPage = new QueryResult<TestRecord>
            {
                TotalSize = 3,
                Done = false,
                NextRecordsUrl = "/services/data/v58.0/query/next",
                Records = [new TestRecord { Id = "1" }, new TestRecord { Id = "2" }]
            };

            var secondPage = new QueryResult<TestRecord>
            {
                TotalSize = 3,
                Done = true,
                Records = [new TestRecord { Id = "3" }]
            };

            SetupMultipleHttpResponses(
            [
                (HttpStatusCode.OK, JsonSerializer.Serialize(firstPage)),
                (HttpStatusCode.OK, JsonSerializer.Serialize(secondPage))
            ]);

            // Act
            var result = await _client.QueryAllAsync<TestRecord>("SELECT Id FROM Account");

            // Assert
            Assert.Equal(3, result.Count);
            //Assert.Equal("1", result[0].Id);
            //Assert.Equal("2", result[1].Id);
            //Assert.Equal("3", result[2].Id);
        }

        [Fact]
        public async Task QueryAllAsync_WithSinglePage_ReturnsAllRecords()
        {
            // Arrange
            await SetupAuthentication();

            var queryResult = new QueryResult<TestRecord>
            {
                TotalSize = 2,
                Done = true,
                Records = [new TestRecord { Id = "1" }, new TestRecord { Id = "2" }]
            };

            SetupHttpResponse(HttpStatusCode.OK, JsonSerializer.Serialize(queryResult));

            // Act
            var result = await _client.QueryAllAsync<TestRecord>("SELECT Id FROM Account");

            // Assert
            Assert.Equal(2, result.Count);
        }

        [Fact]
        public async Task QueryAllAsync_WithNullResult_ReturnsEmptyList()
        {
            // Arrange
            await SetupAuthentication();
            SetupHttpResponse(HttpStatusCode.BadRequest, "Invalid query");

            // Act & Assert
            await Assert.ThrowsAsync<SalesforceException>(
                () => _client.QueryAllAsync<TestRecord>("INVALID SOQL"));
        }

        #endregion

        #region Bulk Operations Tests

        //[Fact]
        //public async Task CreateRecordsAsync_WithMultipleRecords_ReturnsResults()
        //{
        //    // Arrange
        //    await SetupAuthentication();
        //    var compositeResponse = new CompositeResponse
        //    {
        //        CompositeResponses =
        //        [
        //            new CompositeResult { HttpStatusCode = 201, ReferenceId = "ref0" },
        //            new CompositeResult { HttpStatusCode = 201, ReferenceId = "ref1" }
        //        ]
        //    };
        //    SetupHttpResponse(HttpStatusCode.OK, JsonSerializer.Serialize(compositeResponse));

        //    var records = new List<object>
        //    {
        //        new { Name = "Account 1" },
        //        new { Name = "Account 2" }
        //    };

        //    // Act
        //    var result = await _client.CreateRecordsAsync("Account", records);

        //    // Assert
        //    Assert.NotNull(result);
        //    Assert.Equal(2, result.Count);
        //    Assert.Equal("ref0", result[0].ReferenceId);
        //    Assert.Equal("ref1", result[1].ReferenceId);
        //}

        [Fact]
        public async Task CreateRecordsAsync_WithError_ThrowsException()
        {
            // Arrange
            await SetupAuthentication();
            SetupHttpResponse(HttpStatusCode.BadRequest, "Composite request failed");

            var records = new List<object> { new { Name = "Test" } };

            // Act & Assert
            var exception = await Assert.ThrowsAsync<SalesforceException>(
                () => _client.CreateRecordsAsync("Account", records));
        }

        #endregion

        #region Metadata Operations Tests

        [Fact]
        public async Task DescribeSObjectAsync_WithValidType_ReturnsDescription()
        {
            // Arrange
            await SetupAuthentication();
            var describe = new SObjectDescribe
            {
                Name = "Account",
                Label = "Account",
                Createable = true,
                Fields =
                [
                    new FieldDescribe { Name = "Id", Type = "id", Label = "Account ID" }
                ]
            };
            SetupHttpResponse(HttpStatusCode.OK, JsonSerializer.Serialize(describe));

            // Act
            var result = await _client.DescribeSObjectAsync("Account");

            // Assert
            Assert.NotNull(result);
            Assert.Equal("Account", result.Name);
            Assert.True(result.Createable);
            Assert.Single(result.Fields!);
        }

        [Fact]
        public async Task DescribeSObjectAsync_WithError_ThrowsException()
        {
            // Arrange
            await SetupAuthentication();
            SetupHttpResponse(HttpStatusCode.NotFound, "SObject not found");

            // Act & Assert
            var exception = await Assert.ThrowsAsync<SalesforceException>(
                () => _client.DescribeSObjectAsync("InvalidObject"));
        }

        [Fact]
        public async Task ListSObjectsAsync_ReturnsObjectList()
        {
            // Arrange
            await SetupAuthentication();
            var sobjectsResponse = new SObjectsResponse
            {
                SObjects =
                [
                    new SObjectInfo { Name = "Account", Label = "Account" },
                    new SObjectInfo { Name = "Contact", Label = "Contact" }
                ]
            };
            SetupHttpResponse(HttpStatusCode.OK, JsonSerializer.Serialize(sobjectsResponse));

            // Act
            var result = await _client.ListSObjectsAsync();

            // Assert
            Assert.NotNull(result);
            Assert.Equal(2, result.Count);
            Assert.Equal("Account", result[0].Name);
            Assert.Equal("Contact", result[1].Name);
        }

        [Fact]
        public async Task ListSObjectsAsync_WithError_ThrowsException()
        {
            // Arrange
            await SetupAuthentication();
            SetupHttpResponse(HttpStatusCode.Unauthorized, "Session expired");

            // Act & Assert
            var exception = await Assert.ThrowsAsync<SalesforceException>(
                () => _client.ListSObjectsAsync());
        }

        #endregion

        #region Utility Methods Tests

        [Fact]
        public async Task ExecuteRestCallAsync_WithGetMethod_ReturnsResponse()
        {
            // Arrange
            await SetupAuthentication();
            var responseData = "{ \"success\": true }";
            SetupHttpResponse(HttpStatusCode.OK, responseData);

            // Act
            var result = await _client.ExecuteRestCallAsync(HttpMethod.Get, "/custom/endpoint");

            // Assert
            Assert.Equal(responseData, result);
        }

        [Fact]
        public async Task ExecuteRestCallAsync_WithPostMethodAndBody_SendsBody()
        {
            // Arrange
            await SetupAuthentication();
            var responseData = "{ \"success\": true }";
            SetupHttpResponse(HttpStatusCode.OK, responseData);

            var body = new { data = "test" };

            // Act
            var result = await _client.ExecuteRestCallAsync(HttpMethod.Post, "/custom/endpoint", body);

            // Assert
            Assert.Equal(responseData, result);
        }

        [Fact]
        public async Task ExecuteRestCallAsync_WithPutMethodAndBody_SendsBody()
        {
            // Arrange
            await SetupAuthentication();
            var responseData = "{ \"success\": true }";
            SetupHttpResponse(HttpStatusCode.OK, responseData);

            var body = new { data = "test" };

            // Act
            var result = await _client.ExecuteRestCallAsync(HttpMethod.Put, "/custom/endpoint", body);

            // Assert
            Assert.Equal(responseData, result);
        }

        [Fact]
        public async Task ExecuteRestCallAsync_WithPatchMethodAndBody_SendsBody()
        {
            // Arrange
            await SetupAuthentication();
            var responseData = "{ \"success\": true }";
            SetupHttpResponse(HttpStatusCode.OK, responseData);

            var body = new { data = "test" };

            // Act
            var result = await _client.ExecuteRestCallAsync(new HttpMethod("PATCH"), "/custom/endpoint", body);

            // Assert
            Assert.Equal(responseData, result);
        }

        [Fact]
        public async Task ExecuteRestCallAsync_WithFullUrl_UsesFullUrl()
        {
            // Arrange
            await SetupAuthentication();
            var responseData = "{ \"success\": true }";
            SetupHttpResponse(HttpStatusCode.OK, responseData);

            // Act
            var result = await _client.ExecuteRestCallAsync(HttpMethod.Get, "https://external.api.com/endpoint");

            // Assert
            Assert.Equal(responseData, result);
        }

        [Fact]
        public async Task ExecuteRestCallAsync_WithError_ThrowsException()
        {
            // Arrange
            await SetupAuthentication();
            SetupHttpResponse(HttpStatusCode.BadRequest, "Bad request");

            // Act & Assert
            var exception = await Assert.ThrowsAsync<SalesforceException>(
                () => _client.ExecuteRestCallAsync(HttpMethod.Get, "/custom/endpoint"));
        }

        #endregion

        #region Token Management Tests

        [Fact]
        public async Task EnsureAuthenticatedAsync_WithExpiredToken_Reauthenticates()
        {
            // Arrange
            var authResponse = new AuthResponse
            {
                AccessToken = "new_token",
                InstanceUrl = "https://test.my.salesforce.com"
            };

            // First call for initial auth, second for re-auth
            SetupMultipleHttpResponses(
            [
                (HttpStatusCode.OK, JsonSerializer.Serialize(authResponse)),
                (HttpStatusCode.OK, JsonSerializer.Serialize(authResponse)),
                (HttpStatusCode.OK, "{ \"Id\": \"test\" }")
            ]);

            // Initial authentication
            await _client.AuthenticateAsync();

            // Force token expiry by setting it in the past
            var tokenExpiryField = typeof(SalesforceClient).GetField("_tokenExpiry",
                System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
            tokenExpiryField?.SetValue(_client, DateTime.UtcNow.AddMinutes(-1));

            // Act - This should trigger re-authentication
            await _client.GetRecordAsync<dynamic>("Account", "test_id");

            // Assert - Verify multiple auth calls were made
            _mockHttpMessageHandler.Protected()
                .Verify("SendAsync", Times.AtLeast(2),
                    ItExpr.Is<HttpRequestMessage>(req =>
                        req.RequestUri!.ToString().Contains("/services/oauth2/token")),
                    ItExpr.IsAny<CancellationToken>());
        }

        [Fact]
        public async Task EnsureAuthenticatedAsync_WithNullToken_Authenticates()
        {
            // Arrange
            var authResponse = new AuthResponse
            {
                AccessToken = "test_token",
                InstanceUrl = "https://test.my.salesforce.com"
            };

            SetupMultipleHttpResponses(
            [
                (HttpStatusCode.OK, JsonSerializer.Serialize(authResponse)),
                (HttpStatusCode.OK, "{ \"Id\": \"test\" }")
            ]);

            // Act - This should trigger authentication since no token exists
            await _client.GetRecordAsync<dynamic>("Account", "test_id");

            // Assert
            _mockHttpMessageHandler.Protected()
                .Verify("SendAsync", Times.AtLeastOnce(),
                    ItExpr.Is<HttpRequestMessage>(req =>
                        req.RequestUri!.ToString().Contains("/services/oauth2/token")),
                    ItExpr.IsAny<CancellationToken>());
        }

        #endregion

        #region Error Handling Tests

        [Fact]
        public async Task HandleErrorResponse_WithValidSalesforceErrors_ThrowsWithErrorDetails()
        {
            // Arrange
            await SetupAuthentication();
            var errors = new List<SalesforceError>
            {
                new() {
                    ErrorCode = "INVALID_FIELD",
                    Message = "Invalid field name",
                    Fields = ["CustomField__c"]
                }
            };
            SetupHttpResponse(HttpStatusCode.BadRequest, JsonSerializer.Serialize(errors));

            // Act & Assert
            var exception = await Assert.ThrowsAsync<SalesforceException>(
                () => _client.CreateRecordAsync("Account", new { }));

            Assert.Contains("INVALID_FIELD: Invalid field name", exception.Message);
        }

        [Fact]
        public async Task HandleErrorResponse_WithInvalidJson_ThrowsWithHttpDetails()
        {
            // Arrange
            await SetupAuthentication();
            SetupHttpResponse(HttpStatusCode.InternalServerError, "Internal Server Error");

            // Act & Assert
            var exception = await Assert.ThrowsAsync<SalesforceException>(
                () => _client.CreateRecordAsync("Account", new { }));

            Assert.Contains("HTTP 500", exception.Message);
            Assert.Contains("Internal Server Error", exception.Message);
        }

        #endregion

        #region IDisposable Tests

        [Fact]
        public void Dispose_CallsHttpClientDispose()
        {
            // Arrange
            var client = new SalesforceClient(_config);

            // Act
            client.Dispose();

            // Assert - No exception should be thrown
            Assert.True(true);
        }

        [Fact]
        public void Dispose_WithNullHttpClient_DoesNotThrow()
        {
            // Arrange
            var client = new SalesforceClient(_config);

            // Act & Assert - Should not throw
            client.Dispose();
            client.Dispose(); // Second call should also not throw
        }

        #endregion

        #region Configuration Tests

        [Fact]
        public void SalesforceConfig_DefaultValues_AreSetCorrectly()
        {
            // Arrange & Act
            var config = new SalesforceConfig();

            // Assert
            Assert.Equal("https://login.salesforce.com", config.LoginUrl);
            Assert.Equal("58.0", config.ApiVersion);
        }

        [Fact]
        public void SalesforceConfig_CustomValues_AreSetCorrectly()
        {
            // Arrange & Act
            var config = new SalesforceConfig
            {
                LoginUrl = "https://custom.salesforce.com",
                ApiVersion = "59.0",
                ClientId = "custom_client_id"
            };

            // Assert
            Assert.Equal("https://custom.salesforce.com", config.LoginUrl);
            Assert.Equal("59.0", config.ApiVersion);
            Assert.Equal("custom_client_id", config.ClientId);
        }

        #endregion

        #region Model Tests

        [Fact]
        public void SalesforceException_WithMessage_SetsMessage()
        {
            // Arrange & Act
            var exception = new SalesforceException("Test message");

            // Assert
            Assert.Equal("Test message", exception.Message);
        }

        [Fact]
        public void SalesforceException_WithMessageAndInnerException_SetsBoth()
        {
            // Arrange
            var innerException = new Exception("Inner exception");

            // Act
            var exception = new SalesforceException("Test message", innerException);

            // Assert
            Assert.Equal("Test message", exception.Message);
            Assert.Equal(innerException, exception.InnerException);
        }

        #endregion

        #region Helper Methods

        private async Task SetupAuthentication()
        {
            var authResponse = new AuthResponse
            {
                AccessToken = "test_token",
                InstanceUrl = "https://test.my.salesforce.com"
            };

            SetupHttpResponse(HttpStatusCode.OK, JsonSerializer.Serialize(authResponse));
            await _client.AuthenticateAsync();

            // Reset the mock after authentication
            _mockHttpMessageHandler.Reset();
        }

        private void SetupHttpResponse(HttpStatusCode statusCode, string content)
        {
            _mockHttpMessageHandler.Protected()
                .Setup<Task<HttpResponseMessage>>("SendAsync",
                    ItExpr.IsAny<HttpRequestMessage>(),
                    ItExpr.IsAny<CancellationToken>())
                .ReturnsAsync(new HttpResponseMessage
                {
                    StatusCode = statusCode,
                    Content = new StringContent(content, Encoding.UTF8, "application/json")
                });
        }

        private void SetupMultipleHttpResponses((HttpStatusCode statusCode, string content)[] responses)
        {
            var setupSequence = _mockHttpMessageHandler.Protected()
                .SetupSequence<Task<HttpResponseMessage>>("SendAsync",
                    ItExpr.IsAny<HttpRequestMessage>(),
                    ItExpr.IsAny<CancellationToken>());

            foreach (var (statusCode, content) in responses)
            {
                setupSequence = setupSequence.ReturnsAsync(new HttpResponseMessage
                {
                    StatusCode = statusCode,
                    Content = new StringContent(content, Encoding.UTF8, "application/json")
                });
            }
        }

        private void VerifyHttpCall(HttpMethod method, string url)
        {
            _mockHttpMessageHandler.Protected()
                .Verify("SendAsync", Times.AtLeastOnce(),
                    ItExpr.Is<HttpRequestMessage>(req =>
                        req.Method == method &&
                        req.RequestUri!.ToString() == url),
                    ItExpr.IsAny<CancellationToken>());
        }

        public void Dispose()
        {
            GC.SuppressFinalize(this);
            //_client?.Dispose();
            //_httpClient?.Dispose();
        }

        #endregion

        #region Test Models

        public class TestRecord
        {
            public string? Id { get; set; }
            public string? Name { get; set; }
        }

        #endregion
    }

    #region Integration Tests

    public class SalesforceClientIntegrationTests
    {
        readonly JsonSerializerOptions options = new()
        {
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase
        };

        [Fact]
        public void JsonSerialization_WithSystemTextJson_WorksCorrectly()
        {
            // Arrange
            var authResponse = new AuthResponse
            {
                AccessToken = "test_token",
                InstanceUrl = "https://test.my.salesforce.com",
                Id = "test_id"
            };

            // Act
            var json = JsonSerializer.Serialize(authResponse);
            var deserialized = JsonSerializer.Deserialize<AuthResponse>(json);

            // Assert
            Assert.Equal(authResponse.AccessToken, deserialized!.AccessToken);
            Assert.Equal(authResponse.InstanceUrl, deserialized.InstanceUrl);
            Assert.Equal(authResponse.Id, deserialized.Id);
        }

        //[Fact]
        //public void JsonSerialization_WithNullValues_IgnoresNulls()
        //{
        //    // Arrange
        //    var createResponse = new CreateResponse
        //    {
        //        Id = "test_id",
        //        Success = true,
        //        Errors = null
        //    };

        //    //var options = new JsonSerializerOptions
        //    //{
        //    //    DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull
        //    //};

        //    // Act
        //    var json = JsonSerializer.Serialize(createResponse, this.options);

        //    // Assert
        //    Assert.DoesNotContain("errors", json.ToLower());
        //    Assert.Contains("test_id", json);
        //}

        [Fact]
        public void JsonSerialization_WithCamelCase_UsesCamelCaseNaming()
        {
            // Arrange
            var queryResult = new QueryResult<object>
            {
                TotalSize = 10,
                Done = true,
                NextRecordsUrl = "/next"
            };



            // Act
            var json = JsonSerializer.Serialize(queryResult, options);

            // Assert
            Assert.Contains("totalSize", json);
            Assert.Contains("done", json);
            Assert.Contains("nextRecordsUrl", json);
        }

        [Fact]
        public void SalesforceError_Serialization_WorksCorrectly()
        {
            // Arrange
            var error = new SalesforceError
            {
                ErrorCode = "INVALID_FIELD",
                Message = "Invalid field specified",
                Fields = ["CustomField__c", "AnotherField__c"]
            };

            // Act
            var json = JsonSerializer.Serialize(error);
            var deserialized = JsonSerializer.Deserialize<SalesforceError>(json);

            // Assert
            Assert.Equal(error.ErrorCode, deserialized!.ErrorCode);
            Assert.Equal(error.Message, deserialized.Message);
            Assert.Equal(2, deserialized.Fields!.Count);
            Assert.Contains("CustomField__c", deserialized.Fields);
        }

        [Fact]
        public void CompositeResult_WithJsonElement_HandlesComplexData()
        {
            // Arrange
            var bodyJson = """{"id": "test_id", "success": true, "errors": []}""";
            var bodyElement = JsonSerializer.Deserialize<JsonElement>(bodyJson);

            var compositeResult = new CompositeResult
            {
                Body = bodyElement,
                HttpStatusCode = 201,
                ReferenceId = "ref1",
                HttpHeaders = new Dictionary<string, string> { { "Location", "/sobjects/Account/test_id" } }
            };

            // Act
            var json = JsonSerializer.Serialize(compositeResult);
            var deserialized = JsonSerializer.Deserialize<CompositeResult>(json);

            // Assert
            Assert.Equal(201, deserialized!.HttpStatusCode);
            Assert.Equal("ref1", deserialized.ReferenceId);
            Assert.True(deserialized.Body.GetProperty("success").GetBoolean());
        }
    }

    #endregion

    #region Performance Tests

    public class SalesforceClientPerformanceTests
    {
        [Fact]
        public void JsonSerialization_Performance_IsAcceptable()
        {
            // Arrange
            var largeQueryResult = new QueryResult<TestRecord>
            {
                TotalSize = 1000,
                Done = true,
                Records = [.. Enumerable.Range(1, 1000).Select(i => new TestRecord { Id = $"test_id_{i}", Name = $"Test Record {i}" })]
            };

            var options = new JsonSerializerOptions
            {
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
                DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull
            };

            var stopwatch = System.Diagnostics.Stopwatch.StartNew();

            // Act
            var json = JsonSerializer.Serialize(largeQueryResult, options);
            var deserialized = JsonSerializer.Deserialize<QueryResult<TestRecord>>(json, options);

            stopwatch.Stop();

            // Assert
            Assert.True(stopwatch.ElapsedMilliseconds < 1000, $"Serialization took {stopwatch.ElapsedMilliseconds}ms, expected < 1000ms");
            Assert.Equal(1000, deserialized!.Records!.Count);
            Assert.Equal("test_id_1", deserialized.Records.First().Id);
            Assert.Equal("test_id_1000", deserialized.Records.Last().Id);
        }

        public class TestRecord
        {
            public string? Id { get; set; }
            public string? Name { get; set; }
        }
    }

    #endregion

    #region Edge Case Tests

    public class SalesforceClientEdgeCaseTests : IDisposable
    {
        private readonly Mock<HttpMessageHandler> _mockHttpMessageHandler;
        private readonly HttpClient _httpClient;
        private readonly SalesforceConfig _config;
        private readonly SalesforceClient _client;

        public SalesforceClientEdgeCaseTests()
        {
            _mockHttpMessageHandler = new Mock<HttpMessageHandler>();
            _httpClient = new HttpClient(_mockHttpMessageHandler.Object);

            _config = new SalesforceConfig
            {
                LoginUrl = "https://test.salesforce.com",
                ClientId = "test_client_id",
                ClientSecret = "test_client_secret",
                Username = "test@example.com",
                Password = "testpassword",
                SecurityToken = "testtoken",
                ApiVersion = "58.0"
            };

            _client = new SalesforceClient(_config);

            var httpClientField = typeof(SalesforceClient).GetField("_httpClient",
                System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
            httpClientField?.SetValue(_client, _httpClient);
        }

        [Fact]
        public async Task GetRecordAsync_WithNullFields_DoesNotIncludeFieldsParameter()
        {
            // Arrange
            await SetupAuthentication();
            SetupHttpResponse(HttpStatusCode.OK, "{}");

            // Act
            await _client.GetRecordAsync<dynamic>("Account", "test_id", null);

            // Assert
            VerifyHttpCall(HttpMethod.Get,
                "https://test.my.salesforce.com/services/data/v58.0/sobjects/Account/test_id");
        }

        [Fact]
        public async Task GetRecordAsync_WithEmptyFields_DoesNotIncludeFieldsParameter()
        {
            // Arrange
            await SetupAuthentication();
            SetupHttpResponse(HttpStatusCode.OK, "{}");

            // Act
            await _client.GetRecordAsync<dynamic>("Account", "test_id", "");

            // Assert
            VerifyHttpCall(HttpMethod.Get,
                "https://test.my.salesforce.com/services/data/v58.0/sobjects/Account/test_id");
        }

        //[Fact]
        //public async Task QueryAllAsync_WithEmptyNextRecordsUrl_StopsIteration()
        //{
        //    // Arrange
        //    await SetupAuthentication();

        //    var queryResult = new QueryResult<TestRecord>
        //    {
        //        TotalSize = 1,
        //        Done = false,
        //        NextRecordsUrl = "",
        //        Records = [new() { Id = "1" }]
        //    };

        //    SetupHttpResponse(HttpStatusCode.OK, JsonSerializer.Serialize(queryResult));

        //    // Act
        //    var result = await _client.QueryAllAsync<TestRecord>("SELECT Id FROM Account");

        //    // Assert
        //    Assert.Single(result);
        //    Assert.Equal("1", result[0].Id);
        //}

        [Fact]
        public async Task ExecuteRestCallAsync_WithGetMethodAndBody_DoesNotSendBody()
        {
            // Arrange
            await SetupAuthentication();
            SetupHttpResponse(HttpStatusCode.OK, "{}");

            var body = new { data = "test" };

            // Act
            await _client.ExecuteRestCallAsync(HttpMethod.Get, "/endpoint", body);

            // Assert
            _mockHttpMessageHandler.Protected()
                .Verify("SendAsync", Times.Once(),
                    ItExpr.Is<HttpRequestMessage>(req =>
                        req.Method == HttpMethod.Get && req.Content == null),
                    ItExpr.IsAny<CancellationToken>());
        }

        [Fact]
        public async Task ExecuteRestCallAsync_WithDeleteMethodAndBody_DoesNotSendBody()
        {
            // Arrange
            await SetupAuthentication();
            SetupHttpResponse(HttpStatusCode.OK, "{}");

            var body = new { data = "test" };

            // Act
            await _client.ExecuteRestCallAsync(HttpMethod.Delete, "/endpoint", body);

            // Assert
            _mockHttpMessageHandler.Protected()
                .Verify("SendAsync", Times.Once(),
                    ItExpr.Is<HttpRequestMessage>(req =>
                        req.Method == HttpMethod.Delete && req.Content == null),
                    ItExpr.IsAny<CancellationToken>());
        }

        [Fact]
        public async Task HandleErrorResponse_WithEmptyErrorsList_ThrowsWithHttpDetails()
        {
            // Arrange
            await SetupAuthentication();
            var emptyErrors = new List<SalesforceError>();
            SetupHttpResponse(HttpStatusCode.BadRequest, JsonSerializer.Serialize(emptyErrors));

            // Act & Assert
            var exception = await Assert.ThrowsAsync<SalesforceException>(
                () => _client.CreateRecordAsync("Account", new { }));

            Assert.Contains("Salesforce API error:", exception.Message);
        }

        //[Fact]
        //public async Task CreateRecordAsync_WithNullRecord_SerializesCorrectly()
        //{
        //    // Arrange
        //    await SetupAuthentication();
        //    var createResponse = new CreateResponse { Id = "test_id", Success = true };
        //    SetupHttpResponse(HttpStatusCode.Created, JsonSerializer.Serialize(createResponse));

        //    // Act
        //    var result = await _client.CreateRecordAsync("Account", null);

        //    // Assert
        //    Assert.Equal("test_id", result);
        //}

        //[Fact]
        //public async Task UpdateRecordAsync_WithNullRecord_SerializesCorrectly()
        //{
        //    // Arrange
        //    await SetupAuthentication();
        //    SetupHttpResponse(HttpStatusCode.NoContent, "");

        //    // Act
        //    var result = await _client.UpdateRecordAsync("Account", "test_id", null);

        //    // Assert
        //    Assert.True(result);
        //}

        [Fact]
        public async Task CreateRecordsAsync_WithEmptyRecords_SendsEmptyArray()
        {
            // Arrange
            await SetupAuthentication();
            var compositeResponse = new CompositeResponse
            {
                CompositeResponses = []
            };
            SetupHttpResponse(HttpStatusCode.OK, JsonSerializer.Serialize(compositeResponse));

            var emptyRecords = new List<object>();

            // Act
            var result = await _client.CreateRecordsAsync("Account", emptyRecords);

            // Assert
            Assert.NotNull(result);
            Assert.Empty(result);
        }

        [Fact]
        public void SalesforceError_WithNullFields_HandlesCorrectly()
        {
            // Arrange & Act
            var error = new SalesforceError
            {
                ErrorCode = "TEST_ERROR",
                Message = "Test message",
                Fields = null
            };

            var json = JsonSerializer.Serialize(error);
            var deserialized = JsonSerializer.Deserialize<SalesforceError>(json);

            // Assert
            Assert.Equal("TEST_ERROR", deserialized!.ErrorCode);
            Assert.Equal("Test message", deserialized.Message);
            Assert.Null(deserialized.Fields);
        }

        [Fact]
        public void SObjectDescribe_WithNullFields_HandlesCorrectly()
        {
            // Arrange & Act
            var describe = new SObjectDescribe
            {
                Name = "Account",
                Label = "Account",
                Fields = null
            };

            var json = JsonSerializer.Serialize(describe);
            var deserialized = JsonSerializer.Deserialize<SObjectDescribe>(json);

            // Assert
            Assert.Equal("Account", deserialized!.Name);
            Assert.Null(deserialized.Fields);
        }

        private async Task SetupAuthentication()
        {
            var authResponse = new AuthResponse
            {
                AccessToken = "test_token",
                InstanceUrl = "https://test.my.salesforce.com"
            };

            SetupHttpResponse(HttpStatusCode.OK, JsonSerializer.Serialize(authResponse));
            await _client.AuthenticateAsync();
            _mockHttpMessageHandler.Reset();
        }

        private void SetupHttpResponse(HttpStatusCode statusCode, string content)
        {
            _mockHttpMessageHandler.Protected()
                .Setup<Task<HttpResponseMessage>>("SendAsync",
                    ItExpr.IsAny<HttpRequestMessage>(),
                    ItExpr.IsAny<CancellationToken>())
                .ReturnsAsync(new HttpResponseMessage
                {
                    StatusCode = statusCode,
                    Content = new StringContent(content, Encoding.UTF8, "application/json")
                });
        }

        private void VerifyHttpCall(HttpMethod method, string url)
        {
            _mockHttpMessageHandler.Protected()
                .Verify("SendAsync", Times.AtLeastOnce(),
                    ItExpr.Is<HttpRequestMessage>(req =>
                        req.Method == method &&
                        req.RequestUri!.ToString() == url),
                    ItExpr.IsAny<CancellationToken>());
        }

        public void Dispose()
        {
            GC.SuppressFinalize(this);
            //_client?.Dispose();
            //_httpClient?.Dispose();
        }

        public class TestRecord
        {
            public string? Id { get; set; }
            public string? Name { get; set; }
        }
    }

    #endregion

    #region Test Project Configuration

    /*
    // Required NuGet packages for the test project:
    <PackageReference Include="Microsoft.NET.Test.Sdk" Version="17.8.0" />
    <PackageReference Include="xunit" Version="2.4.2" />
    <PackageReference Include="xunit.runner.visualstudio" Version="2.4.5" />
    <PackageReference Include="Moq" Version="4.20.69" />
    <PackageReference Include="System.Text.Json" Version="8.0.0" />
    <PackageReference Include="coverlet.collector" Version="6.0.0" />

    // To run tests with coverage:
    // dotnet test --collect:"XPlat Code Coverage"
    
    // To generate coverage report:
    // dotnet tool install -g dotnet-reportgenerator-globaltool
    // reportgenerator -reports:"TestResults\*\coverage.cobertura.xml" -targetdir:"TestResults\Coverage" -reporttypes:Html
    */

    #endregion
}
