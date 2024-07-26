using System.ComponentModel.DataAnnotations;

namespace INotificationUser.Domain.Base
{
    public class PagedAndSortedResultRequest:BaseRequest
    {
        public virtual string? Sorting { get; set; }
        public virtual int Page { get; set; } = 1;
        [Range(0, 100)]
        public virtual int Rows { get; set; } = 10;

        public int MaxResultCount() => Rows;

        public int SkipCount() => (Page - 1) * Rows;
    }
}
