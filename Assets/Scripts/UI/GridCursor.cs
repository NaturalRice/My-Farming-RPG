// 引用所需的命名空间
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GridCursor : MonoBehaviour
{
    // 私有变量，用于存储画布、网格、主相机
    private Canvas canvas;
    private Grid grid;
    private Camera mainCamera;
    // 可序列化的私有变量，用于在Inspector中设置光标的Image组件、RectTransform组件、绿色和红色精灵
    [SerializeField] private Image cursorImage = null;
    [SerializeField] private RectTransform cursorRectTransform = null;
    [SerializeField] private Sprite greenCursorSprite = null;
    [SerializeField] private Sprite redCursorSprite = null;
    [SerializeField] private SO_CropDetailsList so_CropDetailsList = null;

    // 私有变量，用于标记光标位置是否有效
    private bool _cursorPositionIsValid = false;
    // 公开的属性，用于获取和设置光标位置的有效性
    public bool CursorPositionIsValid { get => _cursorPositionIsValid; set => _cursorPositionIsValid = value; }

    // 私有变量，用于存储物品使用网格半径
    private int _itemUseGridRadius = 0;
    // 公开的属性，用于获取和设置物品使用网格半径
    public int ItemUseGridRadius { get => _itemUseGridRadius; set => _itemUseGridRadius = value; }

    // 私有变量，用于存储选中的物品类型
    private ItemType _selectedItemType;
    // 公开的属性，用于获取和设置选中的物品类型
    public ItemType SelectedItemType { get => _selectedItemType; set => _selectedItemType = value; }

    // 私有变量，用于标记光标是否启用
    private bool _cursorIsEnabled = false;
    // 公开的属性，用于获取和设置光标的启用状态
    public bool CursorIsEnabled { get => _cursorIsEnabled; set => _cursorIsEnabled = value; }

    // 当此脚本禁用时，移除场景加载后事件的监听
    private void OnDisable()
    {
        EventHandler.AfterSceneLoadEvent -= SceneLoaded;
    }

    // 当此脚本启用时，添加场景加载后事件的监听
    private void OnEnable()
    {
        EventHandler.AfterSceneLoadEvent += SceneLoaded;
    }

    // 在第一帧更新前调用
    private void Start()
    {
        mainCamera = Camera.main;
        canvas = GetComponentInParent<Canvas>();
    }

    // 每帧调用一次
    private void Update()
    {
        if (CursorIsEnabled)
        {
            DisplayCursor();
        }
    }

    // 显示光标的方法
    private Vector3Int DisplayCursor()
    {
        if (grid != null)
        {
            // 获取光标网格位置
            Vector3Int gridPosition = GetGridPositionForCursor();

            // 获取玩家网格位置
            Vector3Int playerGridPosition = GetGridPositionForPlayer();

            // 设置光标精灵
            SetCursorValidity(gridPosition, playerGridPosition);

            // 获取光标RectTransform位置
            cursorRectTransform.position = GetRectTransformPositionForCursor(gridPosition);

            return gridPosition;
        }
        else
        {
            return Vector3Int.zero;
        }
    }

    // 场景加载后调用的方法
    private void SceneLoaded()
    {
        grid = GameObject.FindObjectOfType<Grid>();
    }

    // 设置光标有效性的方法
    private void SetCursorValidity(Vector3Int cursorGridPosition, Vector3Int playerGridPosition)
    {
        // 默认设置光标为有效
        SetCursorToValid();

        // 检查物品使用网格半径是否有效
        if (Mathf.Abs(cursorGridPosition.x - playerGridPosition.x) > ItemUseGridRadius
            || Mathf.Abs(cursorGridPosition.y - playerGridPosition.y) > ItemUseGridRadius)
        {
            // 如果光标位置超出使用网格半径，则设置光标为无效
            SetCursorToInvalid();
            return;
        }

        // 获取选中物品的详情
        ItemDetails itemDetails = InventoryManager.Instance.GetSelectedInventoryItemDetails(InventoryLocation.player);

        if (itemDetails == null)
        {
            // 如果没有选中物品，则设置光标为无效
            SetCursorToInvalid();
            return;
        }

        // 获取光标位置的网格属性详情
        GridPropertyDetails gridPropertyDetails = GridPropertiesManager.Instance.GetGridPropertyDetails(cursorGridPosition.x, cursorGridPosition.y);

        if (gridPropertyDetails != null)
        {
            // 根据选中的物品类型和网格属性详情确定光标的有效性
            switch (itemDetails.itemType)
            {
                case ItemType.Seed:
                    if (!IsCursorValidForSeed(gridPropertyDetails))
                    {
                        SetCursorToInvalid();
                        return;
                    }
                    break;

                case ItemType.Commodity:
                    if (!IsCursorValidForCommodity(gridPropertyDetails))
                    {
                        SetCursorToInvalid();
                        return;
                    }
                    break;

                case ItemType.Watering_tool:
                case ItemType.Breaking_tool:
                case ItemType.Chopping_tool:
                case ItemType.Hoeing_tool:
                case ItemType.Reaping_tool:
                case ItemType.Collecting_tool:
                    if (!IsCursorValidForTool(gridPropertyDetails, itemDetails))
                    {
                        SetCursorToInvalid();
                        return;
                    }
                    break;

                case ItemType.none:
                    break;

                case ItemType.count:
                    break;

                default:
                    break;
            }
        }
        else
        {
            // 如果没有找到网格属性详情，则设置光标为无效
            SetCursorToInvalid();
            return;
        }
    }

    /// <summary>
    /// 设置光标为无效
    /// </summary>
    private void SetCursorToInvalid()
    {
        cursorImage.sprite = redCursorSprite;
        CursorPositionIsValid = false;
    }

    /// <summary>
    /// 设置光标为有效
    /// </summary>
    private void SetCursorToValid()
    {
        cursorImage.sprite = greenCursorSprite;
        CursorPositionIsValid = true;
    }

    /// <summary>
    /// 测试商品在目标网格属性详情下的光标有效性。如果有效返回true，如果无效返回false
    /// </summary>
    private bool IsCursorValidForCommodity(GridPropertyDetails gridPropertyDetails)
    {
        return gridPropertyDetails.canDropItem;
    }

    /// <summary>
    /// 测试种子在目标网格属性详情下的光标有效性。如果有效返回true，如果无效返回false
    /// </summary>
    private bool IsCursorValidForSeed(GridPropertyDetails gridPropertyDetails)
    {
        return gridPropertyDetails.canDropItem;
    }


    /// <summary>
    /// 为工具设置目标网格属性详情下的光标有效性。如果有效返回true，如果无效返回false
    /// </summary>
    private bool IsCursorValidForTool(GridPropertyDetails gridPropertyDetails, ItemDetails itemDetails)
    {
        // 根据工具类型进行判断
        switch (itemDetails.itemType)
        {
            case ItemType.Hoeing_tool:
                if (gridPropertyDetails.isDiggable == true && gridPropertyDetails.daysSinceDug == -1)
                {
                    #region 需要获取位置的所有物品，以便检查它们是否可以收获

                    // 获取光标世界位置
                    Vector3 cursorWorldPosition = new Vector3(GetWorldPositionForCursor().x + 0.5f, GetWorldPositionForCursor().y + 0.5f, 0f);

                    // 获取光标位置的所有物品
                    List<Item> itemList = new List<Item>();

                    HelperMethods.GetComponentsAtBoxLocation<Item>(out itemList, cursorWorldPosition, Settings.cursorSize, 0f);

                    #endregion Need to get any items at location so we can check if they are reapable

                    // 循环遍历找到的物品，查看是否有可以收获的类型 - 我们不允许玩家在有可以收获的景观物品的地方挖掘
                    bool foundReapable = false;

                    foreach (Item item in itemList)
                    {
                        if (InventoryManager.Instance.GetItemDetails(item.ItemCode).itemType == ItemType.Reapable_scenary)
                        {
                            foundReapable = true;
                            break;
                        }
                    }

                    if (foundReapable)
                    {
                        return false;
                    }
                    else
                    {
                        return true;
                    }
                }
                else
                {
                    return false;
                }

            case ItemType.Watering_tool:
                if (gridPropertyDetails.daysSinceDug > -1 && gridPropertyDetails.daysSinceWatered == -1)
                {
                    return true;
                }
                else
                {
                    return false;
                }

            case ItemType.Chopping_tool:
            case ItemType.Collecting_tool:
            case ItemType.Breaking_tool:

                // 检查是否可以用选中的工具收获物品，检查物品是否完全成熟

                // 检查是否种植了种子
                if (gridPropertyDetails.seedItemCode != -1)
                {
                    // 获取种子的作物详情
                    CropDetails cropDetails = so_CropDetailsList.GetCropDetails(gridPropertyDetails.seedItemCode);

                    // 如果找到了作物详情
                    if (cropDetails != null)
                    {
                        // 检查作物是否完全成熟
                        if (gridPropertyDetails.growthDays >= cropDetails.growthDays[cropDetails.growthDays.Length - 1])
                        {
                            // 检查作物是否可以用选中的工具收获
                            if (cropDetails.CanUseToolToHarvestCrop(itemDetails.itemCode))
                            {
                                return true;
                            }
                            else
                            {
                                return false;
                            }
                        }
                        else
                        {
                            return false;
                        }
                    }
                }

                return false;


            default:
                return false;
        }
    }

    // 禁用光标的方法
    public void DisableCursor()
    {
        cursorImage.color = Color.clear;

        CursorIsEnabled = false;
    }

    // 启用光标的方法
    public void EnableCursor()
    {
        cursorImage.color = new Color(1f, 1f, 1f, 1f);
        CursorIsEnabled = true;
    }

    // 获取光标网格位置的方法
    public Vector3Int GetGridPositionForCursor()
    {
        Vector3 worldPosition = mainCamera.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, -mainCamera.transform.position.z));  // z是物体在相机前的深度 - 相机在-10位置，所以物体在(-)-10前面=10
        return grid.WorldToCell(worldPosition);
    }

    // 获取玩家网格位置的方法
    public Vector3Int GetGridPositionForPlayer()
    {
        return grid.WorldToCell(Player.Instance.transform.position);
    }

    // 获取光标RectTransform位置的方法
    public Vector2 GetRectTransformPositionForCursor(Vector3Int gridPosition)
    {
        Vector3 gridWorldPosition = grid.CellToWorld(gridPosition);
        Vector2 gridScreenPosition = mainCamera.WorldToScreenPoint(gridWorldPosition);
        return RectTransformUtility.PixelAdjustPoint(gridScreenPosition, cursorRectTransform, canvas);
    }

    // 获取光标世界位置的方法
    public Vector3 GetWorldPositionForCursor()
    {
        return grid.CellToWorld(GetGridPositionForCursor());
    }
}

