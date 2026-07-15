using System.ComponentModel.DataAnnotations;

namespace EduCore.Models
{
    public class LeaveType
    {
        [Key]
        public int LeaveTypeId { get; set; }

        [Required]
        [Display(Name = "Leave Type")]
        [StringLength(100)]
        public string LeaveTypeName { get; set; } = string.Empty;

        [Required]
        [Display(Name = "Leave Code")]
        [StringLength(10)]
        public string LeaveCode { get; set; } = string.Empty;

        [Display(Name = "Description")]
        [StringLength(300)]
        public string? Description { get; set; }

        [Display(Name = "Maximum Days")]
        [Range(1, 365)]
        public int MaximumDays { get; set; }

        [Display(Name = "Requires Approval")]
        public bool RequiresApproval { get; set; } = true;

        [Display(Name = "Requires Supporting Documents")]
        public bool RequiresDocuments { get; set; } = false;

        [Display(Name = "Allow Carry Forward")]
        public bool AllowCarryForward { get; set; } = false;

        [Display(Name = "Allow Back Date Application")]
        public bool AllowBackDateApplication { get; set; } = false;

        [Display(Name = "Available After (Months)")]
        [Range(0, 120)]
        public int AvailableAfterMonths { get; set; } = 0;

        [Display(Name = "Yearly Limit")]
        public bool IsYearlyLimit { get; set; } = true;

        [Display(Name = "Paid Leave")]
        public bool IsPaidLeave { get; set; } = true;

        [Display(Name = "Display Order")]
        public int DisplayOrder { get; set; }

        public bool IsActive { get; set; } = true;

        public DateTime CreatedDate { get; set; }

        // Navigation
        public ICollection<EmployeeLeave> EmployeeLeaves { get; set; }
            = new List<EmployeeLeave>();
    }
}