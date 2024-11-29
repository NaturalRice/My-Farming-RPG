using System; // 引用System命名空间，提供基本的类和基础结构
using System.Collections.Generic; // 引用System.Collections.Generic命名空间，提供泛型集合类
using UnityEngine; // 引用Unity引擎的命名空间

// 定义一个委托，用于处理移动事件
public delegate void MovementDelegate(float inputX, float inputY, bool isWalking, bool isRunning, bool isIdle, bool isCarrying, ToolEffect toolEffect,
    bool isUsingToolRight, bool isUsingToolLeft, bool isUsingToolUp, bool isUsingToolDown,
    bool isLiftingToolRight, bool isLiftingToolLeft, bool isLiftingToolUp, bool isLiftingToolDown,
    bool isPickingRight, bool isPickingLeft, bool isPickingUp, bool isPickingDown,
    bool isSwingingToolRight, bool isSwingingToolLeft, bool isSwingingToolUp, bool isSwingingToolDown,
    bool idleUp, bool idleDown, bool idleLeft, bool idleRight);

public static class EventHandler // 定义一个静态类，用于事件处理
{
    // 掉落选中物品事件
    public static event Action DropSelectedItemEvent;

    public static void CallDropSelectedItemEvent() // 调用掉落选中物品事件
    {
        if (DropSelectedItemEvent != null) // 如果有订阅者
            DropSelectedItemEvent(); // 触发事件
    }

    // 从库存中移除选中物品事件
    public static event Action RemoveSelectedItemFromInventoryEvent;

    public static void CallRemoveSelectedItemFromInventoryEvent() // 调用从库存中移除选中物品事件
    {
        if (RemoveSelectedItemFromInventoryEvent != null) // 如果有订阅者
            RemoveSelectedItemFromInventoryEvent(); // 触发事件
    }

    // 收割动作效果事件
    public static event Action<Vector3, HarvestActionEffect> HarvestActionEffectEvent;

    public static void CallHarvestActionEffectEvent(Vector3 effectPosition, HarvestActionEffect harvestActionEffect) // 调用收割动作效果事件
    {
        if (HarvestActionEffectEvent != null) // 如果有订阅者
            HarvestActionEffectEvent(effectPosition, harvestActionEffect); // 触发事件
    }

    // 库存更新事件
    public static event Action<InventoryLocation, List<InventoryItem>> InventoryUpdatedEvent;

    public static void CallInventoryUpdatedEvent(InventoryLocation inventoryLocation, List<InventoryItem> inventoryList) // 调用库存更新事件
    {
        if (InventoryUpdatedEvent != null) // 如果有订阅者
            InventoryUpdatedEvent(inventoryLocation, inventoryList); // 触发事件
    }

    // 实例化作物预制体事件
    public static event Action InstantiateCropPrefabsEvent;

    public static void CallInstantiateCropPrefabsEvent() // 调用实例化作物预制体事件
    {
        if (InstantiateCropPrefabsEvent != null) // 如果有订阅者
        {
            InstantiateCropPrefabsEvent(); // 触发事件
        }
    }

    // 移动事件
    public static event MovementDelegate MovementEvent;

    // 移动事件调用，供发布者使用
    public static void CallMovementEvent(float inputX, float inputY, bool isWalking, bool isRunning, bool isIdle, bool isCarrying, ToolEffect toolEffect,
    bool isUsingToolRight, bool isUsingToolLeft, bool isUsingToolUp, bool isUsingToolDown,
    bool isLiftingToolRight, bool isLiftingToolLeft, bool isLiftingToolUp, bool isLiftingToolDown,
    bool isPickingRight, bool isPickingLeft, bool isPickingUp, bool isPickingDown,
    bool isSwingingToolRight, bool isSwingingToolLeft, bool isSwingingToolUp, bool isSwingingToolDown,
    bool idleUp, bool idleDown, bool idleLeft, bool idleRight) // 调用移动事件
    {
        if (MovementEvent != null) // 如果有订阅者
            MovementEvent(inputX, inputY, // 传入输入值
                isWalking, isRunning, isIdle, isCarrying, // 传入状态值
                toolEffect, // 传入工具效果
                isUsingToolRight, isUsingToolLeft, isUsingToolUp, isUsingToolDown, // 传入工具使用状态
                isLiftingToolRight, isLiftingToolLeft, isLiftingToolUp, isLiftingToolDown, // 传入工具举起状态
                isPickingRight, isPickingLeft, isPickingUp, isPickingDown, // 传入工具采摘状态
                isSwingingToolRight, isSwingingToolLeft, isSwingingToolUp, isSwingingToolDown, // 传入工具挥动状态
                idleUp, idleDown, idleLeft, idleRight); // 传入空闲状态
    }

    // 时间事件
    // 推进游戏分钟
    public static event Action<int, Season, int, string, int, int, int> AdvanceGameMinuteEvent;

    public static void CallAdvanceGameMinuteEvent(int gameYear, Season gameSeason, int gameDay, string gameDayOfWeek, int gameHour, int gameMinute, int gameSecond) // 调用推进游戏分钟事件
    {
        if (AdvanceGameMinuteEvent != null) // 如果有订阅者
        {
            AdvanceGameMinuteEvent(gameYear, gameSeason, gameDay, gameDayOfWeek, gameHour, gameMinute, gameSecond); // 触发事件
        }
    }

    // 推进游戏小时
    public static event Action<int, Season, int, string, int, int, int> AdvanceGameHourEvent;

