using System.Collections; // 引用System.Collections命名空间，提供非泛型集合接口
using UnityEngine; // 引用Unity引擎的命名空间

public class Crop : MonoBehaviour // 定义一个继承自MonoBehaviour的公共类
{
    private int harvestActionCount = 0; // 私有字段，记录收割动作的次数

    [Tooltip("This should be populated from child transform gameobject showing harvest effect spawn point")]
    [SerializeField] private Transform harvestActionEffectTransform = null; // 可序列化的私有字段，用于存储收割效果的生成点

    [Tooltip("This should be populated from child gameobject")]
    [SerializeField] private SpriteRenderer cropHarvestedSpriteRenderer = null; // 可序列化的私有字段，用于存储收割后作物的SpriteRenderer

    [HideInInspector] // 属性用于在编辑器中隐藏该字段
    public Vector2Int cropGridPosition; // 公开字段，存储作物在网格中的位置

    public void ProcessToolAction(ItemDetails equippedItemDetails, bool isToolRight, bool isToolLeft, bool isToolDown, bool isToolUp) // 公共方法，处理工具动作
    {
        // 获取网格属性详情
        GridPropertyDetails gridPropertyDetails = GridPropertiesManager.Instance.GetGridPropertyDetails(cropGridPosition.x, cropGridPosition.y);

        if (gridPropertyDetails == null)
            return;

        // 获取种子物品详情
        ItemDetails seedItemDetails = InventoryManager.Instance.GetItemDetails(gridPropertyDetails.seedItemCode);
        if (seedItemDetails == null)
            return;

        // 获取作物详情
        CropDetails cropDetails = GridPropertiesManager.Instance.GetCropDetails(seedItemDetails.itemCode);
        if (cropDetails == null)
            return;

        // 获取作物的动画器组件
        Animator animator = GetComponentInChildren<Animator>();

        // 触发工具动画
        if (animator != null)
        {
            if (isToolRight || isToolUp)
            {
                animator.SetTrigger("usetoolright");
            }
            else if (isToolLeft || isToolDown)
            {
                animator.SetTrigger("usetoolleft");
            }
        }

        // 触发工具粒子效果
        if (cropDetails.isHarvestActionEffect)
        {
            EventHandler.CallHarvestActionEffectEvent(harvestActionEffectTransform.position, cropDetails.harvestActionEffect);
        }

        // 获取工具所需的收割动作次数
        int requiredHarvestActions = cropDetails.RequiredHarvestActionsForTool(equippedItemDetails.itemCode);
        if (requiredHarvestActions == -1)
            return; // 这个工具不能用于收割这个作物

        // 增加收割动作次数
        harvestActionCount += 1;

        // 检查是否达到所需的收割动作次数
        if (harvestActionCount >= requiredHarvestActions)
            HarvestCrop(isToolRight, isToolUp, cropDetails, gridPropertyDetails, animator);
    }

    private void HarvestCrop(bool isUsingToolRight, bool isUsingToolUp, CropDetails cropDetails, GridPropertyDetails gridPropertyDetails, Animator animator) // 私有方法，收割作物
    {
        // 如果有收割动画并且有动画器组件
        if (cropDetails.isHarvestedAnimation && animator != null)
        {
            // 如果有收割后的精灵，则添加到SpriteRenderer
            if (cropDetails.harvestedSprite != null)
            {
                if (cropHarvestedSpriteRenderer != null)
                {
                    cropHarvestedSpriteRenderer.sprite = cropDetails.harvestedSprite;
                }
            }

            if (isUsingToolRight || isUsingToolUp)
            {
                animator.SetTrigger("harvestright");
            }
            else
            {
                animator.SetTrigger("harvestleft");
            }
        }

        // 如果有收割声音
        if (cropDetails.harvestSound != SoundName.none)
        {
            AudioManager.Instance.PlaySound(cropDetails.harvestSound);
        }

        // 从网格属性中删除作物
        gridPropertyDetails.seedItemCode = -1;
        gridPropertyDetails.growthDays = -1;
        gridPropertyDetails.daysSinceLastHarvest = -1;
        gridPropertyDetails.daysSinceWatered = -1;

        // 是否在收割动画前隐藏作物
        if (cropDetails.hideCropBeforeHarvestedAnimation)
        {
            GetComponentInChildren<SpriteRenderer>().enabled = false;
        }

        // 是否在收割前禁用盒子碰撞器
        if (cropDetails.disableCropCollidersBeforeHarvestedAnimation)
        {
            // 禁用所有盒子碰撞器
            Collider2D[] collider2Ds = GetComponentsInChildren<Collider2D>();
            foreach (Collider2D collider2D in collider2Ds)
            {
                collider2D.enabled = false;
            }
        }

        GridPropertiesManager.Instance.SetGridPropertyDetails(gridPropertyDetails.gridX, gridPropertyDetails.gridY, gridPropertyDetails);

        // 如果有收割动画 - 在动画完成后销毁这个作物游戏对象
        if (cropDetails.isHarvestedAnimation && animator != null)
        {
            StartCoroutine(ProcessHarvestActionsAfterAnimation(cropDetails, gridPropertyDetails, animator));
        }
        else
        {
            HarvestActions(cropDetails, gridPropertyDetails);
        }
    }

