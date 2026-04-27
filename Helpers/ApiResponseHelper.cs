namespace ECommerceAPI.Helpers
{
    public static class ApiResponseHelper
    {
        public static object Success(string message)
        {
            return new { success = true, message };
        }

        public static object Fail(string message)
        {
            return new { success = false, message };
        }
    }
}