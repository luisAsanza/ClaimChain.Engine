using System.ComponentModel.DataAnnotations;

namespace ClaimsEngine.Infra.Data.Configuration
{
    public class ClaimsDatabaseSettings
    {
        public static string ClaimsDbSection => "DatabaseSettings:ClaimsDb";

        [Required]
        public required string ConnectionString { get; set; }
        [Required]
        public int MaxRetryCount { get; set; }
        [Required]
        public int CommandTimeoutSeconds { get; set; }
    }
}
