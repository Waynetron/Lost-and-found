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


=== tile_description ===
{currentTile=="Goal": -> endGame}
{currentTile=="Forest": -> forest_tile}
{tile_description} I'm in a {currentTile}.{aheadPassable: Ahead is {aheadTile}.} {leftPassable: To me left is a {leftTile}.}{rightPassable: To my right is a {rightTile}.}{backPassable: Behind me is a {backTile}.}
-> movement_choices


=== forest_tile ===
I'm standing in a forest.

~ temp disoriented_roll = RANDOM(1, 5)
{disoriented_roll == 1:
  All the trees are starting to look the same. I'm not even sure which direction I'm facing anymore. # disoriented
}
-> movement_choices


=== movement_choices ===
+ {aheadPassable}[forward]
+ {backPassable}[back]
+ {rightPassable}[right]
+ {leftPassable}[left]
-
-> tile_description


=== endGame ===
I've reached you!
->END