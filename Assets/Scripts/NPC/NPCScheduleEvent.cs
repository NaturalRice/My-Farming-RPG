// 引用Unity引擎的命名空间
using UnityEngine;

// 标记这个类是可以被序列化的，以便在Unity编辑器中使用
[System.Serializable]
public class NPCScheduleEvent
{
    // NPC日程事件的小时和分钟
    public int hour;
    public int minute;
    // NPC日程事件的优先级
    public int priority;
    // NPC日程事件的具体日期
    public int day;
    // NPC日程事件的天气条件
    public Weather weather;
    // NPC日程事件的季节
    public Season season;
    // NPC移动到的目标场景名称
    public SceneName toSceneName;
    // NPC移动到的目标网格坐标
    public GridCoordinate toGridCoordinate;
    // NPC到达目的地时的面向方向
    public Direction npcFacingDirectionAtDestination = Direction.none;
    // NPC到达目的地时播放的动画剪辑
    public AnimationClip animationAtDestination;

    // 计算并返回NPC日程事件的时间码（小时*100+分钟）
    public int Time
    {
        get
        {
            return (hour * 100) + minute;
        }
    }

    // NPC日程事件的构造函数，用于初始化事件的各个属性
    public NPCScheduleEvent(int hour, int minute, int priority, int day, Weather weather, Season season, SceneName toSceneName, GridCoordinate toGridCoordinate, AnimationClip animationAtDestination)
    {
        this.hour = hour;
        this.minute = minute;
        this.priority = priority;
        this.day = day;
        this.weather = weather;
        this.season = season;
        this.toSceneName = toSceneName;
        this.toGridCoordinate = toGridCoordinate;
        this.animationAtDestination = animationAtDestination;
    }

    // 无参数的构造函数
    public NPCScheduleEvent()
    {

    }

    // 重写ToString方法，用于返回NPC日程事件的字符串表示
    public override string ToString()
    {
        return $"Time: {Time}, Priority: {priority}, Day: {day} Weather: {weather}, Season: {season}";
    }
}

/*类定义和序列化标记：

NPCScheduleEvent 类被标记为 System.Serializable，这意味着它可以在Unity编辑器中被序列化和编辑。
字段：

hour、minute：分别表示日程事件发生的小时和分钟。
priority：表示日程事件的优先级。
day：表示日程事件的具体日期。
weather：表示日程事件的天气条件，Weather 可能是一个枚举或类。
season：表示日程事件的季节，Season 可能是一个枚举或类。
toSceneName：表示NPC移动到的目标场景名称，SceneName 可能是一个枚举或类。
toGridCoordinate：表示NPC移动到的目标网格坐标，GridCoordinate 可能是一个自定义的结构或类。
npcFacingDirectionAtDestination：表示NPC到达目的地时的面向方向，Direction 可能是一个枚举。
animationAtDestination：表示NPC到达目的地时播放的动画剪辑。
属性：

Time：一个属性，用于计算并返回日程事件的时间码，计算方式为小时乘以100加上分钟。
构造函数：

NPCScheduleEvent 类有两个构造函数，一个带有所有参数，用于初始化日程事件的各个属性；另一个是无参数的构造函数。
方法：

ToString：重写了 ToString 方法，用于返回日程事件的字符串表示，包含时间、优先级、日期、天气和季节。
这个类的设计目的是为了存储和管理NPC的日程事件，包括时间、优先级、日期、天气、季节、目标位置、面向方向和目的地动画等信息。通过这些信息，游戏可以根据实际情况触发相应的NPC行为和移动。*/