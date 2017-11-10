using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CoolBytes.Core.Models
{
    public class User
    {
        public int Id { get; private set; }
        public string Identifier { get; private set; }

        public User(string identifier)
        {
            Identifier = identifier ?? throw new ArgumentNullException(nameof(identifier));
        }

        private User()
        {
            
        }
    }
}