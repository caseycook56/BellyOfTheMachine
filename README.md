# BellyOfTheMachine
A horror sleath game made in Unity using the High Definetion Render Pipeline.
A lost soul is wondering through a jail and must escape by traveling through the level and attempting not the die

Video of Gameplay: https://youtu.be/2NbQ1DqFo4c

Description of key parts of the game:

The player uses a particle system its supposed to represent the lost soul
The player can travel the level with basic movement and a grapple system that can let you connect to walls and ceilings
Simulation of fluid dynamics
Created audio through out the level, editing free audio tracks
player movement combines both character controller and rigid body.


My contribution:
This game was a massively a collactivtely effort and its hard to split the game up into parts that we each indiviudal worked on becuase everyone worked a bit of everything and the game went through many interations but I'll summarised the things I worked the most on. 
- Asyncoucous loading levels as the levels may require time to load due to us trying to utilise the HDRP we had to load levels asyncriously
- player and game mamangment system, keeping the player stats consistent between levels and when the game restarts.
- player movement, we wrote a lot of code to combine character controller and rigid body
- UI
- enemy detection system
- pick up ability system
- some shader and materials work
