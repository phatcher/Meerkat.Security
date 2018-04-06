Meerkat Security
================

This project provides Role Based Access Control (RBAC) to allow easier authorization of activities in your system and also provides support for this in ASP.NET MVC and WebAPI projects.

It is available as a set of NuGet packages [Meerkat.Security](https://www.nuget.org/packages/Meerkat.Security/), [Meerkat.Security.Mvc](https://www.nuget.org/packages/Meerkat.Security.Mvc/) and [Meerkat.Security.WebApi](https://www.nuget.org/packages/Meerkat.Security.WebApi/)

[![NuGet](https://img.shields.io/nuget/v/Meerkat.Security.svg)](https://www.nuget.org/packages/Meerkat.Security/)
[![Build status](https://ci.appveyor.com/api/projects/status/t7wnsdotj5xj8s20/branch/master?svg=true)](https://ci.appveyor.com/project/PaulHatcher/meerkat-security/branch/master)

Welcome to contributions from anyone.

You can see the version history [here](RELEASE_NOTES.md).

# Build the project
* Windows: Run *build.cmd*

The tooling should be automatically installed by paket/Fake. The default build will compile and test the project, and also produce the nuget packages.

# Library License

The library is available under the [MIT License](http://en.wikipedia.org/wiki/MIT_License), for more information see the [License file][1] in the GitHub repository.

 [1]: https://github.com/phatcher/Meerkat.Security/blob/master/License.md 

# Introduction

Security is hard, and the goal of this project is to make it a bit easier to get a good level of default security with low effort into your project. Having said that, your requirements
will be different from mine so make sure that you first understand what you are trying to do and that this project is a good fit.

* [Basic Concepts](#basic-concepts)
* [Defining your security model](#defining-the-security-model)
* [Securing an MVC application](#Securing an MVC application)
* [Securing a WebApi application](#Securing a WebApi application)

# Basic Concepts

Here we cover off the key building blocks of the framework

* [Security models](#security-models): Conceptual models of how to implement access controls
* [Principals and claims](#princpals-and-claims): .NET model of the subject i.e. the user requesting access and security attributes such as roles.
* [Resources and Activities](#resources-and-activities) The object being secured and the operation being requested
* [Grant vs Deny permissions](#grant-vs-deny-permissions): A permission explicitly granting or denying that the subject is permitted to perform the operation

## Security Models

There are four classic models of access control: 
* Discrectionary Access Control (DAC): Control based on restricting access based on the identity of subjects and/or groups to which they belong. The controls are discretionary in the sense
that a subject with certain access permission is capable of passing that permission on to any other subject.
* Mandatory Access Control (MAC): Any operation by any subject on any object is tested against the set of authorization rules (aka policy) to determine if the operation is allowed. The ability
to grant permissions itself comes under MAC and this distinguishes it from the simpler DAC model.
* Role Based Acccess Control (RBAC): This is a policy neutral access control mechanism around roles and permissions and although different from DAC and MAC can easily be used to enforce such constraints
* Attribute Based Access Control (ABAC): This evolved from RBAC to consider additional attribute of either the subject or secured resource in addition to roles and groups and is policy-based in the sense that
it uses policies rather than static permissions to define what is authorized.

This framework is most closely aligned to RBAC but has some ABAC capabilities as well, e.g. you can secure an operation based on additional attributes about the subject and also use [hierarchies](#hierarchies)
to implement more fine-grained permissions. 

The main advantage of the framework is its speed since operations will take place in memory, and this leads us to its main limitation in that it is intended to secure activities on the class
of the resource rather than individual resources i.e. we can easily secure Order.Create or Order.Edit to a role SalesClerk but restricting editing of Order 231 for example would require domain knowledge and/or 
database access to know who can edit this particular order and so it out of scope.

## Principals and Claims

Most developers are aware that .NET processes execute under a user context called a Principal, this is easily accessed via Thread.CurrentPrincipal or in a web application via the controllers User property. Since .NET 4.5 
these are all ClaimsPrincipals which can contain multiple ClaimsIdentities each holding Claims. A claim is a security attribute with some basic properties...

* Type: The type of claim, e.g. Role, Name, Team, Age
* Value: The value of the claim, e.g. Admin, 18, Team A
* Issuer: Who says this claim is true, can be important to ensure the integrity of the system
* ValueType: What type of data is the claim, default is string but can be any type e.g. integer, date

In this model, roles and the user's name are not anything special, just particular types of claim.

How we arrive at these claims is out of scope for this project but in general they are the responsibility of the authentication system where users, groups and roles are allocated and can also be further enhanced by a 
process called claims transformation which happens as part of the ASP.NET pipeline.

## Resources and Activities

A lot of examples of securing .NET applications show security against activities such as "CreateUser" and "UpdateUser" and one issue I have with this is that the number of securable items can grow very rapidly e.g.
in a fairly small system of 20 resource types with just CRUD operations you have 80 securable items and this quite rapidly becomes difficult to manage. This project took a slightly different perspective akin to how REST models things
and explicitly separated the resource from the activity that is being secured.

This allows you to define granular but wide-ranging permissions e.g. grant "Delete" permission to the "Admin" role on all resources or grant all permissions on "Order" to the "SalesClerk" role.

## Grant vs Deny Permissions

One concept that simplifies security maintenance is the concept of Deny permissions which take precedence over any Grant.  For example, your normal sales clerk Alice is off for the say so you grant Bob the SalesClerk role, but
you don't want him to be able to do too much damage so you also say Bob Deny "Order.Delete", now this Deny permission for Bob will take precedence over the SalesClerk's standard permissions to be able to do anything to an Order.

# Defining the security model

Following on from the idea of resources and activities, your security model should be focussed on your business functionality rather than how you map this to a particular software implementation. If you do this, it will be much
easier to explain to the business sponsors what is being secured and why, and should more naturally flow from any analysis dicussions e.g. "We only want managers to be able to approve invoices".

Another way of developing this structure is rather than resource/activity is noun/verb; nouns represent objects that need securing and verbs are the operations that users will perform.  You can stray from this pattern, but it should
be intentional rather than accidental.

Lets take a simple business model

* Orders: Orders placed in the system
* Invoices: Invoiced orders
* Sales clerk: Enters/amends orders in the system
* Invoice clerk: Processes invoices
* Sales manager: Supervises sales clerks
* Finance manager: Supervises invoice clerks
* Finance Director: Supervises managers

So what we want to implemnt is the following

* Sales Clerk: Can read, create, amend, ship and cancel Orders but can't delete them, has no rights to Invoices
* Sales Manager: Can read, ship and cancel Orders but can only read Invoices
* Invoice Clerk: Can read, create, amend and cancel Invoices but not approve or delete them, has no rights on Orders
* Finance Manger: Can read, approve or cancel Invoices but only read Orders
* Finance Director: Can read and delete Orders and Invoices but has no rights to create, amend or approve

This gives a reasonable separation of powers where it requires at least two actors to colude to create an order, raise an invoice and get it approved.

This can be represented in the following table, blank nodes means no rights

|          | Role     | SC | SM | IC | FM | FD |
| Resource | Activity |    |    |    |    |    |
| -------- | -------- | -- | -- | -- | -- | -- |
| Order    | Read     | x  | x  |    | x  | x  |
| Order    | Create   | x  |    |    |    |    |
| Order    | Edit     | x  |    |    |    |    |
| Order    | Ship     | x  | x  |    |    |    |
| Order    | Cancel   | x  | x  |    |    |    |
| Order    | Delete   |    |    |    |    | x  |
| Invoice  | Read     |    | x  | x  | x  | x  |
| Invoice  | Create   |    |    | x  |    |    |
| Invoice  | Edit     |    |    | x  |    |    |
| Invoice  | Approve  |    |    |    | x  |    |
| Invoice  | Cancel   |    |    |    | x  |    |
| Invoice  | Delete   |    |    |    |    | x  |

We can define the model in either XML or JSON according to preference, here's the XML representation

    <financeAuthorization name="Finance" allowUnauthenticated="false">
        <activity name="Order.Read">
            <allow roles="SalesManager, FinanceManager, FinanceDirector" />
        </activity>
        <activity name="Order">
            <allow roles="SalesClerk" />
        </activity>
        <activity name="Order.Ship">
            <allow roles="SalesManager" />
        </activity>
        <activity name="Order.Cancel">
            <allow roles="SalesManager" />
        </activity>
        <activity name="Order.Delete">
            <allow roles="FinanceDirector" />
            <deny roles="SalesClerk" />
        </activity>
        <activity name="Invoice.Read">
            <allow roles="SalesManager, FinanceManager, FinanceDirector" />
        </activity>
        <activity name="Invoice">
            <allow roles="InvoiceClerk" />
        </activity>
        <activity name="Invoice.Approve">
            <deny roles="InvoiceClerk" />
            <allow roles="FinanceManager" />
        </activity>
        <activity name="Invoice.Cancel">
            <deny roles="InvoiceClerk" />
            <allow roles="FinanceManager" />
        </activity>
        <activity name="Invoice.Delete">
            <deny roles="InvoiceClerk" />
            <approve roles="FinanceDirector" />
        </activity>
    </financeAuthorization>

It does not matter what order the activities are defined in, once they are loaded into memory we produce a canoncial ordering so we always process from most specific to least specific, see [hiearchies](#hierarchies) for
more details.

This allow us to define wider rules and then pare them back with exclusions e.g.

        <activity name="Invoice">
            <allow roles="InvoiceClerk" />
        </activity>
        <activity name="Invoice.Approve">
            <deny roles="InvoiceClerk" />
            <allow roles="FinanceManager" />
        </activity>

This grants all activities on Invoice to the InvoiceClerk, but then specifically disallows them from Approve whilst granting that right to the FinanceManager.

One general principle is not to have hierarchies of roles, if you do so it makes it much more difficult to see the implications of changes to the security model and can lead to inadvertant breaches
of seperation of concern rules. For example, if instead of explicitly denoting a FinanceDirector's permissions we also said that they also inherit the other roles, then a bad actor could create an order, invoice
it and approve it without the security model complainsing.

## Hierarchies

Sometimes you need more fine-grained security, for example you want to provide some property-level permissioning so employee information is available but their salary information is not. Another example might be that
you want people to being to print some reports but exclude them from more sensitive data; to do this we have the concept of both resource and activity hiearchies.

This is acheived by using a path separator "/" in either the resource, the activity or both, the authorization system then checks them using a canonical ordering from most specific to least specific to determine the permission
to apply, and this first one to make a decision is the one applied. For example given the following security fragment...

       <activity name="Reports.Print" authorized="true" />
       <activity name="Reports/Sales.Print" authorized="false">
           <allow roles="Sales" />
       </activity>
       <activity name="Reports/Employees.Print" authorized="false">
           <allow roles="HR" />
       </activity>

This says that all authenticated users are allow to print reports, but only Sales are allowed to print Sales reports and only HR are allowed to print Employee reports.

So if we ask to authorize "Reports/Sales.Print" the following activtiies would be considered in this order

Reports/Sales.Print
Reports.Print
.Print

By using 'authorized="false"' at the sublevels we are forcing the decision to be evaluation there, otherwise it would bubble up to "Reports.Print". The other alternative is to use Deny permissions but this can
get tricky as a Deny is an immediate block on further evaluation, e.g. given

       <activity name="Reports.Print">
           <allow roles="Users" />
       </activity>
       <activity name="Reports/Sales.Print" authorized="false">
           <deny roles="Users" />
           <allow roles="Sales" />
       </activity>
       <activity name="Reports/Employees.Print" authorized="false">
           <deny roles="Users" />
           <allow roles="HR" />
       </activity>

The problem is if you make a HR user a member of Users, they would be locked out of their reports due to the Deny taking precedence. This also brings up a good point of generally avoiding groups such as "Users"; we effectively
get this overall group by virtue of them being authenticated and you can include everyone with for an activity with a 'authorized="true"' clause or even 'allowUnauthenticated="true"', for example one standard entry I have in most security files is

    <activity name="Home.Index" authorized="true" allowUnauthenticated="true" />

To use the hiearchies effectively you might need to make more than one call to the authorization system...

* A employee MVC controller is secured with "Employee.Read" but you do an additional check on "Employee/Salary.Read" before returning that data.
* The top level Reports link might be secured with "Reports.Print" but a particular report might be secured with "Reports/Salary.Print".