using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;

namespace IndxCloudLoader
{
    internal static partial class Program
    {
        #region Public Classes
        public class JWT
        {
            #region Public Properties
            [System.Diagnostics.CodeAnalysis.SuppressMessage("Style", "IDE1006:Naming Styles", Justification = "<Pending>")]
            public string token { get; set; }
            #endregion Public Properties

            // Do not use upper case convention here, rename to "Token" will make login fail
        }
        #endregion Public Classes

        #region Internal Methods

        internal static async Task<bool> ChangePassword(HttpClient client, string currentPassword, string newPassword)
        {
            var request = new { currentPassword, newPassword };
            var jsonContent = new StringContent(JsonSerializer.Serialize(request), Encoding.UTF8, "application/json");
            var response = await client.PostAsync("api/changePassword", jsonContent);
            return response.IsSuccessStatusCode;
        }

        internal static void SetBearerToken(HttpClient client, string bearerToken)
        {
            client.BaseAddress = new Uri(uri);
            var contentType = new MediaTypeWithQualityHeaderValue("application/json");
            client.DefaultRequestHeaders.Accept.Add(contentType);
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", bearerToken);
        }

        internal static string Login(HttpClient client)
        {
            var stringJWT = "";
            client.BaseAddress = new Uri(uri);
            var contentType = new MediaTypeWithQualityHeaderValue("application/json");
            client.DefaultRequestHeaders.Accept.Add(contentType);

            try
            {
                // Create the login credentials object
                var loginInfo = new
                {
                    UserEmail = userEmail,
                    UserPassWord = userPassword
                };

                // Serialize to JSON
                var jsonContent = JsonSerializer.Serialize(loginInfo);
                var httpContent = new StringContent(jsonContent, Encoding.UTF8, "application/json");

                // Send POST request with body
                HttpResponseMessage response = client.PostAsync("api/login", httpContent).Result;
                stringJWT = response.Content.ReadAsStringAsync().Result;
                if (!response.IsSuccessStatusCode)
                {
                    Console.WriteLine($"Error: {response.StatusCode}");
                    Console.WriteLine($"Response: {stringJWT}");
                    throw new Exception($"Login failed: {stringJWT}");
                }

            }
            catch (Exception)
            {
                throw;
            }

            JWT jwt = JsonSerializer.Deserialize<JWT>(stringJWT);
            if (jwt != null)
            {
                // Invariant: Login succeeded, now apply JWT token
                client.DefaultRequestHeaders.Accept.Add(contentType);
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", jwt.token);
                return jwt.token;
            }
            return null;
        }
        #endregion Internal Methods
    }
}