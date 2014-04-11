using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Abs.Common
{
    public interface IPersonData
    {
        int Id { get; set; }
        string Name { get; set; }
        string Origin { get; set; }
    }
}
