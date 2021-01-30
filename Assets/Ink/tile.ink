VAR currentTile = "none"
VAR aheadTile = "none"
VAR leftTile = "none"
VAR rightTile = "none"

VAR aheadPassable = true
VAR leftPassable = true
VAR rightPassable = true
VAR backPassable = true

-> tile_description

==tile_description
{currentTile=="EndTower":->endGame}
{tile_description} I'm in a {currentTile}.{aheadPassable: North is {aheadTile}.} {leftPassable: West is a {leftTile}.}{rightPassable: To the east is a {rightTile}.}
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