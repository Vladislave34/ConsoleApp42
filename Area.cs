using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using Newtonsoft.Json;

namespace ConsoleApp42;

[Table("area")]
public class AreaEntity
{
    public int Id { get; set; }
    public string Ref { get; set; }
    public string Description { get; set; }
    public ICollection<CityEntity> Cities { get; set; }
}
[Table("city")]
public class CityEntity
{
    public int Id { get; set; }
    public string Ref { get; set; }
    public string Description { get; set; }
    
    [JsonProperty("Area")]
    public string AreaRef { get; set; }

    [JsonIgnore]
    public AreaEntity Area { get; set; }

    public ICollection<WarehouseEntity> Warehouses { get; set; }
}
[Table("house")]
public class WarehouseEntity
{
    public int Id { get; set; }
    public string Ref { get; set; }
    public string Description { get; set; }
    public string CityRef { get; set; }
    public CityEntity City { get; set; }
}
