using Personal.Domain.Enums;

namespace Personal.Domain.Services.Interface
{
    public interface ILastReadService
    {
        void Record(ReadableTableKeys key, string userId);
    }
}
