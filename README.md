# BadApple

Trombone Champ mod: Modifies the game to run Bad Apple:

- Synchronizes note scroll position to the frames of the TMB
- Replaces the standard note manager with a more efficient one
- Visually modifies notes
  - Always removes the circles on the start and end of each note
  - Ignores highly-visible-note settings

These only happen at extremely high BPM (10000+) so as not to impact other charts.

Trombone Champ modding Discord: https://discord.gg/KVzKRsbetJ

A video of this in action can be found at https://youtu.be/apG16te7yz4

## Installation

1. Install TrombLoader and this mod.
   - See the Trombone Champ Modding Discord, linked above, for instructions
2. Add the Bad Apple chart as a TrombLoader custom song
   - Download the chart from here: https://drive.google.com/file/d/1UKmTu46Vzt1vZeUTvG0Up8qT0HOX6G5H/view?usp=share_link
3. Open the chart as usual

## Pre-build setup

1. Create a folder `lib` in the same directory as the `.csproj`
2. Copy in these files from `TromboneChamp_Data/Managed`
    - `0Harmony.dll`
    - `Assembly-CSharp-nstrip.dll`
        - To get this, use https://github.com/BepInEx/NStrip and run `NStrip -p Assembly-CSharp.dll`
    - `UnityEngine.dll`
    - `UnityEngine.AudioModule.dll`
    - `UnityEngine.CoreModule.dll`
3. Open the `.csproj` in your preferred IDE and build as normal
