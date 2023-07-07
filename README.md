# Long-running job simulation

## Requirements

A general idea is to simulate hard processing work of the input data items given by user.

##### Basic requirements

A simple SPA with .Net 7 on the backend, and ReactJS on the frontend.

On the front-end side user is able to enter the text into the text field, press "Convert" button, and get this text encoded into the base64 format. Encoding is performed on the backend side. 

Encoded string is returned to the client one character at time, one by one, with random pause on the server 1-5 seconds.

All received characters should form a string in a UI textbox, hence it will be updated in real-time by adding new incoming characters
User cannot start another encoding process while the current one is in progress, but user can press "cancel" button and thus cancel the currently running process.

##### Other requirements that have been implemented
 
- Bootstrap JS to make page look neat
- Default IoC and package manager to build & run the app
- The latest released .Net & C# with all possible new features they provide
- Business logic is implemented as a services
- Server-side app is hosted in Linux Docker container
- API & UI are in different containers

##### Requirements that have _not_ been implemented

- Unit tests
- Basic authentication using nginx in another container
- Internationalization and localization
- Connectivity error handling

## Wireframes

![Hard processing simulator UI](https://github.com/mklimkovich/HardProcessingSimulator/blob/develop/docs/Documentation-Wireframes.jpg "Hard processing simulator UI")

## Solution diagram

![Solution diagram](https://github.com/mklimkovich/HardProcessingSimulator/blob/develop/docs/Documentation-Architecture.jpg "Hard processing simulator solution diagram")

## Build and run

To run all components in Docker containers, you can use this command in the solution directory:

>docker-compose up --build -d

Frontend is now available at http://localhost:4001.

To stop and remove the containers, use:

>docker-compose down
