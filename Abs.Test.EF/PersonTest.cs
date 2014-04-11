using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using Abs.Common;
using Moq;
using Csla;
using Abs.Consumer;

namespace Abs.Test.EF
{
    [TestClass]
    public class PersonTest
    {
        [TestMethod]
        public void TestAddSubmitChangesPersonUsingEF()
        {

            var activator = new Mock<Csla.Server.IDataPortalActivator>(MockBehavior.Strict);
            activator.Setup(a => a.CreateInstance(It.Is<Type>(_ => _ == typeof(Person))))
                .Returns((Type type) => Activator.CreateInstance(type, true));
            activator.Setup(a => a.InitializeInstance(It.IsAny<object>()))
                .Callback((object obj) =>
                {
                    if (obj is IPerson)
                    {
                        ((IPerson)obj).Repository = new Abs.EF.EFRepository();
                    }
                });
            activator.Setup(a => a.FinalizeInstance(It.IsAny<object>()))
                .Callback((object obj) =>
                {
                    if (obj is IPerson)
                    {
                        ((IPerson)obj).Repository = null;
                    }
                });
            ApplicationContext.DataPortalActivator = activator.Object;

            // act
            var person = DataPortal.Create<Person>();
            person = person.Save();


            //assert
            activator.VerifyAll();
            Assert.AreNotEqual(person.PersonId, 0);
            Assert.AreEqual(person.Origin, "EF");
        }
    }
}
