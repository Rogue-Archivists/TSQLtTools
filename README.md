# tSQLt Tools
  - [Features](#features)
  - [Dependencies](#dependencies)
  - [Setup](#setup)
  - [Quick Use](#quick-use)
    - [New Solution](#new-solution)
    - [Add to Existing Projects](#add-to-existing-projects)
  - [What is tSQLt](#what-is-tsqlt)
  - [tSQLt Test Adapter](#tsqlt-test-adapter)
  - [Configuring a Database Solution](#configuring-a-database-solution)
  - [Appendix](#appendix)

Just a few tools to make using [tSQLt](https://tsqlt.org) easier for database unit tests in Visual Studio SQL Projects. 
The Goal is to make SQL unit testing as much like C# just in TSQL. No GUI with code behind and clicking just code.

This allow the developer test in isolation any structural change to tables or stored procedures refactors for correctness before committing code.

## Features 

* Visual Studio Templates
  * Solution
  * Project
  * Project Items

## Dependencies

* Visual Studio 2017/2019 (VS)
* LocalDB (part of must VS installs)
* [tSQLt Test Runner Adapter](https://marketplace.visualstudio.com/items?itemName=vs-publisher-263684.GoEddietSQLt2019)  by Ed Elliott

## Setup

1. add the local extension feed to visual studio
2. install the extension "tSQLt Tools"

## Quick Use

### New Solution

1. File > New Solution > SQL > tSQLt Solution

### Add to Existing Projects

1. Solution > Add Project > SQL > Unit Test > tSQLt
2. Name "_SomeProject_.Test"
3. "_SomeProject_" > References > right-click > add "Database Reference"
4. In Projects
    * Base project ("_SomeProject_")
    * "Same Server, Same Database"

5. as needed "_SomeProject_" > database Item > Add Unit test

## What is tSQLt

TSQL is a SQL Server CLR (database plug-in) that adds a unit testing framework to SQL Server database. This includes, but not limited to:

* Isolating Dependencies (Mocking)
* Assertions
* Expectations

All of this has been bundled up into a database reference (.dacpack file) and the unit testing can be added to you current database solution as a separate SQL Project.

```plantuml
    node "tsqlt.dacpack"{
            rectangle "tSQLtCLR.dll" <<CLR>>
            rectangle "ExpectSomething" <<StoredProc>>
            rectangle "MockSomething" <<StoredProc>>
    }
```

## tSQLt Test Adapter

To Perform database unit test with tSQLt we need Visual Studio to do the following:

1. Visual Studio can create the project database, with all developer changes
2. Publish to a database
3. Run unit tests _in the database as queries_
4. returned the result and displayed in query explorer.

Steps #1, #2 and #4 are part of Visual Studio. Step #3 is implemented by _tSQLt Test Runner Adapter_ by Ed Elliott.

The extension provides the following:

* enumerates the test classes and tests in a Solution
* collects and results from the invoked sql stored procedures

```plantuml
    actor ":Developer" as user
    boundary "Visual Studio" as vs
    control "Unit Test Adapter" as test
    collections "Project Files" as files
    database "localDB" as db

    == Build ==
    user -> vs : Build
    vs -> files: Parse
    files -> vs : model

    == Publish ==
    user -> vs : Publish
    vs -> vs : diff against model
    vs -> db : migrate script

    == Run all Tests ==
    user -> vs: Run all tests
    vs -> test: Enumerate test suites
    test -> files: scan for TSQL Tests
    vs -> test: Enumerate tests in test suit

    vs-> test: Run Suite1
    test -> db: exec [Suite1].[Run all tests]
```

It is important to remember that:

* tests are **enumerated** from the source code and are current as of the last _save_
* tests are **run** from the database and are current as of the last _publish_

## Configuring a Database Solution

The diagram below illustrates the logical dependencies SQL Project `SomeProject.Test` has on `SomeProject` and `tSQLt`.

```plantuml
    database tSQLt as tsqlt
    database "Master" as sys
    database "//SomeProject//" as proj

    database "//SomeProject//.test" as test
    rectangle "tSQLt.dll" <<CLR>> as clr

    clr <-- tsqlt : <<uses>>
    sys <-- tsqlt: <<uses>>

    sys <-- proj: <<uses>>

    proj <|-- test: <<includes>>
    tsqlt <|--  test: <<includes>>
caption: Logical Dependencies in a tSQLt SQL Project
```

The database reference to `tSQLt` *should* be satisfied by a reference to <pre>\\\\<build_server>\build\Reference\Dacpacs\tSQLt\\\$sql_target\$\\tSQLt.dacpac</pre> where `$sql_target` is target engine version for the project. Since the unit testing framework is included in the DACPAK there is no need to install the library local a rebuild will pull the latest CLR DLL with the DPACK at build time.

## Appendix

This project structure dependes on database refrences using _same database_ mode. This allows 

```plantuml
    title  Example Project:\n Detailed Logical Dependencies
    database tsqlt {
        rectangle tSQLt <<schema>> {
            rectangle "ExpectSomething" <<StoredProc>>
            rectangle "MockSomething" <<StoredProc>>
        }
    }
    database "Master" as sys
    database "MyProject" as proj {
        rectangle "dbo" <<schema>> {
            rectangle "SomeAction" <<Stored Proc>> as unit
        }
    }
    database "MyProject.test" as test {
        rectangle SomeAction <<schema>> as test_suit {

            rectangle Test1 <<Stored Proc>>
            rectangle Test2 <<Stored Proc>>
        }
    }

    rectangle "tSQLt CLR" <<CLR>> as clr

    sys <-- tsqlt: <<uses>>
    clr <-- tsqlt : <<uses>>

    sys <-- proj: <<uses>>

    proj <|-- test: <<includes>>
    tsqlt <|--  test: <<includes>>
```
<br/>
After build all the same database references are in-lined into the top level project before compairing agins the active database for deployments.

```plantuml
title  Example Project:\n Detailed Physical Deployment
database "MyProject.Test" as alias {
 rectangle "tSQLt.dll" <<CLR>>

 rectangle tSQLt <<schema>> {
    rectangle "ExpectSomething" <<StoredProc>>
    rectangle "MockSomething" <<StoredProc>>
 }

 rectangle dbo <<schema>> {
     rectangle SomeBusinessAction<<Store Proc>>
 }

 rectangle SomeAction <<schema>> {
    rectangle Test1 <<Stored Proc>>
    rectangle Test2 <<Stored Proc>>
 }
}
```
