// 引用所需的命名空间
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIInventoryBar : MonoBehaviour
{
    // 可序列化的私有变量，用于在Inspector中设置空白图标
    [SerializeField] private Sprite blank16x16sprite = null;
    // 可序列化的私有变量，用于在Inspector中设置物品栏的槽位
    [SerializeField] private UIInventorySlot[] inventorySlot = null;
    // 公开的私有变量，用于存储被拖拽的物品
    public GameObject inventoryBarDraggedItem;
    // 公开的私有变量，用于存储物品栏文本框的游戏对象
    [HideInInspector] public GameObject inventoryTextBoxGameobject;

    // 私有变量，用于存储RectTransform组件
    private RectTransform rectTransform;

    // 私有变量，用于标记物品栏是否位于底部位置
    private bool _isInventoryBarPositionBottom = true;

    // 公开的属性，用于获取和设置物品栏的位置
    public bool IsInventoryBarPositionBottom { get => _isInventoryBarPositionBottom; set => _isInventoryBarPositionBottom = value; }

    // 在对象被创建时调用，用于初始化RectTransform组件
    private void Awake()
    {
        rectTransform = GetComponent<RectTransform>();
    }

    // 当此脚本禁用时，移除库存更新事件的监听
    private void OnDisable()
    {
        EventHandler.InventoryUpdatedEvent -= InventoryUpdated;
    }

    // 当此脚本启用时，添加库存更新事件的监听
    private void OnEnable()
    {
        EventHandler.InventoryUpdatedEvent += InventoryUpdated;
    }

    // 每帧调用，用于根据玩家位置切换物品栏位置
    private void Update()
    {
        SwitchInventoryBarPosition();
    }

    /// <summary>
    /// 清除物品栏上的所有高亮显示
    /// </summary>
    public void ClearHighlightOnInventorySlots()
    {
        if (inventorySlot.Length > 0)
        {
            // 循环遍历物品栏的槽位并清除高亮显示的精灵
            for (int i = 0; i < inventorySlot.Length; i++)
            {
                if (inventorySlot[i].isSelected)
                {
                    inventorySlot[i].isSelected = false;
                    inventorySlot[i].inventorySlotHighlight.color = new Color(0f, 0f, 0f, 0f);
                    // 更新库存以显示物品未被选中
                    InventoryManager.Instance.ClearSelectedInventoryItem(InventoryLocation.player);
                }
            }
        }
    }

    // 清除物品栏的槽位
    private void ClearInventorySlots()
    {
        if (inventorySlot.Length > 0)
        {
            // 循环遍历物品栏的槽位并更新为空白精灵
            for (int i = 0; i < inventorySlot.Length; i++)
            {
                inventorySlot[i].inventorySlotImage.sprite = blank16x16sprite;
                inventorySlot[i].textMeshProUGUI.text = "";
                inventorySlot[i].itemDetails = null;
                inventorySlot[i].itemQuantity = 0;
                SetHighlightedInventorySlots(i);
            }
        }
    }

    // 库存更新事件的回调函数
    private void InventoryUpdated(InventoryLocation inventoryLocation, List<InventoryItem> inventoryList)
    {
        if (inventoryLocation == InventoryLocation.player)
        {
            ClearInventorySlots();

            if (inventorySlot.Length > 0 && inventoryList.Count > 0)
            {
                // 循环遍历物品栏的槽位并更新为对应的库存列表项
                for (int i = 0; i < inventorySlot.Length; i++)
                {
                    if (i < inventoryList.Count)
                    {
                        int itemCode = inventoryList[i].itemCode;

                        // 从库存管理器中获取物品详情
                        ItemDetails itemDetails = InventoryManager.Instance.GetItemDetails(itemCode);

                        if (itemDetails != null)
                        {
                            // 将图像和详情添加到物品栏的槽位
                            inventorySlot[i].inventorySlotImage.sprite = itemDetails.itemSprite;
                            inventorySlot[i].textMeshProUGUI.text = inventoryList[i].itemQuantity.ToString();
                            inventorySlot[i].itemDetails = itemDetails;
                            inventorySlot[i].itemQuantity = inventoryList[i].itemQuantity;
                            SetHighlightedInventorySlots(i);
                        }
                    }
                    else
                    {
                        break;
                    }
                }
            }
        }
    }

    /// <summary>
    ///  如果设置，则为所有物品栏项设置选中高亮
    /// </summary>
    public void SetHighlightedInventorySlots()
    {
        if (inventorySlot.Length > 0)
        {
            // 循环遍历物品栏的槽位并清除高亮显示的精灵
            for (int i = 0; i < inventorySlot.Length; i++)
            {
                SetHighlightedInventorySlots(i);
            }
        }
    }

    /// <summary>
    ///  如果设置，则为给定的物品栏项设置选中高亮
    /// </summary>
    public void SetHighlightedInventorySlots(int itemPosition)
    {
        if (inventorySlot.Length > 0 && inventorySlot[itemPosition].itemDetails != null)
        {
            if (inventorySlot[itemPosition].isSelected)
            {
                inventorySlot[itemPosition].inventorySlotHighlight.color = new Color(1f, 1f, 1f, 1f);

                // 更新库存以显示物品被选中
                InventoryManager.Instance.SetSelectedInventoryItem(InventoryLocation.player, inventorySlot[itemPosition].itemDetails.itemCode);
            }
        }
    }

    // 根据玩家视口位置切换物品栏的位置
    private void SwitchInventoryBarPosition()
    {
        Vector3 playerViewportPosition = Player.Instance.GetPlayerViewportPosition();

        if (playerViewportPosition.y > 0.3f && IsInventoryBarPositionBottom == false)
        {
            // 更新RectTransform的锚点和位置以将物品栏移动到底部
            rectTransform.pivot = new Vector2(0.5f, 0f);
            rectTransform.anchorMin = new Vector2(0.5f, 0f);
            rectTransform.anchorMax = new Vector2(0.5f, 0f);
            rectTransform.anchoredPosition = new Vector2(0f, 2.5f);

            IsInventoryBarPositionBottom = true;
        }
        else if (playerViewportPosition.y <= 0.3f && IsInventoryBarPositionBottom == true)
        {
            // 更新RectTransform的锚点和位置以将物品栏移动到顶部
            rectTransform.pivot = new Vector2(0.5f, 1f);
            rectTransform.anchorMin = new Vector2(0.5f, 1f);
            rectTransform.anchorMax = new Vector2(0.5f, 1f);
            rectTransform.anchoredPosition = new Vector2(0f, -2.5f);

            IsInventoryBarPositionBottom = false;
        }
    }

    // 销毁当前被拖拽的物品
    public void DestroyCurrentlyDraggedItems()
    {
        for (int i = 0; i < inventorySlot.Length; i++)
        {
            if (inventorySlot[i].draggedItem != null)
            {
                Destroy(inventorySlot[i].draggedItem);
            }
        }
    }

    // 清除当前选中的物品
    public void ClearCurrentlySelectedItems()
    {
        for (int i = 0; i < inventorySlot.Length; i++)
        {
            inventorySlot[i].ClearSelectedItem();
        }
    }
}

