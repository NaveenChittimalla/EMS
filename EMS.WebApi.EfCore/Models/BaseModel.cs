using System.Text.Json.Serialization;

namespace EMS.WebApi.EfCore.Models;
public abstract class BaseModel
{
    [JsonPropertyOrder(-2)]
    public int Id { get; set; }
    public bool Deleted { get; set; }
    public string? CreatedBy { get; set; }
    public DateTime? CreatedDate { get; set; }
    public string? ModifiedBy { get; set; }
    public DateTime? ModifiedDate { get; set; }
}
