OpenCards
=========

Open and adaptable client-server system for social card games.

This project is a suite of programs designed to provide a modular, adaptable, and purely OOP system for playing social card games with friends over the Internet.
The suite includes a client, server software, and a simple editor for creating new decks of cards.

The design principles are as follows:

- Provide a dynamic, intuitive interface
- Keep the code modular and adaptable
- Implement a pure OOP methodology by modeling a real-life situation directly

=========

To Do:

- Client: Implement TCP communication [1.0]
- Client: Add multiple card scale options [1.0]
- Client: Implement chat [1.1]
- Client: Implement client/server deck sharing [1.2]
- Client: Provide interface to let players participate in booster deck selection process [1.2]
- Client: Add option for card czar to select a tie [1.1]
- Client: Improve support for overly long Card text [1.1]

- Server: Implement TCP communication [1.0]
- Server: Implement core game logic [1.0]
- Server: Implement chat [1.2]
- Server: Implement a deck manager [1.1]
- Server: Implement client/server deck sharing [1.2]
- Server: Prevent cheating by keeping local record of every client's hand [1.1]

- Editor: Implement loading and merging of deck files [1.0]
- Editor: Replace spreadsheet-style interface with a graphical representation like in the Client [1.2]
- Editor: Add import/export methods for CSV and other common formats used by other social card games [1.1]

- Core: Clean up core classes for efficient serialization [1.0]
- Core: Devise system for maintaining integrity of deck files, i.e. embedded checksum, encrypted merge history [1.1]
- Core: Add field to deck files to track number of client/server transactions for pollenation modeling [1.2]

- General: Move most of the content from this ReadMe to wiki pages [1.1]
- General: Finalize log, icon, and Windows Metro tiles [1.1]
- General: Overhaul use of the OpenCards.dll library to allow selecting game formats by selecting different DLLs [2.0]
- General: Encapsulate all game rules and definitions entirely within the OpenCards.dll library [2.0]

Things that need to be accomplished are followed by the target version, based on necessity, complexity, and importance.

- 1.0: Essential for basic play
- 1.1: Would greatly improve quality of gameplay
- 1.2: Should be included, but not as important as other tasks
- 2.0: Stretch goal, not even worth considering until every other task has been accomplished

=========

Core Architecture:

The core OpenCards library contains the classes required to model the entire game.

- Card: A simple white card. Contains fields for text content and the title of the booster deck it came from.
- BlackCard: The round-initiating black card. Contains fields for text and origin like Card, but also carries a number representing how many white cards can be used to respond.
- Deck: An object containing lists of Card and BlackCard object, as well as a string for the title of the deck. This is mostly for storing decks in files and manageing decks within the server.
- Response: An object containing a number of white Cards. This object is assembled in the Client and sent to the Server once every round. The Server then sends a copy of every Client's response to every client.
- GameStatus: This object is sent from the Server to the Client to inform the client of the current responses, player list, player scores, and who's turn it currently is.

Network Protocol:

In the interests of demonstrating pure OOP, the network communications model is simply a virtual recreation of the actions that take place during an actual card game.

- Server sends GameStatus to every Client, containing a BlackCard.
- Server sends several Cards to every Client.

- Client pack one or more Cards into a Response and sends it to the Server.
- Server sends a GameStatus to every client whenever a Response is received.
- Server waits until it receives Responses from every Client, and then sends all Responses to every Client.

- Server sends a GameStatus to every client to reflect updated scores and turn status.
- Server deals out one or more Cards to every Client.

- Client pack one or more Cards into a Response and sends it to the Server.
- Server sends a GameStatus to every client whenever a Response is received.
- Server waits until it receives Responses from every Client, and then sends all Responses to every Client.

- Repeat

=========

This code is licensed under GNU GENERAL PUBLIC LICENSE Version 2