// 引用所需的命名空间
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

// 继承自MonoBehaviour，并实现多个事件接口，用于处理拖拽、指针进入离开和点击事件
public class UIInventorySlot : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler, IPointerEnterHandler, IPointerExitHandler, IPointerClickHandler
{
    // 私有变量，用于存储主相机、父画布、父物品、网格光标和普通光标
    private Camera mainCamera;
    private Canvas parentCanvas;
    private Transform parentItem;
    private GridCursor gridCursor;
    private Cursor cursor;
    // 公开的私有变量，用于存储被拖拽的物品
    public GameObject draggedItem;

    // 公开的Image组件，用于显示物品栏的高亮和物品图像
    public Image inventorySlotHighlight;
    public Image inventorySlotImage;
    public TextMeshProUGUI textMeshProUGUI;
    // 可序列化的私有变量，用于在Inspector中设置物品栏
    [SerializeField] private UIInventoryBar inventoryBar = null;
    [SerializeField] private GameObject inventoryTextBoxPrefab = null;
    // 公开的私有变量，用于标记物品栏槽位是否被选中
    [HideInInspector] public bool isSelected = false;
    // 公开的私有变量，用于存储物品的详细信息
    [HideInInspector] public ItemDetails itemDetails;
    [SerializeField] private GameObject itemPrefab = null;
    // 公开的私有变量，用于存储物品的数量
    [HideInInspector] public int itemQuantity;
    [SerializeField] private int slotNumber = 0;

    // 在对象被创建时调用，用于初始化父画布
    private void Awake()
    {
        parentCanvas = GetComponentInParent<Canvas>();
    }

    // 当此脚本禁用时，移除事件监听
    private void OnDisable()
    {
        EventHandler.AfterSceneLoadEvent -= SceneLoaded;
        EventHandler.RemoveSelectedItemFromInventoryEvent -= RemoveSelectedItemFromInventory;
        EventHandler.DropSelectedItemEvent -= DropSelectedItemAtMousePosition;
    }

    // 当此脚本启用时，添加事件监听
    private void OnEnable()
    {
        EventHandler.AfterSceneLoadEvent += SceneLoaded;
        EventHandler.RemoveSelectedItemFromInventoryEvent += RemoveSelectedItemFromInventory;
        EventHandler.DropSelectedItemEvent += DropSelectedItemAtMousePosition;
    }

    // 在游戏开始时调用，用于初始化主相机和光标
    private void Start()
    {
        mainCamera = Camera.main;
        gridCursor = FindObjectOfType<GridCursor>();
        cursor = FindObjectOfType<Cursor>();
    }

    // 清除光标的方法
    private void ClearCursors()
    {
        // 禁用光标
        gridCursor.DisableCursor();
        cursor.DisableCursor();

        // 设置物品类型为无
        gridCursor.SelectedItemType = ItemType.none;
        cursor.SelectedItemType = ItemType.none;
    }

    /// <summary>
    /// 设置此物品栏槽位的物品为选中状态
    /// </summary>
    private void SetSelectedItem()
    {
        // 清除当前高亮显示的物品
        inventoryBar.ClearHighlightOnInventorySlots();

        // 高亮显示物品栏上的物品
        isSelected = true;

        // 设置高亮显示的物品栏槽位
        inventoryBar.SetHighlightedInventorySlots();

        // 设置光标使用半径
        gridCursor.ItemUseGridRadius = itemDetails.itemUseGridRadius;
        cursor.ItemUseRadius = itemDetails.itemUseRadius;

        // 如果物品需要网格光标，则启用光标
        if (itemDetails.itemUseGridRadius > 0)
        {
            gridCursor.EnableCursor();
        }
        else
        {
            gridCursor.DisableCursor();
        }

        // 如果物品需要普通光标，则启用光标
        if (itemDetails.itemUseRadius > 0f)
        {
            cursor.EnableCursor();
        }
        else
        {
            cursor.DisableCursor();
        }

        // 设置物品类型
        gridCursor.SelectedItemType = itemDetails.itemType;
        cursor.SelectedItemType = itemDetails.itemType;

        // 设置物品在库存中被选中
        InventoryManager.Instance.SetSelectedInventoryItem(InventoryLocation.player, itemDetails.itemCode);

        if (itemDetails.canBeCarried == true)
        {
            // 显示玩家携带物品
            Player.Instance.ShowCarriedItem(itemDetails.itemCode);
        }
        else // 显示玩家未携带物品
        {
            Player.Instance.ClearCarriedItem();
        }
    }

