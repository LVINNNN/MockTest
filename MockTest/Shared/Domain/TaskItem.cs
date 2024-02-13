using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MockTest.Shared.Domain
{
    public class TaskItem
    {
        public int Id { get; set; }
        [Required(ErrorMessage = "Please Input A Name.")]
        public string? TaskName { get; set; }
        public bool IsCompleted { get; set; }
        [Required(ErrorMessage = "Please Input A Date.")]
        public DateTime DueDate { get; set; }
        public DateTime CreatedDate { get; set; }
        public String? CreatedBy { get; set; }

    }
}
