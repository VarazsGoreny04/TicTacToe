TicTacToe
=========
I don't think I have to introduce this game to anyone.

This is just a simple console UI to play the game either on PVP or on PVE mode because the app has a built in unbeatable bot setting too.

Intro:
------
Launching the app will show an old-school slow text output saying the name of the game and the creator.

Settings:
---------
After the intro ends the game presents the user with the settings screen and the following text:
```
You can choose a cell by using the ARROWS
and select it with the ENTER key.
   Start    Bot    Delay   Turns
```

The options are the following:
- **Start**: Will result in starting the game with the current settings.
- **Bot**: Will enter the state where the user can switch weather they wish to play against a bot or not. (The default is yes.)
- **Delay**: Only has affect in PVE mode. Delays the bots move by a certain amount of time. (The default is 700ms.)
- **Turns**: Only has affect in PVE mode. The user can choose weather they want to go first (crosses) or not (noughts). (The default is crosses.)

In-game:
--------
The player can select the desired cell using the arrows and the enter just like in the settings.

When the game ends, the program will tell the result of the game and ask the user how to continue:
```
Do you want to play again?
y - Yes
n - No
s - Settings
Your answer:
```
The user can answer with the letter presented before an option and hitting the enter key.
- *Yes*: Restarts the game.
- *No*: Closes the application.
- *Settings*: Takes the user to the settings page.
