// 使用System.Collections.Generic命名空间，它提供了泛型集合类，如List<T>和Dictionary<K,V>
using System.Collections.Generic;

// 标记GameSave类为可序列化，这意味着它可以被转换成一系列可以存储或传输的字节
[System.Serializable]
public class GameSave
{
    // 字典，其键为字符串类型，值为GameObjectSave类型，用于存储不同游戏对象的保存数据
    // string key = GUID gameobject ID
    public Dictionary<string, GameObjectSave> gameObjectData;

    // 无参数的构造函数，初始化gameObjectData字典
    public GameSave()
    {
        gameObjectData = new Dictionary<string, GameObjectSave>();
    }
}

/*GameSave类用于存储整个游戏的保存数据。它包含一个名为gameObjectData的字典，该字典的键是字符串（代表游戏对象的唯一标识符，GUID），值是GameObjectSave类型（可能是另一个定义了单个游戏对象保存数据的类）。

GameSave类有一个无参数的构造函数：

这个构造函数在创建GameSave对象时初始化gameObjectData字典。这样，每次创建GameSave对象时，都会创建一个新的空字典来存储游戏对象的保存数据。
[System.Serializable]属性标记了GameSave类为可序列化的，这意味着它可以被转换成一系列字节，以便存储到文件中或通过网络传输。这是在游戏开发中常用的特性，用于保存和加载游戏状态。

这个类可能是游戏保存和加载系统的核心部分，用于在游戏会话之间存储和恢复整个游戏的状态。通过将游戏对象的GUID作为字典的键，可以方便地管理和访问不同游戏对象的保存数据。这种设计允许游戏开发者为每个游戏对象保存定制的数据，同时保持整个游戏状态的组织和可维护性。*/