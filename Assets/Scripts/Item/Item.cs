using UnityEngine; // 引用Unity引擎的命名空间

public class Item : MonoBehaviour // 定义一个公共类Item，继承自MonoBehaviour
{
    [ItemCodeDescription] // 自定义属性，可能用于显示物品代码的描述
    [SerializeField] // 标记为可序列化，使其在Unity编辑器中可见
    private int _itemCode; // 私有字段，存储物品的代码或标识符

    private SpriteRenderer spriteRenderer; // 私有字段，存储物品的精灵渲染器

    public int ItemCode // 公开属性，提供对_itemCode字段的访问
    {
        get { return _itemCode; } // 获取_itemCode的值
        set { _itemCode = value; } // 设置_itemCode的值
    }

    private void Awake() // 在对象被创建时调用
    {
        spriteRenderer = GetComponentInChildren<SpriteRenderer>(); // 获取子对象中的SpriteRenderer组件
    }

    private void Start() // 在对象启用时调用
    {
        if (ItemCode != 0) // 如果ItemCode不为0
        {
            Init(ItemCode); // 调用Init方法初始化物品
        }
    }

    public void Init(int itemCodeParam) // 公共方法，用于初始化物品
    {
        if (itemCodeParam != 0) // 如果传入的物品代码不为0
        {
            ItemCode = itemCodeParam; // 设置ItemCode

            ItemDetails itemDetails = InventoryManager.Instance.GetItemDetails(ItemCode); // 从库存管理器获取物品详情

            spriteRenderer.sprite = itemDetails.itemSprite; // 设置物品的精灵图

            // 如果物品类型是可收割的，则添加可推动组件
            if (itemDetails.itemType == ItemType.Reapable_scenary) // 检查物品类型是否为可收割
            {
                gameObject.AddComponent<ItemNudge>(); // 为游戏对象添加ItemNudge组件
            }
        }
    }
}

/*Item 类：这个类表示游戏中的一个物品，继承自MonoBehaviour，可以附加到Unity场景中的GameObject上。

字段：

_itemCode：一个私有字段，用于存储物品的唯一代码或标识符。
spriteRenderer：一个私有字段，用于存储物品的精灵渲染器组件。
ItemCode 属性：一个公开属性，提供对_itemCode字段的访问。

Awake 方法：在对象被创建时调用，用于获取子对象中的SpriteRenderer组件。

Start 方法：在对象启用时调用，如果ItemCode不为0，则调用Init方法初始化物品。

Init 方法：一个公共方法，用于初始化物品。它设置ItemCode，获取物品详情，并根据物品类型添加相应的组件（如ItemNudge）。

这个类通常用于游戏中物品的表示和管理，通过ItemCode可以唯一标识每个物品，并根据物品的类型和属性进行相应的初始化和配置。通过这种方式，游戏可以方便地管理和渲染各种不同的物品。*/