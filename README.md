OpenCards
=========

Open and adaptable client-server system for social card games.

This project is a suite of programs designed to provide a modular, adaptable, and purely OOP system for playing social card games with friends over the Internet.

The suite includes a client, server software, and a simple editor for creating new decks of cards.

The design principles are as follows:

- Provide a dynamic, intuitive interface
- Keep the code modular and adaptable by sticking to a pure OOP methodology

To Do:

- Client: Code the rendering of all Responses for the round
- Client: Manage selection and deselection of Hand Cards, including multiple Cards while preserving order of selection
- Client: Clean up Card.Render() methods to return a PictureBox instead of a Bitmap
- Client: Implement TCP communication

- Server: Implement TCP communication
- Server: Implement core game logic

- Editor: Implement saving of input data as Deck files

- General: Overhaul use of the OpenCards.dll library to allow selecting game formats by selecting different DLLs
- Encapsulate all game rules and definitions entirely within the OpenCards.dll library

