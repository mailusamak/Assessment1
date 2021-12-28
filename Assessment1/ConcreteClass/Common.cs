using Azure.Storage.Blobs;
using Microsoft.Azure.KeyVault;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Clients.ActiveDirectory;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace Assessment1.ConcreteClass
{
    public class Common
    {
        static IConfiguration Configuration = (new ConfigurationBuilder().SetBasePath(Directory.GetCurrentDirectory()).AddJsonFile("appsettings.json").Build());
        //private IConfiguration Configuration { get; }
        public static bool IsDateTime(string tempDate)
        {
            DateTime fromDateValue;
            var formats = new[] { "MM/dd/yyyy", "dd/MM/yyyy h:mm:ss", "MM/dd/yyyy hh:mm tt", "yyyy'-'MM'-'dd'T'HH':'mm':'ss" };
            bool b = DateTime.TryParseExact(tempDate, formats, System.Globalization.CultureInfo.InvariantCulture, System.Globalization.DateTimeStyles.None, out fromDateValue);
            return b;
        }

        public static void UploadFilesInContainer(BlobContainerClient blobContainerClient, string[] files)
        {
            foreach (var file in files)
            {
                using (MemoryStream stream = new MemoryStream(System.IO.File.ReadAllBytes(file)))
                {
                    blobContainerClient.UploadBlob(Path.GetFileName(file), stream);
                }
            }
        }

        public static List<string> GetExistFileName(BlobContainerClient blobContainerClient, string[] files)
        {
            List<string> lstFileNameExist = new List<string>();
            foreach (var file in files)
            {
                BlobClient blobClient = blobContainerClient.GetBlobClient(Path.GetFileName(file));
                if (blobClient.Exists())
                    lstFileNameExist.Add(Path.GetFileName(file));
            }
            return lstFileNameExist;
        }

        public string getConnectionStringFromSecret()
        {
            string url = Configuration.GetValue<string>("KeyVault:url");
            string clientId = Configuration.GetValue<string>("KeyVault:ClientId");
            string clientSecret = Configuration.GetValue<string>("KeyVault:ClientSecret");
            var _keyVaultClient = new KeyVaultClient(
            async (string authority, string resource, string scope) =>
            {
                var authContext = new AuthenticationContext(authority);
                //var clientCred = new ClientCredential("73f718ec-faa9-4b40-bc9b-0854988b3cdd", "~Ye7Q~.bP~fMWe5f3EsD8-mISKoByzbvTrhoj");
                var clientCred = new ClientCredential(clientId, clientSecret);
                var result = await authContext.AcquireTokenAsync(resource, clientCred);
                return result.AccessToken;
            });

            //_keyVaultClient.SetSecretAsync(url, "Password", "This is my password");

            var result = _keyVaultClient.GetSecretAsync(url, "SecretKey-AssesmentOne-ConnectionString").GetAwaiter().GetResult();
            return result.Value;
        }

        public string getConnectionStringFromSecretForFileUploadContainer()
        {
            string url = Configuration.GetValue<string>("KeyVault:url");
            string clientId = Configuration.GetValue<string>("KeyVault:ClientId");
            string clientSecret = Configuration.GetValue<string>("KeyVault:ClientSecret");
            var _keyVaultClient = new KeyVaultClient(
            async (string authority, string resource, string scope) =>
            {
                var authContext = new AuthenticationContext(authority);
                //var clientCred = new ClientCredential("73f718ec-faa9-4b40-bc9b-0854988b3cdd", "~Ye7Q~.bP~fMWe5f3EsD8-mISKoByzbvTrhoj");
                var clientCred = new ClientCredential(clientId, clientSecret);
                var result = await authContext.AcquireTokenAsync(resource, clientCred);
                return result.AccessToken;
            });

            //_keyVaultClient.SetSecretAsync(url, "Password", "This is my password");

            var result = _keyVaultClient.GetSecretAsync(url, "Secret-Key-File-Upload-Container").GetAwaiter().GetResult();
            return result.Value;
        }

        public static string GetConfigValue(string key)
        {
            string value = Configuration.GetValue<string>(key);
            return value;
        }
    }
}