    // 清除选中物品的方法
    public void ClearSelectedItem()
    {
        ClearCursors();

        // 清除当前高亮显示的物品
        inventoryBar.ClearHighlightOnInventorySlots();

        isSelected = false;

        // 设置库存中无物品被选中
        InventoryManager.Instance.ClearSelectedInventoryItem(InventoryLocation.player);

        // 清除玩家携带的物品
        Player.Instance.ClearCarriedItem();
    }

    /// <summary>
    /// 在当前鼠标位置丢弃选中的物品（如果选中）。由DropItem事件调用。
    /// </summary>
    private void DropSelectedItemAtMousePosition()
    {
        if (itemDetails != null && isSelected)
        {
            // 如果有效的光标位置
            if (gridCursor.CursorPositionIsValid)
            {
                Vector3 worldPosition = mainCamera.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, -mainCamera.transform.position.z));
                // 在鼠标位置创建物品
                GameObject itemGameObject = Instantiate(itemPrefab, new Vector3(worldPosition.x, worldPosition.y - Settings.gridCellSize / 2f, worldPosition.z), Quaternion.identity, parentItem);
                Item item = itemGameObject.GetComponent<Item>();
                item.ItemCode = itemDetails.itemCode;

                // 从玩家库存中移除物品
                InventoryManager.Instance.RemoveItem(InventoryLocation.player, item.ItemCode);

                // 如果没有更多物品，则清除选中
                if (InventoryManager.Instance.FindItemInInventory(InventoryLocation.player, item.ItemCode) == -1)
                {
                    ClearSelectedItem();
                }
            }
        }
    }

    // 从库存中移除选中物品的方法
    private void RemoveSelectedItemFromInventory()
    {
        if (itemDetails != null && isSelected)
        {
            int itemCode = itemDetails.itemCode;

            // 从玩家库存中移除物品
            InventoryManager.Instance.RemoveItem(InventoryLocation.player, itemCode);

            // 如果没有更多物品，则清除选中
            if (InventoryManager.Instance.FindItemInInventory(InventoryLocation.player, itemCode) == -1)
            {
                ClearSelectedItem();
            }
        }
    }


    // 实现IBeginDragHandler接口的方法，开始拖拽时调用
    public void OnBeginDrag(PointerEventData eventData)
    {
        if (itemDetails != null)
        {
            // 禁用键盘输入
            Player.Instance.DisablePlayerInputAndResetMovement();

            // 实例化游戏对象作为被拖拽的物品
            draggedItem = Instantiate(inventoryBar.inventoryBarDraggedItem, inventoryBar.transform);

            // 获取被拖拽物品的图像
            Image draggedItemImage = draggedItem.GetComponentInChildren<Image>();
            draggedItemImage.sprite = inventorySlotImage.sprite;

            SetSelectedItem();
        }
    }

    // 实现IDragHandler接口的方法，拖拽时调用
    public void OnDrag(PointerEventData eventData)
    {
        // 移动被拖拽的游戏对象
        if (draggedItem != null)
        {
            draggedItem.transform.position = Input.mousePosition;
        }
    }

    // 实现IEndDragHandler接口的方法，结束拖拽时调用
    public void OnEndDrag(PointerEventData eventData)
    {
        // 销毁被拖拽的游戏对象
        if (draggedItem != null)
        {
            Destroy(draggedItem);

            // 如果拖拽结束在物品栏上，获取物品拖拽结束的槽位并交换它们
            if (eventData.pointerCurrentRaycast.gameObject != null && eventData.pointerCurrentRaycast.gameObject.GetComponent<UIInventorySlot>() != null)
            {
                // 获取拖拽结束的槽位编号
                int toSlotNumber = eventData.pointerCurrentRaycast.gameObject.GetComponent<UIInventorySlot>().slotNumber;

                // 在库存列表中交换物品
                InventoryManager.Instance.SwapInventoryItems(InventoryLocation.player, slotNumber, toSlotNumber);

                // 销毁库存文本框
                DestroyInventoryTextBox();

                // 清除选中物品
                ClearSelectedItem();
            }
            // 否则尝试丢弃物品（如果它可以被丢弃）
            else
            {
                if (itemDetails.canBeDropped)
                {
                    DropSelectedItemAtMousePosition();
                }
            }

            // 启用玩家输入
            Player.Instance.EnablePlayerInput();
        }
    }

    // 实现IPointerClickHandler接口的方法，点击时调用
    public void OnPointerClick(PointerEventData eventData)
    {
        // 如果左键点击
        if (eventData.button == PointerEventData.InputButton.Left)
        {
            // 如果物品栏槽位当前被选中则取消选中
            if (isSelected == true)
            {
                ClearSelectedItem();
            }
            else
            {
                if (itemQuantity > 0)
                {
                    SetSelectedItem();
                }
            }
        }
    }
    // 实现IPointerEnterHandler接口的方法，指针进入时调用
    public void OnPointerEnter(PointerEventData eventData)
    {
        // Populate text box with item details
        if (itemQuantity != 0)// 如果物品数量不为0
        {
            // 实例化库存文本框
            inventoryBar.inventoryTextBoxGameobject = Instantiate(inventoryTextBoxPrefab, transform.position, Quaternion.identity);
            inventoryBar.inventoryTextBoxGameobject.transform.SetParent(parentCanvas.transform, false);

            UIInventoryTextBox inventoryTextBox = inventoryBar.inventoryTextBoxGameobject.GetComponent<UIInventoryTextBox>();

            // 设置物品类型描述
            string itemTypeDescription = InventoryManager.Instance.GetItemTypeDescription(itemDetails.itemType);

            // 填充文本框
            inventoryTextBox.SetTextboxText(itemDetails.itemDescription, itemTypeDescription, "", itemDetails.itemLongDescription, "", "");

            // 根据物品栏位置设置文本框位置
            if (inventoryBar.IsInventoryBarPositionBottom)

            {
                inventoryBar.inventoryTextBoxGameobject.GetComponent<RectTransform>().pivot = new Vector2(0.5f, 0f);
                inventoryBar.inventoryTextBoxGameobject.transform.position = new Vector3(transform.position.x, transform.position.y + 50f, transform.position.z);
            }
            else
            {
                inventoryBar.inventoryTextBoxGameobject.GetComponent<RectTransform>().pivot = new Vector2(0.5f, 1f);
                inventoryBar.inventoryTextBoxGameobject.transform.position = new Vector3(transform.position.x, transform.position.y - 50f, transform.position.z);
            }
        }
    }

    // 实现IPointerExitHandler接口的方法，指针离开时调用
    public void OnPointerExit(PointerEventData eventData)
    {
        DestroyInventoryTextBox();
    }

    // 销毁库存文本框的方法
    public void DestroyInventoryTextBox()
    {
        if (inventoryBar.inventoryTextBoxGameobject != null)
        {
            Destroy(inventoryBar.inventoryTextBoxGameobject);
        }
    }

    // 场景加载时调用的方法
    public void SceneLoaded()
    {
        parentItem = GameObject.FindGameObjectWithTag(Tags.ItemsParentTransform).transform;
    }
}

