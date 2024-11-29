// 使类可以被序列化
[System.Serializable]
public class GridProperty
{
    // 网格的坐标
    public GridCoordinate gridCoordinate;
    // 网格的布尔属性类型
    public GridBoolProperty gridBoolProperty;
    // 布尔属性的值，默认为false
    public bool gridBoolValue = false;

    // 构造函数，用于创建GridProperty对象时初始化属性
    public GridProperty(GridCoordinate gridCoordinate, GridBoolProperty gridBoolProperty, bool gridBoolValue)
    {
        this.gridCoordinate = gridCoordinate;
        this.gridBoolProperty = gridBoolProperty;
        this.gridBoolValue = gridBoolValue;
    }
}

/*类定义：

[System.Serializable]：这是一个属性，它指示 GridProperty 类可以被序列化。序列化是将对象的状态信息转换为可以存储（如文件或内存缓冲区）或传输（如通过网络）的过程。
public class GridProperty：定义了一个公共类 GridProperty。
成员变量：

public GridCoordinate gridCoordinate：一个公共变量，用于存储网格的坐标。GridCoordinate 可能是一个定义了网格位置（如x和y坐标）的另一个类或结构体。
public GridBoolProperty gridBoolProperty：一个公共变量，用于存储网格的布尔属性类型。GridBoolProperty 可能是一个枚举，定义了不同的布尔属性类型，例如是否可挖掘、是否可以放置物品等。
public bool gridBoolValue = false：一个公共变量，用于存储布尔属性的值，默认为 false。
构造函数：

public GridProperty(GridCoordinate gridCoordinate, GridBoolProperty gridBoolProperty, bool gridBoolValue)：这是一个构造函数，它允许在创建 GridProperty 对象时初始化其属性。
this.gridCoordinate = gridCoordinate：将传入的 gridCoordinate 参数赋值给对象的 gridCoordinate 成员变量。
this.gridBoolProperty = gridBoolProperty：将传入的 gridBoolProperty 参数赋值给对象的 gridBoolProperty 成员变量。
this.gridBoolValue = gridBoolValue：将传入的 gridBoolValue 参数赋值给对象的 gridBoolValue 成员变量。
总的来说，GridProperty 类用于表示游戏世界中一个特定位置的网格的布尔属性，例如是否可以挖掘、是否可以放置物品等。这个类通过其构造函数允许在创建对象时设置这些属性，并通过序列化支持，可以在游戏中保存和加载这些属性。*/