using System.Collections.Generic; // 引用System.Collections.Generic命名空间，提供泛型集合类
using UnityEngine; // 引用Unity引擎的命名空间

[CreateAssetMenu(fileName = "so_ItemList", menuName = "Scriptable Objects/Item/Item List")]
// 自定义属性，用于在Unity编辑器中创建该ScriptableObject时指定文件名和菜单路径
public class SO_ItemList : ScriptableObject // 定义一个继承自ScriptableObject的公共类
{
    [SerializeField] // 标记为可序列化，使其在Unity编辑器中可见
    public List<ItemDetails> itemDetails; // 公开字段，存储物品详情列表
}

/*SO_ItemList 类：这个类是一个ScriptableObject，用于存储和管理游戏中所有物品的详细信息。它允许开发者在Unity编辑器中创建和管理物品列表，而不需要将数据硬编码在脚本中。

CreateAssetMenu 属性：

这是一个自定义属性，用于在Unity编辑器中创建该ScriptableObject时指定文件名（so_ItemList）和菜单路径（Scriptable Objects/Item/Item List）。这使得在创建新的ScriptableObject实例时，可以直接在Assets菜单下找到这个选项，方便创建和管理。
itemDetails 字段：

一个公开的List<ItemDetails>字段，用于存储ItemDetails对象的列表。每个ItemDetails对象包含一个物品的所有详细信息，如物品代码、类型、描述、精灵图等。
这个类是游戏中物品系统的一个核心组件，它提供了一个集中的方式来管理和访问所有物品的数据。通过使用ScriptableObject，可以轻松地在编辑器中添加、修改和删除物品信息，同时保持代码的整洁和可维护性。这种方式也使得物品数据可以被版本控制，便于团队协作和项目管理。*/