using Takamol.Domain.Entities;

namespace Takamol.Domain.Entities
{
    public class Client:BaseEntity

    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string Address { get; set; }
        public string Description { get; set; }
        public string JobTitle { get; set; }
        public string ClientClassification { get; set; }
        public string ClientSource { get; set; }
        public string SalesMan { get; set; }


    }
}
