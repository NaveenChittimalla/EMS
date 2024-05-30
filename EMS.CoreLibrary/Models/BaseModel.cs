namespace EMS.CoreLibrary.Models
{
    public abstract class BaseModel
    {
        public int Id { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime LastModdifiedDate { get; set; }

        public string Print()
        {
            return string.Concat(Id, CreatedDate, LastModdifiedDate);
        }
    }
}
