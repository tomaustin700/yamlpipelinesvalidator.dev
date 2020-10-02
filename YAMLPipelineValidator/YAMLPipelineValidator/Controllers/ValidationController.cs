using System;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using YAMLPipelineValidator.Classes;
using System.Net.Http;
using System.Net.Http.Headers;
using Newtonsoft.Json;

namespace YAMLPipelineValidator.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class ValidationController : ControllerBase
    {

        private readonly IHttpClientFactory _clientFactory;
        public ValidationController(IHttpClientFactory clientFactory)
        {
            _clientFactory = clientFactory;
        }

        [HttpPost]
        public async Task<IActionResult> Validate([FromBody] ValidationItem validationItem)
        {
            if (string.IsNullOrEmpty(validationItem.YAML))
                return BadRequest("YAML Required");

            var escapedYaml = validationItem.YAML.Replace('"', '\"');

            var personalaccesstoken = !string.IsNullOrEmpty(validationItem.PAT) ? validationItem.PAT : Environment.GetEnvironmentVariable("PAT");

            var client = _clientFactory.CreateClient();
            client.DefaultRequestHeaders.Accept.Add(
                    new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));

            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Basic",
                Convert.ToBase64String(
                    Encoding.ASCII.GetBytes(
                        string.Format("{0}:{1}", "", personalaccesstoken))));

            JsonPayload payload = new JsonPayload();
            payload.YamlOverride = escapedYaml;


            var pipelineUrl = !string.IsNullOrEmpty(validationItem.ProjectUrl) && !string.IsNullOrEmpty(validationItem.BuildDefinitionId) ?
                validationItem.ProjectUrl + "_apis/pipelines/" + validationItem.BuildDefinitionId + "/runs?api-version=5.1-preview" :
                Environment.GetEnvironmentVariable("PIPELINE_URL");

            using (var content = new StringContent(JsonConvert.SerializeObject(payload), System.Text.Encoding.UTF8, "application/json"))
            using (HttpResponseMessage response = await client.PostAsync(pipelineUrl, content))
            {
                string responseBody = await response.Content.ReadAsStringAsync();

                if (response.StatusCode == System.Net.HttpStatusCode.BadRequest)
                {
                    var errorResponse = JsonConvert.DeserializeObject<ErrorResponse>(responseBody);
                    return BadRequest(errorResponse.Message.Replace("/Pipelines/azure-pipeline.yaml:", "").Replace("/Pipelines/azure-pipeline.yaml", ""));
                }

            }


            return Ok("Valid YAML Pipeline");
        }


    }
}