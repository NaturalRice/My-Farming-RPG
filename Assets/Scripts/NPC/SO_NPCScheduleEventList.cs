// 引用系统集合和Unity引擎的命名空间
using System.Collections.Generic;
using UnityEngine;

// 创建一个可以在Unity编辑器中创建的新ScriptableObject类型的菜单项
[CreateAssetMenu(fileName = "so_NPCScheduleEventList", menuName = "Scriptable Objects/NPC/NPC Schedule Event List")]
public class SO_NPCScheduleEventList : ScriptableObject
{
    // 序列化字段，用于在Unity编辑器中显示和编辑NPC日程事件列表
    [SerializeField]
    public List<NPCScheduleEvent> npcScheduleEventList;
}

/*类定义和ScriptableObject：

SO_NPCScheduleEventList 类继承自 ScriptableObject，这意味着它可以作为一个可序列化的资源在Unity编辑器中创建和管理。
属性：

CreateAssetMenu：这是一个自定义属性，用于在Unity编辑器中创建一个新的ScriptableObject时提供文件名和菜单路径。用户可以通过指定的菜单路径快速创建这个类的实例。
fileName：指定创建资产时的文件名。
menuName：指定在Unity编辑器中创建资产的菜单路径。
字段：

npcScheduleEventList：一个序列化字段，它是一个 List<NPCScheduleEvent> 类型，用于存储NPC的日程事件。这个列表可以在Unity编辑器中直接编辑，允许开发者添加或移除日程事件。
这个类的设计目的是为了创建一个包含多个NPC日程事件的资源，这些事件可以被存储、编辑和在游戏中使用。通过继承ScriptableObject，它提供了一种灵活的方式来管理游戏数据，而无需将数据硬编码在脚本中。这种设计在游戏开发中常用于配置数据、游戏状态、AI行为等，可以提高数据管理的效率和灵活性。*/