VAR currentTile = "none"
VAR aheadTile = 0
VAR backTile = 0
VAR leftTile = 0
VAR rightTile = 0

VAR aheadPassable = true
VAR leftPassable = true
VAR rightPassable = true
VAR backPassable = true
VAR badWeather = true

-> tile_description

==tile_description
{currentTile=="Goal":->endGame}
{tile_description} I'm in a {currentTile}.{aheadPassable: Ahead is {aheadTile}.} {leftPassable: To me left is a {leftTile}.}{rightPassable: To my right is a {rightTile}.}{backPassable: Behind me is a {backTile}.}
+ {aheadPassable}[forward]
+ {backPassable}[back]
+ {rightPassable}[right]
+ {leftPassable}[left]
-
-> movement

==movement
-> tile_description

==endGame
I've reached you!
->END