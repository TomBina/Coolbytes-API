using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CoolBytes.Core.Models;

namespace CoolBytes.Core.Interfaces
{
    public interface IAuthorService
    {
        Task<Author> GetAuthor();
        Task<Author> GetAuthorWithProfile();
    }
}
