namespace ChicTrash;

public class Cart
{
    private int _itemId;
    private double _itemPrice;
    private int _itemQuantity;
    private string _itemName;
    private string _itemCategory;
    private string _itemImage;
    private string _itemDescription;

    public int itemId
    {
        get => _itemId;
        set => _itemId = value;
    }

    public double itemPrice
    {
        get => _itemPrice;
        set => _itemPrice = value;
    }

    public int itemQuantity
    {
        get { return _itemQuantity; }
        set => _itemQuantity = value;
    }

    public string itemName
    {
        get { return _itemName; }
        set { _itemName = value; }
    }

    public string itemCategory
    {
        get => _itemCategory;
        set => _itemCategory = value;
    }

    public string itemImage
    {
        get => _itemImage;
        set => _itemImage = value;
    }

    public string itemDescription
    {
        get { return _itemDescription; }
        set { _itemDescription = value; }
    }
}
