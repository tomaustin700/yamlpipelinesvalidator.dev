using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using API.Classes;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Http;
using Microsoft.Extensions.Logging;

namespace API
{
    public class API
    {

        private readonly IHttpClientFactory _clientFactory;
        public API(IHttpClientFactory clientFactory)
        {
            _clientFactory = clientFactory;
        }

        [Function("Validate")]
        public async Task<HttpResponseData> Run([HttpTrigger(AuthorizationLevel.Anonymous, "Validate")] HttpRequestData req)
        {
            var serializeOptions = new JsonSerializerOptions
            {
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
                WriteIndented = true,
                PropertyNameCaseInsensitive = true
            };

            var validationItem = await JsonSerializer.DeserializeAsync<ValidationItem>(req.Body, serializeOptions);


            if (string.IsNullOrEmpty(validationItem.YAML))
            {
                var badResponse = req.CreateResponse(HttpStatusCode.BadRequest);
                badResponse.WriteString("YAML required");
                return badResponse;

            }

            var escapedYaml = validationItem.YAML.Replace('"', '\"');

            var personalaccesstoken = Environment.GetEnvironmentVariable("PAT");

            var client = _clientFactory.CreateClient();
            client.DefaultRequestHeaders.Accept.Add(
                    new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));

            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Basic",
                Convert.ToBase64String(
                    Encoding.ASCII.GetBytes(
                        string.Format("{0}:{1}", "", personalaccesstoken))));

            JsonPayload payload = new JsonPayload();
            payload.YamlOverride = escapedYaml;

            using (var content = new StringContent(JsonSerializer.Serialize(payload), System.Text.Encoding.UTF8, "application/json"))
            using (HttpResponseMessage response = await client.PostAsync(
                        Environment.GetEnvironmentVariable("PIPELINE_URL"), content))
            {
                string responseBody = await response.Content.ReadAsStringAsync();

                if (response.StatusCode == System.Net.HttpStatusCode.BadRequest)
                {
                    var errorResponse = JsonSerializer.Deserialize<ErrorResponse>(responseBody, serializeOptions);
                    var badResponse = req.CreateResponse(HttpStatusCode.BadRequest);
                    badResponse.WriteString(errorResponse.Message.Replace("/Pipelines/azure-pipeline.yaml:", "").Replace("/Pipelines/azure-pipeline.yaml", ""));
                    return badResponse;
                }

            }


            var okReponse = req.CreateResponse(HttpStatusCode.OK);
            okReponse.WriteString("Valid YAML Pipeline");
            return okReponse;
        }
    }
}
