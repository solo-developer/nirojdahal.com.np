namespace Personal.Domain.Dto
{
    public class BlogFilterDto
    {
        public string Category { get; set; }
        public int PageNo { get; set; } = 1;
        public int? Take { get; set; }
        public bool OnlyPublished { get; set; }
        public int Skip => Take.HasValue ? (PageNo - 1) * Take.Value:0;
    }
}
