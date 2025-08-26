using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations.Schema;

namespace Core.Automation.DAL
{
    [Table("Job")]
    [PrimaryKey(nameof(Id))]
    public class JobHistory(long jobId, DateTime startDate)
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long Id { get; set; }
        public long JobId { get; set; } = jobId;
        [DatabaseGenerated(DatabaseGeneratedOption.Computed)]
        public DateTime CreatedDate { get; set; }
        public DateTime StartDate { get; set; } = startDate;
        public DateTime? FinishDate { get; set; }
        public string? Status { get; set; }
    }
}
