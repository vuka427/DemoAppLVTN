using Domain.Common;
using Domain.Enum;
using System;
using System.Collections.Generic;
using System.Text;

namespace Domain.Entities
{
    public class PostNew : BaseEntity
    {
        public string Title { get; set; }
        public string Content { get; set; }
        public PostStatus Status { get; set; }

    }
}
