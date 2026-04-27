namespace ECommerceAPI.Helpers
{
    public static class PaginationHelper
    {
        public static int Skip(int pageNumber, int pageSize)
        {
            return (pageNumber - 1) * pageSize;
        }
    }
}