/*1. `GridCursor`类继承自`MonoBehaviour`，使其可以附加到Unity游戏对象上。

2. `canvas`、`grid`和`mainCamera`是私有变量，用于存储画布、网格和主相机。

3. `cursorImage`和`cursorRectTransform`是可序列化的私有变量，用于在Unity编辑器的Inspector面板中设置光标的Image组件和RectTransform组件。

4. `greenCursorSprite`和`redCursorSprite`是可序列化的私有变量，用于存储光标的绿色和红色精灵。

5. `so_CropDetailsList`是可序列化的私有变量，用于存储作物详情列表。

6. `_cursorPositionIsValid`和`CursorPositionIsValid`是私有变量和公开属性，用于控制光标位置的有效性。

7. `_itemUseGridRadius`和`ItemUseGridRadius`是私有变量和公开属性，用于控制物品使用网格半径。

8. `_selectedItemType`和`SelectedItemType`是私有变量和公开属性，用于存储选中的物品类型。

9. `_cursorIsEnabled`和`CursorIsEnabled`是私有变量和公开属性，用于控制光标的启用状态。

10. `OnDisable`和`OnEnable`方法分别在脚本禁用和启用时调用，用于添加和移除场景加载后事件的监听。

11. `Start`方法在游戏开始时调用，用于初始化主相机和画布。

12. `Update`方法每帧调用一次，用于更新光标显示。

13. `DisplayCursor`方法用于显示光标，获取光标的网格位置，并设置光标的RectTransform位置。

14. `SceneLoaded`方法在场景加载后调用，用于获取网格组件。

15. `SetCursorValidity`方法用于设置光标的有效性，根据光标位置和玩家位置判断光标是否在物品的使用网格半径内。

16. `SetCursorToValid`和`SetCursorToInvalid`方法用于设置光标为有效或无效，并更新光标的精灵。

17. `IsCursorValidForCommodity`、`IsCursorValidForSeed`和`IsCursorValidForTool`方法用于测试商品、种子和工具在目标网格属性详情下的光标有效性。

18. `DisableCursor`和`EnableCursor`方法用于禁用和启用光标。

19. `GetGridPositionForCursor`、`GetGridPositionForPlayer`、`GetRectTransformPositionForCursor`和`GetWorldPositionForCursor`方法用于获取光标和玩家的网格位置、RectTransform位置和世界位置。

这个类的主要用途是管理游戏中的网格光标行为，包括光标的显示、有效性和位置。通过实现这些方法，它可以响应用户的输入事件，并更新游戏世界中的光标状态。*/