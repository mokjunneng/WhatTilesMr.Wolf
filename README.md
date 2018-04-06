# WhatTilesMr.Wolf
## Game description 
The aim of our game is to occupy the most amount of tiles within the set amount of time. However, there is an AI (wolf) on the map which rotates randomly to detect players’ motion. Player must stop moving when detected by AI or 3 of his/her tiles will be converted to his/her opponent. (The wolf will turn based on a random time in seconds.) Players can occupy a tile by stepping onto opponent’s colored tile and the tile will be converted to the player’s color. The game ends in 1 minutes and the player with the most amount of tiles wins. 

## Tools used 
- Unity

## Features of the game
The game features can be categorises into the following categories 
- Wolf AI
- Tile Generation 
- Player Movement

From the three categories, the features found in each categories is as followed: <br />
__1) Wolf AI__ 
* Wolf Random Turning 
* Vibration alert (visual) to indicate wolf is turning 
* Detection of player movement 

__2) Tile Generation__ 
* Random Assignment of Tile location for each player
* Equal Tiles assigned for each player 
* Correct changing of tiles colour when player passes the centre of the tile 
* Random Power Up Tiles

__3) Player Movement__
* Moving to the pressed location 
* Do not merge into other player 
* Vibration alert when player makes a penalty

## Test Plan 
We have developed 2 different test plan, one for physical testing and another for software testing. They are as followed: <br /> 

**Physical Test** <br />
The following test was done with one game session per test scenario. During the game scenario, we will look out for only that specific test case to see if it fails and pass. This is repeated for at least 2 times per test scenario.
1) the player move to the position tapped
2) the tiles change only iff player move over the centre of tile 
3) match making system 
4) penalty is handled when the wolf has turned (vibration felt)
5) wolf vibrates before turning
6) correct number of tiles deducted from penalised player
7) Map generated for both players is the same
8) player movement speed (before and after picking powerup)


**Software Test** <br /> 
Details of the software testing can be found in the testCases.cs script of our project. The following are test cases done by software testing: 
1)  gameobject generation
2)  random tile generation
3)  correct wolf timing
4)  player movements with powerups 
5)  Count for red tiles and blue tiles increase and decrease correctly when a penalty is assigned
6)  Count for red tiles and blue tiles increase and decrease correctly when players move over tiles

## Game Reviews
During the phyiscal testing, we have asked questions to the players and their comments/answers are as followed: 

**After PM3 game construction:** <br /> 
Questions asked were 
1) What are some difficulties in playing the game?
> " Maybe can make **wolf vibrate** cause a bit hard to see turning " - Song Shan <br /> 
> " Some **power up** so that the game will be slightly more interesting " - Rayson <br /> 
> " The model a bit ugly " - Wen Qi <br /> 
> " **Alert** me when I **lose a tile** " - Angelia <br /> 
> " I can just keep **moving without caring about penalty** cause I still can win" -  Eunice <br /> 

2) Is the gameplay intuitive?
> " No, I need to anyhow tap on the phone before playing " - Song Shan <br /> 
> " Yes, easy to pick up " - Rayson <br /> 
> " Yes, simple gameplay and not too complex  " -  Wen Qi <br /> 
> " No, **no start scene before game** " - Angelia  <br /> 
> " Yes, quite fun to play and intuitive " -  Eunice <br /> 

3) One feature you like and/or dislike in the game ? 
> " Hex style " - Song Shan <br /> 
> " Simple player movement " - Rayson <br /> 
> " Penalty system but **hard to tell about penalty** " -  Wen Qi <br /> 
> " Cute Wolf XD , maybe can **add start scene** - Angelia  <br /> 
> " **Concept similar to childhood game**, makes it easier to pick up " -  Eunice <br /> 

4) General Comments
> " Quite **innovative game** " - Song Shan <br /> 
> " **Interesting game**, maybe can add incentives to make it more interesting " - Rayson <br /> 
> " It was quite **easy to play** cause I can slide at the side of the tile to gain it" -  Wen Qi <br /> 
> " **Fun** game, maybe can change the model " - Angelia  <br /> 
> " **Simple** gameplay" -  Eunice <br /> 

