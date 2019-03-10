#### 3.1.2 (2019-03-10)
* Introduce SourceLink, update dependencies

### 3.1.1 (2018-09-03)
* Improve AuthorizationScope json serialization
* Tidy up package dependencies

### 3.1.0 (2018-08-30)
* Change ActivityAuthorizer so reason is given if it is the default granting/denying permission
* Target net462, net471 and net472

### 3.0.0 (2018-04-03)
* Target net45 and netstandard2.0 where appropriate
* Breaking change: Replaced IActivityProvider with IAuthorizationScopeProvider
* Introduced json serialization for configuration

### 2.2.4 (2017-09-14)
* Fix ActivityLinkExtension to use IControllerActivityMapper

### 2.2.3 (2017-06-13)
* Extend IActivityProvider/IActivityAuthorizer with async methods

### 2.2.2 (2017-05-22)
* Expose cache duration on CachingActivityProvider/CachingActivityAuthorizer
* Build using Pdbgit to get linked sources

#### 2.2.1 (2017-02-04)
* Allow Activity.AllowUnauthenticated flag to be null so it can bubble up like Default.
* Change from Common.Logging to Meerkat.Logging, unifies logging framework across libraries.

#### 2.2.0 (2016-09-22)
* Introduce IControllerActivityMapper to allow resource/action names to be mapped from controller/action e.g. Get -> Read
* Revised MVC and WebApi ActivityAuthorize attributes, removed extension points InferredResource/Activity should be done via IControllerActivityMapper
* Change MVC ActivityAuthorize attributes to allow for unauthenticated to be handled by IActivityAuthorizer
* Add NuGet dependency to Meerkat.Caching and updated min dependency versions

#### 2.1.0 (2016-09-20)
* Introduce allowUnauthenticated flag to allow grant/deny unauthenticated users access to menu items

#### 2.0.0 (2016-06-06)
* Introduce IActivityProvider interface to source Activities
* Introduce ConfigurationSectionActivityProvider, AggregatingActivityProvider and CachingActivityProvider to acquire Activities
* Remove ActivityAuthorizerFactory as IActivityProvider makes it easier to use straight IoC to construct ActivityAuthorizerFactory

#### 1.1.0 (2016-05-24)
* Introduce claims based authorization, permission can support multiple claims types/claims
* Breaking change: Removed Role from AuthorizationReason and replaced with Reason which records the User, Role or Claim that satisfies a permission

#### 1.0.2 (2016-05-23)
* Renamed Authorise -> Authorise throughout for consistency
* Fix package reference, should be Common.Logging not Common.Logging.Core

#### 1.0.1 (2016-05-19)
* Strong name the assemblies.

#### 1.0.0 (2016-05-19)
* First release.