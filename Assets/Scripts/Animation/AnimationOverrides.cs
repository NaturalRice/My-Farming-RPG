using System.Collections.Generic; // 引用集合类
using System.Diagnostics; // 引用诊断工具类，用于性能测试（虽然这部分代码被注释掉了）
using UnityEngine; // 引用Unity引擎的核心命名空间

public class AnimationOverrides : MonoBehaviour // 定义一个继承自MonoBehaviour的公共类
{
    [SerializeField] private GameObject character = null; // 可序列化的私有字段，用于存储角色对象
    [SerializeField] private SO_AnimationType[] soAnimationTypeArray = null; // 可序列化的私有字段，用于存储动画类型数组

    private Dictionary<AnimationClip, SO_AnimationType> animationTypeDictionaryByAnimation; // 用于存储按动画剪辑键入的动画类型字典
    private Dictionary<string, SO_AnimationType> animationTypeDictionaryByCompositeAttributeKey; // 用于存储按复合属性键入的动画类型字典

    private void Start() // 在游戏开始时调用
    {
        // 初始化按动画剪辑键入的动画类型字典
        animationTypeDictionaryByAnimation = new Dictionary<AnimationClip, SO_AnimationType>();

        foreach (SO_AnimationType item in soAnimationTypeArray) // 遍历动画类型数组
        {
            animationTypeDictionaryByAnimation.Add(item.animationClip, item); // 将动画剪辑和类型添加到字典中
        }

        // 初始化按字符串键入的动画类型字典
        animationTypeDictionaryByCompositeAttributeKey = new Dictionary<string, SO_AnimationType>();

        foreach (SO_AnimationType item in soAnimationTypeArray) // 再次遍历动画类型数组
        {
            string key = item.characterPart.ToString() + item.partVariantColour.ToString() + item.partVariantType.ToString() + item.animationName.ToString(); // 创建一个复合键
            animationTypeDictionaryByCompositeAttributeKey.Add(key, item); // 将复合键和类型添加到字典中
        }
    }

    public void ApplyCharacterCustomisationParameters(List<CharacterAttribute> characterAttributesList) // 公共方法，用于应用角色定制参数
    {
        // Stopwatch s1 = Stopwatch.StartNew(); // 用于计时的代码被注释掉了

        // 遍历所有角色属性，并为每个属性设置动画覆盖控制器
        foreach (CharacterAttribute characterAttribute in characterAttributesList)
        {
            Animator currentAnimator = null; // 当前Animator对象
            List<KeyValuePair<AnimationClip, AnimationClip>> animsKeyValuePairList = new List<KeyValuePair<AnimationClip, AnimationClip>>(); // 用于存储动画剪辑键值对的列表

            string animatorSOAssetName = characterAttribute.characterPart.ToString(); // 获取Animator的名称

            // 在场景中找到匹配脚本对象Animator类型的Animator
            Animator[] animatorsArray = character.GetComponentsInChildren<Animator>();

            foreach (Animator animator in animatorsArray) // 遍历Animator数组
            {
                if (animator.name == animatorSOAssetName) // 如果找到匹配的Animator
                {
                    currentAnimator = animator; // 赋值给当前Animator
                    break; // 跳出循环
                }
            }

            // 获取Animator的基础当前动画
            AnimatorOverrideController aoc = new AnimatorOverrideController(currentAnimator.runtimeAnimatorController); // 创建一个新的AnimatorOverrideController
            List<AnimationClip> animationsList = new List<AnimationClip>(aoc.animationClips); // 获取AnimatorOverrideController中的所有动画剪辑

            foreach (AnimationClip animationClip in animationsList) // 遍历所有动画剪辑
            {
                // 在字典中查找动画
                SO_AnimationType so_AnimationType;
                bool foundAnimation = animationTypeDictionaryByAnimation.TryGetValue(animationClip, out so_AnimationType); // 尝试获取动画类型

                if (foundAnimation) // 如果找到了动画
                {
                    string key = characterAttribute.characterPart.ToString() + characterAttribute.partVariantColour.ToString() + characterAttribute.partVariantType.ToString() + so_AnimationType.animationName.ToString(); // 创建一个新的复合键

                    SO_AnimationType swapSO_AnimationType;
                    bool foundSwapAnimation = animationTypeDictionaryByCompositeAttributeKey.TryGetValue(key, out swapSO_AnimationType); // 尝试获取替换的动画类型

                    if (foundSwapAnimation) // 如果找到了替换的动画
                    {
                        AnimationClip swapAnimationClip = swapSO_AnimationType.animationClip; // 获取替换的动画剪辑

                        animsKeyValuePairList.Add(new KeyValuePair<AnimationClip, AnimationClip>(animationClip, swapAnimationClip)); // 将原始动画剪辑和替换动画剪辑添加到列表中
                    }
                }
            }

            // 将动画更新应用到动画覆盖控制器，然后更新Animator以使用新的控制器
            aoc.ApplyOverrides(animsKeyValuePairList); // 应用动画剪辑的覆盖
            currentAnimator.runtimeAnimatorController = aoc; // 将Animator的控制器设置为新的AnimatorOverrideController
        }

        // s1.Stop(); // 停止计时器
        // UnityEngine.Debug.Log("Time to apply character customisation : " + s1.Elapsed + "   elapsed seconds"); // 打印应用角色定制所需的时间
    }
}

/*类定义：AnimationOverrides类继承自MonoBehaviour，使其可以附加到Unity场景中的GameObject上。

字段和字典：类中定义了两个字典，一个用于按AnimationClip键入，另一个用于按复合属性键入（如角色部分、颜色、类型和动画名称）。

初始化字典：在Start方法中，初始化这两个字典，将soAnimationTypeArray数组中的元素添加到字典中。

应用角色定制参数：ApplyCharacterCustomisationParameters方法接受一个CharacterAttribute列表，遍历每个属性，找到对应的Animator，并为每个Animator设置动画覆盖控制器。它通过查找原始动画剪辑和替换动画剪辑，然后将它们添加到AnimatorOverrideController中，最后更新Animator以使用新的控制器。

性能测试：代码中注释掉了性能测试的部分，这部分代码可以用来测量应用角色定制参数所需的时间。

这段代码的主要功能是根据角色的定制属性，动态地替换角色动画，实现角色外观和动画的个性化。*/