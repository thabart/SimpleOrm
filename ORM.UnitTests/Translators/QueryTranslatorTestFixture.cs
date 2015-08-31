using Moq;

using NUnit.Framework;

using ORM.Core;
using ORM.Mappings;
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

            protected ColumnDefinition ColumnDefinition;

            public override void Arrange()
            {
                var mappingRuleTranslatorStub = new Mock<IMappingRuleTranslator>();
                ColumnName = "COLUMN_NAME";
                TableName = "TABLE_NAME";
                ColumnDefinition = new ColumnDefinition("COLUMN_NAME", typeof(string));
                mappingRuleTranslatorStub.Setup(m => m.GetTableName(It.IsAny<Type>())).Returns(TableName);
                mappingRuleTranslatorStub.Setup(m => m.GetColumnName(It.IsAny<Type>(), It.IsAny<string>())).Returns(ColumnName);
                mappingRuleTranslatorStub.Setup(m => m.GetColumnDefinition(It.IsAny<Type>(), It.IsAny<string>())).Returns(ColumnDefinition);
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

        [TestFixture]
        public sealed class WhenTranslatingSelectWhereInstructionWithAndConditionIntoSqlScript : GivenQueryTranslator
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
                var expectedResult = string.Format("SELECT {1} FROM {0} WHERE {1} = '{2}' OR {1} = '{3}'", TableName, ColumnName, "Thierry", "Loki");
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
                _selectExpression = queryableCustomers
                    .Where(q => q.FirstName == "Thierry" || q.FirstName == "Loki")
                    .Select(q => q.FirstName).Expression;
            }

            public override void Act()
            {
                _sqlScript = QueryTranslator.Translate(_selectExpression);
            }
        }
        
        [TestFixture]
        public sealed class WhenTranslatingInsertInstruction : GivenQueryTranslator
        {
            private Expression _insertExpression;

            private string _sqlScript;

            private readonly string _firstName = "Laetitia";

            private readonly string _lastName = "Buyse";

            [Test]
            public void ThenSqlScriptIsGenerated()
            {
                Assert.IsNotNullOrEmpty(_sqlScript);
            }

            [Test]
            public void ThenSqlScriptIsCorrect()
            {
                var expectedResult = string.Format("INSERT INTO TABLE_NAME ({0},{0}) VALUES ('{1}','{2}')", ColumnName, _firstName, _lastName);
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
                var customer = new Customer
                {
                    FirstName = _firstName,
                    LastName = _lastName
                };

                var type = typeof(Customer);
                var addMethodInfo = typeof(LinqToSql.QueryableExtensions).GetMethod("Add");
                _insertExpression = Expression.Call(
                    null,
                    (addMethodInfo).MakeGenericMethod(new Type[]
                    {
                        type
                    }),
                    new Expression[]
                    {
                        customers.AsQueryable().Expression,
                        Expression.Constant(customer)
                    });
            }

            public override void Act()
            {
                _sqlScript = QueryTranslator.Translate(_insertExpression);
            }
        }
    }
}
