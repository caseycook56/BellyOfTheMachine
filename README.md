# Belly Of The Machine
A horror stealth game made in Unity using the High Definition Render Pipeline.
A lost soul wonder through a jail and must escape by traveling through the level and attempting not the die.


Video of Gameplay: https://youtu.be/2NbQ1DqFo4c


Description of key parts of the game:

- The player uses a particle system, representing the lost soul
- The player can travel the level with basic movement and a grapple system that can let you connect to walls and ceilings
- Simulation of fluid dynamics
- Created audio throughout the level, editing free audio tracks
- Player movement combines both character controller and rigid body.
- Utilising HDRP with shaders and volumetric lighting and post processing in the scenes


My contribution:

This game was a massively a collaborative effort and it’s hard to split the game up into parts that we each individual worked on because everyone worked a bit of everything, and the game went through many interactions, but I'll summarised the things I worked the most on. 
- Asynchronous loading levels as the levels may require time to load due to us trying to utilise the HDRP we had to load levels asynchronously
- Player and game management system, keeping the player stats consistent between levels and when the game restarts.
- Player movement, we wrote a lot of code to combine character controller and rigid body
- UI
- Enemy detection system
- Pick up ability system
- Some shader and materials work
