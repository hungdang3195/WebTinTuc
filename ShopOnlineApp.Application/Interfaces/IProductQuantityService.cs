namespace ShopOnlineApp.Application.Interfaces
{
    public interface IProductQuantityService
    {
        bool SellProduct(int productId, int quantity);
        void UpdateQuantityProduct(int productId, int quantity);
    }
}
