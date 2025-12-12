# RedRiver-book-quotes
A full-stack CRUD application built with .NET 9 Web API and Angular 20.

## Preview

![Application Screenshot](docs/assets/screenshot-app.jpg)

## Project Information

**Author:** [philip0000000](https://github.com/philip0000000)  
**Completion Date:** 2025-12-02  
**Deployment:** Successfully deployed to Azure Static Web Apps  

## Testing & Compatibility

The application has been tested on:

- Desktop (Windows 10, Chrome & Firefox)
- Smartphone (Android, Chrome)

Tablet testing is planned and tracked in this issue:  
[Request: Tablet layout testing](https://github.com/philip0000000/RedRiver-book-quotes/issues/66)

## Project Overview

The goal is to create a responsive CRUD web application using Angular 20 for the front end and .NET 9 C# for the back-end API. In addition to this, this must be implemented: token-based authentication for user verification, use Bootstrap and Font Awesome icons for styling, and add a "Mina citat" ("My Quotes" in Swedish) page where users can add and view their favorite quotes.

## Requirements

 - Implement a web application with a page that displays a list of all books.
 - Create a home page with a button to add a new book.
 - When clicking the "Lägg till ny bok" ("Add New Book" in Swedish) button, the user should be redirected to a form where they can enter information about a new book (e.g., title, author, publication date).
 - After submitting the form, the user should be redirected back to the home page where the newly added book appears in the list.
 - Each book in the list should have an "Redigera" ("Edit" in Swedish) button that takes the user to a form where they can update the book’s details.
 - After submitting the edit form, the user should be redirected back to the home page where the updated information is visible.
 - Each book in the list should also have a "Radera" ("Delete" in Swedish) button that allows the user to remove the book.
 - After deleting a book, it should be removed from the list.

Token handling:
 - Implement user authentication using JWT (JSON Web Tokens).
 - Create a simple login page where users can enter their credentials (e.g., username and password).
 - Users must be able to register a new account and then use it to log in.
 - After a successful login, the back end should generate a token and return it to the front end.
 - The front end should store the token securely (e.g., local storage or a cookie) and use it for subsequent API requests to the back end.
 - Implement token validation on the back end to ensure that only authenticated users can access the CRUD operations.

My Quotes page:
 - Create a separate view called "Mina citat" ("My Quotes" in Swedish).
 - Display a list of five quotes you like.
 - The user should be able to add, delete, and edit quotes.
 - There should be a navigation menu that allows switching between the books view and the quotes view.

Bootstrap and Font Awesome
 - Use Bootstrap to create a responsive and visually appealing layout for the application.
 - Use Bootstrap classes to style buttons, forms, and other UI components.
 - Include Font Awesome icons to enhance the visual elements of the application.

Responsive Design Testing
 - Ensure the application's layout and components adapt smoothly to various screen sizes, including desktops, tablets, and mobile devices.
 - Test the application by resizing the browser window and verify that all elements adjust correctly.
 - Confirm that navigation menus collapse into a responsive mobile menu on smaller screens.
 - Verify that form fields, buttons, and other UI elements maintain proper spacing and alignment across different viewports.
 - Test the application on multiple devices (e.g., smartphones, tablets) and browsers to ensure consistent behavior.

## Running the application locally

### Backend (.NET 9 API)
cd RedRiver.BookQuotes.Api<br>
dotnet restore<br>
dotnet ef database update<br>
dotnet run<br>

### Frontend (Angular 20)
cd client<br>
npm install<br>
ng serve -o<br>

## API Endpoints

### POST /auth/register

Registers a new user account.

**Request Body**
```json
{
  "username": "string",
  "password": "string"
}
```

### POST /auth/login

Authenticates the user and returns a JWT token.

#### Request Body

```json
{
  "username": "string",
  "password": "string"
}
```
#### Successful Response

```json
{
  "token": "string"
}
```

### Using the Token

Include the token in each request to protected endpoints:

```
Authorization: Bearer <token>
```
