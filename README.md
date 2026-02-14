# Blue-Prince-Neuro-Sama-Integration 

This is a work in progress mod for the game [Blue Prince](https://www.blueprincegame.com/) that allows [Neuro-sama](https://www.twitch.tv/vedal987) to control various aspect of the game. 

## Features

The mod currently provides basic Draft and Inventory information and permits Neuro to decide which Room to pick when drafting. It also sends locational information whenever the player moves from one room to another.

Current work is focused on handling the various effects that change how drafts work. Adding handling of other decision based moments like Experiments is also planned.

The mod is being developed with coop in mind due to the complexity that would go into an autonomous navigation.

**To be clear, the current version works but lacks many important and necessary features. Use at your own *deadly* peril.**

## Contributing

Please do.

Really though, all help is welcome. Even if you cannot program, testing, feedback and suggestions are always important. The puzzle nature of the game is also prone to cause edge cases to be missed, so that's another avenue through which to contribute.

If you are an artist, I have also looked into and found asset modding perfectly doable. You will not catch me going into the Blender mines though.

## Tools used

The framework used for the mod is the [MelonLoader Mod Loader](https://github.com/LavaGang/MelonLoader)

Communication with the Neuro-Sama API is achieved through an IL2CPP compatible version of the [Neuro SDK for C#](https://github.com/InYourHeart/CSharp-Neuro-SDK).

## Installation

I am an evil person and so the project currently uses absolute paths to get the game's DLLs, which are not included in this repository for copyright reasons. Keep that in mind if you are planning on opening it on your own machine.

Once compiled, two files are needed in the "Blue Prince/Mods" folder: "Blue_Prince_Neuro_Sama_Integration_Mod.dll" and "NeuroSDKCsharp.dll".
