# KungFuCircle

**What is the Kung Fu Circle?**

The "Kung Fu Circle" is a combat system that manages how NPCs will engage the player during fights in order to make the game less punishing and fast paced. 
This system is usually applied for games that try to make the combat flow in a more cinematic way and for fights with a big amount of enemies.
Spiderman and Batman Arkham Asylum are the two games that I took as guidance in developing my implementation of this technique.

![](https://github.com/ariannalopreiato/KungFuCircle/blob/main/Media/Final.gif)


**My implementation**

The first step of the implementation was to mimic the main movements of the enemies: the moving to the player and back and the circulating around the player.
With this done, I moved on to setting up a State Machine for the different states that the enemies could have been in.
In the beginning it didn't turn out to be extremely useful but as I proceeded I found it to make my code cleaner and clearer.

The enemies are all controlled by an Enemy Manager that depending on what the enemies are available for, can proceed to change their state to the desired behavior and update it.
The enemies can be of different types (ranged, melee), which means that I made a specific script for the type of enemy that I needed and made it inherit from the "EnemyBehavior" script.
Inside of the Enemy Behavior script is all the code common to all the enemies and the abstract classes that refer to the behaviors called depending on the state the enemy is in.
The behaviors are specific to the type of enemy, which means that in the script for it, there will be a definition for each behavior.
In the State script there is a definition for each behavior that takes as a parameter the current enemy and calls its implementation for given behavior. In this way, the Enemy Manager doesn't need to look at what type of enemy is acting, but just update the state for every enemy.

In the Enemy Behavior, the enemies are either set to attacking or far away. This information is used by the Enemy Manager to decide the behavior that the enemy should have.
It checks through all the enemies that can attack, which are the ones inside of the attack range, chooses a random one out of all of them and changes its state to attacking, while the other circulate around the player.
Once the enemy has attacked a timer starts and it steps back and is not allowed to attack anymore until it has reached a position bigger or equal to the attack range.
When the timer goes off, a new enemy is set to attacking and so on.
It then checks through all the enemies signed as far away, which are all the ones furher away than the attack range. This enemies can either circulate or get closer to the player.
To make it look a bit more realistic, I made it so that if the enemies enter each others range they will move away either by stepping back or by circulating. This avoids the creation of big crowds or lines of enemies following the player. It leaves enough space for the player to move and be able to avoid/attack.
The enemies outside of the camera view space won't attack the player since they are not visible, they will also either circulate or move closer to the player.

The result is a very basic product but that can be a good stepping stone for developing a far more complex behavior.

This behavior works for places with big fields and a bunch of enemies.
If the combat were to take place in smaller spaces and with less enemies, it would probably turn out ot be boring and quite repetitive.
Some improvements that could be done on the project could be the placement of the enemies when nothing specificis going on by using for example the technique explained by the developers of Spiderman.

