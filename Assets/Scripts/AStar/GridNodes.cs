using UnityEngine; // 引用Unity引擎的命名空间

public class GridNodes // 定义一个公共类
{
    private int width; // 私有字段，存储网格的宽度
    private int height; // 私有字段，存储网格的高度

    private Node[,] gridNode; // 私有字段，存储网格节点的二维数组

    public GridNodes(int width, int height) // 类的构造函数，用于初始化网格的宽度和高度
    {
        this.width = width; // 设置网格的宽度
        this.height = height; // 设置网格的高度

        gridNode = new Node[width, height]; // 创建一个Node类型的二维数组

        for (int x = 0; x < width; x++) // 遍历宽度
        {
            for (int y = 0; y < height; y++) // 遍历高度
            {
                gridNode[x, y] = new Node(new Vector2Int(x, y)); // 为每个位置创建一个新的Node对象，并初始化其网格位置
            }
        }
    }

    public Node GetGridNode(int xPosition, int yPosition) // 公共方法，用于获取指定位置的网格节点
    {
        if (xPosition < width && yPosition < height) // 如果请求的位置在网格范围内
        {
            return gridNode[xPosition, yPosition]; // 返回对应的网格节点
        }
        else
        {
            Debug.Log("Requested grid node is out of range"); // 如果请求的位置超出网格范围，在控制台输出日志
            return null; // 返回null
        }
    }
}

/*GridNodes 类：这个类用于表示一个网格，其中每个网格位置都有一个对应的Node对象。这个类是路径查找算法中管理网格节点的基础。

构造函数：GridNodes类的构造函数接受两个参数，width和height，分别用于设置网格的宽度和高度，并初始化一个Node对象的二维数组。

Node 对象的初始化：在构造函数中，使用两个嵌套的for循环遍历网格的每个位置，并为每个位置创建一个新的Node对象，初始化其gridPosition属性为当前的x和y坐标。

GetGridNode 方法：这个方法接受两个参数，xPosition和yPosition，用于获取网格中指定位置的Node对象。如果请求的位置在网格范围内，则返回对应的Node对象；如果超出范围，则在Unity的控制台输出一条日志消息，并返回null。

这个类是实现A*算法时管理网格节点的关键组件，它提供了一个简单的方式来访问和操作网格中的每个节点。通过这种方式，算法可以评估每个节点的成本，更新路径，并找到从起点到终点的最短路径。*/