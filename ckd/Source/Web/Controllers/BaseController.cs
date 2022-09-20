namespace Web.Controllers;

//[Authorize]
public class BaseController<T> : Controller
{
    protected ILogger<T> logger;

    private string baseUrl = "https://localhost:7108/api/"; //to be moved to settings
    private readonly IConfiguration _config;

    public BaseController(ILogger<T> logger, IConfiguration config)
    {
        this.logger = logger;
        _config = config;
    }
    private string GetApiUrl(string urlConfigName, string queryParameter)
    {
        var url = _config.GetValue<string>(urlConfigName);

        if (!string.IsNullOrEmpty(queryParameter))
        {
            if (!queryParameter.StartsWith("?"))
                queryParameter = "?" + queryParameter;

            url = url + queryParameter;
        }
        return url;
    }

    public async Task<TOut> GetRequest<TOut>(string urlConfigName, string queryParameter)
    {
        var url = GetApiUrl(urlConfigName, queryParameter);
        try
        {
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(baseUrl);
                client.DefaultRequestHeaders.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                var r = await client.GetAsync(url);
                var response = r.Content.ReadAsStringAsync().Result;
                return JsonConvert.DeserializeObject<TOut>(response);

            }
        }
        catch (Exception e)
        {
            logger.LogError(e, "Exception in BaseController.GetRequest to " + url, typeof(T));
            return default(TOut);
        }
    }

    public async Task<TOut> PostRequest<TIn, TOut>(string urlConfigName, string queryParameter, TIn content)
    {
        var url = GetApiUrl(urlConfigName, queryParameter);
        try
        {
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(baseUrl);
                client.DefaultRequestHeaders.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                var serialized = new StringContent(JsonConvert.SerializeObject(content), Encoding.UTF8, "application/json");

                using (HttpResponseMessage response = await client.PostAsync(url, serialized))
                {
                    string responseBody = await response.Content.ReadAsStringAsync();
                    return JsonConvert.DeserializeObject<TOut>(responseBody);
                }
            }
        }
        catch (Exception e)
        {
            logger.LogError(e, "Exception in BaseController.PosttRequest to " + url, typeof(TIn));
            return default(TOut);
        }
    }

}
