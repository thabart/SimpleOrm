using Moq;
using NUnit.Framework;
using ORM.Core;
using ORM.Translators;
using ORM.UnitTests.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace ORM.UnitTests.Translators
{
    public class QueryTranslatorTestFixture
    {
        public abstract class GivenQueryTranslator : BaseFixture
        {
            protected QueryTranslator QueryTranslator;

            public override void Arrange()
            {
                var mappingTranslator = new Mock<IMappingRuleTranslator>();
                mappingTranslator.Setup(m => m.GetTableName(It.IsAny<Type>())).Returns("TABLE_NAME");
                mappingTranslator.Setup(m => m.GetColumnName(It.IsAny<Type>(), It.IsAny<string>())).Returns("COLUMN_NAME");
                QueryTranslator = new QueryTranslator(mappingTranslator.Object);
            }
        }

        [TestFixture]
        public class WhenTranslateExpressionIntoSqlScript : GivenQueryTranslator
        {
            private Expression _selectExpression;

            private string _sqlScript;

            [Test]
            public void ThenSqlScriptIsGenerated()
            {
                Assert.IsNotEmpty(_sqlScript);
            }

            public override void Arrange()
            {
                base.Arrange();
                var customers = new List<Customer>
                {
                    new Customer
                    {
                        FirstName = "Thierry",
                        LastName = "Habart"
                    }
                };

                var queryableCustomers = customers.AsQueryable();
                _selectExpression = queryableCustomers.Select(q => q.FirstName).Expression;
            }

            public override void Act()
            {
                _sqlScript = QueryTranslator.Translate(_selectExpression);
            }
        }
    }
}
