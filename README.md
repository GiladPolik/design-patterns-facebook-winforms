# Facebook Desktop Clone - Design Patterns Implementation

## About The Project
A Windows Forms (WinForms) desktop application that simulates core functionalities of a social network, inspired by Facebook. 
This project was developed as part of the Design Patterns course at The Academic College of Tel Aviv-Jaffa, with a strong emphasis on clean architecture, object-oriented programming (OOP) principles, and the practical application of software design patterns.

## Core Features
* User Authentication (Login/Logout)
* View and interact with a dynamic news feed
* Create new posts 
* Like and interact with friends' posts

## Applied Design Patterns
This project focuses heavily on architectural design. Here are the main Gang of Four (GoF) design patterns implemented to solve various software challenges within the app:

* **Singleton**: Implemented to manage the `LoggedInUser` session, ensuring only one instance of the user state exists throughout the application lifecycle.
* **Observer**: Used in the news feed system. When a new post is created or an interaction occurs, all relevant UI components (observers) are automatically notified and updated.
* **Factory Method**: Utilized for creating different types of UI elements or posts (e.g., Text Post, Image Post) without coupling the client code to the concrete classes.
* **Strategy**: Applied to the feed sorting mechanism, allowing dynamic switching between different sorting algorithms (e.g., 'Sort by Recent', 'Sort by Popularity') at runtime.
* **Facade**: Implemented to hide the complexity of the underlying data management and provide a simplified interface for the UI layer to interact with.

## Built With
* C#
* .NET Framework (WinForms)

## Getting Started
1. Clone the repository:
   ```sh
  git clone https://github.com/GiladPolik/design-patterns-facebook-winforms.git
2. Open the .sln file in VS
3. Build and run the project (F5).
