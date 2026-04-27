namespace ECommerceAPI.Helpers
{
    public static class InvoiceHelper
    {
        public static decimal CalculateTax(decimal subtotal)
        {
            return subtotal * 0.10m;
        }

        public static decimal CalculateTotal(decimal subtotal)
        {
            return subtotal + CalculateTax(subtotal);
        }
    }
}