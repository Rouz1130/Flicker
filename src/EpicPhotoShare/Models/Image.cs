using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace EpicPhotoShare.Models
{
    [Table("Images")]
    public class Image
    {
        [Key]
        public int ImageId { get; set; }
        public string Source { get; set; }
        public virtual ApplicationUser User { get; set; }
    }
}
