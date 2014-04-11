using System;
using Csla;
using Abs.Common;

namespace Abs.Consumer
{
    [Serializable]
    internal sealed class Person : BusinessBase<Person>, IPerson
    {
        #region Business Methods

        public static readonly PropertyInfo<int> PersonIdProperty = RegisterProperty<int>(c => c.PersonId);
        public int PersonId
        {
            get { return GetProperty(PersonIdProperty); }
            set { SetProperty(PersonIdProperty, value); }
        }

        public static readonly PropertyInfo<string> NameProperty = RegisterProperty<string>(c => c.Name);
        public string Name
        {
            get { return GetProperty(NameProperty); }
            set { SetProperty(NameProperty, value); }
        }

        public static readonly PropertyInfo<string> OriginProperty = RegisterProperty<string>(c => c.Origin);
        public string Origin
        {
            get { return GetProperty(OriginProperty); }
            set { SetProperty(OriginProperty, value); }
        }

        #endregion

        #region Business Rules

        protected override void AddBusinessRules()
        {
            // TODO: add validation rules
            base.AddBusinessRules();

            //BusinessRules.AddRule(new Rule(IdProperty));
        }

        private static void AddObjectAuthorizationRules()
        {
            // TODO: add authorization rules
            //BusinessRules.AddRule(...);
        }

        #endregion

        #region Factory Methods

        #endregion

        #region Data Access
        
        protected override void DataPortal_Create()
        {
            IPersonData data = this.Repository.Create();
            using(BypassPropertyChecks)
            {            
                this.Origin = data.Origin;
            }

            base.DataPortal_Create();
        }

        private void DataPortal_Fetch(int personId)
        {
            IPersonData data = this.Repository.Get(personId);
            using(BypassPropertyChecks)
            {
                this.PersonId = data.Id;
                this.Name = data.Name;
                this.Origin = data.Origin;
            }
        }

        [Transactional(TransactionalTypes.TransactionScope)]
        protected override void DataPortal_Insert()
        {
            IPersonData data = this.Repository.Create();
            using (BypassPropertyChecks)
            {
                data.Id = this.PersonId;
                data.Name = this.Name;
                data.Origin = this.Origin; 
            }

            this.Repository.Add(data);
            this.Repository.SaveChanges();
            this.LoadProperty(PersonIdProperty, data.Id);
        }

        [Transactional(TransactionalTypes.TransactionScope)]
        protected override void DataPortal_Update()
        {
            // TODO: update values
        }

        [Transactional(TransactionalTypes.TransactionScope)]
        protected override void DataPortal_DeleteSelf()
        {
            DataPortal_Delete(this.PersonId);
        }

        [Transactional(TransactionalTypes.TransactionScope)]
        private void DataPortal_Delete(int criteria)
        {
            // TODO: delete values
        }

        #endregion

        #region Dependency

        public IPersonRepository Repository
        {
            get;
            set;
        }

        #endregion
    }
}
