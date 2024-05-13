using Ecommerce.Core.src.ValueObject;

namespace Ecommerce.Core.src.Common
{
    public class LINQParams
    {
        public string SearchKey { get; set; }
        public int SkipFrom { get; set; }
        public SortBy SortBy { get; set; }

        public LINQParams(QueryOptions options)
        {
            SearchKey = options?.SearchKey ?? "";
            SkipFrom = (int)((options?.StartingAfter == null ? 0 : options?.StartingAfter) + 1);
            SortBy = options?.SortBy ?? AppConstants.DEFAULT_SORT_BY;
        }
    }
}
