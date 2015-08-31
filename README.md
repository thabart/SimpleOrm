# Simple ORM

### Build status on AppVeyor
[![Build status](https://ci.appveyor.com/api/projects/status/usrcqqyotittcu9t?svg=true)](https://ci.appveyor.com/project/thabart/simpleorm)

This small ORM has a limited set of features :

* It supports some basic SQL actions such as "select" & "where" instructions. Behind the scene, the expression tree is parsed and converted into T-SQL script.
* It also allows a project to use the code first-approach. 

Some snippet-code examples :

### Select request

The custom select linq-to-sql instruction :

```
var result = context.Customers
	.Select(c => new
	{
	    c.FirstName,
	    c.Id
	});
```

The generated sql script :
```
SELECT ID,FIRST_NAME FROM dbo.Customers
```

### Where request

The custom where linq-to-sql instruction :

```
var result = context.Customers.Where(c => c.FirstName == "Thierry");
```

The generated sql script :

```
SELECT * FROM dbo.Customers WHERE FIRST_NAME = 'Thierry'
```

### Insert request

The custom insert linq-to-sql instruction :

```
using (var context = new CustomDbContext())
{
    var firstRecord = new Customer
    {
        Id = Guid.NewGuid(),
        FirstName = "temp 3",
        LastName = "temp 3"
    };

    var sql = context.Customers.Add(firstRecord);
    context.SaveChanges();
}
```

The generated sql script :

```
INSERT INTO dbo.Customers('67C6B660-1463-4065-96D5-F1C1D966B6F8', 'temp 3', 'temp 3')
```