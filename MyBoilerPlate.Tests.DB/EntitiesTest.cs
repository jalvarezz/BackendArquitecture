using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Linq;
using System.Collections.Generic;
using System.Reflection;
using Core.Common.Base;
using MyBoilerPlate.Web.Infrastructure.Installers;
using MyBoilerPlate.Business.Entities;

namespace MyBoilerPlate.Tests.DB.Entities
{
    [TestClass]
    public class EntitiesTest : TestBase
    {
        public EntitiesTest() { }

        [TestInitialize]
        public void Setup()
        {
            this.Initialize(new List<Type>()
            {
                typeof(HttpContextInstaller),
                typeof(DataInstaller),
                typeof(RepositoryInstaller)
            });
        }

        [TestMethod]
        public void TestAllTentities()
        {
            // Arrange
            var entityTypes = this.GetRepositoryEntityTypes();

            // Act
            this.TestEntitiesRepo(entityTypes);

            // Assert
            Assert.IsTrue(true);
        }

        [TestMethod]
        public void TestEmployeeEntities()
        {
            // Arrange
            var entityTypes = this.GetRepositoryEntityTypes("Employee");

            // Act
            this.TestEntitiesRepo(entityTypes);

            // Assert
            Assert.IsTrue(true);
        }

        private IEnumerable<Type> GetRepositoryEntityTypes(string prefix = null)
        {
            var excludedTypes = new List<Type>()
            {
                typeof(StoredProcedureEntityBase),
                typeof(FunctionEntityBase)
            };

            var result = new List<Type>();

            if(!string.IsNullOrEmpty(prefix))
                prefix = prefix.ToUpper();

            foreach(Type type in Assembly.GetAssembly(typeof(Employee)).GetTypes()
                                         .Where(myType => myType.IsClass && !myType.IsAbstract && 
                                                          myType.IsSubclassOf(typeof(EntityBase<>)) &&
                                                          !excludedTypes.Contains(myType) && 
                                                          (prefix == null || myType.Name.ToUpper().StartsWith(prefix)))
                                         .OrderBy(x => x.Name))
            {
                result.Add(type);
            }

            return result;
        }

        private void TestEntitiesRepo(IEnumerable<Type> entityTypes)
        {
            foreach(var myType in entityTypes)
            {
                MethodInfo method = typeof(TestBase).GetMethod("TestRepo");
                MethodInfo generic = method.MakeGenericMethod(myType);

                generic.Invoke(this, null);
            }
        }
    }
}
