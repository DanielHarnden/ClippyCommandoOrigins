# Infinite Mode Enemy Scripts

## All of the scripts relating to the enemies spawned in Infinite Mode. Each script contains all enemy movement functions and stats

<br><br>

## GluePuddle.cs is used by Gluers

## Staple.cs is used by Gunners

## Gluer Speed Function (X is the distance between the Gluer and Player, C is the Gluer's movement speed) (Movement floor of 2, otherwise Gluer would be stationary if player was too far away):

![Gluer Speed](gs.png)

<br><br>

## Design philosophy:

#### Rulers intended to be melee enemies, slow enough to easily dodge but persistent enough to keep the player's attention.

#### Gluers intended to be rushing enemies that swarm the player and overwhelm them with their speed; they also disadvantage the player with the glue puddles spawned upon a successful suicide death (player killed Gluers don't spawn puddles).

#### Gunners intended to be ranged enemies that provide support to Rulers and Gluers.