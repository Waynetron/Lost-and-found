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
// Only print the weather state after checking if we're reached the goal, we don't care if the weather is bad if we're at the exit
{badWeather: The weather looks like it's had a turn for the worse.}
{currentTile=="Forest": -> forest_tile}
{tile_description} I'm in a {currentTile}.{aheadPassable: {&Up ahead|Straight ahead|In front of me} is a {aheadTile}.} {leftPassable: {&To my left is|On my left is|Left I can see} a {leftTile}.}{rightPassable: {&To my right|On my right} is a {rightTile}.}{backPassable: Behind me is a {backTile}.}
-> movement_choices


=== forest_tile ===
I'm standing in a forest.

~ temp disoriented_roll = RANDOM(1, 5)
{disoriented_roll == 1:
  All the trees are starting to look the same. I'm not even sure which direction I'm facing anymore. # disoriented
- else:
{&The growth here is so thick, it almost entirely blocks out light. Really struggling to see.|The flora here is so dense, it's pretty much impossible to see whats up ahead.}
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