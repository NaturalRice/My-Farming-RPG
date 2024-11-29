// 引用System.Collections.Generic命名空间，以便使用泛型集合类，如List<T>
using System.Collections.Generic;

// 标记SceneRoute类为可序列化，这意味着它可以被转换成一系列可以存储或传输的字节
[System.Serializable]
public class SceneRoute
{
    // 起始场景的名称
    public SceneName fromSceneName;
    // 目标场景的名称
    public SceneName toSceneName;
    // 存储ScenePath对象的列表，每个对象代表从起始场景到目标场景的一条路径
    public List<ScenePath> scenePathList;
}

/*SceneRoute类被标记为[System.Serializable]，这意味着它可以被序列化，即将对象的状态信息转换为可以存储或网络传输的格式。

fromSceneName是一个公开的SceneName类型的变量，用于存储路径的起始场景名称。SceneName可能是一个枚举或类，用于标识不同的场景。

toSceneName是一个公开的SceneName类型的变量，用于存储路径的目标场景名称。

scenePathList是一个公开的List<ScenePath>类型的变量，用于存储多个ScenePath对象。每个ScenePath对象代表从fromSceneName指定的场景到toSceneName指定的场景的一条具体路径。这个列表允许存在多条路径，可能用于不同的导航场景或条件。

这个类的主要用途是表示从一个场景到另一个场景的多条可能路径，这在游戏开发中可能用于AI寻路、玩家导航或其他需要路径信息的场景。通过序列化这个类，可以将场景间路径信息保存到文件中或通过网络传输，这对于游戏的保存/加载机制和网络同步等功能至关重要。*/