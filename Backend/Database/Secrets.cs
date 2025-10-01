
using System;
using System.Text;
using Google.Api.Gax.ResourceNames;
using Google.Cloud.SecretManager.V1;
using Google.Protobuf;

public class QuickstartSample
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
}