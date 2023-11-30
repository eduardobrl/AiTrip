using System.Collections.Specialized;

namespace AiTrip.Infrastructure.Configurations
{
    public class DatabaseConfiguration
    {
        public const string Section = "Database";

        public string ConnectionStringSecretName { get; set; } = null!;

        public string DatabaseName { get; set; } = null!;

        public string CollectionName { get; set; } = null!;
    }
}
