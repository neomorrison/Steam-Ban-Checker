# Steam-Ban-Checker

**Lore:**

The other day I was playing Counter-Strike and getting absolutely demolished. 
Humbly put, I'm good at the game and so when someone is destroying me as much as the guy was there are two scenarios in my head: 1) He's just a lot better 2) He's cheating (more probable)
Anyways, I went back and watched the demo and saw that he was indeed cheating (who would've guessed? oh me)

The next day I got into a match with him except this time he was on my team. I called him out on cheating yesterday and he played it off telling me he is just much better
with much more experience. I found myself more urked than usual (as there are always cheaters on that game) so I saved his steam profile in my bookmarks on Chrome.
For the next few days, I refreshed at least once a day waiting to see he was banned. To no avail, he's not been banned and I realized I would soon forget about it.

Instead, I wrote a program to use the Steam Web API to track and monitor accounts that are inputted into it and refresh for VAC/Game Ban status.

**Program Information:**
The program needs a user API key which can be retrieved by anyone with a paid steam account game. (https://steamcommunity.com/dev/apikey) 
You do not need a real website to register one it is just for clerical sake. 

When you start the program for the first time, it will check for two files (steamapi.json and steam_ids.json).
Since these files aren't present it will get you to input your API key (because I can't publically share mine) and disable the Watch User(s) button.
Simply enter and save your API key to access the program (you can see the source code, there is nothing malicous going on).

Enter the Steam ID's 64BIT Representation into the main textbox and then press Watch Users. You can save your user list by pressing Save User(s) and it will create a steam_ids.json file.

Every time you open the program OR press the Watch User(s) button it will refresh the list.

Note: If you would like to add more user monitoring space feel free to extend the window size in the source code.

**PROGRAM DEPENDENCIES:**
For the .json utilization there is a dependency:
  - **Newtonsoft**

![046f67e9f294e99563548215344641fb](https://user-images.githubusercontent.com/44535532/218917341-4cfbf211-b4b5-499e-99a4-be70c89b7497.png)
![ezgif-4-044cd1cfc7](https://user-images.githubusercontent.com/44535532/218917357-a275e5f7-2dea-46e5-b09f-c2c4c3493c9a.gif)
