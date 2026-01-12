using AiKakeiboBackend.Attributes;
using System.Reflection;

namespace AiKakeiboBackend.Extensions
{
    /// <summary>
    /// IServiceCollectionの拡張メソッド
    /// </summary>
    public static class ServiceCollectionExtensions
    {
        /// <summary>
        /// カスタム属性（ServiceAttribute、RepositoryAttribute）が付与されたクラスを
        /// 自動的に依存性注入コンテナに登録します
        /// </summary>
        /// <param name="services">サービスコレクション</param>
        /// <returns>サービスコレクション</returns>
        public static IServiceCollection AddAutoRegisteredServices(this IServiceCollection services)
        {
            // 現在のアセンブリを取得
            Assembly assembly = Assembly.GetExecutingAssembly();

            // ServiceAttribute が付与されたクラスを登録
            RegisterAttributedClasses(services, assembly, typeof(ServiceAttribute));

            // RepositoryAttribute が付与されたクラスを登録
            RegisterAttributedClasses(services, assembly, typeof(RepositoryAttribute));

            return services;
        }

        /// <summary>
        /// 指定された属性が付与されたクラスを依存性注入コンテナに登録します
        /// </summary>
        /// <param name="services">サービスコレクション</param>
        /// <param name="assembly">検索対象のアセンブリ</param>
        /// <param name="attributeType">検索する属性の型</param>
        private static void RegisterAttributedClasses(IServiceCollection services, Assembly assembly, Type attributeType)
        {
            // 属性が付与されたクラスを取得
            IEnumerable<Type> types = assembly.GetTypes()
                .Where(t => t.IsClass && !t.IsAbstract && t.GetCustomAttribute(attributeType) != null);

            foreach (Type implementationType in types)
            {
                // クラスが実装しているインターフェースを取得（自分自身のインターフェースのみ）
                Type? interfaceType = implementationType.GetInterfaces()
                    .FirstOrDefault(i => i.Name == $"I{implementationType.Name}");

                if (interfaceType != null)
                {
                    // インターフェースと実装クラスのペアで登録
                    services.AddScoped(interfaceType, implementationType);
                }
                else
                {
                    // インターフェースがない場合は実装クラスのみ登録
                    services.AddScoped(implementationType);
                }
            }
        }
    }
}
