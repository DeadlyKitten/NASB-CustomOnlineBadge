# Custom Online Badges
BepInEx plugin for Nickelodeon All-Star Brawl that allows you to set custom badges for online play.

![image](https://github.com/DeadlyKitten/NASB-CustomOnlineBadge/assets/9684760/36013158-fa53-4d72-94ef-3393beff673d)


## Installation
*If your game isn't modded with BepinEx, DO THAT FIRST!*
Simply go to the [latest BepinEx release](https://github.com/BepInEx/BepInEx/releases) and extract `BepinEx_x64_VERSION.zip` directly into your game's folder, then run the game once to install BepinEx properly.

This mod also requires the following:
- SlimeModdingUtilities. You can download a copy [here](https://nasb.thunderstore.io/package/Steven/Slime_Modding_Utilities/).

Then, go to the [latest release of this mod](https://github.com/DeadlyKitten/NASB-CustomOnlineBadge/releases/latest) and place the dll into `BepInEx/plugins`.

## Usage
Place a png file named `CustomBadge.png` into the root `BepInEx` folder. It must be in png format and named exactly as written.

## Configuration
You can customize the mod's behavior by editing `BepInEx/config/com.steven.nasb.custombadge.cfg`.
- `Enable Mod`: Set this to false to disable the mod altogether.
- `Share Custom Badge`: Set this to false if you don't want your custom badge to be shared online with other players.
- `Download Custom Badges`: Set this to true to view other players' custom badges. (enable this **AT YOUR OWN RISK**.)

----

### Disclaimer
There is nothing in place to prevent people from using inappropriate images as their custom badge. It is highly recommended to leave the `Download Custom Badges` setting off if you're livestreaming.
