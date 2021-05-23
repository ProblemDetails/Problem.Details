using System.ComponentModel.DataAnnotations;

namespace Sample.WebApi
{
    public class SampleRequest
    {
        [Required]
        public string RequiredField { get; set; }
    }
}