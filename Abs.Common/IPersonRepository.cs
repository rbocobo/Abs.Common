using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Abs.Common
{
    public interface IPersonRepository
    {
        IPersonData Create();
        void Add(IPersonData person);
        IPersonData Get(int personId);
        void SaveChanges();
    }
}
