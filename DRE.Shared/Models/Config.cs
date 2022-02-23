using System;
using System.Collections.Generic;
using System.Text;

namespace DRE.Models
{
    public record Config
    {
        public String Title { get; set; }
        public int SavedWidth { get; set; }
        public int SavedHeight { get; set; }

    }
}