/*1. `UIInventorySlot`类继承自`MonoBehaviour`，并实现了多个事件接口，用于处理拖拽、指针进入离开和点击事件。

2. `mainCamera`、`parentCanvas`、`parentItem`、`gridCursor`和`cursor`是私有变量，用于存储主相机、父画布、父物品、网格光标和普通光标。

3. `draggedItem`是公开的私有变量，用于存储被拖拽的物品。

4. `inventorySlotHighlight`、`inventorySlotImage`和`textMeshProUGUI`是公开的Image和TextMeshProUGUI组件，用于显示物品栏的高亮和物品图像。

5. `inventoryBar`和`inventoryTextBoxPrefab`是可序列化的私有变量，用于在Inspector中设置物品栏和库存文本框的预制体。

6. `isSelected`是公开的私有变量，用于标记物品栏槽位是否被选中。

7. `itemDetails`是公开的私有变量，用于存储物品的详细信息。

8. `itemPrefab`是可序列化的私有变量，用于存储物品的预制体。

9. `itemQuantity`是公开的私有变量，用于存储物品的数量。

10. `slotNumber`是可序列化的私有变量，用于存储槽位编号。

11. `Awake`方法在游戏对象创建时调用，用于初始化父画布。

12. `OnEnable`和`OnDisable`方法分别在脚本启用和禁用时调用，用于添加和移除事件监听。

13. `Start`方法在游戏开始时调用，用于初始化主相机和光标。

14. `ClearCursors`方法清除光标。

15. `SetSelectedItem`方法设置物品栏槽位的物品为选中状态。

16. `ClearSelectedItem`方法清除选中物品。

17. `DropSelectedItemAtMousePosition`方法在当前鼠标位置丢弃选中的物品。

18. `RemoveSelectedItemFromInventory`方法从库存中移除选中物品。

19. `OnBeginDrag`、`OnDrag`和`OnEndDrag`方法分别在开始拖拽、拖拽和结束拖拽时调用。

20. `OnPointerClick`方法在点击时调用。

21. `OnPointerEnter`和`OnPointerExit`方法分别在指针进入和离开时调用。

22. `DestroyInventoryTextBox`方法销毁库存文本框。

23. `SceneLoaded`方法在场景加载时调用。

这个类的主要用途是管理游戏中的物品栏UI槽位，包括物品的显示、选中、拖拽和丢弃。通过实现多个事件接口，它可以响应用户的输入事件，并更新游戏世界中的物品状态。*/