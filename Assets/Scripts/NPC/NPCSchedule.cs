// 引用系统集合和Unity引擎的命名空间
using System.Collections.Generic;
using UnityEngine;

// 这个类需要依附于包含NPCPath组件的游戏对象
[RequireComponent(typeof(NPCPath))]
public class NPCSchedule : MonoBehaviour
{
    // 序列化字段，用于存储NPC日程事件列表的序列化对象
    [SerializeField] private SO_NPCScheduleEventList so_NPCScheduleEventList = null;
    // 私有字段，用于存储排序后的NPC日程事件集合
    private SortedSet<NPCScheduleEvent> npcScheduleEventSet;
    // 私有字段，用于存储NPC路径组件的引用
    private NPCPath npcPath;

    // 唤醒时调用，用于加载NPC日程事件列表到排序集合并获取NPC路径组件
    private void Awake()
    {
        // 将NPC日程事件列表加载到排序集合中
        npcScheduleEventSet = new SortedSet<NPCScheduleEvent>(new NPCScheduleEventSort());

        foreach (NPCScheduleEvent npcScheduleEvent in so_NPCScheduleEventList.npcScheduleEventList)
        {
            npcScheduleEventSet.Add(npcScheduleEvent);
        }

        // 获取NPC路径组件
        npcPath = GetComponent<NPCPath>();
    }

    // 启用时调用，用于注册游戏分钟推进事件
    private void OnEnable()
    {
        EventHandler.AdvanceGameMinuteEvent += GameTimeSystem_AdvanceMinute;
    }

    // 禁用时调用，用于注销游戏分钟推进事件
    private void OnDisable()
    {
        EventHandler.AdvanceGameMinuteEvent -= GameTimeSystem_AdvanceMinute;
    }

    // 游戏时间系统分钟推进事件处理方法
    private void GameTimeSystem_AdvanceMinute(int gameYear, Season gameSeason, int gameDay, string gameDayOfWeek, int gameHour, int gameMinute, int gameSecond)
    {
        int time = (gameHour * 100) + gameMinute;

        // 尝试获取匹配的日程事件

        NPCScheduleEvent matchingNPCScheduleEvent = null;

        foreach (NPCScheduleEvent npcScheduleEvent in npcScheduleEventSet)
        {
            if (npcScheduleEvent.Time == time)
            {
                // 时间匹配，现在检查参数是否匹配
                if (npcScheduleEvent.day != 0 && npcScheduleEvent.day != gameDay)
                    continue;

                if (npcScheduleEvent.season != Season.none && npcScheduleEvent.season != gameSeason)
                    continue;

                if (npcScheduleEvent.weather != Weather.none && npcScheduleEvent.weather != GameManager.Instance.currentWeather)
                    continue;

                // 日程匹配
                // Debug.Log("Schedule Matches! " + npcScheduleEvent);
                matchingNPCScheduleEvent = npcScheduleEvent;
                break;
            }
            else if (npcScheduleEvent.Time > time)
            {
                break;
            }
        }

        // 现在测试是否找到了匹配的日程事件，并执行操作；
        if (matchingNPCScheduleEvent != null)
        {
            // 为匹配的日程事件构建路径
            npcPath.BuildPath(matchingNPCScheduleEvent);
        }
    }
}

/*类定义和组件依赖：

NPCSchedule 类继承自 MonoBehaviour，用于根据游戏时间管理和触发NPC的日程事件。
使用 RequireComponent 属性确保该脚本依附的游戏对象包含 NPCPath 组件。
字段：

so_NPCScheduleEventList：一个序列化字段，用于存储NPC日程事件列表的序列化对象。
npcScheduleEventSet：一个 SortedSet<NPCScheduleEvent> 类型的私有字段，用于存储排序后的NPC日程事件集合。
npcPath：一个 NPCPath 类型的私有字段，用于存储NPC路径组件的引用。
方法：

Awake：在对象唤醒时调用，用于将NPC日程事件列表加载到排序集合中，并获取NPC路径组件。
OnEnable 和 OnDisable：分别在对象启用和禁用时调用，用于注册和注销游戏分钟推进事件。
GameTimeSystem_AdvanceMinute：游戏时间系统分钟推进事件处理方法，用于根据当前游戏时间查找匹配的日程事件，并触发相应的NPC路径构建。
日程事件匹配逻辑：

GameTimeSystem_AdvanceMinute 方法首先计算当前时间的表示（小时和分钟）。
遍历排序后的日程事件集合，查找时间匹配的事件，并检查其他参数（如日期、季节、天气）是否匹配。
如果找到匹配的日程事件，调用 npcPath.BuildPath 方法为该事件构建路径。
这个类的设计目的是为了根据游戏时间自动管理和触发NPC的日程事件，使得NPC能够按照预定的时间表行动。通过监听游戏时间的变化，并与日程事件进行匹配，可以实现NPC行为的自动化管理。*/