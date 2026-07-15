using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using EduCore.Enums;

namespace EduCore.Models
{
    public class EmployeeLeave
    {
        [Key]


        public int EmployeeLeaveId { get; set; }

        [Required]
        [Display(Name = "Leave Number")]
        [StringLength(20)]
        public string LeaveNumber { get; set; } = string.Empty;

        // Employee
        [Required]
        public int EmployeeId { get; set; }

        [ForeignKey(nameof(EmployeeId))]
        public Employee? Employee { get; set; }

        // Leave Type
        [Required]
        public int LeaveTypeId { get; set; }

        [ForeignKey(nameof(LeaveTypeId))]
        public LeaveType? LeaveType { get; set; }

        // Leave Dates
        [Required]
        [Display(Name = "From Date")]
        public DateTime FromDate { get; set; }

        [Required]
        [Display(Name = "To Date")]
        public DateTime ToDate { get; set; }

        [Display(Name = "Total Leave Days")]
        public decimal TotalDays { get; set; }
        

        // Reason
        [Required]
        [Display(Name = "Reason for Leave")]
        [StringLength(1000)]
        public string Reason { get; set; } = string.Empty;

        //=========================================================
        // Additional Information
        //=========================================================

        [Display(Name = "Half Day Leave")]
        public bool IsHalfDay { get; set; } = false;

        [Display(Name = "Supporting Document")]
        [StringLength(250)]
        public string? AttachmentPath { get; set; }

        [Display(Name = "Expected Rejoining Date")]
        public DateTime? ExpectedRejoiningDate { get; set; }

        [Display(Name = "Actual Rejoining Date")]
        public DateTime? ActualRejoiningDate { get; set; }

        // Approval
        public LeaveStatus Status { get; set; }
            = LeaveStatus.Pending;

        public int? ApprovedByEmployeeId { get; set; }

        [ForeignKey(nameof(ApprovedByEmployeeId))]
        public Employee? ApprovedBy { get; set; }

        public DateTime? ApprovalDate { get; set; }

        [StringLength(500)]
        public string? ApprovalRemarks { get; set; }


        [Display(Name = "Cancelled")]
        public bool IsCancelled { get; set; }

        public DateTime? CancelledDate { get; set; }
        public DateTime AppliedDate { get; set; }
            = DateTime.Now;
    }
}