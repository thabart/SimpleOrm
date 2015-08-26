# Simple ORM

### Build status on Travis CI
[![Build Status](https://travis-ci.org/thabart/SimpleOrm.svg?branch=master)](https://travis-ci.org/thabart/SimpleOrm)

This small ORM has a limited set of features :

* It supports some basic SQL actions such as "select" & "where" instructions. Behind the scene, the expression tree is parsed and converted into T-SQL sript.
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