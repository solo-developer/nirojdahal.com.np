namespace Personal.Domain.Dto
{
    public class PagedResultDto
    {
        public int TotalRecords { get; set; }

        public int Skip { get; set; }

        public int CurrentPageNumber => (Skip / Take) + 1;

        public int Take { get; set; }

        public object Data { get; set; }
    }
}
