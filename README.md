# Simple ORM

### Build status on AppVeyor
[![Build status](https://ci.appveyor.com/api/projects/status/usrcqqyotittcu9t?svg=true)](https://ci.appveyor.com/project/thabart/simpleorm)

This small ORM has a limited set of features :

* It supports some basic SQL actions such as "select", "where" & "insert" instructions. Behind the scene, the expression tree is parsed and converted into T-SQL script.
* It also allows a project to use the code first-approach.
* A visual studio extension is available to generate the entities from a database (model-first-approach)

## Code-First approach

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

## Model-First approach

Pre-requisites to use the SimpleOrm Visual studio extension:
* Visual Studio version 2013 & 2015 are supported
* The extension can work only on c# project (WPF or ASP.NET MVC)
* Add this feed (http://vsixgallery.com/feed/extension/8fb8f658-e088-4b06-bd4a-87d34fbf8c80) to Visual Studio's extension manager from tools : Tools -> Options -> Environment -> Extension and updates.

A VSPackage is available at this URL. It enables a developer to connect to a SQL-Server database, select the tables and generate the entities in the selected project.
Follow those steps, if you want to use the extension :
* In the Visual Studio Extension Gallery (the one you've added) download the "SimpleOrm" VSIX package and install it.
* Once the extension is installed, open a .NET solution (ASP.NET MVC or WPF) and select a c# project.
* Right click on the "c# project" and click on the item "Generate classes (database-first approach")

![alt tag](https://raw.githubusercontent.com/thabart/SimpleOrm/master/Images/SelectedProjectOption.png)

* A new window is displayed

![alt tag](https://raw.githubusercontent.com/thabart/SimpleOrm/master/Images/SelectTablesWindow.png)

* Specify the information to connect to your database. Check if your connection information are correct by clicking on the button "test connection", if the connection success then a list of tables is displayed. 
* Choose the tables for which you want to generate an entity and click on "Generate tables". Once you've confirmed the generation, all the assets are generated (entities, mappings, DBContext) and added to the selected project, the connection string is added to the configuration file (either APP.CONFIG or WEB.CONFIG) and the Nuget package "SimpleOrm" which coming from the Nuget feed is installed. The following screenshot shows the result of the transformation :

![alt tag](https://raw.githubusercontent.com/thabart/SimpleOrm/master/Images/ProjectStructure.png)


### Release process

Each time a label is pushed on the "master" branch to the remote repository, the following actions happened :
* A new release is created on GITHUB with the artifacts (VSIX package, Library.zip & Nuget package)
* The nuget package is published.
* The VSIX package is published.