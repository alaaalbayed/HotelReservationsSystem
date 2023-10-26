namespace Domain.DTO_s
{
    public class LookUpProperty
    {
        public long Id { get; set; }

        public string NameAr { get; set; }

        public string NameEn { get; set; }

        public string Details { get; set; }

        public long TypeId { get; set; }

        public LookUpType LookUpType { get; set; } = new LookUpType();
    }
}
