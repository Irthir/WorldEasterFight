## [0.6.1] - 2022-09-03 - G.P.
###Ajout
-Animator du Lapin
###Modification
-Correction du bug empechant la lecture de l'action du joueur local

## [0.5.2] - 2022-08-03 - G.P.
###Ajout
-Systeme de verification des dessins
-Actions joueurs

## [0.4.1] - 2022-08-03 - G.P.
### Ajout
-Ajout de la condition de fin.

## [0.0.8] - 2022-08-03 - R.S.
### Ajout
-Mise en place de la réception de l'évènement d'attaque dans le gamemanager.
-Mise en place de la réception de l'évènement de déconnexion dans la scène Game.
-Mise en place de l'envoie de l'attaque en réseau et de sa réception.
-Liaison du réseau à la condition de fin.
-Liaison de l'envoie de l'atatque au réseau.

## [0.3.1] - 2022-07-03 - G.P.
### Ajout
-Ajout du système empêchant au joueur de réutiliser deux fois la même case 2 tours d'affilée
### Modification
-Modification de l'affichage des dessins

## [0.2.1] - 2022-07-03 - G.P.
###Modification
-Finalisation du systeme de dessin

## [0.0.7] - 2022-07-03 - R.S.
### Ajout
-Mise en place la liaison entre la scène de menu et la scène de jeu.
-Mise en place de la communication pour les attaques des joueurs.
-Mise en place de la détermination du joueur pour le jeu.
-Mise en place du choix du personnage au démarrage de la scène du jeu.
### Modification
-Correction d'un bug sur le gamemanager.

## [0.1.1] - 2022-02-03 - G.P.
###Ajout
-Création des tableaux d'équilibrage
###Modification
-Refonte du système de jeu, avec les attaques, parades, esquives et soins
-Travail sur l'ajout du nouveau système de jeu sur Unity

## [0.0.6] - 2022-02-03 - R.S.
### Ajout
-Mise en place de la structure AttaqueV3.
-Lecture et écrite d'AttaqueV3 en string.

## [0.0.5] - 2022-01-03 - R.S.
### Ajout
-Mise en place de la classe abstraite ASteamManager.
### Modification
-Refonte du code sans les interfaces et basé sur la classe abstraite ASteamManager.

## [0.0.4] - 2022-28-02 - R.S.
### Ajout
-Création d'une interface de gestion des actions sur l'API Steam.
### Recherches
-Recherches quant aux patterns de programmation :
# http://gameprogrammingpatterns.com/observer.html
# http://gameprogrammingpatterns.com/event-queue.html
# http://gameprogrammingpatterns.com/service-locator.html

## [0.0.3] - 2022-23-02 - R.S.
### Ajout
-Mise en place de l'invitation à un lobby.
-Mise en place de l'envoie de message vers un lobby.
-Mise en place de la déconnexion à un lobby.
-Passage de l'objet lobby en singleton.
-Évènement lors de l'entrée ou la sortie dans le lobby.
-Évènement lors de la réception d'un message dans le lobby.
-Évènement lors de la modification des données d'un lobby.
-Évènement lors de la réception d'une invitation à un lobby.

## [0.0.2] - 2022-22-02 - R.S.
### Ajout
-Création d'un début d'interface pour un écran 16:9.
-Mise en place de la réponse au message d'ami steam.
-Mise en place de la création d'un lobby.
-Mise en place de la connexion à un lobby.
-Mise en place du filtre pour la liste des lobbys.
-Évènement lors de la réception d'une liste de lobbys.
-Évènement lors de la création d'un lobby.
-Évènement lors de la connexion à un lobby.
-Évènement lors de la réception d'une message d'ami steam.

## [0.0.1] - 2022-21-02 - R.S.
### Ajout
-Mise en place et test de l'API Steam pour Unity.