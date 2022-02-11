# Cube-Surfer-Infinity-Games
 
The game uses a GameManager that initializes the game given all prefabs needed to start it. The important prefabs used in the GameManager are the Player, the LevelGenerator and the UIManager prefabs.
The GameManager is the holder of all possible levels of the game, and these are scriptable objects.These scriptable objects have a given amount of parameters that will be passed to the LevelGenerator that will generate the level with a random seed, or a given seed, depending on the level's parameters..

After all is set, when the game starts the GameManager will pass the first level to the Level Generator that initializes it and raises an event that signals that the level is loaded, as soon as this event is raised the player can start to play.

The players will move by dragging their fingers in the horizontal axis, moving left and right. The players can collect cubes (stacking them) and collect cheeses that count as score. If the players have more than one stacked cube and hit an obstacle they'll lose a cube, if they don't have any cubes they'll fail the level. When completing a level the player will keep their collected cheese and can move to the next level, unlocking it. Once unlocked the levels can be players whenever the players desires.

UIManager handles all the updates to the UI, being mainly called upon from the GameManager himself that handles almost all events raised. The UIManager can communicate only with the LevelSelection script that handles the possible playable levels that the player has unlocked.

The game has a Sound Manager that makes the game reactive to all players inputs, from button presses to eating cheese.
