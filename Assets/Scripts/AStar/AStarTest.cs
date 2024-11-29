using UnityEngine; // 引用Unity引擎的命名空间

public class AStarTest : MonoBehaviour // 定义一个继承自MonoBehaviour的公共类
{
    // [SerializeField] private AStar aStar; // 可序列化的私有字段，用于存储AStar算法的实例，已被注释掉

    [SerializeField] private NPCPath npcPath = null; // 可序列化的私有字段，用于存储NPC路径
    [SerializeField] private bool moveNPC = false; // 可序列化的私有字段，用于控制是否移动NPC
    [SerializeField] private SceneName sceneName = SceneName.Scene1_Farm; // 可序列化的私有字段，用于存储当前场景名称
    [SerializeField] private Vector2Int finishPosition; // 可序列化的私有字段，用于存储NPC的结束位置
    [SerializeField] private AnimationClip idleDownAnimationClip = null; // 可序列化的私有字段，用于存储NPC空闲时向下看的动画剪辑
    [SerializeField] private AnimationClip eventAnimationClip = null; // 可序列化的私有字段，用于存储事件动画剪辑
    private NPCMovement npcMovement; // 私有字段，用于存储NPC的移动组件

    private void Start() // 在游戏开始时调用
    {
        npcMovement = npcPath.GetComponent<NPCMovement>(); // 获取NPC路径上的NPCMovement组件
        npcMovement.npcFacingDirectionAtDestination = Direction.down; // 设置NPC到达目的地时的面向方向为向下
        npcMovement.npcTargetAnimationClip = idleDownAnimationClip; // 设置NPC的目标动画剪辑为空闲时向下看的动画剪辑
    }

    private void Update() // 每帧调用一次
    {
        if (moveNPC) // 如果需要移动NPC
        {
            moveNPC = false; // 重置移动标志

            NPCScheduleEvent npcScheduleEvent = new NPCScheduleEvent(0, 0, 0, 0, Weather.none, Season.none, sceneName, new GridCoordinate(finishPosition.x, finishPosition.y), eventAnimationClip); // 创建NPC日程事件

            npcPath.BuildPath(npcScheduleEvent); // 构建NPC的路径
        }
    }
}

/*AStarTest 类：这个类用于测试A*路径查找算法，继承自MonoBehaviour，可以附加到Unity场景中的GameObject上。

序列化字段：

npcPath：存储NPC路径的组件。
moveNPC：控制是否移动NPC的布尔值。
sceneName：当前场景的名称。
finishPosition：NPC的结束位置。
idleDownAnimationClip：NPC空闲时向下看的动画剪辑。
eventAnimationClip：事件动画剪辑。
NPCMovement 字段：存储NPC移动组件的私有字段。

Start 方法：在游戏开始时调用，用于初始化NPC的移动组件和设置NPC的面向方向及目标动画剪辑。

Update 方法：每帧调用一次，如果moveNPC为真，则创建一个NPC日程事件并调用npcPath.BuildPath方法来构建NPC的路径。

这个脚本通常用于游戏中NPC的路径测试，可以根据NPC的起始位置和目标位置自动计算出一条路径，并指导NPC移动。通过设置动画剪辑，还可以控制NPC在空闲时和事件发生时的动画表现。*/