# Introduction

NetCorePosSample is a basic example of e-commerce-pos Rest API which is developed to experience dotnet-core and new technologies.

# Used Libraries
  - Asp.Net Core 2.1 Web API
  - Entity Framework Core 2.1.1 (InMemory)
  - .NET Standart 2.0
  - NLog
  - AutoMapper
  - JWT
  - Swagger UI
  - XUnit Tests
  - Moq
   
# API Explanation
  - You should open the project in Visual Studio 2017 with Administrator mode.
  - You can use swagger.ui for all of your requests after you run the application.
    > http://localhost:32348/swagger/index.html
  - First, you have to login and get an authenticate token. After that use this token in the Authorize section.
  - Now you can try other endpoints. You can create users, products, campaigns, and orders.
  
## EndPoints


|Http Verb|Endpoint        |Description                  |
|----------------|----------------|-----------------------------|
|POST|v1/login        |Creates user token with user name and password            |
|GET|v1/me           |Returns authenticated user info            |
|POST|v1/users           |Returns user list            |
|GET|v1/products           |Returns product list            |
|GET|v1/products/{id}           |Returns specific product            |
|POST|v1/products           |Creates a new product            |
|PUT|v1/products/{id}           |Updates specific product             |
|DELETE|v1/products/{id}           |Deletes specific product            |
|GET|v1/campaigns           |Returns campaign list            |
|POST|v1/campaigns           |Creates a new campaign             |
|POST|v1/orders           |Creates an order (sales-invoice)            |
|GET|v1/orders/{id}           |Returns order info            |
|GET|v1/orders{id}/items           |Returns order products list            |
