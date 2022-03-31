namespace Personal.Domain.Dto
{
    public class BlogFilterDto
    {
        public string Category { get; set; }
        public int? PageNo { get; set; } = 1;
        public int? Take { get; set; } = 6;
        public bool OnlyPublished { get; set; }
        public int Skip => (PageNo.Value - 1) * Take.Value;
    }
}
