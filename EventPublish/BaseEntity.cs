using System;
using System.Collections.Generic;
using System.Text;

namespace EventPublish
{
   public abstract class BaseEntity<T>
    {
       public string Topic { get; set; }
        public T Data { get; set; }
        public string Subject { get; set; }
        public string RandomId { get; set; }

    }
}
