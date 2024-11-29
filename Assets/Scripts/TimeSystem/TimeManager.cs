// 引用System命名空间，提供基本的数据结构和操作
using System;
// 引用System.Collections.Generic命名空间，提供对集合类的支持
using System.Collections.Generic;
// 引用UnityEngine命名空间，提供Unity引擎相关功能的支持
using UnityEngine;

// 继承自SingletonMonobehaviour，确保TimeManager类在游戏场景中是唯一的，并实现ISaveable接口
public class TimeManager : SingletonMonobehaviour<TimeManager>, ISaveable
{
    // 私有变量，用于存储游戏的年份、季节、日、小时、分钟和秒
    private int gameYear = 1;
    private Season gameSeason = Season.Spring;
    private int gameDay = 1;
    private int gameHour = 6;
    private int gameMinute = 30;
    private int gameSecond = 0;
    private string gameDayOfWeek = "Mon";

    // 私有变量，用于标记游戏时钟是否暂停
    private bool gameClockPaused = false;

    // 私有变量，用于存储游戏的时间刻度
    private float gameTick = 0f;

    // 实现ISaveable接口的属性，用于存储唯一标识符
    private string _iSaveableUniqueID;
    public string ISaveableUniqueID { get { return _iSaveableUniqueID; } set { _iSaveableUniqueID = value; } }

    // 实现ISaveable接口的属性，用于存储游戏对象的保存数据
    private GameObjectSave _gameObjectSave;
    public GameObjectSave GameObjectSave { get { return _gameObjectSave; } set { _gameObjectSave = value; } }

    // 在对象被创建时调用，用于初始化
    protected override void Awake()
    {
        base.Awake();

        ISaveableUniqueID = GetComponent<GenerateGUID>().GUID;
        GameObjectSave = new GameObjectSave();
    }

    // 当此脚本启用时，注册保存和加载事件
    private void OnEnable()
    {
        ISaveableRegister();

        EventHandler.BeforeSceneUnloadEvent += BeforeSceneUnloadFadeOut;
        EventHandler.AfterSceneLoadEvent += AfterSceneLoadFadeIn;
    }

    // 当此脚本禁用时，注销保存和加载事件
    private void OnDisable()
    {
        ISaveableDeregister();

        EventHandler.BeforeSceneUnloadEvent -= BeforeSceneUnloadFadeOut;
        EventHandler.AfterSceneLoadEvent -= AfterSceneLoadFadeIn;
    }

    // 在场景卸载前调用，暂停游戏时钟
    private void BeforeSceneUnloadFadeOut()
    {
        gameClockPaused = true;
    }

    // 在场景加载后调用，恢复游戏时钟
    private void AfterSceneLoadFadeIn()
    {
        gameClockPaused = false;
    }

    // 在游戏开始时调用，触发游戏分钟更新事件
    private void Start()
    {
        EventHandler.CallAdvanceGameMinuteEvent(gameYear, gameSeason, gameDay, gameDayOfWeek, gameHour, gameMinute, gameSecond);
    }

    // 每帧调用，更新游戏时间
    private void Update()
    {
        if (!gameClockPaused)
        {
            GameTick();
        }
    }

    // 更新游戏的时间刻度
    private void GameTick()
    {
        gameTick += Time.deltaTime;

        if (gameTick >= Settings.secondsPerGameSecond)
        {
            gameTick -= Settings.secondsPerGameSecond;

            UpdateGameSecond();
        }
    }

    // 更新游戏的秒数
    private void UpdateGameSecond()
    {
        gameSecond++;

        if (gameSecond > 59)
        {
            gameSecond = 0;
            gameMinute++;

            if (gameMinute > 59)
            {
                gameMinute = 0;
                gameHour++;

                if (gameHour > 23)
                {
                    gameHour = 0;
                    gameDay++;

                    if (gameDay > 30)
                    {
                        gameDay = 1;

                        int gs = (int)gameSeason;
                        gs++;

                        gameSeason = (Season)gs;

                        if (gs > 3)
                        {
                            gs = 0;
                            gameSeason = (Season)gs;

                            gameYear++;

                            if (gameYear > 9999)
                                gameYear = 1;


                            EventHandler.CallAdvanceGameYearEvent(gameYear, gameSeason, gameDay, gameDayOfWeek, gameHour, gameMinute, gameSecond);
                        }

                        EventHandler.CallAdvanceGameSeasonEvent(gameYear, gameSeason, gameDay, gameDayOfWeek, gameHour, gameMinute, gameSecond);
                    }

                    gameDayOfWeek = GetDayOfWeek();
                    EventHandler.CallAdvanceGameDayEvent(gameYear, gameSeason, gameDay, gameDayOfWeek, gameHour, gameMinute, gameSecond);
                }

                EventHandler.CallAdvanceGameHourEvent(gameYear, gameSeason, gameDay, gameDayOfWeek, gameHour, gameMinute, gameSecond);
            }

            EventHandler.CallAdvanceGameMinuteEvent(gameYear, gameSeason, gameDay, gameDayOfWeek, gameHour, gameMinute, gameSecond);

        }

        // 如果需要，在这里调用更新游戏秒数事件
    }

