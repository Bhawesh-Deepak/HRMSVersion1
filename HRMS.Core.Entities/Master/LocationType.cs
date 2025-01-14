﻿using HRMS.Core.Entities.Common;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HRMS.Core.Entities.Master
{
    [Table("LocationType", Schema = "Master")]
    public class LocationType : BaseModel<int>
    {
        [Required(ErrorMessage = "This field is required.")]
        [Display(Prompt = "Location Type")]
        public string Name { get; set; }
      
    }
}
