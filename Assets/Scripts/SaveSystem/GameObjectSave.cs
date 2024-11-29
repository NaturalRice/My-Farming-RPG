// 使用System.Collections.Generic命名空间，它提供了泛型集合类，如List<T>和Dictionary<K,V>
using System.Collections.Generic;

// 标记GameObjectSave类为可序列化，这意味着它可以被转换成一系列可以存储或传输的字节
[System.Serializable]
public class GameObjectSave
{
    // 字典，其键为字符串类型，值为SceneSave类型，用于存储不同场景的保存数据
    // string key = scene name
    public Dictionary<string, SceneSave> sceneData;

    // 无参数的构造函数，初始化sceneData字典
    public GameObjectSave()
    {
        sceneData = new Dictionary<string, SceneSave>();
    }

    // 带有一个Dictionary参数的构造函数，允许在创建GameObjectSave对象时传入一个已经存在的字典
    public GameObjectSave(Dictionary<string, SceneSave> sceneData)
    {
        this.sceneData = sceneData;
    }
}

/*GameObjectSave类用于存储游戏对象的保存数据。它包含一个名为sceneData的字典，该字典的键是字符串（代表场景名称），值是SceneSave类型（可能是另一个定义了场景保存数据的类）。

GameObjectSave类有两个构造函数：

第一个构造函数是无参数的，当创建GameObjectSave对象时，会初始化sceneData字典。
第二个构造函数接受一个Dictionary<string, SceneSave>类型的参数，允许在创建GameObjectSave对象时传入一个已经存在的字典，这样可以在不同的上下文中重用已有的数据。
[System.Serializable]属性标记了GameObjectSave类为可序列化的，这意味着它可以被转换成一系列字节，以便存储到文件中或通过网络传输。这是在游戏开发中常用的特性，用于保存和加载游戏状态。

这个类可能是游戏保存和加载系统的一部分，用于在不同的场景之间存储和恢复游戏对象的状态。通过将场景名称作为字典的键，可以方便地管理和访问不同场景的保存数据。*/