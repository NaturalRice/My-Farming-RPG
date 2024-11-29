// 引用Unity引擎的命名空间
using UnityEngine;

// 定义一个类，用于存储NPC的单个移动步骤信息
public class NPCMovementStep
{
    // 公开字段，用于存储NPC移动步骤所在的场景名称
    public SceneName sceneName;

    // 公开字段，用于存储NPC移动步骤应该发生的时间（小时）
    public int hour;

    // 公开字段，用于存储NPC移动步骤应该发生的时间（分钟）
    public int minute;

    // 公开字段，用于存储NPC移动步骤应该发生的时间（秒）
    public int second;

    // 公开字段，用于存储NPC移动步骤的目标网格坐标
    public Vector2Int gridCoordinate;
}

/*类定义：

NPCMovementStep 类用于表示NPC在游戏世界中的一个具体的移动步骤。
字段：

sceneName：一个SceneName类型的公开字段，用于存储NPC移动步骤发生的场景名称。SceneName可能是一个枚举或类，用于标识不同的游戏场景。
hour、minute、second：三个整数类型的公开字段，分别表示NPC移动步骤应该发生的具体时间点，以小时、分钟和秒为单位。
gridCoordinate：一个Vector2Int类型的公开字段，用于存储NPC移动步骤的目标网格坐标。Vector2Int是一个二维向量，包含两个整数分量，通常用于表示网格或地图上的坐标位置。
这个类的设计目的是为了将NPC的移动步骤与具体的时间点和位置关联起来，使得游戏开发者可以创建一个时间表，根据这个时间表控制NPC在不同时间点移动到不同的场景和位置。这种设计在角色扮演游戏（RPG）或策略游戏中非常常见，用于模拟NPC的日常活动和行为模式。*/