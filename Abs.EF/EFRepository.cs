using Abs.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Abs.EF
{
    public class EFRepository : IPersonRepository
    {
        private AbsDBEntities dataContext = new AbsDBEntities();
        public IPersonData Create()
        {
            return new Person()
            {
                Origin = "EF"
            };
        }

        public void Add(IPersonData person)
        {
            dataContext.Person.Add(person as Person);
        }

        public IPersonData Get(int personId)
        {

            var personSource = (from item in dataContext.Person
                                where item.Id == personId
                                select item
                                ).FirstOrDefault();
            return personSource;
        }

        public void SaveChanges()
        {
            dataContext.SaveChanges();
        }
    }
}
