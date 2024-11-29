// 引用所需的命名空间
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

// 继承自MonoBehaviour，并实现多个事件接口，用于处理拖拽和指针进入离开事件
public class PauseMenuInventoryManagementSlot : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler, IPointerEnterHandler, IPointerExitHandler
{
    // 公开的Image组件，用于显示库存槽位的图像
    public Image inventoryManagementSlotImage;
    // 公开的TextMeshProUGUI组件，用于显示库存槽位的数量
    public TextMeshProUGUI textMeshProUGUI;
    // 公开的GameObject，用于显示库存槽位的灰色占位图
    public GameObject greyedOutImageGO;
    // 可序列化的私有变量，用于在Inspector中设置库存管理
    [SerializeField] private PauseMenuInventoryManagement inventoryManagement = null;
    [SerializeField] private GameObject inventoryTextBoxPrefab = null;

    // 公开的私有变量，用于存储物品的详细信息和数量
    [HideInInspector] public ItemDetails itemDetails;
    [HideInInspector] public int itemQuantity;
    [SerializeField] private int slotNumber = 0;

    // 公开的私有变量，用于存储被拖拽的物品
    public GameObject draggedItem;
    // 私有变量，用于存储父画布
    private Canvas parentCanvas;

    // 在对象被创建时调用，用于初始化父画布
    private void Awake()
    {
        parentCanvas = GetComponentInParent<Canvas>();
    }

    // 实现IBeginDragHandler接口的方法，开始拖拽时调用
    public void OnBeginDrag(PointerEventData eventData)
    {
        if (itemQuantity != 0)
        {
            // 实例化游戏对象作为被拖拽的物品
            draggedItem = Instantiate(inventoryManagement.inventoryManagementDraggedItemPrefab, inventoryManagement.transform);

            // 获取被拖拽物品的图像
            Image draggedItemImage = draggedItem.GetComponentInChildren<Image>();
            draggedItemImage.sprite = inventoryManagementSlotImage.sprite;
        }
    }

    // 实现IDragHandler接口的方法，拖拽时调用
    public void OnDrag(PointerEventData eventData)
    {
        // 移动游戏对象作为被拖拽的物品
        if (draggedItem != null)
        {
            draggedItem.transform.position = Input.mousePosition;
        }
    }

    // 实现IEndDragHandler接口的方法，结束拖拽时调用
    public void OnEndDrag(PointerEventData eventData)
    {
        // 销毁游戏对象作为被拖拽的物品
        if (draggedItem != null)
        {
            Destroy(draggedItem);

            // 获取拖拽结束时所在的对象
            if (eventData.pointerCurrentRaycast.gameObject != null && eventData.pointerCurrentRaycast.gameObject.GetComponent<PauseMenuInventoryManagementSlot>() != null)
            {
                // 获取拖拽结束的槽位编号
                int toSlotNumber = eventData.pointerCurrentRaycast.gameObject.GetComponent<PauseMenuInventoryManagementSlot>().slotNumber;

                // 在库存列表中交换物品
                InventoryManager.Instance.SwapInventoryItems(InventoryLocation.player, slotNumber, toSlotNumber);

                // 销毁库存文本框
                inventoryManagement.DestroyInventoryTextBoxGameobject();
            }
        }
    }

    // 实现IPointerEnterHandler接口的方法，指针进入时调用
    public void OnPointerEnter(PointerEventData eventData)
    {
        // 如果物品数量不为0，填充文本框与物品详情
        if (itemQuantity != 0)
        {
            // 实例化库存文本框
            inventoryManagement.inventoryTextBoxGameobject = Instantiate(inventoryTextBoxPrefab, transform.position, Quaternion.identity);
            inventoryManagement.inventoryTextBoxGameobject.transform.SetParent(parentCanvas.transform, false);

            UIInventoryTextBox inventoryTextBox = inventoryManagement.inventoryTextBoxGameobject.GetComponent<UIInventoryTextBox>();

            // 设置物品类型描述
            string itemTypeDescription = InventoryManager.Instance.GetItemTypeDescription(itemDetails.itemType);

            // 填充文本框
            inventoryTextBox.SetTextboxText(itemDetails.itemDescription, itemTypeDescription, "", itemDetails.itemLongDescription, "", "");

            // 设置文本框位置
            if (slotNumber > 23)
            {
                inventoryManagement.inventoryTextBoxGameobject.GetComponent<RectTransform>().pivot = new Vector2(0.5f, 0f);
                inventoryManagement.inventoryTextBoxGameobject.transform.position = new Vector3(transform.position.x, transform.position.y + 50f, transform.position.z);
            }
            else
            {
                inventoryManagement.inventoryTextBoxGameobject.GetComponent<RectTransform>().pivot = new Vector2(0.5f, 1f);
                inventoryManagement.inventoryTextBoxGameobject.transform.position = new Vector3(transform.position.x, transform.position.y - 50f, transform.position.z);
            }
        }
    }

    // 实现IPointerExitHandler接口的方法，指针离开时调用
    public void OnPointerExit(PointerEventData eventData)
    {
        inventoryManagement.DestroyInventoryTextBoxGameobject();
    }
}

/*PauseMenuInventoryManagementSlot类继承自MonoBehaviour，并实现了多个事件接口，用于处理拖拽和指针进入离开事件。

inventoryManagementSlotImage和textMeshProUGUI是公开的Image和TextMeshProUGUI组件，用于显示库存槽位的图像和数量。

greyedOutImageGO是公开的GameObject，用于显示库存槽位的灰色占位图。

inventoryManagement和inventoryTextBoxPrefab是可序列化的私有变量，用于在Inspector中设置库存管理和库存文本框的预制体。

itemDetails和itemQuantity是公开的私有变量，用于存储物品的详细信息和数量。

slotNumber是可序列化的私有变量，用于存储槽位编号。

draggedItem是公开的私有变量，用于存储被拖拽的物品。

parentCanvas是私有变量，用于存储父画布。

Awake方法在游戏对象创建时调用，用于初始化父画布。

OnBeginDrag、OnDrag和OnEndDrag方法分别在开始拖拽、拖拽和结束拖拽时调用。

OnPointerEnter和OnPointerExit方法分别在指针进入和离开时调用。

这个类的主要用途是管理游戏中暂停菜单下的单个库存槽位的交互，包括拖拽物品和显示物品详情。通过实现多个事件接口，它可以响应用户的输入事件，并更新游戏世界中的物品状态。*/