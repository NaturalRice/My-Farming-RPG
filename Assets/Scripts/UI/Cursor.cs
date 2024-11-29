// 引用所需的命名空间
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Cursor : MonoBehaviour
{
    // 私有变量，用于存储画布和主相机
    private Canvas canvas;
    private Camera mainCamera;
    // 可序列化的私有变量，用于在Inspector中设置光标的Image组件和RectTransform组件
    [SerializeField] private Image cursorImage = null;
    [SerializeField] private RectTransform cursorRectTransform = null;
    // 可序列化的私有变量，用于在Inspector中设置光标的两种精灵：绿色和透明
    [SerializeField] private Sprite greenCursorSprite = null;
    [SerializeField] private Sprite transparentCursorSprite = null;
    // 可序列化的私有变量，用于在Inspector中设置网格光标
    [SerializeField] private GridCursor gridCursor = null;

    // 私有变量，用于标记光标是否启用
    private bool _cursorIsEnabled = false;
    // 公开的属性，用于获取和设置光标的启用状态
    public bool CursorIsEnabled { get => _cursorIsEnabled; set => _cursorIsEnabled = value; }

    // 私有变量，用于标记光标位置是否有效
    private bool _cursorPositionIsValid = false;
    // 公开的属性，用于获取和设置光标位置的有效性
    public bool CursorPositionIsValid { get => _cursorPositionIsValid; set => _cursorPositionIsValid = value; }

    // 私有变量，用于存储选中的物品类型
    private ItemType _selectedItemType;
    // 公开的属性，用于获取和设置选中的物品类型
    public ItemType SelectedItemType { get => _selectedItemType; set => _selectedItemType = value; }

    // 私有变量，用于存储物品的使用半径
    private float _itemUseRadius = 0f;
    // 公开的属性，用于获取和设置物品的使用半径
    public float ItemUseRadius { get => _itemUseRadius; set => _itemUseRadius = value; }

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
    private void DisplayCursor()
    {
        // 获取光标的世界位置
        Vector3 cursorWorldPosition = GetWorldPositionForCursor();

        // 设置光标的有效性
        SetCursorValidity(cursorWorldPosition, Player.Instance.GetPlayerCentrePosition());

        // 获取光标的RectTransform位置
        cursorRectTransform.position = GetRectTransformPositionForCursor();
    }

    // 设置光标有效性的方法
    private void SetCursorValidity(Vector3 cursorPosition, Vector3 playerPosition)
    {
        // 默认设置光标为有效
        SetCursorToValid();

        // 检查使用半径的角落
        if (
            cursorPosition.x > (playerPosition.x + ItemUseRadius / 2f) && cursorPosition.y > (playerPosition.y + ItemUseRadius / 2f)
            ||
            cursorPosition.x < (playerPosition.x - ItemUseRadius / 2f) && cursorPosition.y > (playerPosition.y + ItemUseRadius / 2f)
            ||
            cursorPosition.x < (playerPosition.x - ItemUseRadius / 2f) && cursorPosition.y < (playerPosition.y - ItemUseRadius / 2f)
            ||
            cursorPosition.x > (playerPosition.x + ItemUseRadius / 2f) && cursorPosition.y < (playerPosition.y - ItemUseRadius / 2f)
            )
        {
            // 如果光标位置超出使用半径，则设置光标为无效
            SetCursorToInvalid();
            return;
        }

        // 检查物品使用半径是否有效
        if (Mathf.Abs(cursorPosition.x - playerPosition.x) > ItemUseRadius
            || Mathf.Abs(cursorPosition.y - playerPosition.y) > ItemUseRadius)
        {
            // 如果光标位置超出使用半径，则设置光标为无效
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

        // 根据选中的物品类型和光标所在对象确定光标的有效性
        switch (itemDetails.itemType)
        {
            case ItemType.Watering_tool:
            case ItemType.Breaking_tool:
            case ItemType.Chopping_tool:
            case ItemType.Hoeing_tool:
            case ItemType.Reaping_tool:
            case ItemType.Collecting_tool:
                // 对于工具类型的物品，调用SetCursorValidityTool方法来设置光标有效性
                if (!SetCursorValidityTool(cursorPosition, playerPosition, itemDetails))
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

    /// <summary>
    /// 设置光标为有效
    /// </summary>
    private void SetCursorToValid()
    {
        cursorImage.sprite = greenCursorSprite;
        CursorPositionIsValid = true;

        gridCursor.DisableCursor();
    }

    /// <summary>
    /// 设置光标为无效
    /// </summary>
    private void SetCursorToInvalid()
    {
        cursorImage.sprite = transparentCursorSprite;
        CursorPositionIsValid = false;

        gridCursor.EnableCursor();
    }

    /// <summary>
    /// 为工具设置光标为有效或无效。如果有效返回true，如果无效返回false
    /// </summary>
    private bool SetCursorValidityTool(Vector3 cursorPosition, Vector3 playerPosition, ItemDetails itemDetails)
    {
        // 根据工具类型进行判断
        switch (itemDetails.itemType)
        {
            case ItemType.Reaping_tool:
                return SetCursorValidityReapingTool(cursorPosition, playerPosition, itemDetails);

            default:
                return false;
        }
    }

    // 为收割工具设置光标有效性的方法
    private bool SetCursorValidityReapingTool(Vector3 cursorPosition, Vector3 playerPosition, ItemDetails equippedItemDetails)
    {
        List<Item> itemList = new List<Item>();

        if (HelperMethods.GetComponentsAtCursorLocation<Item>(out itemList, cursorPosition))
        {
            if (itemList.Count != 0)
            {
                foreach (Item item in itemList)
                {
                    if (InventoryManager.Instance.GetItemDetails(item.ItemCode).itemType == ItemType.Reapable_scenary)
                    {
                        return true;
                    }
                }
            }
        }

        return false;
    }

    // 禁用光标的方法
    public void DisableCursor()
    {
        cursorImage.color = new Color(1f, 1f, 1f, 0f);
        CursorIsEnabled = false;
    }

    // 启用光标的方法
    public void EnableCursor()
    {
        cursorImage.color = new Color(1f, 1f, 1f, 1f);
        CursorIsEnabled = true;
    }

    // 获取光标世界位置的方法
    public Vector3 GetWorldPositionForCursor()
    {
        Vector3 screenPosition = new Vector3(Input.mousePosition.x, Input.mousePosition.y, 0f);

        Vector3 worldPosition = mainCamera.ScreenToWorldPoint(screenPosition);

        return worldPosition;
    }

    // 获取光标RectTransform位置的方法
    public Vector2 GetRectTransformPositionForCursor()
    {
        Vector2 screenPosition = new Vector2(Input.mousePosition.x, Input.mousePosition.y);

        return RectTransformUtility.PixelAdjustPoint(screenPosition, cursorRectTransform, canvas);
    }
}

/*Cursor类继承自MonoBehaviour，使其可以附加到Unity游戏对象上。

canvas和mainCamera是私有变量，用于存储画布和主相机。

cursorImage和cursorRectTransform是可序列化的私有变量，用于在Unity编辑器的Inspector面板中设置光标的Image组件和RectTransform组件。

greenCursorSprite和transparentCursorSprite是可序列化的私有变量，用于存储光标的绿色和透明精灵。

gridCursor是可序列化的私有变量，用于存储网格光标。

_cursorIsEnabled和CursorIsEnabled是私有变量和公开属性，用于控制光标的启用状态。

_cursorPositionIsValid和CursorPositionIsValid是私有变量和公开属性，用于控制光标位置的有效性。

_selectedItemType和SelectedItemType是私有变量和公开属性，用于存储选中的物品类型。

_itemUseRadius和ItemUseRadius是私有变量和公开属性，用于存储物品的使用半径。

Start方法在游戏开始时调用，用于初始化主相机和画布。

Update方法每帧调用一次，用于更新光标显示。

DisplayCursor方法用于显示光标，获取光标的世界位置，并设置光标的RectTransform位置。

SetCursorValidity方法用于设置光标的有效性，根据光标位置和玩家位置判断光标是否在物品的使用半径内。

SetCursorToValid和SetCursorToInvalid方法用于设置光标为有效或无效，并更新光标的精灵和颜色。

SetCursorValidityTool方法用于为工具类型的物品设置光标有效性。

SetCursorValidityReapingTool方法用于为收割工具设置光标有效性。

DisableCursor和EnableCursor方法用于禁用和启用光标。

GetWorldPositionForCursor和GetRectTransformPositionForCursor方法用于获取光标的世界位置和独立位置*/