using System.Text;
using ConsoleApp42;

using Newtonsoft.Json;




//Console.InputEncoding = Encoding.Unicode;
//Console.OutputEncoding = Encoding.Unicode;

string apiKey = "27e98316d8976226c4d4185fa2650f10";

////Хочу отримтаи список областей

//var model = new NovaPostaRequest
//{
//    ApiKey = apiKey,
//    ModelName = "Address",
//    CalledMethod = "getSettlementAreas",
//    MethodProperties = new() { Page = 1, Limit = 200 }
//};

//string json = JsonConvert.SerializeObject(model); //перетворює модел у json

//string url = "https://api.novaposhta.ua/v2.0/json/ ";

//HttpClient client = new();

//HttpContent content = new StringContent(json, Encoding.UTF8, "application/json");
//HttpResponseMessage resp = await client.PostAsync(url, content);
//if (resp.IsSuccessStatusCode)
//{
//    var respJson = await resp.Content.ReadAsStringAsync();

//    if (respJson is not null)
//    {
//        var areasData = JsonConvert.DeserializeObject<NovaPoshtaResponse<AreaEntity>>(respJson);
//        if(areasData is not null) { 
//        foreach(var area in areasData.Data)
//        {
//                Console.WriteLine($"{area.Ref}: {area.Description}");
//        }
//        }
//    }

//    Console.WriteLine("Result {0}", respJson);
//}
//else
//{
//    Console.WriteLine("Помилка запиту");
//}





//Console.WriteLine(Guid.NewGuid().ToString());

async Task<NovaPoshtaResponse<T>> PostToNovaPoshtaAsync<T>(object model)
{
    using HttpClient client = new();
    var json = JsonConvert.SerializeObject(model);
    var content = new StringContent(json, Encoding.UTF8, "application/json");

    var resp = await client.PostAsync("https://api.novaposhta.ua/v2.0/json/", content);

    if (resp.IsSuccessStatusCode)
    {
        var respJson = await resp.Content.ReadAsStringAsync();
        return JsonConvert.DeserializeObject<NovaPoshtaResponse<T>>(respJson);
    }

    return null;
}
var areasModel = new NovaPostaRequest
{
    ApiKey = apiKey,
    ModelName = "Address",
    CalledMethod = "getSettlementAreas",
    MethodProperties = new NovaPoshtaMethodProperties { Page = 1, Limit = 200 }
};

var areaResponse = await PostToNovaPoshtaAsync<AreaEntity>(areasModel);
var areas = areaResponse?.Data.ToList() ?? new();
MyAppContext dbContext = new MyAppContext();

await dbContext.Areas.AddRangeAsync(areas.Select(a => new AreaEntity
{
    Ref = a.Ref,
    Description = a.Description
}));

await dbContext.SaveChangesAsync();

Parallel.ForEach(areas, new ParallelOptions { MaxDegreeOfParallelism = 10 }, async area =>
{
    for (int page = 1; page <= 10; page++) 
    {
        var cityRequest = new NovaPostaRequest
        {
            ApiKey = apiKey,
            ModelName = "Address",
            CalledMethod = "getSettlements",
            MethodProperties = new NovaPoshtaMethodProperties
            {
                AreaRef = area.Ref,
                Page = page,
                Limit = 100
            }
        };

        var citiesResponse = await PostToNovaPoshtaAsync<CityEntity>(cityRequest);

        if (citiesResponse?.Data != null)
        {
            lock (dbContext)
            {
                dbContext.Cities.AddRange(citiesResponse.Data.Select(c => new CityEntity
                {
                    Ref = c.Ref,
                    Description = c.Description,
                    AreaRef = area.Ref
                }));
                dbContext.SaveChanges();
            }
        }
    }
});

