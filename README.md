# Settlers of Catan
This game was developed for the 3rd Semester of software development.  
Settlers of Catan is the computer version of the original board game. You can play it with up to 4 Players in your local network. The basic functionallity is implementet. 
Some trade and development cards are still under construction but this wont stop you from playing the game ;)

![Startscreen](/images/start_screen.jpg )  
![Lobby](/images/lobby.jpg)  
![Mainscreen](/images/in_game.jpg)  
![Endscreen](/images/end_screen.jpg)  

## Installation
You can clone the repository either via SSH:
```bash
git clone git@gitlab.mi.hdm-stuttgart.de:tw086/se3-siedler-von-catan.git
```
Alternatively use HTTPS:
```bash
git clone https://gitlab.mi.hdm-stuttgart.de/tw086/se3-siedler-von-catan.git
```
### For development
You need to install [Unity](https://unity.com/). If you don't have it already you may want to install some kind of IDE for development with C#.
Once you are ready import the project into Unity. You can find all scripts in the folder [```/Assets/Scripts```](https://gitlab.mi.hdm-stuttgart.de/tw086/se3-siedler-von-catan/-/tree/master/Assets/Scripts).


### For gameplay
You dont need to install Unity at all.  
Just get the folder [```/Buit Game```](https://gitlab.mi.hdm-stuttgart.de/tw086/se3-siedler-von-catan/-/tree/master/Built%20Game). To run the game you only need to double click the ```SettlersOfCatan.exe``` file.
If you want to play with your friend just share this folder.

## Play the game
All basic rules are implemented.  
One player needs to host a lobby. The host can now pass the IP Address displayed in the lobby to his friends. 
They can join the game by clicking on the join lobby button and entering the hosts IP Address.

## Developers
### Networking:  
- Andrea Feurer - af099  
- Simon Geupel - sg184

### UI/Trading:  
- Lena Sophie Musse - lm138

### Board logic:  
- Marco de Jesus António - md131
- Timo Waldherr - tw086

### Game logic:  
- Maximilian Dolbaum - md127

## Contributing
Pull requests are welcome. For major changes, please open an issue first to discuss what you would like to change.

Please make sure to update tests as appropriate.
