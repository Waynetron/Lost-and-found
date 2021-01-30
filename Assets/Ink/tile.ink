VAR tile = "none"

-> tile_description

==tile_description
{tile_description} I'm in a {tile}.{tile == "Plain": It's very grassy here.}{tile == "River": My feet are wet.}{tile == "Lake": Maybe I can catch a fish.}{tile == "Forest": I'm worried about getting lost.}
+ [NORTH]
+ [SOUTH]
+ [EAST]
+ [WEST]
-
-> movement

==movement
-> tile_description