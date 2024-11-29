using UnityEngine; // 引用Unity引擎的命名空间

[System.Serializable] // 标记这个类为可序列化，使其可以被保存或传输
public class ItemDetails // 定义一个公共类ItemDetails
{
    public int itemCode; // 公开字段，存储物品的代码或标识符
    public ItemType itemType; // 公开字段，存储物品的类型
    public string itemDescription; // 公开字段，存储物品的简短描述
    public Sprite itemSprite; // 公开字段，存储物品的精灵图
    public string itemLongDescription; // 公开字段，存储物品的详细描述
    public short itemUseGridRadius; // 公开字段，存储物品使用时的网格半径
    public float itemUseRadius; // 公开字段，存储物品使用时的半径
    public bool isStartingItem; // 公开字段，标记物品是否为起始物品
    public bool canBePickedUp; // 公开字段，标记物品是否可以被拾取
    public bool canBeDropped; // 公开字段，标记物品是否可以被丢弃
    public bool canBeEaten; // 公开字段，标记物品是否可以被食用
    public bool canBeCarried; // 公开字段，标记物品是否可以被携带
}

/*ItemDetails 类：这个类用于表示游戏中一个物品的详细信息，继承自System.Object。

字段：

itemCode：一个整数字段，用于唯一标识一个物品。
itemType：一个ItemType枚举字段，用于分类物品的类型。
itemDescription：一个字符串字段，提供物品的简短描述。
itemSprite：一个Sprite字段，关联物品的图像或图标。
itemLongDescription：一个字符串字段，提供物品的详细描述。
itemUseGridRadius：一个短整型字段，定义物品使用时影响的网格半径。
itemUseRadius：一个浮点型字段，定义物品使用时影响的实际半径。
isStartingItem：一个布尔型字段，指示物品是否为玩家起始时拥有的物品。
canBePickedUp：一个布尔型字段，指示物品是否可以被玩家拾取。
canBeDropped：一个布尔型字段，指示物品是否可以被玩家丢弃。
canBeEaten：一个布尔型字段，指示物品是否可以被玩家食用。
canBeCarried：一个布尔型字段，指示物品是否可以被玩家携带。
这个类是游戏中物品系统的基础，它提供了一个结构化的方式来存储和管理游戏中各种物品的属性和行为。通过这种方式，开发者可以轻松地为游戏中添加新物品，并定义它们的特性和功能。*/