    public static void CallAdvanceGameHourEvent(int gameYear, Season gameSeason, int gameDay, string gameDayOfWeek, int gameHour, int gameMinute, int gameSecond) // 调用推进游戏小时事件
    {
        if (AdvanceGameHourEvent != null) // 如果有订阅者
        {
            AdvanceGameHourEvent(gameYear, gameSeason, gameDay, gameDayOfWeek, gameHour, gameMinute, gameSecond); // 触发事件
        }
    }

    // 推进游戏天
    public static event Action<int, Season, int, string, int, int, int> AdvanceGameDayEvent;

    public static void CallAdvanceGameDayEvent(int gameYear, Season gameSeason, int gameDay, string gameDayOfWeek, int gameHour, int gameMinute, int gameSecond) // 调用推进游戏天事件
    {
        if (AdvanceGameDayEvent != null) // 如果有订阅者
        {
            AdvanceGameDayEvent(gameYear, gameSeason, gameDay, gameDayOfWeek, gameHour, gameMinute, gameSecond); // 触发事件
        }
    }

    // 推进游戏季节
    public static event Action<int, Season, int, string, int, int, int> AdvanceGameSeasonEvent;

    public static void CallAdvanceGameSeasonEvent(int gameYear, Season gameSeason, int gameDay, string gameDayOfWeek, int gameHour, int gameMinute, int gameSecond) // 调用推进游戏季节事件
    {
        if (AdvanceGameSeasonEvent != null) // 如果有订阅者
        {
            AdvanceGameSeasonEvent(gameYear, gameSeason, gameDay, gameDayOfWeek, gameHour, gameMinute, gameSecond); // 触发事件
        }
    }

    // 推进游戏年
    public static event Action<int, Season, int, string, int, int, int> AdvanceGameYearEvent;

    public static void CallAdvanceGameYearEvent(int gameYear, Season gameSeason, int gameDay, string gameDayOfWeek, int gameHour, int gameMinute, int gameSecond) // 调用推进游戏年事件
    {
        if (AdvanceGameYearEvent != null) // 如果有订阅者
        {
            AdvanceGameYearEvent(gameYear, gameSeason, gameDay, gameDayOfWeek, gameHour, gameMinute, gameSecond); // 触发事件
        }
    }

    // 场景加载事件 - 按发生的顺序

    // 场景卸载前淡出事件
    public static event Action BeforeSceneUnloadFadeOutEvent;

    public static void CallBeforeSceneUnloadFadeOutEvent() // 调用场景卸载前淡出事件
    {
        if (BeforeSceneUnloadFadeOutEvent != null) // 如果有订阅者
        {
            BeforeSceneUnloadFadeOutEvent(); // 触发事件
        }
    }

    // 场景卸载前事件
    public static event Action BeforeSceneUnloadEvent;

    public static void CallBeforeSceneUnloadEvent() // 调用场景卸载前事件
    {
        if (BeforeSceneUnloadEvent != null) // 如果有订阅者
        {
            BeforeSceneUnloadEvent(); // 触发事件
        }
    }

    // 场景加载后事件
    public static event Action AfterSceneLoadEvent;

    public static void CallAfterSceneLoadEvent() // 调用场景加载后事件
    {
        if (AfterSceneLoadEvent != null) // 如果有订阅者
        {
            AfterSceneLoadEvent(); // 触发事件
        }
    }

    // 场景加载后淡入事件

    // After Scene Load Fade In Event
    public static event Action AfterSceneLoadFadeInEvent;

    public static void CallAfterSceneLoadFadeInEvent() // 调用场景加载后淡入事件
    {
        if (AfterSceneLoadFadeInEvent != null) // 如果有订阅者
        {
            AfterSceneLoadFadeInEvent(); // 触发事件
        }
    }
}

/*1. **EventHandler 类**：这个静态类用于定义和触发游戏中的各种事件。它使用C#的事件机制，允许其他部分的代码订阅和响应这些事件。

2. **委托和事件**：
   - `MovementDelegate`：一个委托，定义了移动事件的签名，包括输入值、状态值和工具使用状态。
   - `DropSelectedItemEvent`：掉落选中物品的事件。
   - `RemoveSelectedItemFromInventoryEvent`：从库存中移除选中物品的事件。
   - `HarvestActionEffectEvent`：收割动作效果事件，包括效果位置和收割动作效果。
   - `InventoryUpdatedEvent`：库存更新事件，包括库存位置和库存项列表。
   - `InstantiateCropPrefabsEvent`：实例化作物预制体事件。
   - `AdvanceGameMinuteEvent`、`AdvanceGameHourEvent`、`AdvanceGameDayEvent`、`AdvanceGameSeasonEvent`、`AdvanceGameYearEvent`：时间推进事件，包括年份、季节、天数、星期几、小时、分钟和秒。
   - `BeforeSceneUnloadFadeOutEvent`、`BeforeSceneUnloadEvent`、`AfterSceneLoadEvent`、`AfterSceneLoadFadeInEvent`：场景加载和卸载事件。

3. **事件触发方法**：每个事件都有一个对应的触发方法，如`CallDropSelectedItemEvent`、`CallHarvestActionEffectEvent`等。这些方法检查是否有订阅者，如果有，则触发事件。

这个类是游戏中事件系统的一个核心组件，它允许不同部分的代码通过事件进行通信和协调，从而实现复杂的游戏逻辑和行为。*/