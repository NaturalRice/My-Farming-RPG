// 引用所需的命名空间
using UnityEngine;
using UnityEngine.UI;

// 继承自SingletonMonobehaviour，确保UIManager类在游戏场景中是唯一的
public class UIManager : SingletonMonobehaviour<UIManager>
{
    // 私有变量，用于标记暂停菜单是否开启
    private bool _pauseMenuOn = false;
    // 可序列化的私有变量，用于在Inspector中设置UI物品栏
    [SerializeField] private UIInventoryBar uiInventoryBar = null;
    // 可序列化的私有变量，用于在Inspector中设置暂停菜单库存管理
    [SerializeField] private PauseMenuInventoryManagement pauseMenuInventoryManagement = null;
    // 可序列化的私有变量，用于在Inspector中设置暂停菜单游戏对象
    [SerializeField] private GameObject pauseMenu = null;
    // 可序列化的私有变量，用于在Inspector中设置菜单标签数组
    [SerializeField] private GameObject[] menuTabs = null;
    // 可序列化的私有变量，用于在Inspector中设置菜单按钮数组
    [SerializeField] private Button[] menuButtons = null;

    // 公开的属性，用于获取和设置暂停菜单的开启状态
    public bool PauseMenuOn { get => _pauseMenuOn; set => _pauseMenuOn = value; }

    // 在对象被创建时调用，用于初始化暂停菜单为关闭状态
    protected override void Awake()
    {
        base.Awake();

        pauseMenu.SetActive(false);
    }

    // 每帧调用一次
    private void Update()
    {
        PauseMenu();
    }

    // 处理暂停菜单逻辑的方法
    private void PauseMenu()
    {
        // 如果按下Esc键，则切换暂停菜单的开启状态
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (PauseMenuOn)
            {
                DisablePauseMenu();
            }
            else
            {
                EnablePauseMenu();
            }
        }
    }

    // 启用暂停菜单的方法
    private void EnablePauseMenu()
    {
        // 销毁当前被拖拽的物品
        uiInventoryBar.DestroyCurrentlyDraggedItems();

        // 清除当前选中的物品
        uiInventoryBar.ClearCurrentlySelectedItems();

        // 设置暂停菜单为开启状态
        PauseMenuOn = true;
        Player.Instance.PlayerInputIsDisabled = true;
        Time.timeScale = 0; // 暂停游戏时间
        pauseMenu.SetActive(true);

        // 触发垃圾回收
        System.GC.Collect();

        // 高亮显示选中的标签按钮
        HighlightButtonForSelectedTab();
    }

    // 禁用暂停菜单的方法
    public void DisablePauseMenu()
    {
        // 销毁当前被拖拽的物品
        pauseMenuInventoryManagement.DestroyCurrentlyDraggedItems();

        // 设置暂停菜单为关闭状态
        PauseMenuOn = false;
        Player.Instance.PlayerInputIsDisabled = false;
        Time.timeScale = 1; // 恢复游戏时间
        pauseMenu.SetActive(false);
    }

    // 为选中的标签设置高亮显示的方法
    private void HighlightButtonForSelectedTab()
    {
        for (int i = 0; i < menuTabs.Length; i++)
        {
            if (menuTabs[i].activeSelf)
            {
                SetButtonColorToActive(menuButtons[i]);
            }
            else
            {
                SetButtonColorToInactive(menuButtons[i]);
            }
        }
    }

    // 为按钮设置激活颜色的方法
    private void SetButtonColorToActive(Button button)
    {
        ColorBlock colors = button.colors;

        colors.normalColor = colors.pressedColor;

        button.colors = colors;
    }

    // 为按钮设置非激活颜色的方法
    private void SetButtonColorToInactive(Button button)
    {
        ColorBlock colors = button.colors;

        colors.normalColor = colors.disabledColor;

        button.colors = colors;
    }

    // 切换暂停菜单标签的方法
    public void SwitchPauseMenuTab(int tabNum)
    {
        for (int i = 0; i < menuTabs.Length; i++)
        {
            if (i != tabNum)
            {
                menuTabs[i].SetActive(false);
            }
            else
            {
                menuTabs[i].SetActive(true);
            }
        }

        HighlightButtonForSelectedTab();
    }

    // 退出游戏的方法
    public void QuitGame()
    {
        Application.Quit();
    }
}

/*UIManager类继承自SingletonMonobehaviour，这是一个单例模式的实现，确保UIManager在游戏场景中是唯一的实例。

_pauseMenuOn是私有变量，用于标记暂停菜单是否开启。

uiInventoryBar、pauseMenuInventoryManagement、pauseMenu、menuTabs和menuButtons是可序列化的私有变量，用于在Unity编辑器的Inspector面板中设置UI组件。

PauseMenuOn是公开的属性，用于获取和设置暂停菜单的开启状态。

Awake方法在游戏对象创建时调用，用于初始化暂停菜单为关闭状态。

Update方法每帧调用一次，用于处理暂停菜单逻辑。

PauseMenu方法处理暂停菜单的逻辑，包括监听Esc键的按下事件来切换暂停菜单的开启状态。

EnablePauseMenu方法启用暂停菜单，包括销毁被拖拽的物品、清除选中的物品、暂停游戏时间、激活暂停菜单游戏对象和触发垃圾回收。

DisablePauseMenu方法禁用暂停菜单，包括销毁被拖拽的物品、恢复游戏时间、关闭暂停菜单游戏对象。

HighlightButtonForSelectedTab方法为选中的标签设置高亮显示。

SetButtonColorToActive和SetButtonColorToInactive方法分别为按钮设置激活和非激活颜色。

SwitchPauseMenuTab方法切换暂停菜单的标签。

QuitGame方法退出游戏。

这个类的主要用途是管理游戏中的用户界面，特别是暂停菜单的显示和隐藏，以及与之相关的UI逻辑。通过实现这些方法，它可以响应用户的输入事件，并更新游戏世界中的UI状态。*/