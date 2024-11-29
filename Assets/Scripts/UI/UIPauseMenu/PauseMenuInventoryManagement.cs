// 引用所需的命名空间
using System.Collections.Generic;
using UnityEngine;

public class PauseMenuInventoryManagement : MonoBehaviour
{
    // 可序列化的私有变量，用于在Inspector中设置库存管理槽位
    [SerializeField] private PauseMenuInventoryManagementSlot[] inventoryManagementSlot = null;
    // 公开的私有变量，用于存储被拖拽的物品的预制体
    public GameObject inventoryManagementDraggedItemPrefab;
    // 可序列化的私有变量，用于在Inspector中设置透明图标
    [SerializeField] private Sprite transparent16x16 = null;
    // 公开的私有变量，用于存储库存文本框的游戏对象
    [HideInInspector] public GameObject inventoryTextBoxGameobject;

    // 当此脚本启用时，添加库存更新事件的监听，并初始化玩家库存
    private void OnEnable()
    {
        EventHandler.InventoryUpdatedEvent += PopulatePlayerInventory;

        // 初始化玩家库存
        if (InventoryManager.Instance != null)
        {
            PopulatePlayerInventory(InventoryLocation.player, InventoryManager.Instance.inventoryLists[(int)InventoryLocation.player]);
        }
    }

    // 当此脚本禁用时，移除库存更新事件的监听，并销毁库存文本框游戏对象
    private void OnDisable()
    {
        EventHandler.InventoryUpdatedEvent -= PopulatePlayerInventory;

        DestroyInventoryTextBoxGameobject();
    }

    // 销毁库存文本框游戏对象的方法
    public void DestroyInventoryTextBoxGameobject()
    {
        // 如果创建了库存文本框，则销毁它
        if (inventoryTextBoxGameobject != null)
        {
            Destroy(inventoryTextBoxGameobject);
        }
    }

    // 销毁当前被拖拽的物品的方法
    public void DestroyCurrentlyDraggedItems()
    {
        // 循环遍历所有玩家库存物品
        for (int i = 0; i < InventoryManager.Instance.inventoryLists[(int)InventoryLocation.player].Count; i++)
        {
            if (inventoryManagementSlot[i].draggedItem != null)
            {
                Destroy(inventoryManagementSlot[i].draggedItem);
            }
        }
    }

    // 填充玩家库存的方法
    private void PopulatePlayerInventory(InventoryLocation inventoryLocation, List<InventoryItem> playerInventoryList)
    {
        if (inventoryLocation == InventoryLocation.player)
        {
            InitialiseInventoryManagementSlots();

            // 循环遍历所有玩家库存物品
            for (int i = 0; i < InventoryManager.Instance.inventoryLists[(int)InventoryLocation.player].Count; i++)
            {
                // 获取库存物品详情
                inventoryManagementSlot[i].itemDetails = InventoryManager.Instance.GetItemDetails(playerInventoryList[i].itemCode);
                inventoryManagementSlot[i].itemQuantity = playerInventoryList[i].itemQuantity;

                if (inventoryManagementSlot[i].itemDetails != null)
                {
                    // 更新库存管理槽位的图像和数量
                    inventoryManagementSlot[i].inventoryManagementSlotImage.sprite = inventoryManagementSlot[i].itemDetails.itemSprite;
                    inventoryManagementSlot[i].textMeshProUGUI.text = inventoryManagementSlot[i].itemQuantity.ToString();
                }
            }
        }
    }

    // 初始化库存管理槽位的方法
    private void InitialiseInventoryManagementSlots()
    {
        // 清除库存槽位
        for (int i = 0; i < Settings.playerMaximumInventoryCapacity; i++)
        {
            inventoryManagementSlot[i].greyedOutImageGO.SetActive(false);
            inventoryManagementSlot[i].itemDetails = null;
            inventoryManagementSlot[i].itemQuantity = 0;
            inventoryManagementSlot[i].inventoryManagementSlotImage.sprite = transparent16x16;
            inventoryManagementSlot[i].textMeshProUGUI.text = "";
        }

        // 灰色显示不可用的槽位
        for (int i = InventoryManager.Instance.inventoryListCapacityIntArray[(int)InventoryLocation.player]; i < Settings.playerMaximumInventoryCapacity; i++)
        {
            inventoryManagementSlot[i].greyedOutImageGO.SetActive(true);
        }
    }
}

/*PauseMenuInventoryManagement类继承自MonoBehaviour，使其可以附加到Unity游戏对象上。

inventoryManagementSlot是一个可序列化的私有变量，用于在Unity编辑器的Inspector面板中设置库存管理槽位。

inventoryManagementDraggedItemPrefab是一个公开的私有变量，用于存储被拖拽的物品的预制体。

transparent16x16是一个可序列化的私有变量，用于存储透明图标。

inventoryTextBoxGameobject是一个公开的私有变量，用于存储库存文本框的游戏对象。

OnEnable方法在脚本启用时调用，用于添加库存更新事件的监听，并初始化玩家库存。

OnDisable方法在脚本禁用时调用，用于移除库存更新事件的监听，并销毁库存文本框游戏对象。

DestroyInventoryTextBoxGameobject方法用于销毁库存文本框游戏对象。

DestroyCurrentlyDraggedItems方法用于销毁当前被拖拽的物品。

PopulatePlayerInventory方法用于填充玩家库存，更新库存管理槽位的图像和数量。

InitialiseInventoryManagementSlots方法用于初始化库存管理槽位，清除库存槽位，并灰色显示不可用的槽位。

这个类的主要用途是管理游戏中暂停菜单下的库存操作，包括初始化库存槽位、更新库存槽位的图像和数量、销毁被拖拽的物品和文本框游戏对象。通过响应库存更新事件，它可以确保库存UI与游戏世界中的库存状态同步。*/