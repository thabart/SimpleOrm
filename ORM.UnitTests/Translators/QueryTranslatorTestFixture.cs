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

            protected string ColumnName;

            protected string TableName;

            public override void Arrange()
            {
                var mappingRuleTranslatorStub = new Mock<IMappingRuleTranslator>();
                ColumnName = "COLUMN_NAME";
                TableName = "TABLE_NAME";
                mappingRuleTranslatorStub.Setup(m => m.GetTableName(It.IsAny<Type>())).Returns(TableName);
                mappingRuleTranslatorStub.Setup(m => m.GetColumnName(It.IsAny<Type>(), It.IsAny<string>())).Returns(ColumnName);
                QueryTranslator = new QueryTranslator(mappingRuleTranslatorStub.Object);
            }
        }

        [TestFixture]
        public sealed class WhenTranslatingSelectInstructionOneColumnIntoSqlScript : GivenQueryTranslator
        {
            private Expression _selectExpression;

            private string _sqlScript;

            [Test]
            public void ThenSqlScriptIsGenerated()
            {
                Assert.IsNotEmpty(_sqlScript);
            }

            [Test]
            public void ThenSqlScriptIsCorrect()
            {
                var expectedResult = string.Format("SELECT {0} FROM {1}", ColumnName, TableName);
                Assert.That(_sqlScript, Is.EqualTo(expectedResult));
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

        [TestFixture]
        public sealed class WhenTranslatingSelectInstructionOnTwoColumnsIntoSqlScript : GivenQueryTranslator
        {
            private Expression _selectExpression;

            private string _sqlScript;

            [Test]
            public void ThenSqlScriptIsGenerated()
            {
                Assert.IsNotNullOrEmpty(_sqlScript);
            }

            [Test]
            public void ThenSqlScriptIsCorrect()
            {
                var expectedResult = string.Format("SELECT {0},{1} FROM {2}", ColumnName, ColumnName, TableName);
                Assert.That(_sqlScript, Is.EqualTo(expectedResult));
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
                _selectExpression = queryableCustomers.Select(q => new
                {
                    q.FirstName,
                    q.LastName
                }).Expression;
            }

            public override void Act()
            {
                _sqlScript = QueryTranslator.Translate(_selectExpression);
            }
        } 

        [TestFixture]
        public sealed class WhenTranslatingWhereInstructionWithOnlyOneConditionIntoSqlScript : GivenQueryTranslator
        {
            private Expression _selectExpression;

            private string _sqlScript;

            [Test]
            public void ThenSqlScriptIsGenerated()
            {
                Assert.IsNotNullOrEmpty(_sqlScript);
            }

            [Test]
            public void ThenSqlScriptIsCorrect()
            {
                var expectedResult = string.Format("SELECT * FROM {0} WHERE {1} = '{2}'", TableName, ColumnName, "Thierry");
                Assert.That(_sqlScript, Is.EqualTo(expectedResult));
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
                _selectExpression = queryableCustomers.Where(q => q.FirstName == "Thierry").Expression;
            }

            public override void Act()
            {
                _sqlScript = QueryTranslator.Translate(_selectExpression);
            }
        }
    }
}
