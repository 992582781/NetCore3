using IBase;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace RongKang.IBase
{
    [Table("User1")]
    public class User1 : IEntity
    {
        //[ExplicitKey]
        public string ID { get; set; }
        public string Name { get; set; }
        public string PassWord { get; set; }
    }
}
