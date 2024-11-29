using UnityEngine; // 引用Unity引擎的命名空间

public class GameManager : SingletonMonobehaviour<GameManager> // 定义一个公共类GameManager，继承自SingletonMonobehaviour
{
    public Weather currentWeather; // 公开字段，存储当前天气

    protected override void Awake() // 保护的重写方法，Awake在对象被创建时调用
    {
        base.Awake(); // 调用基类的Awake方法

        // TODO: 需要一个分辨率设置选项屏幕
        Screen.SetResolution(1920, 1080, FullScreenMode.FullScreenWindow, 0); // 设置屏幕分辨率为1920x1080，全屏窗口模式，0秒的缩放

        // 设置开始天气
        currentWeather = Weather.dry; // 将当前天气设置为干燥
    }
}

/*GameManager 类：这个类作为游戏的管理器，继承自SingletonMonobehaviour，确保整个游戏中只有一个GameManager实例。

currentWeather 字段：一个公开的字段，用于存储当前的游戏天气状态。

Awake 方法：Unity的生命周期方法，当游戏对象被创建时调用。这个方法被重写以包含游戏启动时的初始化代码。

设置分辨率：使用Screen.SetResolution方法设置游戏的屏幕分辨率为1920x1080，并使用全屏窗口模式。这里的FullScreenMode.FullScreenWindow表示窗口将全屏显示，但仍然是一个窗口，而不是真正的全屏模式。0是缩放的参数，这里设置为0表示不缩放。

设置开始天气：将currentWeather字段初始化为Weather.dry，表示游戏开始时的天气是干燥的。

TODO 注释：代码中的TODO注释表示这里有一个待办事项，即需要实现一个分辨率设置选项屏幕，允许玩家在游戏设置中调整分辨率。

这个类通常用作游戏的中心管理器，负责管理游戏的状态和行为，如天气控制、游戏设置等。通过继承SingletonMonobehaviour，GameManager确保了其单例模式，即在整个游戏生命周期中只存在一个实例。*/