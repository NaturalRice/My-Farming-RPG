[System.Serializable] // 标记这个结构体为可序列化，这意味着它可以被保存到文件中或通过网络传输
public struct CharacterAttribute // 定义一个公共结构体
{
    // 结构体的公共字段
    public CharacterPartAnimator characterPart; // 角色的部分，例如头部、身体等
    public PartVariantColour partVariantColour; // 角色部分的颜色变体
    public PartVariantType partVariantType; // 角色部分的类型变体

    // CharacterAttribute结构体的构造函数，用于创建新的CharacterAttribute实例
    public CharacterAttribute(CharacterPartAnimator characterPart, PartVariantColour partVariantColour, PartVariantType partVariantType)
    {
        // 将构造函数的参数赋值给结构体的字段
        this.characterPart = characterPart;
        this.partVariantColour = partVariantColour;
        this.partVariantType = partVariantType;
    }
}

/*[System.Serializable]：这个属性使得CharacterAttribute结构体可以被序列化，即它可以被转换成一种格式（如JSON或二进制），以便存储或传输。

CharacterAttribute 结构体：这是一个值类型，用于封装与角色自定义相关的数据。

字段：

characterPart：一个CharacterPartAnimator类型的字段，表示角色的一部分，例如头部、身体、手臂等。
partVariantColour：一个PartVariantColour类型的字段，表示角色部分的颜色变体。
partVariantType：一个PartVariantType类型的字段，表示角色部分的类型变体。
构造函数：CharacterAttribute结构体的构造函数接受三个参数，并将它们分别赋值给结构体的三个字段。这使得在创建CharacterAttribute实例时可以初始化其所有字段。

这个结构体通常用于游戏开发中，特别是在需要自定义角色外观和属性的情况下。通过这种方式，开发者可以轻松地创建和管理角色的不同变体，例如不同的服装、颜色和类型。*/