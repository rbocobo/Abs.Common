using Abs.Common;
using Csla;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Abs.Consumer
{
    public interface IPerson : IBusinessBase
    {
        int PersonId { get; set; }
        string Name { get; set; }
        string Origin { get; set; }
        IPersonRepository Repository { get; set; }
    }
}
