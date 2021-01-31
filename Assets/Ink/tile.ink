VAR currentTile = "none"
VAR aheadTile = 0
VAR backTile = 0
VAR leftTile = 0
VAR rightTile = 0

VAR aheadPassable = true
VAR leftPassable = true
VAR rightPassable = true
VAR backPassable = true

-> tile_description

==tile_description
{currentTile=="Goal":->endGame}
{tile_description} I'm in a {currentTile}.{aheadTile: North is {aheadTile}.} {leftTile: West is a {leftTile}.}{rightTile: To the east is a {rightTile}.}{backTile: South is a {backTile}.}
+ {aheadPassable}[NORTH]
+ {backPassable}[SOUTH]
+ {rightPassable}[EAST]
+ {leftPassable}[WEST]
-
-> movement

==movement
-> tile_description

==endGame
I've reached you!
->END