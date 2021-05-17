<meta http-equiv='Content-Type' content='text/html; charset=utf-8' />

# Brown Bag - MVC Caching #

## Understanding Caching ##

Caching is a most important aspect of high-performance web application. Caching provides a way of storing frequently accessed data and reusing that data. Practically, this is an effective way for improving web application’s performance.

### Advantage of Caching ###
 * Reduce hosting server round-trips
 * When content is cached at the client or in proxies, it cause minimum request to server.
 * Reduce database server round-trips
 *  When content is cached at the web server, it can eliminate the database request.
 *  Reduce network traffic
 * When content is cached at the client side, it it also reduce the network traffic.
 * Avoid time-consumption for regenerating reusable content
 * When reusable content is cached, it avoid the time consumption for regenerating reusable content.
 * Improve performance
 * Since cached content reduce round-trips, network traffic and avoid time consumption for regenerating reusable content which cause a boost in the performance.

### Key points about Caching ###
 * Use caching for contents that are accessed frequently.
 * Avoid caching for contents that are unique per user (in the server).
 * Avoid caching for contents that are accessed infrequently/rarely.
 * Use the VaryByCustom function to cache multiple versions of a page based on customization aspects of the request such as cookies, role, theme, browser, and so on.
 * For efficient caching use 64-bit version of Windows Server and SQL Server.
 * For database caching make sure your database server has sufficient RAM otherwise, it may degrade the performance.
 * For caching of dynamic contents that change frequently, define a short cache–expiration time rather than disabling caching.

## Output Caching ##

The OutputCache filter allow you to cache the data that is output of an action method. By default, this attribute filter cache the data till 60 seconds. After 60 sec, if a call was made to this action then ASP.NET MVC will cache the output again.

### Where Content is Cached ###

By default, when you use the [OutputCache] attribute, content is cached in three locations: the web server, any proxy servers, and the web browser. You can control exactly where content is cached by modifying the Location property of the [OutputCache] attribute.

![enter image description here](http://media-www-iis.azureedge.net/media/7429499/reverse-and-forward-proxies.jpg)

You can set the Location property to any one of the following values:

 * **Any**:  The output cache can be located on the browser client (where the request originated), on a proxy server (or any other server) participating in the request, or on the server where the request was processed.
 
 * **Client**: The output cache is located on the browser client where the request originated.
 
 * **Downstream**: The output cache can be stored in any HTTP 1.1 cache-capable devices other than the origin server. This includes proxy servers and the client that made the request.
 
 * **None**: The output cache is disabled for the requested page.
 
 * **Server**:	The output cache is located on the Web server where the request was processed.

 * **ServerAndClient**: The output cache can be stored only at the origin server or at the requesting client. Proxy servers are not allowed to cache the response.

By default, the Location property has the value Any. However, there are situations in which you might want to cache only on the browser or only on the server. For example, if you are caching information that is personalized for each user then you should not cache the information on the server. If you are displaying different information to different users then you should cache the information only on the client.

Parameters:

 * **CacheProfile**

 * **Duration**

 * **Location**

 * **NoStore**

 * **SqlDependency**

 * **VaryByContentEncoding**

 * **VaryByCustom**

 * **VaryByHeader**

 * **VaryByParam**

### Web page strategies

 * For static pages: Full page caching
 * For dynamic pages: Full page caching + AJAX, Donut Caching, Donut Hole Caching
 
### Donut Caching and Donut Hole Caching

![enter image description here](https://dotnettrickscloud.blob.core.windows.net/img/mvc/donut.png)

Donut caching is the best way to cache an entire web page except for one or more parts of the web page. Donut caching is very useful in the scenarios where most of the elements in your page are rarely changed except the few sections that dynamically change, or changed based on a request parameter.

>For implementing Donut caching you need to install MvcDonutCaching NuGet package

Donut Hole Caching is the inverse of Donut caching means while caching the entire page it cached only a small part of the page(the donut hole).

Donut Hole caching is very useful in the scenarios where most of the elements in your page are dynamic except the few sections that rarely change, or changed based on a request parameter. Asp.Net MVC has great support for Donut Hole caching through the use of Child Actions.

### Static Files versioning

 The version is a kind of hash taken on all files in bundle content. This enables browser caching, if content of bundle is not change browser will take it from cache, which is much faster. In case of changes, new version token is generated, so browser would be forced to reload bundle.

### Creating a Cache Profile ###

As an alternative to configuring output cache properties by modifying properties of the [OutputCache] attribute, you can create a cache profile in the web configuration (web.config) file. Creating a cache profile in the web configuration file offers a couple of important advantages.

First, by configuring output caching in the web configuration file, you can control how controller actions cache content in one central location. You can create one cache profile and apply the profile to several controllers or controller actions.

Second, you can modify the web configuration file without recompiling your application. If you need to disable caching for an application that has already been deployed to production, then you can simply modify the cache profiles defined in the web configuration file. Any changes to the web configuration file will be detected automatically and applied.

## HTTP ETags ##

### Typical usage ###
In typical usage, when a URL is retrieved, the web server will return the resource's current representation along with its corresponding ETag value, which is placed in an HTTP response header "ETag" field:

####ETag: "686897696a7c876b7e"####

The client may then decide to cache the representation, along with its ETag. Later, if the client wants to retrieve the same URL again, it will send its previously saved copy of the ETag along with the request in a "If-None-Match" field.

####If-None-Match: "686897696a7c876b7e"####

On this subsequent request, the server may now compare the client's ETag with the ETag for the current version of the resource. If the ETag values match, meaning that the resource has not changed, then the server may send back a very short response with a HTTP 304 Not Modified status. The 304 status tells the client that its cached version is still good and that it should use that.

However, if the ETag values do not match, meaning the resource has likely changed, then a full response including the resource's content is returned, just as if ETags were not being used. In this case the client may decide to replace its previously cached version with the newly returned representation of the resource and the new ETag.

### Strong and weak validation
The ETag mechanism supports both strong validation and weak validation. They are distinguished by the presence of an initial "W/" in the ETag identifier, as:

#### "123456789"    – A strong ETag validator ####
#### W/"123456789"  – A weak ETag validator ####

Strong ETag indicates that resource content is same for response body and the response headers.

Weak ETag indicates that the two representations are semantically equivalent. It compares only the response body.

### Optimistic concurrency 

ETags can be used in combination with the If-Match header to let the server decide if a resource should be updated.

This works by letting a client know that a resource has been updated since the last time they checked. The server informs the client via a 412 Precondition Failed response.

>If a resource changes, so does it's ETag. In this way, each "version" of the resource has a unique ETag. If a client attempts and update with a mismatching ETag, then the update is cancelled and a 412 status is given.

### Packages ###

#### [Cache Cow](https://github.com/aliostad/CacheCow)

#### [ASP.NET Web API CacheOutput](https://github.com/filipw/Strathweb.CacheOutput)
