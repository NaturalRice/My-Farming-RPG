// 引用System.Collections.Generic命名空间，以便使用泛型集合类，如List<T>
using System.Collections.Generic;
// 引用UnityEngine命名空间，提供Unity引擎相关功能的支持
using UnityEngine;

// 创建一个可以在Unity编辑器中创建的新ScriptableObject类型的菜单项
[CreateAssetMenu(fileName = "so_SceneSoundsList", menuName = "Scriptable Objects/Sounds/Scene Sounds List")]
public class SO_SceneSoundsList : ScriptableObject
{
    // 可序列化的私有变量，用于在Inspector中设置场景声音配置的列表
    [SerializeField]
    public List<SceneSoundsItem> sceneSoundsDetails;
}

/*SO_SceneSoundsList类继承自ScriptableObject，这意味着它可以作为一个可序列化的资源文件被创建和使用，这在Unity中常用于存储游戏数据。

[CreateAssetMenu]属性是一个自定义属性，它提供了一个在Unity编辑器中创建这个ScriptableObject的方法。用户可以通过指定的菜单路径（"Scriptable Objects/Sounds/Scene Sounds List"）来创建一个名为"so_SceneSoundsList"的ScriptableObject实例。

sceneSoundsDetails是一个公开的List<SceneSoundsItem>类型的变量，用于存储SceneSoundsItem对象的列表。每个SceneSoundsItem对象包含了一个场景的声音配置，包括场景名称、环境声音名称和音乐声音名称。

这个类的主要用途是作为一个容器，存储和管理游戏中所有场景的声音配置数据。通过将这个类实例化为ScriptableObject，可以在Unity编辑器中轻松创建和管理这些声音配置数据，而不需要将它们硬编码在脚本中。这样做的好处是可以轻松地调整和更新数据，而不需要修改代码，同时也使得数据更加集中和易于管理。*/