    // 根据游戏的日和季节计算星期几
    private string GetDayOfWeek()
    {
        int totalDays = (((int)gameSeason) * 30) + gameDay;
        int dayOfWeek = totalDays % 7;

        switch (dayOfWeek)
        {
            case 1:
                return "Mon";

            case 2:
                return "Tue";

            case 3:
                return "Wed";

            case 4:
                return "Thu";

            case 5:
                return "Fri";

            case 6:
                return "Sat";

            case 0:
                return "Sun";

            default:
                return "";
        }
    }

    // 获取游戏的时间
    public TimeSpan GetGameTime()
    {
        TimeSpan gameTime = new TimeSpan(gameHour, gameMinute, gameSecond);

        return gameTime;
    }

    // 测试方法，前进1游戏分钟
    public void TestAdvanceGameMinute()
    {
        for (int i = 0; i < 60; i++)
        {
            UpdateGameSecond();
        }
    }

    // 测试方法，前进1天
    public void TestAdvanceGameDay()
    {
        for (int i = 0; i < 86400; i++)
        {
            UpdateGameSecond();
        }
    }

    // 实现ISaveable接口的方法，注册保存对象
    public void ISaveableRegister()
    {
        SaveLoadManager.Instance.iSaveableObjectList.Add(this);
    }

    // 实现ISaveable接口的方法，注销保存对象
    public void ISaveableDeregister()
    {
        SaveLoadManager.Instance.iSaveableObjectList.Remove(this);
    }

    // 实现ISaveable接口的方法，保存游戏状态
    public GameObjectSave ISaveableSave()
    {
        // 如果存在，则删除现有的场景保存数据
        GameObjectSave.sceneData.Remove(Settings.PersistentScene);

        // 创建新的场景保存数据
        SceneSave sceneSave = new SceneSave();

        // 创建新的整型字典
        sceneSave.intDictionary = new Dictionary<string, int>();

        // 创建新的字符串字典
        sceneSave.stringDictionary = new Dictionary<string, string>();

        // 添加整型值到字典
        sceneSave.intDictionary.Add("gameYear", gameYear);
        sceneSave.intDictionary.Add("gameDay", gameDay);
        sceneSave.intDictionary.Add("gameHour", gameHour);
        sceneSave.intDictionary.Add("gameMinute", gameMinute);
        sceneSave.intDictionary.Add("gameSecond", gameSecond);

        // 添加字符串值到字典
        sceneSave.stringDictionary.Add("gameDayOfWeek", gameDayOfWeek);
        sceneSave.stringDictionary.Add("gameSeason", gameSeason.ToString());

        // 将场景保存数据添加到游戏对象的持久场景中
        GameObjectSave.sceneData.Add(Settings.PersistentScene, sceneSave);

        return GameObjectSave;
    }
    // 实现ISaveable接口的方法，加载游戏状态
    public void ISaveableLoad(GameSave gameSave)
    {
        // 从gameSave数据中获取保存的游戏对象
        if (gameSave.gameObjectData.TryGetValue(ISaveableUniqueID, out GameObjectSave gameObjectSave))
        {
            GameObjectSave = gameObjectSave;

            // 获取游戏对象的保存场景数据
            if (GameObjectSave.sceneData.TryGetValue(Settings.PersistentScene, out SceneSave sceneSave))
            {
                // 如果找到整型和字符串字典
                if (sceneSave.intDictionary != null && sceneSave.stringDictionary != null)
                {
                    // 填充保存的整型值
                    if (sceneSave.intDictionary.TryGetValue("gameYear", out int savedGameYear))
                        gameYear = savedGameYear;

                    if (sceneSave.intDictionary.TryGetValue("gameDay", out int savedGameDay))
                        gameDay = savedGameDay;

                    if (sceneSave.intDictionary.TryGetValue("gameHour", out int savedGameHour))
                        gameHour = savedGameHour;

                    if (sceneSave.intDictionary.TryGetValue("gameMinute", out int savedGameMinute))
                        gameMinute = savedGameMinute;

                    if (sceneSave.intDictionary.TryGetValue("gameSecond", out int savedGameSecond))
                        gameSecond = savedGameSecond;

                    // 填充保存的字符串值
                    if (sceneSave.stringDictionary.TryGetValue("gameDayOfWeek", out string savedGameDayOfWeek))
                        gameDayOfWeek = savedGameDayOfWeek;

                    if (sceneSave.stringDictionary.TryGetValue("gameSeason", out string savedGameSeason))
                    {
                        if (Enum.TryParse<Season>(savedGameSeason, out Season season))
                        {
                            gameSeason = season;
                        }
                    }

                    // 重置游戏时间刻度
                    gameTick = 0f;

                    // 触发游戏分钟更新事件
                    EventHandler.CallAdvanceGameMinuteEvent(gameYear, gameSeason, gameDay, gameDayOfWeek, gameHour, gameMinute, gameSecond);

                    // 刷新游戏时钟
                }
            }
        }
    }
    public void ISaveableStoreScene(string sceneName)
    {
        // 实现ISaveable接口的方法，存储场景数据（由于Time Manager在持久场景中运行，这里不需要做任何操作）
    }

