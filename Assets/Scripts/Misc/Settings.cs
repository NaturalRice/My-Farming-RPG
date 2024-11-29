// 使用Unity引擎的命名空间
using UnityEngine;

// 静态类，用于存储游戏设置
public static class Settings
{
    // 场景
    public const string PersistentScene = "PersistentScene"; // 持久场景的名称

    // 遮蔽物品淡入淡出 - ObscuringItemFader
    public const float fadeInSeconds = 0.25f; // 淡入时间，单位秒
    public const float fadeOutSeconds = 0.35f; // 淡出时间，单位秒
    public const float targetAlpha = 0.45f; // 目标透明度

    // Tilemap
    public const float gridCellSize = 1f; // 网格单元大小，单位为Unity单位
    public const float gridCellDiagonalSize = 1.41f; // 网格单元中心之间的对角线距离
    public const int maxGridWidth = 99999; // 最大网格宽度
    public const int maxGridHeight = 99999; // 最大网格高度
    public static Vector2 cursorSize = Vector2.one; // 光标大小

    // 玩家
    public static float playerCentreYOffset = 0.875f; // 玩家中心的Y轴偏移

    // 玩家移动
    public const float runningSpeed = 5.333f; // 跑步速度
    public const float walkingSpeed = 2.666f; // 行走速度
    public static float useToolAnimationPause = 0.25f; // 使用工具动画暂停时间
    public static float liftToolAnimationPause = 0.4f; // 举起工具动画暂停时间
    public static float pickAnimationPause = 1f; // 拾取动画暂停时间
    public static float afterUseToolAnimationPause = 0.2f; // 使用工具后动画暂停时间
    public static float afterLiftToolAnimationPause = 0.4f; // 举起工具后动画暂停时间
    public static float afterPickAnimationPause = 0.2f; // 拾取后动画暂停时间

    // NPC移动
    public static float pixelSize = 0.0625f; // 像素大小

    // 库存
    public static int playerInitialInventoryCapacity = 24; // 玩家初始库存容量
    public static int playerMaximumInventoryCapacity = 48; // 玩家最大库存容量

    // NPC动画参数
    public static int walkUp;
    public static int walkDown;
    public static int walkLeft;
    public static int walkRight;
    public static int eventAnimation;

    // 玩家动画参数
    public static int xInput;
    public static int yInput;
    public static int isWalking;
    public static int isRunning;
    public static int toolEffect;
    public static int isUsingToolRight;
    public static int isUsingToolLeft;
    public static int isUsingToolUp;
    public static int isUsingToolDown;
    public static int isLiftingToolRight;
    public static int isLiftingToolLeft;
    public static int isLiftingToolUp;
    public static int isLiftingToolDown;
    public static int isSwingingToolRight;
    public static int isSwingingToolLeft;
    public static int isSwingingToolUp;
    public static int isSwingingToolDown;
    public static int isPickingRight;
    public static int isPickingLeft;
    public static int isPickingUp;
    public static int isPickingDown;

    // 共享动画参数
    public static int idleUp;
    public static int idleDown;
    public static int idleLeft;
    public static int idleRight;

    // 工具
    public const string HoeingTool = "Hoe"; // 锄头工具
    public const string ChoppingTool = "Axe"; // 斧头工具
    public const string BreakingTool = "Pickaxe"; // 镐头工具
    public const string ReapingTool = "Scythe"; // 镰刀工具
    public const string WateringTool = "Watering Can"; // 浇水壶工具
    public const string CollectingTool = "Basket"; // 收集篮工具

    // 收割
    public const int maxCollidersToTestPerReapSwing = 15; // 每次收割摆动测试的最大碰撞器数量
    public const int maxTargetComponentsToDestroyPerReapSwing = 2; // 每次收割摆动销毁的最大目标组件数量

    // 时间系统
    public const float secondsPerGameSecond = 0.012f; // 每游戏秒的秒数

    // 静态构造函数
    static Settings()
    {
        // NPC动画参数
        walkUp = Animator.StringToHash("walkUp");
        walkDown = Animator.StringToHash("walkDown");
        walkLeft = Animator.StringToHash("walkLeft");
        walkRight = Animator.StringToHash("walkRight");
        eventAnimation = Animator.StringToHash("eventAnimation");

        // 玩家动画参数
        xInput = Animator.StringToHash("xInput");
        yInput = Animator.StringToHash("yInput");
        isWalking = Animator.StringToHash("isWalking");
        isRunning = Animator.StringToHash("isRunning");
        toolEffect = Animator.StringToHash("toolEffect");
        isUsingToolRight = Animator.StringToHash("isUsingToolRight");
        isUsingToolLeft = Animator.StringToHash("isUsingToolLeft");
        isUsingToolUp = Animator.StringToHash("isUsingToolUp");
        isUsingToolDown = Animator.StringToHash("isUsingToolDown");
        isLiftingToolRight = Animator.StringToHash("isLiftingToolRight");
        isLiftingToolLeft = Animator.StringToHash("isLiftingToolLeft");
        isLiftingToolUp = Animator.StringToHash("isLiftingToolUp");
        isLiftingToolDown = Animator.StringToHash("isLiftingToolDown");
        isSwingingToolRight = Animator.StringToHash("isSwingingToolRight");
        isSwingingToolLeft = Animator.StringToHash("isSwingingToolLeft");
        isSwingingToolUp = Animator.StringToHash("isSwingingToolUp");
        isSwingingToolDown = Animator.StringToHash("isSwingingToolDown");
        isPickingRight = Animator.StringToHash("isPickingRight");
        isPickingLeft = Animator.StringToHash("isPickingLeft");
        isPickingUp = Animator.StringToHash("isPickingUp");
        isPickingDown = Animator.StringToHash("isPickingDown");

        // 共享动画参数
        idleUp = Animator.StringToHash("idleUp");
        idleDown = Animator.StringToHash("idleDown");
        idleLeft = Animator.StringToHash("idleLeft");
        idleRight = Animator.StringToHash("idleRight");
    }
}

/*静态类定义：

public static class Settings：定义了一个公共静态类 Settings，它用于存储全局可访问的设置和参数。
场景设置：

PersistentScene：定义了一个常量字符串，用于存储持久场景的名称。
物品淡入淡出设置：

fadeInSeconds、fadeOutSeconds 和 targetAlpha：定义了物品淡入淡出的时间和目标透明度。
Tilemap设置：

gridCellSize 和 gridCellDiagonalSize：定义了网格单元的大小和对角线距离。
maxGridWidth 和 maxGridHeight：定义了网格的最大宽度和高度。
cursorSize：定义了光标的大小。
玩家设置：

playerCentreYOffset：定义了玩家中心的Y轴偏移。
玩家移动设置：

runningSpeed 和 walkingSpeed：定义了玩家的跑步速度和行走速度。
一系列与使用工具和拾取动作相关的动画暂停时间。
NPC移动设置：

pixelSize：定义了像素的大小。
库存设置：

playerInitialInventoryCapacity 和 playerMaximumInventoryCapacity：定义了玩家的初始和最大库存容量。
NPC和玩家动画参数：

定义了一系列与NPC和玩家动画状态相关的整型参数，这些参数在静态构造函数中被初始化为Animator的哈希值。
共享动画参数：

定义了与共享动画状态相关的整型参数。
工具设置：

定义了一系列与工具相关的常量字符串。
收割设置：

maxCollidersToTestPerReapSwing 和 maxTargetComponentsToDestroyPerReapSwing：定义了每次收割摆动测试的最大碰撞器数量和销毁的最大目标组件数量。*/