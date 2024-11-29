// 引用Unity引擎的命名空间
using UnityEngine;

// 附加到游戏对象上的脚本，用于处理物品拾取
public class ItemPickUp : MonoBehaviour
{
    // 当其他游戏对象进入该游戏对象的碰撞器时调用
    private void OnTriggerEnter2D(Collider2D collision)
    {
        // 尝试从碰撞的游戏对象中获取Item组件
        Item item = collision.GetComponent<Item>();

        // 如果找到了Item组件
        if (item != null)
        {
            // 获取物品的详细信息
            ItemDetails itemDetails = InventoryManager.Instance.GetItemDetails(item.ItemCode);

            // 如果物品可以被拾取
            if (itemDetails.canBePickedUp == true)
            {
                // 将物品添加到玩家的库存中
                InventoryManager.Instance.AddItem(InventoryLocation.player, item, collision.gameObject);

                // 播放拾取物品的声音
                AudioManager.Instance.PlaySound(SoundName.effectPickupSound);
            }
        }
    }
}

/*类定义：

ItemPickUp 类继承自 MonoBehaviour，表示这是一个可以附加到Unity游戏对象上的脚本。
方法：

OnTriggerEnter2D：这是一个Unity的回调方法，当一个游戏对象的碰撞器进入另一个游戏对象的触发器时被调用。这里使用的是2D版本的触发器。
GetComponent<Item>()：这个方法尝试从进入触发器的碰撞器所在的游戏对象上获取Item组件。
物品拾取逻辑：

如果找到了Item组件，脚本会从InventoryManager单例中获取该物品的详细信息，检查物品是否可以被拾取。
如果ItemDetails中的canBePickedUp属性为true，则表示物品可以被拾取，脚本会将物品添加到玩家的库存中。
InventoryManager.Instance.AddItem()：这个方法将物品添加到指定位置的库存中，这里的位置是InventoryLocation.player，表示玩家的库存。
AudioManager.Instance.PlaySound()：播放一个拾取物品的声音效果，声音的名称是SoundName.effectPickupSound。
这个类的设计目的是为了简化物品拾取的过程，当玩家的游戏对象与物品的游戏对象发生碰撞时，会自动检查物品是否可以被拾取，并将可拾取的物品添加到玩家的库存中，同时播放相应的声音效果。这种设计在角色扮演游戏（RPG）或冒险游戏中非常常见，用于增强游戏的互动性和沉浸感。*/