namespace AiKakeiboBackend.Attributes
{
    /// <summary>
    /// サービスクラスに付与する属性
    /// この属性が付与されたクラスは、依存性注入コンテナに自動的に登録されます
    /// </summary>
    [AttributeUsage(AttributeTargets.Class, Inherited = false, AllowMultiple = false)]
    public class ServiceAttribute : Attribute
    {
    }
}
