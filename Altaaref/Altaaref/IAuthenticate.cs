using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Altaaref
{
    public interface IAuthenticate
    {
        Task<bool> Authenticate();
    }
}