Extra Comments made by Mr Yoga after PM3 Meeting
> Add an alert system for the wolf and player to know about penalty.
> Improve the User Interface system for the user ; add a start scene / end game scene

From the questions asked and Mr Yoga's comments, keywords were picked up and implemented before Project Meeting 4 as an improvement to the game

**After PM4 game construction:** <br />
Questions asked were 
1) What are some difficulties in playing the game?
> " **Hard to tell** that I need to tap the centre to **trigger the tile** and **when penalty is given** " - Rachel <br /> 
> " Wolf AI **lighting too dark** " - Reuben <br /> 
> " **UI end scene too small** " - Cyrus <br /> 
> " Game suddenly stop/ **not paying attention to timer**" - Yue Ran  <br /> 
> " **Hard to prevent penalty** when the wolf turn. Cannot **sudden stop**" -  Jeffery <br /> 

2) Is the gameplay intuitive?
> " No, I need to have a few attempts to know I need to rigger at the centre " - Rachel <br /> 
> " Yes, easy to pick up and fun to play " - Reuben <br /> 
> " Yes, simple gameplay  " -  Cyrus <br /> 
> " Yes, quite fun and intuitve except for the **sudden end game** " - Yue Ran  <br /> 
> " No, **cannot suddenly stop my player to avoid penalty** " -  Jeffery <br /> 

3) One feature you like and/or dislike in the game ? 
> " Dark Theme but I can **play lonely** " - Rachel <br /> 
> " Nice models but **timer abit short** " - Reuben <br /> 
> " Short Gameplay, nice maybe **increase overall UI size**" -  Cyrus <br /> 
> " **Game concept** but the **player cube is not consistent** red cube but blue tiles " - Yue Ran  <br /> 
> " **Power up** makes game **interesting** but need to **spawn evenly** " -  Jeffery <br /> 

4) General Comments
> " **Spread the power up** " - Rachel <br /> 
> " **Player tile and cube dont match** but overall fun game " - Reuben <br /> 
> " **Add a ready button (?)** such that both player will spawn at the same time " -  Cyrus <br /> 
> " **Concept similar to childhood game** " - Yue Ran  <br /> 
> " Add **music** in game abit boring" -  Jeffery <br /> 

Extra Comments made by Mr Yoga after PM4 Meeting
> Either **increase the collider size** or **add a dot on the tile** as an indictation the collision is there  <br />  
> **Increase the scale of the UI** as it is hard to see <br />  
> Add a **further indication** to the user **after tile lost** <br />  
> Add a **'stop' button** during **wolf turning** for player to emergency stop <br />  

From the questions asked and Mr Yoga's comments, keywords were picked up and implemented as a final improvement to the game



## Work in progress
**Game**<br />
- [x] Sign In (multiplayer) <br />
- [x] Lobby creation (multiplayer) <br />
- [x] Wolf AI <br />
- [x] Tiles - Terrain <br />
- [x] Tiles - Collision area <br />
- [x] Character Movement - Tiles <br />
- [x] Game Testing <br />
- [x] PM3 Meeting Review : Vibration alert of wolf <br />
- [x] PM3 Meeting Review : Vibration alert of penalty for player <br />
- [ ] Player Model <br />
- [ ] Lighting <br /> 
- [x] Login Scene <br />
- [x] End Scene <br />
- [x] Bonus Feature: Spells <br />
- [ ] PM4 Meeting Review: Reload to Login Scene <br />
- [ ] PM4 Meeting Review: UI Scale up <br />
- [ ] PM4 Meeting Review: Collider Balancing <br />
- [ ] PM4 Meeting Review: Tile lost Indication <br />
- [ ] PM4 Meeting Review: Stop button during wolf <br /> 


**Project Report**<br />
- [x] Project Meeting 1 (PM1) Report <br />
- [x] Project Meeting 2 (PM2) Report <br />
- [x] Project Meeting 3 (PM3) Report <br />
- [x] Project Meeting 4 (PM4) Report <br />
- [ ] Final Report <br />
