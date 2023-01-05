# ecommerce-on-microservices-server
Ecommerce application server-side implemented with Microservices Event
Driven Architecture, RabbitMQ,MySQL, and Docker Containers on .NET.

The client communicates with the server via the ocelot API Gateway service which routes API requests to the server and returns response back to the client.
The API gateway decouples the client from the backend, allowing to change the backend without affecting the client.
What is exposed to the client is an API.
The ocelot gives us the ability to make authorization. Ocelot checks in the header of the request if the request has a bearer token of the user, 
if the request has a token, then the request will be routed to the routing configured in the ocelot.json file.

Each microservice is a docker container built from an image. The communication between the microservices is with the NServiceBus framework, which is configured
in the microservice to listen to the RabbitMQ message broker. NserviceBus uses Observer that listens to events and gets messages from the queues based on the 
microservice configuration.

Each microservice has its own database. There is an implementation of the Saga pattern, which manages a transaction that makes updates to different databases and also
manages the rollback in case the transaction fails.


