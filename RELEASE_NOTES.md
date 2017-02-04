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
* Breaking change, removed Role from AuthorizationReason and replaced with Reason which records the User, Role or Claim that satisfies a permission

#### 1.0.2 (2016-05-23)
* Renamed Authorise -> Authorise throughout for consistency
* Fix package reference, should be Common.Logging not Common.Logging.Core

#### 1.0.1 (2016-05-19)
* Strong name the assemblies.

#### 1.0.0 (2016-05-19)
* First release.