using System.ComponentModel.DataAnnotations;

namespace ClaimsEngine.Infra.Data.Configuration
{
    public class DatabaseSettings
    {
        public static string ClaimsDbSection => "DatabaseSettings:ClaimsDb";

        [Required]
        public string? DefaultStatus { get; set; }
        [Required]
        public int MaxRetryAttempts { get; set; }
        [Required]
        public int CommandTimeoutSeconds { get; set; }
    }
}
