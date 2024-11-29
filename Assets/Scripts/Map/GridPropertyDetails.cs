// 使类可以被序列化
[System.Serializable]
public class GridPropertyDetails
{
    // 网格的X坐标
    public int gridX;
    // 网格的Y坐标
    public int gridY;
    // 网格是否可以被挖掘，默认为false
    public bool isDiggable = false;
    // 网格上是否可以掉落物品，默认为false
    public bool canDropItem = false;
    // 网格上是否可以放置家具，默认为false
    public bool canPlaceFurniture = false;
    // 网格是否是路径，默认为false
    public bool isPath = false;
    // 网格是否是NPC障碍物，默认为false
    public bool isNPCObstacle = false;
    // 自挖掘以来的天数，默认为-1
    public int daysSinceDug = -1;
    // 自浇水以来的天数，默认为-1
    public int daysSinceWatered = -1;
    // 种子项目的代码，默认为-1
    public int seedItemCode = -1;
    // 作物生长的天数，默认为-1
    public int growthDays = -1;
    // 上次收获以来的天数，默认为-1
    public int daysSinceLastHarvest = -1;

    // 无参构造函数
    public GridPropertyDetails()
    {
    }
}

/*类定义：

[System.Serializable]：这是一个属性，它指示 GridPropertyDetails 类可以被序列化。序列化是将对象的状态信息转换为可以存储（如文件或内存缓冲区）或传输（如通过网络）的过程。
public class GridPropertyDetails：定义了一个公共类 GridPropertyDetails。
成员变量：

public int gridX：一个公共变量，用于存储网格的X坐标。
public int gridY：一个公共变量，用于存储网格的Y坐标。
public bool isDiggable = false：一个公共变量，表示网格是否可以被挖掘，默认值为 false。
public bool canDropItem = false：一个公共变量，表示网格上是否可以掉落物品，默认值为 false。
public bool canPlaceFurniture = false：一个公共变量，表示网格上是否可以放置家具，默认值为 false。
public bool isPath = false：一个公共变量，表示网格是否是路径，默认值为 false。
public bool isNPCObstacle = false：一个公共变量，表示网格是否是NPC障碍物，默认值为 false。
public int daysSinceDug = -1：一个公共变量，表示自挖掘以来的天数，默认值为 -1，表示尚未挖掘。
public int daysSinceWatered = -1：一个公共变量，表示自浇水以来的天数，默认值为 -1，表示尚未浇水。
public int seedItemCode = -1：一个公共变量，表示种子项目的代码，默认值为 -1，表示没有种植。
public int growthDays = -1：一个公共变量，表示作物生长的天数，默认值为 -1，表示尚未生长。
public int daysSinceLastHarvest = -1：一个公共变量，表示上次收获以来的天数，默认值为 -1，表示尚未收获。
构造函数：

public GridPropertyDetails()：这是一个无参构造函数，用于创建 GridPropertyDetails 类的实例。这个构造函数没有执行任何特定的初始化操作，它只是简单地创建了一个对象。
总的来说，GridPropertyDetails 类用于存储和跟踪游戏世界中每个网格的具体属性和状态，如位置、是否可挖掘、是否可放置物品、生长和收获状态等。这个类通过序列化支持，可以在游戏中保存和加载这些属性。*/