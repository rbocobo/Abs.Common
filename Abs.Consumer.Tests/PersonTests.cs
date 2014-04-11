using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Abs.Common;
using Csla;
using System.Collections.Generic;

namespace Abs.Consumer.Tests
{
    [TestClass]
    public class PersonTests
    {
        [TestMethod]
        public void TestCreatePersonUsingMock()
        {
            // arrange
            var personData = new Mock<IPersonData>(MockBehavior.Strict);
            personData.SetupGet(p => p.Origin).Returns("Mock");

            var repository = new Mock<IPersonRepository>(MockBehavior.Strict);
            repository.Setup(r => r.Create()).Returns(personData.Object);

            var activator = new Mock<Csla.Server.IDataPortalActivator>(MockBehavior.Strict);
            activator.Setup(a => a.CreateInstance(It.Is<Type>(_ => _ == typeof(Person))))
                .Returns((Type type) => Activator.CreateInstance(type, true));
            activator.Setup(a => a.InitializeInstance(It.IsAny<object>()))
                .Callback((object obj) =>
                {
                    if (obj is IPerson)
                    {
                        ((IPerson)obj).Repository = repository.Object;
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

            // assert
            personData.VerifyAll();
            repository.VerifyAll();
            activator.VerifyAll();
            Assert.IsNotNull(person);
            Assert.AreEqual(person.Origin, "Mock");
        }

        [TestMethod]
        public void TestAddSubmitChangesPersonUsingMock()
        {

            // arrange
            List<IPersonData> changeSet = new List<IPersonData>();
            var personData = new Mock<IPersonData>();
            personData.SetupAllProperties();
            personData.SetupGet(p => p.Origin).Returns("Mock");

            var repository = new Mock<IPersonRepository>(MockBehavior.Strict);
            repository.Setup(r => r.Create()).Returns(personData.Object);
            repository.Setup(r => r.Add(It.IsAny<IPersonData>())).Callback((IPersonData obj) =>
            {
                //retain changeset
                changeSet.Add(obj);
            });
            repository.Setup(r => r.SaveChanges()).Callback(() =>
            {
                int counter = 0;
                foreach (IPersonData data in changeSet)
                {
                    data.Id = ++counter;
                }

            });

            var activator = new Mock<Csla.Server.IDataPortalActivator>(MockBehavior.Strict);
            activator.Setup(a => a.CreateInstance(It.Is<Type>(_ => _ == typeof(Person))))
                .Returns((Type type) => Activator.CreateInstance(type, true));
            activator.Setup(a => a.InitializeInstance(It.IsAny<object>()))
                .Callback((object obj) =>
                {
                    if (obj is IPerson)
                    {
                        ((IPerson)obj).Repository = repository.Object;
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
            //personData.VerifyAll();
            repository.VerifyAll();
            activator.VerifyAll();
            Assert.AreNotEqual(person.PersonId, 0);
            Assert.AreEqual(person.Origin, "Mock");
        }
    }
}
