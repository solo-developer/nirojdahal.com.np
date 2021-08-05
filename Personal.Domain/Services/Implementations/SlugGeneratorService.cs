using Personal.Domain.Exceptions;
using Personal.Domain.Services.Interface;

namespace Personal.Domain.Services.Implementations
{
    public class SlugGeneratorService : ISlugGeneratorService
    {
        public string Generate(string title)
        {
            title = title.Length >= 200 ? title.Substring(0, 200) : title;

            if (string.IsNullOrWhiteSpace(title))
            {
                throw new NonEmptyValueException("Slug title is required.");
            }
            title = title.ToLower();
            title = title.Replace(" ", "-");
            return title;
        }
    }
}
