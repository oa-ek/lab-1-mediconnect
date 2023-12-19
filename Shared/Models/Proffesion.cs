using System.Collections.Generic;

namespace Shared.Models
{
    public class Proffesion
    {
        public int ID { get; set; }
        public string Name { get; set; }

        public ICollection<User> Users { get; set; }
    }
}
