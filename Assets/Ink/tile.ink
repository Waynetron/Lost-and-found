VAR previousTile = 0
VAR currentTile = "none"
VAR aheadTile = 0
VAR backTile = 0
VAR leftTile = 0
VAR rightTile = 0

VAR aheadPassable = true
VAR leftPassable = true
VAR rightPassable = true
VAR backPassable = true

VAR stormRemaining = 0

VAR tileDistance = -1

-> turn_loop


=== turn_loop ===
    {currentTile=="Goal": -> endGame}
    <- weather
    <- intro_text
    <- tile_description
    <- describe_surroundings
    <- closing_text
    -> movement_choices


=== intro_text ===
    -> DONE


=== closing_text ===
    {&What should I do|What should I do next|Uh, what else}?
    -> DONE


=== weather ===
    {
        - stormRemaining == 0:
            {
                // chance to start storm
                - RANDOM(1, 6) == 6:
                    The weather looks like it's taken a turn for the worse.
                    // set random storm duration
                    ~ stormRemaining = RANDOM(1, 3)
            }
        - stormRemaining == 1:
            Looks like the storm is cleaning.
            ~ stormRemaining--
        - else:
            ~ stormRemaining--
    }
    -> DONE


=== tile_description ===
    {
        - currentTile=="Forest": <- forest_tile
        - currentTile=="Plain": <- plain_tile
        - currentTile=="Swamp": <- swamp_tile
        - currentTile=="Rocky": <- rocky_tile
        - currentTile=="River": <- river_tile
        - else: <- generic_tile
    }
    -> DONE


=== describe_surroundings ===
    {
        - stormRemaining > 0:
            {&There's a cold thick fog|The fog is too thick|There is fog all around me}. I can't see ahead.
        - currentTile == "Forest":
            {&The forest is too dense|The trees are too dense}. I can't see ahead.
        - else:
            {aheadTile: {&Up ahead|Straight ahead|In front of me} is a {aheadTile}.}
            {leftTile: {&To my left is|On my left is|Left I can see} a {leftTile}.}
            {rightTile: {&To my right|On my right} is a {rightTile}.}
            {backTile: Behind me is a {backTile}.}
    }
    -> DONE


=== forest_tile ===
    I'm standing in a forest.
    ~ temp disoriented_roll = RANDOM(1, 5)
    {
        - disoriented_roll == 1:
            All the trees are starting to look the same. I'm not even sure which direction I'm facing anymore. # disoriented
        - else:
            {&The growth here is so thick, it almost entirely blocks out light. Really struggling to see.|The flora here is so dense, it's pretty much impossible to see whats up ahead.}
    }
    -> DONE


=== plain_tile ===
    I'm in a plain.
    -> DONE


== swamp_tile ===
    The smell makes me nauesous.
    -> DONE


== rocky_tile ===
    {&The view from up here is incredible, I wish you could see it.|The rock here seems almost mettalic. It makes this odd chime when I clip it with my boot.}
    -> DONE


== river_tile ===
    I am standing in the river.
    -> DONE


== success_crossed_river ===
    I managed to cross the river.
    -> turn_loop


== swept_downstream ===
    I got swept downstream.
    -> turn_loop


== failed_river_crossing ===
    I crossed the river, but couldn't find a way forward. So I turned around and headed back.
    -> turn_loop


== failed_movement ===
    The terrain was too treacherous. So I turned around and headed back.
    -> turn_loop


== generic_tile ===
    I'm in a {currentTile}.
    -> DONE


=== movement_choices ===
    ~ previousTile = currentTile // set previous tile just before we move

    +	{ aheadTile == "River" } 	[↑ Continue forward and cross the river]
    + 	{ aheadTile != "River" 	 } 		[↑ Continue forward]
    
    +	{ backTile == "River" } 	[↶ Turn around and cross the river]
    + 	{ backTile != "River" 	 } 		[↶ Turn around and go back]
    
    +	{ leftTile == "River" } 	[↰ Cross the river to your left]
    + 	{ leftTile != "River" 	 } 		[↰ Go to your left]
    
    +	{ rightTile == "River" } 	[↱ Cross the river to your right]
    + 	{ rightTile != "River" 	 } 		[↱ Go to your right]
    -
    -> turn_loop


=== endGame ===
    I've reached you!
    -> END