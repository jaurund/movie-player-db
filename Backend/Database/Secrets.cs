
using System;
using System.Text;
using Google.Api.Gax.ResourceNames;
using Google.Cloud.SecretManager.V1;
using Google.Protobuf;

/*public class QuickstartSample
{
    public void Quickstart(string projectId = "my-project", string secretId = "my-secret")
    {
        // Create the client.
        SecretManagerServiceClient client = SecretManagerServiceClient.Create();

        // Build the parent project name.
        ProjectName projectName = new ProjectName(projectId);

        // Build the secret to create.
        Secret secret = new Secret
        {
            Replication = new Replication
            {
                Automatic = new Replication.Types.Automatic(),
            },
        };

        Secret createdSecret = client.CreateSecret(projectName, secretId, secret);

        // Build a payload.
        SecretPayload payload = new SecretPayload
        {
            Data = ByteString.CopyFrom("my super secret data", Encoding.UTF8),
        };

        // Add a secret version.
        SecretVersion createdVersion = client.AddSecretVersion(createdSecret.SecretName, payload);

        // Access the secret version.
        AccessSecretVersionResponse result = client.AccessSecretVersion(createdVersion.SecretVersionName);

        // Print the results
        //
        // WARNING: Do not print secrets in production environments. This
        // snippet is for demonstration purposes only.
        string data = result.Payload.Data.ToStringUtf8();
        Console.WriteLine($"Plaintext: {data}");
    }
}*/

// Example within your C# REST API controller
[ApiController]
[Route("[controller]")]
public class VideoController : ControllerBase
{
    private readonly SecretAccessor _secretAccessor; // Inject this
    private readonly IHttpClientFactory _httpClientFactory; // Inject this

    public VideoController(SecretAccessor secretAccessor, IHttpClientFactory httpClientFactory)
    {
        _secretAccessor = secretAccessor;
        _httpClientFactory = httpClientFactory;
    }

    [HttpGet("stream/{videoId}")]
    public async Task<IActionResult> StreamVideo(string videoId)
    {
        string homeServerAddress = await _secretAccessor.GetSecretValueAsync(
            "SECRET", // Your Project ID
            "IPv4_port_streaming_service" // Your Secret ID
        );

        if (string.IsNullOrEmpty(homeServerAddress))
        {
            return StatusCode(500, "Could not retrieve home server address.");
        }

        // Assuming the secret is "192.168.1.100:8080"
        // You might need more robust parsing depending on your secret format
        string[] parts = homeServerAddress.Split(':');
        if (parts.Length != 2)
        {
            return StatusCode(500, "Invalid home server address format in secret.");
        }
        string ip = parts[0];
        string port = parts[1];

        // Construct the internal URL to your home server
        string homeServerVideoUrl = $"http://{ip}:{port}/videos/{videoId}"; // Adjust path as needed

        var httpClient = _httpClientFactory.CreateClient();
        var response = await httpClient.GetAsync(homeServerVideoUrl, HttpCompletionOption.ResponseHeadersRead);

        if (!response.IsSuccessStatusCode)
        {
            return StatusCode((int)response.StatusCode, $"Error fetching video from home server: {response.ReasonPhrase}");
        }

        // Stream the content directly back to the client
        return new FileStreamResult(await response.Content.ReadAsStreamAsync(), response.Content.Headers.ContentType.ToString())
        {
            EnableRangeProcessing = true // Important for video streaming
        };
    }
}