/*UIInventoryBar类继承自MonoBehaviour，使其可以附加到Unity游戏对象上。

blank16x16sprite是一个可序列化的私有变量，用于存储空白图标。

inventorySlot是一个可序列化的私有变量，用于存储物品栏的槽位。

inventoryBarDraggedItem是一个公开的私有变量，用于存储被拖拽的物品。

inventoryTextBoxGameobject是一个公开的私有变量，用于存储物品栏文本框的游戏对象。

rectTransform是一个私有变量，用于存储RectTransform组件。

IsInventoryBarPositionBottom是一个公开的属性，用于获取和设置物品栏的位置。

Awake方法在游戏对象创建时调用，用于初始化RectTransform组件。

OnEnable和OnDisable方法分别在脚本启用和禁用时调用，用于添加和移除对InventoryUpdatedEvent事件的监听。

Update方法每帧调用，用于根据玩家位置切换物品栏位置。

ClearHighlightOnInventorySlots方法清除物品栏上的所有高亮显示。

ClearInventorySlots方法清除物品栏的槽位。

InventoryUpdated方法是库存更新事件的回调函数。

SetHighlightedInventorySlots方法为所有物品栏项设置选中高亮。

SwitchInventoryBarPosition方法根据玩家视口位置切换物品栏的位置。

DestroyCurrentlyDraggedItems方法销毁当前被拖拽的物品。

ClearCurrentlySelectedItems方法清除当前选中的物品。

这个类的主要用途是管理游戏中的物品栏UI，包括物品的显示、高亮显示和位置切换。通过响应库存更新事件，它可以确保物品栏UI与游戏世界中的物品状态同步。*/