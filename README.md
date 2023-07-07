# Long-running job simulation

## Requirements

This project aims to create a user-friendly Single-Page Application (SPA) using .NET 7 on the backend and ReactJS on the frontend.

Users can enter text in a text field and click the "Convert" button to encode the text into the base64 format. The encoding process happens on the backend.

The encoded string is then sent back to the user character by character, with random delays between 1 and 5 seconds.

The received characters are displayed in a UI textbox, providing real-time updates as new characters arrive. It's worth noting that users cannot start another encoding process while one is already in progress. However, they can cancel the ongoing process by clicking the "Cancel" button.

![Demo](/docs/Demo.gif "Demo")

### Other requirements that have been implemented
 
- Bootstrap JS to make UI look clean
- Default IoC and package manager to build & run the app
- The latest released .Net & C# with all possible new features they provide
- Business logic is implemented as a services
- Server-side app is hosted in Linux Docker container
- API & UI are in different containers
- Basic authentication using nginx in another container

### Requirements that have _not_ been implemented

- Unit tests
- Internationalization and localization
- Connectivity error handling

## Wireframes

![Hard processing simulator UI](/docs/Documentation-Wireframes.jpg "Hard processing simulator UI")

## Solution diagram

![Solution diagram](/docs/Documentation-Architecture.jpg "Hard processing simulator solution diagram")

## Build and run

To run all components in Docker containers, you can use this command in the solution directory:

> docker-compose up --build -d

The frontend is now available at http://localhost:4002 (**proxy** container).

Test user credentials:

> User:     **userA**
>
> Password: **SuperSecurePassword**

To stop and remove the containers, use:

> docker-compose down
