using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations.Schema;

namespace Webshop.EntityFramework.Data
{
    //This table contains everything for Products
    public class Products
    {
        public int Id { get; set; }
        [Column(TypeName = "nvarchar(1000)")]
        public string ProductName { get; set; }
        public int Quanity { get; set; }
        public int Price { get; set; }
        [Column(TypeName = "nvarchar(1000)")]
        public string[] Tags { get; set; }
        [Column(TypeName = "nvarchar(MAX)")]
        public string DescriptionSerialized { get; set; }
        [NotMapped]
        public string[] Description
        {
            get => string.IsNullOrEmpty(DescriptionSerialized)? new string[0] : JsonConvert.DeserializeObject<string[]>(DescriptionSerialized);
            set =>DescriptionSerialized=JsonConvert.SerializeObject(value);
        }
        public byte[] ImageData { get; set; }
        public string MimeType { get; set; }

        public ICollection<CartItem> CartItems { get; set; } = new List<CartItem>();
    }
}
