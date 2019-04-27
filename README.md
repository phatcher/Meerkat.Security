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
* [Securing an application](#securing-an-application)

# Basic Concepts

Here we cover off the key building blocks of the framework

* [Security models](#security-models): Conceptual models of how to implement access controls
* [Principals and claims](#principals-and-claims): .NET model of the subject i.e. the user requesting access and security attributes such as roles.
* [Resources and Activities](#resources-and-activities) The object being secured and the operation being requested
* [Grant vs Deny permissions](#grant-vs-deny-permissions): A permission explicitly granting or denying that the subject is permitted to perform the operation

## Security Models

There are four classic models of access control: 
* **Discretionary Access Control (DAC)**: Control based on restricting access based on the identity of subjects and/or groups to which they belong. The controls are discretionary in the sense
that a subject with certain access permission is capable of passing that permission on to any other subject.
* **Mandatory Access Control (MAC)**: Any operation by any subject on any object is tested against the set of authorization rules (aka policy) to determine if the operation is allowed. The ability
to grant permissions itself comes under MAC and this distinguishes it from the simpler DAC model.
* **Role Based Access Control (RBAC)**: This is a policy neutral access control mechanism around roles and permissions and although different from DAC and MAC can easily be used to enforce such constraints
* **Attribute Based Access Control (ABAC)**: This evolved from RBAC to consider additional attribute of either the subject or secured resource in addition to roles and groups and is policy-based in the sense that
it uses policies rather than static permissions to define what is authorized.

This framework is most closely aligned to RBAC but has some ABAC capabilities as well, e.g. you can secure an operation based on additional attributes about the subject and also use [hierarchies](#hierarchies)
to implement more fine-grained permissions. 

The main advantage of the framework is its ease of implementation and speed since operations will take place in memory, and this leads us to its main limitation in that it is intended to secure activities on the class
of the resource rather than individual resources i.e. we can easily secure **_Order.Create_** or **_Order.Edit_** to a role **_SalesClerk_** but restricting editing of **_Order 231_** for example would require domain knowledge and/or 
database access to know who can edit this particular order and so it out of scope.

## Principals and Claims

Most developers are aware that .NET processes execute under a user context called a Principal, this is easily accessed via Thread.CurrentPrincipal or in a web application via the controllers User property. Since .NET 4.5 
these are all ClaimsPrincipals which can contain multiple ClaimsIdentities each holding Claims. A claim is a security attribute with some basic properties...

* **Type**: The type of claim, e.g. Role, Name, Team, Age
* **Value**: The value of the claim, e.g. Admin, 18, Team A
* **Issuer**: Who says this claim is true, can be important to ensure the integrity of the system
* **ValueType**: What type of data is the claim, default is string but can be any type e.g. integer, date

In this model, roles and the user's name are not anything special, just particular types of claim.

How we arrive at these claims is out of scope for this project but in general they are the responsibility of the authentication system where users, groups and roles are allocated and can also be further enhanced by a 
process called claims transformation which happens as part of the ASP.NET pipeline.

## Resources and Activities

A lot of examples of securing .NET applications show security against activities such as **_CreateUser_** and **_UpdateUser_** and one issue I have with this is that the number of securable items can grow very rapidly e.g.
in a fairly small system of 20 resource types with just CRUD operations you have 80 securable items and this quite rapidly becomes difficult to manage. This project took a slightly different perspective akin to how REST models things
and explicitly separated the resource from the activity that is being secured.

This allows you to define granular but wide-ranging permissions e.g. grant **_Delete_** permission to the **_Admin_** role on all resources or grant all permissions on **_Order_** to the **_SalesClerk_** role.

## Grant vs Deny Permissions

One concept that simplifies security maintenance is the concept of Deny permissions which take precedence over any Grant.  For example, your normal sales clerk Alice is off for the say so you grant Bob the **_SalesClerk_** role, but
you don't want him to be able to do too much damage so you also say Bob Deny **_Order.Delete_**, now this Deny permission for Bob will take precedence over the **_SalesClerk_**'s standard permissions to be able to do anything to an Order.

# Defining the security model

Following on from the idea of resources and activities, your security model should be focused on your business functionality rather than how you map this to a particular software implementation. If you do this, it will be much
easier to explain to the business sponsors what is being secured and why, and should more naturally flow from any analysis discussions e.g. "We only want managers to be able to approve invoices".

Another way of developing this structure is rather than resource/activity is noun/verb; nouns represent objects that need securing and verbs are the operations that users will perform.  You can stray from this pattern, but it should
be intentional rather than accidental.

Lets take a simple business model

* **Orders**: Orders placed in the system
* **Invoices**: Invoiced orders
* **Sales Clerk**: Enters/amends orders in the system
* **Invoice Clerk**: Processes invoices
* **Sales Manager**: Supervises sales clerks
* **Finance Manager**: Supervises invoice clerks
* **Finance Director**: Supervises managers

So what we want to implement is the following

* **Sales Clerk**: Can read, create, amend, ship and cancel Orders but can't delete them, has no rights to Invoices
* **Sales Manager**: Can read, ship and cancel Orders but can only read Invoices
* **Invoice Clerk**: Can read, create, amend and cancel Invoices but not approve or delete them, has no rights on Orders
* **Finance Manger**: Can read, approve or cancel Invoices but only read Orders
* **Finance Director**: Can read and delete Orders and Invoices but has no rights to create, amend or approve

This gives a reasonable separation of powers where it requires at least two actors to colude to create an order, raise an invoice and get it approved.

This can be represented in the following table, blank nodes means no rights

| Resource | Activity | SC | SM | IC | FM | FD |
| -------- | -------- | --- | --- | --- | --- | --- |
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

This grants all activities on **_Invoice_** to the **_InvoiceClerk_**, but then specifically disallows them from **_Approve_** whilst granting that right to the **_FinanceManager_**.

One general principle is not to have hierarchies of roles, if you do so it makes it much more difficult to see the implications of changes to the security model and can lead to inadvertent breaches
of separation of concern rules. For example, if instead of explicitly denoting a **_FinanceDirector's_** permissions we also said that they also inherit the other roles, then a bad actor could create an order, invoice
it and approve it without the security model complaining.

## Hierarchies

Sometimes you need more fine-grained security, for example you want to provide some property-level permissioning so employee information is available but their salary information is not. Another example might be that
you want people to being to print some reports but exclude them from more sensitive data; to do this we have the concept of both resource and activity hierarchies.

This is achieved by using a path separator "/" in either the resource, the activity or both, the authorization system then checks them using a canonical ordering from most specific to least specific to determine the permission
to apply, and this first one to make a decision is the one applied. For example given the following security fragment...

       <activity name="Reports.Print" authorized="true" />
       <activity name="Reports/Sales.Print" authorized="false">
           <allow roles="Sales" />
       </activity>
       <activity name="Reports/Employees.Print" authorized="false">
           <allow roles="HR" />
       </activity>

This says that all authenticated users are allow to print reports, but only Sales are allowed to print Sales reports and only HR are allowed to print Employee reports.

So if we ask to authorize **_Reports/Sales.Print_** the following activities would be considered in this order

Reports/Sales.Print -> Reports.Print -> .Print

By using 'authorized="false"' at the sublevels we are forcing the decision to be evaluated there, otherwise it would bubble up to **_Reports.Print_**. The other alternative is to use Deny permissions but this can
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

The problem is if you make a HR user a member of Users, they would be locked out of their reports due to the Deny taking precedence. This also brings up a good point of generally avoiding groups such as **_Users_**. We effectively
get this overall group by virtue of users being authenticated and you can include everyone with for an activity with a 'authorized="true"' clause or even 'allowUnauthenticated="true"', for example one standard entry I have in most security files is

    <activity name="Home.Index" authorized="true" allowUnauthenticated="true" />

To use the hierarchies effectively you might need to make more than one call to the authorization system...

* A employee MVC controller is secured with **_Employee.Read_** but you do an additional check on **_Employee/Salary.Read_** before returning that data.
* The top level Reports link might be secured with **_Reports.Print_** but a particular report might be secured with **_Reports/Salary.Print_**.

# Securing an Application

To secure application we need to:

* Tell the system to enforce the security model
* Map the controller/action to the resource/activity security model

Some of this code is generic and is in the Meerkat.Security assembly, but the MVC specific code is held in Meerkat.Security.Mvc and this should be referenced from your MVC project. The process to secure a WebApi application is the same as that for an MVC application, but a different assembly must be referenced as MVC and WebApi do not share the same code base.

## Enforce security model

We want the security model to be enforced with as little programmer intervention as possible, so the easy way is to introduce a custom attribute similar to the built-in **_Authorize_** attribute. Our one is called **_ActivityAuthorize_** and can either be registered via a global filter, or explicitly placed on a controller.  If introduced as a global filter, the system automatically determines the relevant controller/action information from the routing information.

It is possible to put multiple **_ActivityAuthorize_** on a controller action but be aware that all such attributes must evaluate to true for the user to be granted access; this may be useful where more complex security requirements arise.

## Mapping controller/action

There is a fairly natural mapping from controller/action to resource/activity but it sometimes needs a little help so that we do not unnecessarily multiple up activities. For a standard MVC controller you will typically have the following actions, the relevant activity is shown afterwards

* Index: Displays the list of data - **_Read_**
* Details: Displays a single item - **_Read_**
* Create: Creates an entity - **_Create_**
* Edit: Modifies an entity - **_Update_**
* Delete: Deletes an entity - **_Delete_**

For a WebAPI controller we have a slightly different set if we are following REST API rules

* Get: Displays the list of data - **_Read_**
* Get Displays a single item - **_Read_**
* Post: Creates an entity - **_Create_**
* Put: Modifies an entity - **_Update_**
* Patch: Modifies an entity - **_Update_**
* Delete: Deletes an entity - **_Delete_**

If you are using an OData controller you may also need to support sub-types of your main entities

* GetFromXXX: Displays the list of data - Resource: XXXX, **_Read_**
* GetXXX Displays a single item - Resource: XXXX, **_Read_**
* PostXXX: Creates an entity - Resource: XXXX, **_Create_**
* PutXXX: Modifies an entity - Resource: XXXX, **_Update_**
* PatchXXX: Modifies an entity - Resource: XXXX, **_Update_**

To avoid having to specify the changed values on every controller, we introduce a new service **_IControllerActivityMapper_** with a default implementation that performs the basic mappings shown above.

