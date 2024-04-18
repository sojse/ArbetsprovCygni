# Rock, Paper, Scissors - Arbetsprov
This is a simple API used to let two players finish a game of rock, paper, scissors. The application is built up in a modular way where different modules have different responsibilities. My controllers, services and repository uses asynchronous functions to prepare the application to handle multiple requests in a more efficent way. 

### Games Controller
This part has responsibility for the responses and requests made to the API. DTO's are used to handle input and output to give the developer control over the infromation given to the user and to make sure that the clients using the API will not be affected by changes of the models used in the API.

### Game Service
The game service contains all the game logic and for now uses the Game Repository to get and set data. 

### Game Repository
The game repository stores the game state in a dictionary in the memory. When the server is closed all memory is lost. The game repository is using the Singleton pattern, meaning that all objects using the game repository will use the same instance.

### Revision Control
The application is developed using Git. For each new feature that has been developed a new branch has been created. When the feature is done the feature branch has been merged into the main branch.

### Security
The application comes with some basic security implementations. By default the application only accepts requests made by HTTPS. It has implemented basic input sanitation to prevent the risk for XSS attacks. There is also prepared for rate limiting to prevent the risk of DOS and DDOS attacks.

### Clean up service
To handle the amount of games that are stored in memory a clean up service is run that deletes games that has not been active for some time. A game gets a new active time on creation, join game and make move. By default the clean up function is executed once every day and removes games that hasn't had any activities the latest 24 hours. 


## Frameworks
The application is created with C# and ASP.NET Core due to my familiarity with the languages and for it's features in creating scalable and high-performance applications. Since C# is a popular object-oriented and strongly typed language with lots of features and libraries available to elevate the developement process. 

In this project the following dependencies have been used:

### Swagger
Swagger UI implementates OpenAPI's to create an implementation of the specification of the API. It provides an interface where the user can see documenation over the API and interact with it. When working in teams it's important to provide clear instructions on how to elevate the application and therefor Swagger was implemented to make it easier for others to implement the API in a correct manner. 

### Moq
Moq is a framework used to test components with dependencies in an isolated way. It is used in my API to test the Game Service and Game Controller independently and therefore provide tests that will show where the error is happening. 

### XUnit
XUnit is a framework used to write unit tests for ASP.NET applications. The tests are written using the AAA pattern:
- Arrange, setting up the initial state
- Act, invoke the behaviour that the test is controlling
- Assert, validating the result


## To open and start project
- Open the project with Visual Studio 
- Build and run the project in Visual Studio
- To access SwaggerUI, navigate to 


## Documentation and instructions for the API
To find documentation for the usage of the different endpoints, the parameters they accept and responses they create see [Swagger](https://localhost:8000/swagger/index.html)


## Usage
- To customize the port in wich the application runs on, go to launchSettings.json and update the https applicationUrl.
- The application must be used with HTTPS because of UseHttpsRedirection middleware
- The project also has unit tests for the Game Service and Game Controller. To run the tests press CTRL + R, A


## Areas of improvement
- The usage of an in memory repository offers limited future scalability. The data will also be lost in case of a system failure. For a production grade application some kind of database would be needed. In case of the need for data persistence there would be needed security measurements for this (i.e SQL Injections). in cases of integration to more systems more security measurments would also be needed for the output of the API. 
- The rate limiting that is implemented today only contains the default values and the default limit algorithm. Before going to production this would need to be reevaluted to meet the needs depending on amount of users etc. 
- In case of changes to the API that will affect how the current clients will have to implement the API there will be a need to implement versioning in the API to enable old users to still use the API with it's old functionality. 
