using System;
using System.Collections.Generic;
using System.Text;

namespace Altaaref.Models
{
    public class UserNotification
    {
        public int Id { get; set; }
        public string Topic { get; set; }
        public string Title { get; set; }
        public string Body { get; set; }
        public int StudentId { get; set; }
    }
}
