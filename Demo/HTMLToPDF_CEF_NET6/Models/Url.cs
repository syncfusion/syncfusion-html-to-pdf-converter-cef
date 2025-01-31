using System.ComponentModel.DataAnnotations;

namespace HTMLToPDF_CEF_NET6.Models
{
    public class Url
    {
        [Required]
        public string ConversionURL { get; set; }
    }
}
