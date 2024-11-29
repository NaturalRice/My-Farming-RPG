// 引用系统集合的命名空间
using System.Collections.Generic;

// 实现IComparer接口，用于比较NPCScheduleEvent对象
public class NPCScheduleEventSort : IComparer<NPCScheduleEvent>
{
    // 实现Compare方法，用于比较两个NPCScheduleEvent对象
    public int Compare(NPCScheduleEvent npcScheduleEvent1, NPCScheduleEvent npcScheduleEvent2)
    {
        // 如果两个事件的时间相同，则比较它们的优先级
        if (npcScheduleEvent1?.Time == npcScheduleEvent2?.Time)
        {
            // 如果第一个事件的优先级小于第二个事件的优先级，则返回-1
            if (npcScheduleEvent1?.priority < npcScheduleEvent2?.priority)
            {
                return -1;
            }
            // 否则返回1
            else
            {
                return 1;
            }
        }
        // 如果第一个事件的时间大于第二个事件的时间，则返回1
        else if (npcScheduleEvent1?.Time > npcScheduleEvent2?.Time)
        {
            return 1;
        }
        // 如果第一个事件的时间小于第二个事件的时间，则返回-1
        else if (npcScheduleEvent1?.Time < npcScheduleEvent2?.Time)
        {
            return -1;
        }
        // 否则返回0
        else
        {
            return 0;
        }
    }
}

/*类定义和接口实现：

NPCScheduleEventSort 类实现了 IComparer<NPCScheduleEvent> 接口，这意味着它提供了一个方法来比较两个 NPCScheduleEvent 对象。
方法：

Compare：这是 IComparer 接口中定义的方法，用于比较两个 NPCScheduleEvent 对象。它返回一个整数，表示第一个对象相对于第二个对象的顺序。
比较逻辑：

如果两个事件的时间相同，那么比较它们的优先级。优先级较低的事件（数值较小）被认为是较小的，因此返回 -1。否则，返回 1。
如果第一个事件的时间大于第二个事件的时间，返回 1。
如果第一个事件的时间小于第二个事件的时间，返回 -1。
如果两个事件的时间和优先级都相等，返回 0。
这个类的设计目的是为了根据时间和优先级对NPC的日程事件进行排序。这确保了事件可以按照正确的顺序被处理，优先级较高的事件会先于优先级较低的事件执行，如果时间相同的话。*/