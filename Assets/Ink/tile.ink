VAR currentTile = "none"
VAR aheadTile = 0
VAR backTile = 0
VAR leftTile = 0
VAR rightTile = 0

VAR aheadPassable = true
VAR leftPassable = true
VAR rightPassable = true
VAR backPassable = true

VAR weatherIntensity = 0

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
    ~ weatherIntensity = RANDOM(3, 5)
    {weatherIntensity == 5: The weather looks like it's had a turn for the worse.}
    -> DONE


=== tile_description ===
    {
        - currentTile=="Forest": <- forest_tile
        - currentTile=="Plain": <- plain_tile
        - currentTile=="Swamp": <- swamp_tile
        - currentTile=="Rocky": <- rocky_tile
        - else: <- generic_tile
    }
    -> DONE


=== describe_surroundings ===
    {
        - weatherIntensity == 5:
            {&There's a cold thick fog|The fog is too thick|There is fog all around me}. I can't see ahead.
        - currentTile == "Forest":
            {&The forest is too dense|The trees are too dense}. I can't see ahead.
        - else:
            {aheadPassable: {&Up ahead|Straight ahead|In front of me} is a {aheadTile}.}
            {leftPassable: {&To my left is|On my left is|Left I can see} a {leftTile}.}
            {rightPassable: {&To my right|On my right} is a {rightTile}.}
            {backPassable: Behind me is a {backTile}.}
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


== generic_tile ===
    I'm in a {currentTile}.
    -> DONE


=== movement_choices ===
    + {aheadPassable}[forward]
    + {backPassable}[back]
    + {rightPassable}[right]
    + {leftPassable}[left]
    -
    -> turn_loop


=== endGame ===
    I've reached you!
    ->END