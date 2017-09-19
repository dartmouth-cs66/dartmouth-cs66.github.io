---
layout: page
title: Short Assignment 1
published: true
---

# Up and Running with Unity

It's hard to take a class on game development without an environment to develop games in. Enter Unity. In your first short assignment, we'll be walking through the ins and outs of the Unity editor to get you up to speed on all of its basic features.

This list is not at all exhaustive! There's plenty to explore in the upcoming weeks, and you'll be doing a lot of it yourself. But, by the end of this assignment, hopefully you'll start to feel comfortable enough that experimenting will be easier.

## Unity
![](./img/unity.png)

🚀 Install [Unity](https://unity3d.com/get-unity/download). The Personal Edition is more than enough for this class. Unity is a game engine that was designed to democratize game development. It's super easy to learn and extremely powerful. We'll be using Unity for all of our development, except for script writing.

🚀 Install [Visual Studio](https://www.visualstudio.com/downloads/). The Community Edition is fine. Visual Studio is a nice IDE that recently became available for Mac users in addition to the standard Windows userbase. It's a great IDE for C#, which is the language we'll be writing all of our scripts in.

🚀 Install [GitHub Desktop](https://desktop.github.com/). We will be mostly using `git` from the commandline but installing the app gives us a nice git tree visualization.

## Command Line

We’ll be doing a lot of commandline stuff. We’ll introduce stuff as we go, so do not fear, commandline is best.

If you are interested here’s a tutorial that gives you more than enough to be a master: [learn just enough commandline to be dangerous](https://www.learnenough.com/command-line-tutorial).

Before you open your default Terminal, you should download [iTerm2](https://www.iterm2.com/downloads.html). It’s completely open-source, and it’s vastly superior to the default terminal.

You should also download a git command prompt to make life easier when we work with git later on. You can find a cool one [here](https://github.com/jimeh/git-aware-prompt). Follow the instructions on the README at that link to install it!

Here’s some commandline basics:

* ls -la will list files in current directory
* pwd will show current directory
* cd somedirectory will change directory to somedirectory
* cp source target will copy files
* mv source target will move
* rm somefile will remove that file (permanently, bypassing trash)
* mkdir somedirectory will create a folder (directory)

On OSX:

* open somefile will open the file in the default app associated with it
* open somedirectory will open that folder in Finder

## Let's Get Going!
After Unity is installed, go ahead and open it up. You'll most likely be seeing this screen:

![](img/launchscreen.png)

We'll want to start up a new project, so hit "New" in the top right corner. On the next page, fill in all the information:
* Project name: sa1
* Location: wherever you want to save it. I picked Desktop.
* 3D/2D: choose 3D
* Enable Unity Analytics: off for now, we won't need it for this assignment

Click create project to get started!

## The Editor
Wow, this sure looks complicated! Not to worry, it'll all make sense soon. Here's some more detail on everything you're seeing:

### Scene View
![](img/sceneview.png)

That big screen in the middle is the Scene View. It's where you'll compile all the pieces together to produce the current scene (Unity's word for the current game stage is a "scene". A game can be made up of lots of scenes, each serving a different purpose. Some can be levels, some are main menu screens, some credits, and so on.)

You can navigate around the scene view in a couple of ways.
* Use the mouse's scroll wheel to zoom in and out. You can also zoom with the drag tool selected (top left, looks like a hand) by holding down the `ctrl` key.
* That drag tool is used to navigate sideways along the current view.
* You can rotate the view by clicking and dragging while holding the `alt`/`option` key.

### Hierarchy
![](img/hierarchy.png)

The Hierarchy contains all of the GameObjects that are currently in place in the scene. You may notice that we already have two of these by default: Main Camera and Directional Light. You can see these in the scene view as well. The main camera is the perspective from which the player will see the game in the Game View, and the Directional Light is the global light raycaster for the scene, and will help determine directions of shadows, etc.

Let's create a new GameObject. At the very top of the hierarchy, open the "Create" dropdown, and select 3D Object > Cube. A new cube will appear at the center of wherever your camera is looking. Click on the cube to highlight it, then move your mouse into the scene view and press the "F" key to focus the scene view on the cube.

### The Inspector
![](img/inspector.png)

Your new best friend, the inspector, allows us to view and change the attributes of individual game objects. By default, the inspector is already filled with a lot of default *components*. A component is a specialized thing attached to a GameObject. We don't need to know what all of the cube's default components do, but in their current configuration, we should know that they make this cube uniquely a cube and not, say, our Main Camera.

Right now, we should definitely know about the *transform* component. Simply put, it describes the current location in space of this GameObject. Play around with the numbers a bit so you get a sense of how they work.

Unity also has a nice feature for changing the numbers. To change the Y Position, for example, simply click on the "Y" next to the text field and drag up or down to change the value 🆒

### The Game View
![](img/gameview.png)

Next to the tab that says "Scene" about the scene view, there's another tab that says "Game" with a little Pac-Man icon next to it. If you click that tab, your scene view will change into the Game View, which is the view you'll see when you're actually playing the game.

Recall from earlier that the game view is from the perspective of the Main Camera. So, if we move the camera's transform, what will happen? Try it out! Select "Main Camera" in the hierarchy and change the values of its transform in the Inspector.

:exclamation: What happens when you try to change the scale of the camera? Why do you think that is?

### The Play Button
Alright, navigate back to the scene view. Then, click the Play Button at the top of the editor.

![](img/playbutton.png)

You should now be seeing the game view again. It might look the same, but different things are happening now. The engine is now working its magic, running background methods like `Update()` and `FixedUpdate()`, optimizing for dynamic vs kinematic and static rigidbody physics, and lots of other things that don't make sense now but will in no time!

As a quick demonstration, let's add a component to our cube.

### Adding a Rigidbody
Click the Cube again in the Hierarchy. At the bottom of the Inspector, click the button labeled "Add Component". Then select Physics > Rigidbody. The component will appear in the inspector with all the defaults selected.

A rigidbody is a required component for Unity to invoke physics on this GameObject. You'll notice right away that the component has things we would associate with physics calculations: Mass, Drag, Gravity, etc. Leave these all be for now.

Navigate back to the Game View. Nothing has really changed, the cube is in the last place we left it. But what happens if we hit "Play" again..?

The cube falls out of view! Unity detected that the cube object has a rigidbody component attached to it. Since the default setting enables gravity, Unity applied a constant downward force of 9.8 m/s<sup>2</sup> on our cube. This is just a quick example of the power of the Unity engine, which will become more apparent as we go through this term.

### The Project View
![](img/projectview.png)

The last important view for now is the Project View. This contains the directory tree for our current project, starting with the Assets folder, which is created by default. In a given project, you'll probably have folders for your prefabs, scenes, sprites, scripts, and materials. We'll cover each of those soon.

For now, we should save our scene in an aptly-named folder. Right click on the Assets folder and select Create > Folder. Rename this folder "\_Scenes".

Save the scene by clicking File > Save Scenes or by pressing the hotkey combination `command-s` on Mac or `control-s` on Windows. Call the Scene "Main" and save it in the \_Scenes folder.

### And We Are Done!
Congratulations! You've explored the basics of the Unity Editor, and are ready to build your first game. We've explored:

* The Scene View, where you'll edit your scene
* The Hierarchy, where all of your scene's GameObjects are contained
* The Inspector, where you can view and edit the components attached to a GameObject
* The Game View, where you can see the game from the player's perspective
* The Play Button, where you can run the Unity engine on this scene
* The Project View, where you can view the project's directory tree and all its assets

Keep an eye out for the next short assignment, where we'll be building our first simple game!
