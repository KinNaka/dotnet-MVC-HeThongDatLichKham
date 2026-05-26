using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;

public class CreateScheduleViewModel
{
    [Required]
    public string MaBacSi { get; set; }
    [Required]
    [DataType(DataType.Date)]
    public DateTime Ngay { get; set; }

    [Required]
    public string Ca { get; set; }


    public List<SelectListItem>? BacSiList { get; set; }
}
