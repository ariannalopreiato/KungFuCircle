# KungFuCircle
A simple AI for enemies to make them act and move accordingly to the player's position and following the logic of a Kung Fu Circle.

**What is the Kung Fu Circle?**

The "Kung Fu Circle" is a combat system that manages how NPCs will engage the player during fights in order to make the game less punishing and fast paced. 
This system is usually applied for games that try to make the combat flow in a more cinematic way and for fights with a big amount of enemies.
Spiderman and Batman Arkham Asylum are the two games that I took as guidance in developing my implementation of this technique.

![](https://github.com/ariannalopreiato/KungFuCircle/blob/main/Media/Final.gif)


**My implementation**

All the code and setup has been done in Unity using C#.
The first step of the implementation was to mimic the main movements of the enemies: the moving to the player and back and the circulating around the player.
With this done, I moved on to setting up a **State Machine** for the different states that the enemies could have been in: idle, movingToTarget, moveInCircles, walkingBack, attacking.

As there can be different types of enemies, I made a base class for the enemies so that each one could edit the behavior that gets called from the state machine.
Since each state asks for a base enemy as parameter, this implementation allows the Enemy Manager to simply update or change the enemy state without having to know which enemy it is. 

So far, this KungFu circle has two radiuses. The attacking radius melee and a far radius.
The way this works is that when an enemy will be in the attacking radius, it will be marked as able to attack, and will be saved in a list of enemies that can also attack. The manager for the enemies goes through all of them and chooses a random one to attack the player changing its state to attacking. 
The others will continue to either move around the player or move away from it.
Once the attacking enemy is done attacking, it will retreat to leave space to the other enemies.

All the other enemies beyond the far radius will come closer to the player and either start circulating or retreat as well.

![](https://github.com/ariannalopreiato/KungFuCircle/blob/main/Media/FarAwayEnemies.gif)

To make it look a bit more realistic, I made it so that if the enemies enter each others range they will move away either by stepping back or by circulating. This avoids the creation of big crowds or lines of enemies following the player. It leaves enough space for the player to move and be able to avoid/attack.
The enemies outside of the camera view space won't attack the player since they are not visible, they will also either circulate or move closer to the player.


**Conclusion and future works**

The result is a very basic product but that can be a good stepping stone for developing a far more complex behavior.
Currently, this works for places with big fields and a bunch of enemies, if the combat were to take place in smaller spaces and with less enemies, it would probably turn out ot be boring and quite repetitive.
This is the reason that in the Spiderman game, for example, the movement of the enemies is chosen between four different positions, calculated depending on the position of the player. This could be for sure an improvement that could be applied.
The implementation is now open to add different types of enemies such as ranged or heavier enemies.

**Sources**
https://gdcvault.com/play/1025828/-Marvel-s-Spider-Man 
https://www.youtube.com/watch?v=CT5gUNeTS1w&t=77s
https://www.youtube.com/watch?v=GFOpKcpKGKQ

