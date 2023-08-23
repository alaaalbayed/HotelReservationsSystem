using Domain.DTO_s;

namespace Domain.Models
{
    public class EditLookUpPropertyViewModel
    {
        public LookUpProperty LookUpProperty { get; set; }
        public IEnumerable<LookUpType> AllLookUpTypes { get; set; }
    }
}