    public void ISaveableRestoreScene(string sceneName)
    {
        // 实现ISaveable接口的方法，恢复场景数据（由于Time Manager在持久场景中运行，这里不需要做任何操作）
    }
}

/*
1. `TimeManager`类继承自`SingletonMonobehaviour`，这是一个单例模式的实现，确保`TimeManager`在游戏场景中是唯一的实例。同时，它实现了`ISaveable`接口，用于保存和加载游戏时间状态。

2. `gameYear`、`gameSeason`、`gameDay`、`gameHour`、`gameMinute`和`gameSecond`是私有变量，用于存储游戏的当前年、季节、日、小时、分钟和秒。

3. `gameClockPaused`是私有变量，用于标记游戏时钟是否暂停。

4. `gameTick`是私有变量，用于存储游戏的时间刻度，以秒为单位。

5. `_iSaveableUniqueID`和`_gameObjectSave`是私有变量，分别用于存储`ISaveable`接口的唯一标识符和游戏对象的保存数据。

6. `Awake`方法在游戏对象创建时调用，用于初始化唯一标识符和游戏对象的保存数据。

7. `OnEnable`和`OnDisable`方法分别在脚本启用和禁用时调用，用于注册和注销事件。

8. `BeforeSceneUnloadFadeOut`和`AfterSceneLoadFadeIn`方法分别在场景卸载前和加载后调用，用于暂停和恢复游戏时钟。

9. `Start`方法在游戏开始时调用，触发游戏分钟更新事件。

10. `Update`方法每帧调用，更新游戏时间。

11. `GameTick`方法更新游戏的时间刻度。

12. `UpdateGameSecond`方法更新游戏的秒数，并在必要时更新分钟、小时、日、季节和年。

13. `GetDayOfWeek`方法根据游戏的日和季节计算星期几。

14. `GetGameTime`方法获取游戏的时间。

15. `TestAdvanceGameMinute`和`TestAdvanceGameDay`是测试方法，用于前进游戏时间。

16. `ISaveableRegister`和`ISaveableDeregister`方法实现了`ISaveable`接口，用于注册和注销保存对象。

17. `ISaveableSave`和`ISaveableLoad`方法实现了`ISaveable`接口，用于保存和加载游戏状态。

18. `ISaveableStoreScene`和`ISaveableRestoreScene`方法实现了`ISaveable`接口，但由于`Time Manager`在持久场景中运行，这里不需要做任何操作。

这个类的主要用途是管理游戏中的时间和日期，并提供保存和加载游戏时间状态的功能。通过响应事件和更新时间刻度，它可以确保游戏时间与游戏世界中的时间同步。*/