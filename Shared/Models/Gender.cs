using System.Collections.Generic;

namespace Shared.Models
{
    public class Gender
    {
        public int ID { get; set; }
        public string Name { get; set; }
        public ICollection<User> Users { get; set; }
    }
}
