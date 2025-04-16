using Newtonsoft.Json;

namespace ConsoleApp42;

public class NovaPostaRequest
{
    [JsonProperty(PropertyName = "apiKey")]
    public string ApiKey { get; init; }

    [JsonProperty(PropertyName = "modelName")]
    public string ModelName { get; init; }

    [JsonProperty(PropertyName = "calledMethod")]
    public string CalledMethod { get; init; }

    [JsonProperty(PropertyName = "methodProperties")]
    public object MethodProperties { get; init; }
}

public class NovaPoshtaMethodProperties
{
    /// <summary>
    /// Номер сторінки
    /// </summary>
    [JsonProperty("AreaRef")]
    public string AreaRef { get; set; }
    public int Page { get; set; } = 1;

    /// <summary>
    /// Кількість населених пунктів за 1 запит
    /// </summary>
    public int Limit { get; set; }
}
