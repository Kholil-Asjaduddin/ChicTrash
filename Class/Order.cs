namespace ChicTrash;

public class Order
{
    private int _orderId;
    private int _productId;
    private int _quantity;
    private string _productName;
    private string _address;
    
    public int OrderId { get => _orderId; set => _orderId = value; }
    public int ProductId { get => _productId; set => _productId = value; }
    public int Quantity { get => _quantity; set => _quantity = value; }
    public string ProductName { get => _productName; set => _productName = value; }
    public string Address { get => _address; set => _address = value; }

}