    private IEnumerator ProcessHarvestActionsAfterAnimation(CropDetails cropDetails, GridPropertyDetails gridPropertyDetails, Animator animator) // 私有协程方法，在动画后处理收割动作
    {
        while (!animator.GetCurrentAnimatorStateInfo(0).IsName("Harvested"))
        {
            yield return null;
        }

        HarvestActions(cropDetails, gridPropertyDetails);
    }

    private void HarvestActions(CropDetails cropDetails, GridPropertyDetails gridPropertyDetails) // 私有方法，执行收割动作
    {
        SpawnHarvestedItems(cropDetails); // 产生收割后的物品

        // 这个作物是否转变为另一个作物
        if (cropDetails.harvestedTransformItemCode > 0)
        {
            CreateHarvestedTransformCrop(cropDetails, gridPropertyDetails); // 创建转变后的作物
        }

        Destroy(gameObject); // 销毁游戏对象
    }

    private void SpawnHarvestedItems(CropDetails cropDetails) // 私有方法，产生收割后的物品
    {
        // 产生物品
        for (int i = 0; i < cropDetails.cropProducedItemCode.Length; i++)
        {
            int cropsToProduce;

            // 计算要产生的作物数量
            if (cropDetails.cropProducedMinQuantity[i] == cropDetails.cropProducedMaxQuantity[i] ||
                cropDetails.cropProducedMaxQuantity[i] < cropDetails.cropProducedMinQuantity[i])
            {
                cropsToProduce = cropDetails.cropProducedMinQuantity[i];
            }
            else
            {
                cropsToProduce = Random.Range(cropDetails.cropProducedMinQuantity[i], cropDetails.cropProducedMaxQuantity[i] + 1);
            }

            for (int j = 0; j < cropsToProduce; j++)
            {
                Vector3 spawnPosition;
                if (cropDetails.spawnCropProducedAtPlayerPosition)
                {
                    // 将物品添加到玩家的库存
                    InventoryManager.Instance.AddItem(InventoryLocation.player, cropDetails.cropProducedItemCode[i]);
                }
                else
                {
                    // 随机位置
                    spawnPosition = new Vector3(transform.position.x + Random.Range(-1f, 1f), transform.position.y + Random.Range(-1f, 1f), 0f);
                    SceneItemsManager.Instance.InstantiateSceneItem(cropDetails.cropProducedItemCode[i], spawnPosition);
                }
            }
        }
    }

    private void CreateHarvestedTransformCrop(CropDetails cropDetails, GridPropertyDetails gridPropertyDetails) // 私有方法，创建转变后的作物
    {
        // 更新网格属性中的作物
        gridPropertyDetails.seedItemCode = cropDetails.harvestedTransformItemCode;
        gridPropertyDetails.growthDays = 0;
        gridPropertyDetails.daysSinceLastHarvest = -1;
        gridPropertyDetails.daysSinceWatered = -1;

        GridPropertiesManager.Instance.SetGridPropertyDetails(gridPropertyDetails.gridX, gridPropertyDetails.gridY, gridPropertyDetails);

        // 显示种植的作物
        GridPropertiesManager.Instance.DisplayPlantedCrop(gridPropertyDetails);
    }
}

/*Crop 类：这个类表示一个作物，继承自MonoBehaviour，可以附加到Unity场景中的GameObject上。

字段：

harvestActionCount：记录收割动作的次数。
harvestActionEffectTransform：存储收割效果的生成点。
cropHarvestedSpriteRenderer：存储收割后作物的SpriteRenderer。
cropGridPosition：存储作物在网格中的位置。
ProcessToolAction 方法：处理工具动作，如收割作物。它获取网格属性详情、种子物品详情和作物详情，然后根据工具类型触发相应的动画和粒子效果。

HarvestCrop 方法：执行收割作物的逻辑，包括播放动画、播放声音、更新网格属性和销毁作物GameObject。

 ProcessHarvestActionsAfterAnimation 方法**：一个协程方法，等待收割动画完成后执行收割动作。

HarvestActions 方法：执行收割动作，包括产生收割后的物品和创建转变后的作物。

SpawnHarvestedItems 方法：产生收割后的物品，可以是随机数量的物品，并且可以在玩家位置或随机位置生成。

CreateHarvestedTransformCrop 方法：创建转变后的作物，更新网格属性并显示种植的作物。

这个脚本通常用于游戏中的农业模拟，允许玩家使用工具收割作物，并根据作物的属性产生相应的物品。通过这种方式，游戏可以模拟真实的农业活动，增加游戏的互动性和趣味性。*/