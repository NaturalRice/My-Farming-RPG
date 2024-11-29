// 引用TextMeshPro命名空间，用于使用TextMeshPro的文本组件
using TMPro;
// 引用UnityEngine命名空间，提供Unity引擎相关功能的支持
using UnityEngine;

public class GameClock : MonoBehaviour
{
    // 可序列化的私有变量，用于在Inspector中设置用于显示时间的TextMeshProUGUI组件
    [SerializeField] private TextMeshProUGUI timeText = null;
    // 可序列化的私有变量，用于在Inspector中设置用于显示日期的TextMeshProUGUI组件
    [SerializeField] private TextMeshProUGUI dateText = null;
    // 可序列化的私有变量，用于在Inspector中设置用于显示季节的TextMeshProUGUI组件
    [SerializeField] private TextMeshProUGUI seasonText = null;
    // 可序列化的私有变量，用于在Inspector中设置用于显示年份的TextMeshProUGUI组件
    [SerializeField] private TextMeshProUGUI yearText = null;

    // 当此脚本启用时，添加AdvanceGameMinuteEvent事件的监听
    private void OnEnable()
    {
        EventHandler.AdvanceGameMinuteEvent += UpdateGameTime;
    }

    // 当此脚本禁用时，移除AdvanceGameMinuteEvent事件的监听
    private void OnDisable()
    {
        EventHandler.AdvanceGameMinuteEvent -= UpdateGameTime;
    }

    // 更新游戏时间的方法，当游戏时间变化时被调用
    private void UpdateGameTime(int gameYear, Season gameSeason, int gameDay, string gameDayOfWeek, int gameHour, int gameMinute, int gameSecond)
    {
        // 更新时间
        gameMinute = gameMinute - (gameMinute % 10);

        string ampm = "";
        string minute;

        if (gameHour >= 12)
        {
            ampm = " pm";
        }
        else
        {
            ampm = " am";
        }

        if (gameHour >= 13)
        {
            gameHour -= 12;
        }

        if (gameMinute < 10)
        {
            minute = "0" + gameMinute.ToString();
        }
        else
        {
            minute = gameMinute.ToString();
        }

        string time = gameHour.ToString() + " : " + minute + ampm;

        // 设置时间文本
        timeText.SetText(time);
        // 设置日期文本
        dateText.SetText(gameDayOfWeek + ". " + gameDay.ToString());
        // 设置季节文本
        seasonText.SetText(gameSeason.ToString());
        // 设置年份文本
        yearText.SetText("Year " + gameYear);
    }
}

/*GameClock类继承自MonoBehaviour，使其可以附加到Unity游戏对象上。

timeText、dateText、seasonText和yearText是可序列化的私有变量，用于在Unity编辑器的Inspector面板中设置用于显示时间、日期、季节和年份的TextMeshProUGUI组件。

OnEnable和OnDisable方法分别在脚本启用和禁用时调用，用于添加和移除对EventHandler.AdvanceGameMinuteEvent事件的监听。这个事件可能在游戏时间每分钟更新时触发。

UpdateGameTime方法是事件的回调函数，当游戏时间变化时被调用。它接收游戏的年、季节、日、星期、小时、分钟和秒作为参数。

在UpdateGameTime方法中，首先对分钟数进行处理，使其以10为单位显示（例如，58分钟显示为50分钟）。

根据小时数确定是上午（am）还是下午（pm），并对小时数进行相应的调整（13点或以上减去12，以适应12小时制）。

如果分钟数小于10，前面补零以保持时间格式的一致性。

最后，使用SetText方法更新TextMeshProUGUI组件显示的时间、日期、季节和年份。

这个类的主要用途是在游戏界面上显示和更新游戏的时间和日期信息，增强游戏的沉浸感和真实感。通过响应AdvanceGameMinuteEvent事件，它可以确保时间显示与游戏世界中的时间同步。*/