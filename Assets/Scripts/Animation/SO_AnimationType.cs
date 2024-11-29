using System.Collections; // 引用System.Collections命名空间，提供非泛型的集合接口
using System.Collections.Generic; // 引用System.Collections.Generic命名空间，提供泛型集合接口
using UnityEngine; // 引用Unity引擎的命名空间

[CreateAssetMenu(fileName = "so_AnimationType", menuName = "Scriptable Objects/Animation/Animation Type")] // 属性用于在Unity编辑器中创建该ScriptableObject时指定文件名和菜单路径
public class SO_AnimationType : ScriptableObject // 定义一个公共类，继承自ScriptableObject
{
    public AnimationClip animationClip; // 公开的字段，用于存储一个AnimationClip，表示一个动画剪辑
    public AnimationName animationName; // 公开的字段，用于存储一个AnimationName，表示动画的名称
    public CharacterPartAnimator characterPart; // 公开的字段，用于存储一个CharacterPartAnimator，表示角色的动画部分
    public PartVariantColour partVariantColour; // 公开的字段，用于存储一个PartVariantColour，表示部分的颜色变体
    public PartVariantType partVariantType; // 公开的字段，用于存储一个PartVariantType，表示部分的类型变体
}

/*命名空间引用：

System.Collections：提供了非泛型的集合接口，如ArrayList。
System.Collections.Generic：提供了泛型集合接口，如List<T>。
UnityEngine：提供了Unity引擎的核心功能，包括游戏对象、组件、资产管理等。
CreateAssetMenu 属性：

这是一个自定义属性，用于在Unity编辑器中创建该ScriptableObject时指定文件名（so_AnimationType）和菜单路径（Scriptable Objects/Animation/Animation Type）。这使得在创建新的ScriptableObject实例时，可以直接在Assets菜单下找到这个选项，方便创建和管理。
SO_AnimationType 类：

这个类继承自ScriptableObject，使其成为一个可以在Unity编辑器中创建和管理的资源对象。
它包含几个公开字段，用于存储和管理动画相关的数据。
字段：

animationClip：存储一个AnimationClip，这是Unity中表示动画剪辑的类，包含动画的关键帧数据。
animationName：存储一个AnimationName，这是一个自定义的枚举或类，用于标识不同的动画名称。
characterPart：存储一个CharacterPartAnimator，这是一个自定义的枚举或类，用于标识角色的不同部分（如头部、手臂等）。
partVariantColour：存储一个PartVariantColour，这是一个自定义的枚举或类，用于标识部分的颜色变体。
partVariantType：存储一个PartVariantType，这是一个自定义的枚举或类，用于标识部分的类型变体。
这个ScriptableObject通常用于游戏中的动画管理，可以通过创建不同的SO_AnimationType实例来定义和管理角色的不同动画状态和属性。这种方式使得动画数据更加模块化和易于管理，特别是在处理复杂角色动画时。*/