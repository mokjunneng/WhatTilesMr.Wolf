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

**Software Test** <br /> 
Details of the software testing can be found in the testCases.cs script of our project. The following are test cases done by software testing: 
1)  gameobject generation
2)  random tile generation
3)  correct wolf timing
4)  player movements with powerups 

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
- [ ] Login Scene <br />
- [ ] End Scene <br />
- [x] Bonus Feature: Spells <br />

**Project Report**<br />
- [x] Project Meeting 1 (PM1) Report <br />
- [x] Project Meeting 2 (PM2) Report <br />
- [x] Project Meeting 3 (PM3) Report <br />
- [ ] Project Meeting 4 (PM4) Report <br />
- [ ] Final Report <br />
