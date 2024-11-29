// 使用System.Collections.Generic命名空间，提供对集合类的访问，如字典和列表
using System.Collections.Generic;

// 标记SceneSave类为可序列化，使得该类的实例可以被转换成一系列可以存储或传输的字节
[System.Serializable]
public class SceneSave
{
    // 存储字符串键和整型值的字典，用于存储与场景相关的整型数据
    public Dictionary<string, int> intDictionary;
    // 存储字符串键和布尔型值的字典，字符串键是我们为这个列表选择的标识名称
    public Dictionary<string, bool> boolDictionary;
    // 存储字符串键和字符串值的字典，用于存储与场景相关的字符串数据
    public Dictionary<string, string> stringDictionary;
    // 存储字符串键和可序列化Vector3值的字典，用于存储与场景相关的三维向量数据
    public Dictionary<string, Vector3Serializable> vector3Dictionary;
    // 存储字符串键和整型数组值的字典，用于存储与场景相关的数组数据
    public Dictionary<string, int[]> intArrayDictionary;
    // 存储SceneItem对象的列表，用于存储场景中的物品信息
    public List<SceneItem> listSceneItem;
    // 存储字符串键和GridPropertyDetails值的字典，用于存储与网格属性相关的详细信息
    public Dictionary<string, GridPropertyDetails> gridPropertyDetailsDictionary;
    // 存储InventoryItem对象数组的列表，用于存储与场景相关的库存项数组
    public List<InventoryItem>[] listInvItemArray;
}

/*SceneSave类被标记为[System.Serializable]，这意味着它可以被序列化，即将对象的状态信息转换为可以存储或网络传输的格式。

intDictionary是一个字典，其键为字符串类型，值为整型。这个字典可能用于存储与场景相关的整型数据，例如物品的数量或特定的整型标识符。

boolDictionary是一个字典，其键为字符串类型，值为布尔型。这个字典可能用于存储与场景相关的布尔状态，例如某个开关是否被激活。

stringDictionary是一个字典，其键和值都是字符串类型，用于存储与场景相关的字符串数据，例如物品的名称或描述。

vector3Dictionary是一个字典，其键为字符串类型，值为Vector3Serializable类型，用于存储与场景相关的三维空间位置数据。

intArrayDictionary是一个字典，其键为字符串类型，值为整型数组，用于存储与场景相关的数组数据，例如一系列整型标识符。

listSceneItem是一个列表，存储SceneItem对象，用于存储场景中的物品信息，如物品的位置、名称和代码。

gridPropertyDetailsDictionary是一个字典，其键为字符串类型，值为GridPropertyDetails类型，用于存储与网格属性相关的详细信息。

listInvItemArray是一个数组，每个元素都是一个InventoryItem对象的列表，用于存储与场景相关的库存项数组。

这个类的主要用途是在游戏场景中存储和管理各种类型的数据，包括整型、布尔型、字符串、三维向量、数组以及物品信息等。通过序列化这些信息，可以将它们保存到文件中或通过网络传输，这对于游戏的保存/加载机制和网络同步等功能至关重要。*/