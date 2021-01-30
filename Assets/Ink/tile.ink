VAR currentTile = "none"
VAR aheadTile = "none"
VAR leftTile = "none"
VAR rightTile = "none"

-> tile_description

==tile_description
{tile_description} I'm in a {currentTile}. North is a {aheadTile}, west is a {leftTile} and to the east is a {rightTile}.
+ [NORTH]
+ [SOUTH]
+ [EAST]
+ [WEST]
-
-> movement

==movement
-> tile_description