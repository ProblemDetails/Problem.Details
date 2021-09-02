using System.ComponentModel.DataAnnotations;

namespace Sample.WebApi
{
    public class SampleRequest
    {
        [Required]
        public string SomeField { get; set; }
    }
}