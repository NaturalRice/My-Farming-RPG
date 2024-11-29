// 标记ScenePath类为可序列化，这意味着它可以被转换成一系列可以存储或传输的字节
[System.Serializable]
public class ScenePath
{
    // 场景的名称
    public SceneName sceneName;
    // 路径的起始网格坐标
    public GridCoordinate fromGridCell;
    // 路径的结束网格坐标
    public GridCoordinate toGridCell;
}

/*ScenePath类被标记为[System.Serializable]，这意味着它可以被序列化，即将对象的状态信息转换为可以存储或网络传输的格式。

sceneName是一个公开的SceneName类型的变量，用于存储场景的名称。SceneName可能是一个枚举或类，用于标识不同的场景。

fromGridCell是一个公开的GridCoordinate类型的变量，用于存储路径的起始网格坐标。GridCoordinate可能是一个自定义的结构或类，用于表示网格系统中的坐标位置。

toGridCell是一个公开的GridCoordinate类型的变量，用于存储路径的结束网格坐标。

这个类的主要用途是表示从一个网格坐标到另一个网格坐标的路径，这在游戏开发中可能用于AI寻路、玩家导航或其他需要路径信息的场景。通过序列化这个类，可以将路径信息保存到文件中或通过网络传输，这对于游戏的保存/加载机制和网络同步等功能至关重要。*/