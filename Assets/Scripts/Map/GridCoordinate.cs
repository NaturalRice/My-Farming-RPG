using UnityEngine; // 引用Unity引擎的命名空间

[System.Serializable] // 标记这个类为可序列化，使其可以被保存或传输
public class GridCoordinate // 定义一个公共类GridCoordinate
{
    public int x; // 公开字段，存储网格坐标的x值
    public int y; // 公开字段，存储网格坐标的y值

    public GridCoordinate(int p1, int p2) // 类的构造函数，用于初始化网格坐标
    {
        x = p1; // 设置x值为传入的参数p1
        y = p2; // 设置y值为传入的参数p2
    }

    public static explicit operator Vector2(GridCoordinate gridCoordinate) // 显式转换操作符，将GridCoordinate转换为Vector2
    {
        return new Vector2((float)gridCoordinate.x, (float)gridCoordinate.y); // 返回一个新的Vector2对象，其值来自GridCoordinate的x和y
    }

    public static explicit operator Vector2Int(GridCoordinate gridCoordinate) // 显式转换操作符，将GridCoordinate转换为Vector2Int
    {
        return new Vector2Int(gridCoordinate.x, gridCoordinate.y); // 返回一个新的Vector2Int对象，其值来自GridCoordinate的x和y
    }

    public static explicit operator Vector3(GridCoordinate gridCoordinate) // 显式转换操作符，将GridCoordinate转换为Vector3
    {
        return new Vector3((float)gridCoordinate.x, (float)gridCoordinate.y, 0f); // 返回一个新的Vector3对象，其值来自GridCoordinate的x和y，z值为0
    }

    public static explicit operator Vector3Int(GridCoordinate gridCoordinate) // 显式转换操作符，将GridCoordinate转换为Vector3Int
    {
        return new Vector3Int(gridCoordinate.x, gridCoordinate.y, 0); // 返回一个新的Vector3Int对象，其值来自GridCoordinate的x和y，z值为0
    }
}

/*GridCoordinate 类：这个类表示网格中的一个坐标点，包含两个整数字段x和y，分别表示坐标的横纵值。

构造函数：GridCoordinate类的构造函数接受两个整数参数p1和p2，分别用于初始化x和y字段。

显式转换操作符：

operator Vector2：将GridCoordinate对象显式转换为Vector2对象。这是通过将x和y字段的值转换为浮点数并传递给Vector2构造函数来实现的。
operator Vector2Int：将GridCoordinate对象显式转换为Vector2Int对象。这是通过直接将x和y字段的值传递给Vector2Int构造函数来实现的。
operator Vector3：将GridCoordinate对象显式转换为Vector3对象。这是通过将x和y字段的值转换为浮点数，并设置z值为0，然后传递给Vector3构造函数来实现的。
operator Vector3Int：将GridCoordinate对象显式转换为Vector3Int对象。这是通过将x和y字段的值传递给Vector3Int构造函数，并设置z值为0来实现的。
这个类提供了一种方便的方式来在网格坐标和Unity的向量类型之间进行转换，这在游戏开发中处理网格布局和位置时非常有用。通过这种方式，开发者可以轻松地将网格坐标转换为Unity引擎中使用的向量类型，从而简化代码和提高效率。*/