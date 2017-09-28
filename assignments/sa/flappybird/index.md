---
layout: page
title: Short Assignment 2
published: true
---

# Flappy Bird

![](https://media.giphy.com/media/xrDdo5kuHzwxG/giphy.gif)

Welcome back! In this assignment, we'll be designing our first full-fledged game, a clone of the cult classic, FlappyBird.

To start, make a new Unity Project (in 2D) and open it.

## Getting Started
### Setting Up Our Assets Folder
One thing we didn't quite cover in the last short assignment is best practices for organizing your Assets folder (since we didn't really have that many assets). As the projects get bigger, it helps a lot to keep everything nicely organized. Here's what my directory looked like at the end of the project:

![](img/assets.png)

I recommend doing something similar to keep all your assets in easily searchable spots. Note that "\_Scenes" has an underscore in it by convention, so that our scenes are always ordered first.

### Importing Assets
Let's start by downloading some assets from the Unity Asset Store. Navigate to the Asset Store tab (probably next to your Scene and Game View tabs). If it doesn't work on the first go, try flipping to another tab and back again.

Once there, search for "2D Sprites Pack". It should be the first result, free, and made by Unity Technologies. Hit the download button, and when the window pops up on your screen to import the assets, select only the eight we'll need for the assignment:

- BirdEnemyDeathSprite
- BirdEnemyIdleSprite
- ColumnShortSprite
- GrassSprite
- RestartIdleSprite
- SkyTileSprite
- StartIdleSprite

Hit import. Unity should automatically place these sprites in the `Sprites` folder that you created in the last step.

### A Few Notes On 2D
This is the first time we're working in 2D, but don't worry! This should in theory simplify things by removing a whole dimension, and it does.

The only thing to really watch out for is that when creating any physics components, we want to make sure we select them from the "Physics 2D", as the normal 3D versions might cause some problems that we don't want.

Also note that our camera works a little differently here. If you look in the camera's inspector, under the Camera component, you can see that the projection field is labeled "Orthographic". In the gif below, you can sort of see the difference. Essentially, an orthographic camera sees objects at a seemingly equal distance regardless of their distance from the camera itself (i.e. a box 300 pixels away will look the same size as a box 3 million pixels away.) In 3D we are used to the perspective camera, which is also shown in the gif, and makes things looks smaller as they get further and further away, as they do in our 3D real world.

![](img/camera-explanation.gif)

## Background
Our background is going to have two parts, the sky and the ground. From the `Sprites` folder, drag the GrassSprite so mostly the grass is in the camera's field of view, and the SkyTileSprite so it covers the rest of the space. You can drag these out quite a bit, and then you can duplicate them with `command+d` on Mac or `control+d` on Windows and drag them out further. Mine looked something like this:

![](img/background.png)

## The Player
FlappyBird is nothing if we don't have a player to lead us on our epic expedition through the never-ending obstacles. The provided bird isn't quite as heroic as our beloved 8-bit hero...

![](img/flappy-bird-icon.png)

...but we do get something close enough. Drag the `BirdEnemyIdleSprite` into the scene, near the top left corner. Change its name to "Player" in the hierarchy view.

He's facing the wrong way! Easy fix. In the inspector, under the "Sprite Renderer" component, check the box under "flip" next to "X".

The player is going to need to handle some basic physics too. What do we need to add? If you guessed `Rigidbody`, you're close. Click "Add Component", and then click "Physics 2D" > "Rigidbody 2D".

Press the play button to test.

Our poor bird never learned to fly! Let's fix that.

Create a new script called "PlayerController". Open it up for editing. Paste in the following code:

```csharp
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour {

	public int jumpForce;

	private Rigidbody2D rb2d;

	void Start () {
		rb2d = gameObject.GetComponent<Rigidbody2D> ();
	}

	void Update () {
		if (Input.GetKeyDown (KeyCode.Space)) {
			rb2d.AddForce(new Vector2(0, jumpForce * 100));
		}
	}
}
```

What's going on here? We create a public int called jumpForce, which is some value that we'll set in the editor. We also have a Rigidbody2D component that we initialize to point to the Rigidbody2D on the player object. (Recall that this script is attached to the Player GameObject. The `gameObject.getComponent` call will therefore get the component of the specified type that is also attached to this player GameObject.)

In `Update()`, we are just listening for any time the user hits the spacebar. When this happens, we add an upward force of 100 times our jumpForce.

Save the script and return to the editor. When you get back there, we'll have to change two values: "Jump Force" and "Gravity Scale". The former is in our script and determines the force with which we jump up. The latter is in the Rigidbody2D and determines how much gravity will pull our player downwards.

Play around with these values a bit and test until you find something you like. I used Gravity Scale 2 and Jump Force 6, but you may find something you prefer. Once you're satisfied, you should be able to hop in place after hitting the play button.

![](img/bouncing.gif)

### Clamping Velocity
You might notice that if you hit the spacebar multiple times in close succession, the force we are applying makes the bird go up way faster than it should. This is because we are applying essentially a constant force over time. Recall that F=ma. Since force and mass are remaining constant, acceleration also remains constant, and a constant acceleration results in an exponentially increasing velocity.

We like realistic physics, but they have their place, and games like flappy bird ain't it. We want to make sure the player's velocity stays consistently in a specific range, so we want to `clamp` any value to that range. In your `PlayerController.cs` script, add in a public int `maxVelocity`, and add the following lines to your `Update()` function:

```csharp
rb2d.velocity = new Vector2(
  rb2d.velocity.x,
  Mathf.Clamp (rb2d.velocity.y, -maxVelocity, maxVelocity)
);
```

This sets the velocity of the player in each frame to stay the same in the x direction but clamps the y direction between the values of -maxVelocity and maxVelocity.

In the editor, play around with the values again until you find something you like, and press play to test.

### Moving Right
In the final game, there will be obstacles to avoid, and we'll want to move right so we can get to them. What's the best way to get this right-scroll behavior? The easiest is to just initialize the player character to have a constant rightbound velocity. (Recall that x and y velocities are independent of each other--if you have two bullets in a vacuum, you can drop one from ten feet up and fire another straight out of a gun from ten feet up and they'll hit the ground at the same time.)

In `PlayerController.cs`, add a public int called `scrollSpeed` and then add the following to the start function:

```csharp
rb2d.velocity = new Vector2 (scrollSpeed, 0);
```

In the editor, set the scrollSpeed to 4. Our bird should now be moving, so the next step is to get the camera to follow it.

## CameraController
There are a few ways that we can get the camera to follow our player. The quick-and-dirty way is to make the camera a child of the Player object. Since all children are spaced relative to their parents, the camera will move along with the player. Try it out! Drag the Main Camera in the hierarchy over the Player to make it a child of Player. Hit play!

Well, it's close. The camera is following the player all the time, though, and we want it to be fixed in the y-direction. We can't do that with parent-child relationships, but we can do it with a script.

Move the Main Camera back to its own position in the hierarchy. Then, with it selected, add a new script component to it in the inspector, and call it
`CameraController`. Open the script for editing.

Here's the code for the camera controller:

```csharp
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour {

    public GameObject player;

    private float offset;

	// Use this for initialization
	void Start () {
        offset = transform.position.x - player.transform.position.x;
	}

	// Update is called once per frame
	void Update () {
        transform.position = new Vector3(player.transform.position.x + offset, transform.position.y, transform.position.z);
	}
}
```

This gets the difference of the x coordinates between the player and camera on startup, and then in each frame updates the camera's transform position to move that offset in the x direction. This makes the camera appear fixed relative to the player, but only along the x-axis, which is just what we wanted.

Back in the editor, we're about to see one of the many cool things Unity can do. Recall that we set a variable `public GameObject player` in our script. Like any other public variable, we can set it in our editor. It wants a GameObject, and specifically we want it to point to our `Player` GameObject. With the Main Camera selected in the Inspector, simply drag the Player from the Hierarchy to the space next to Player in the Camera Controller script component. That was a lot of words. Here's a gif:

![](img/drag.gif)

Couldn't be easier! Hit play and make sure it's working.

## Colliders
In any video game, there needs to be some notion of physical intersections. When a ball hits a player's foot, for example, the ball should respond to the kick by flying away from the foot. Or, when a daring plumber head-butts an inquisitive brick, a coin should emerge from the top. Unity's method of dealing with these collisions is the aptly named Collider.

Colliders are just components of any GameObject, with the special property that if two colliders meet, they should not be allowed to intersect into each other. They'll need to be on any objects that we want this property for, so if we want the bird to stop dead when he hits the ground, then both the player and the ground will need colliders.

### Capsule Collider
From the Player object's inspector, select Add Component > Physics 2D > Capsule Collider 2D. With the mouse over the scene view, press the F key to focus on the player object.

We want the collider to fit as closely as possible to the outline of the player. In the inspector, under the collider component, change the Direction dropdown to "Horizontal". At the top of that panel, click the button next to "Edit Collider". Then drag the top and bottom of the collider until they wrap nicely around the bird, like so:

![](img/capsule_collider.png)

### Box Collider
Now, do the same thing for the ground! Add a 2D box collider component to the grass sprite just like we did for the player, and wrap it so it fits nicely. The bottom doesn't really matter as much as the top.

Press the play button to test.

Hah, stupid bird. He just falls and gets stuck on the bottom. Don't worry, we'll fix right just now:

### Triggers
![](https://media.giphy.com/media/vk7VesvyZEwuI/giphy.gif)

There's a special behavior of colliders that we want to utilize here: a trigger. Triggers are used when we don't want the standard collider behavior of bouncing off each other. We can set the ground to be a trigger so that when the player collides with it, it *triggers* some function like, say, a gameOver boolean switches to true.

To set this up, just check the box in the collider component labeled "Is Trigger".

### Tags
One other thing, before we get back to code. The trigger will need to be referenced somehow. The easiest way to do this is to add a tag to all trigger colliders that we consider obstacles. The ground is one, and the pillars will be the others. A tag is just a special property of a GameObject used for identification.

For now, in the ground object's inspector, at the very top, click the dropdown labeled "Tag", and select "Add New". At this new menu, click the "+" icon, and enter "Obstacle".

NOTE: You just created the tag, but you still have to apply it. Go back to the ground object's inspector and select "Obstacle" from the Tag dropdown.

Okay, now we're ready! Back to the codes.

## Game Controller
So far, we've basically built a jumping bird. It could have its place, but it might be cool to actually make a game out of this. To keep track of all of the high-level game stuff, it would be good to keep it all in one script. The GameController will keep track of scoring, whether the game is currently running or if the player died, the current sprites being rendered, and eventually will even reload the scene when a player clicks the restart button.

We'll go through this one step at a time.

First, create a new Empty GameObject in the scene. To do that, above the hierarchy view, select "Create" > "Create Empty". Rename it "GameController". Then, add a new script component to it, and also call the script "GameController". Open the script for editing.

### Triggering Game Over
Paste the following two lines of code into your `GameController` script:

```csharp
public GameObject player;
public static bool gameOver;
```

You know the drill. Save the script and attach the Player GameObject to the player variable in the editor.

Note that we made the gameOver boolean `static`. We want only one instance of it per game, and we want it easily accessible to other classes. Why? The colliders that isn't a trigger is attached to the player. That means when it meets with a trigger collider, the functionality for this is dealt with by the Player GameObject. Open the PlayerController script for editing, and add in the following function:

```csharp
void OnTriggerEnter2D(Collider2D other) {
	if (other.CompareTag("Obstacle")) {
		Debug.Log ("Died");
		GameController.gameOver = true;
	}
}
```

This is a special function that is called when the GameObject that this script is attached to intersects with a trigger collider. The trigger collider is passed to the function as `other`. We then check `other`'s tag, and if it is an obstacle, then we will set the gameOver boolean to true.

Note the use of `Debug.Log`. This is Unity's print function, and it sends output straight to Unity's console. I'd recommend getting to know it pretty well, because it might save you quite a few times down the road.

If you hit play, you should print "Died" to the console when your player touches the ground!

### Changing Sprites on Trigger
Sprites are actually pretty easy to deal with in Unity. We have two Sprites saved in our sprite folder that we want to make use of: the BirdEnemyIdleSprite (default) and BirdEnemyDeathSprite. Add some more variables to your `GameController`:

```csharp
public Sprite normalSprite;
public Sprite deadSprite;

private SpriteRenderer playerSprite;
```

In the inspector, drag the BirdEnemyIdleSprite onto normalSprite, and the BirdEnemyDeathSprite onto deadSprite.

We already have a reference to the Player, so it's easy to get it's SpriteRenderer component. Put this in your `Start()` function:

```csharp
void Start () {
	playerSprite = player.GetComponent<SpriteRenderer> ();
}
```

As you might guess, this just returns the SpriteRenderer component of the Player object.

Finally, replace the `Update()` function with these lines:

```csharp
void LateUpdate() {
	if (gameOver) {
		playerSprite.sprite = deadSprite;
	}
}
```

`LateUpdate()` runs once per frame, just like `Update()`. It just runs a little after `Update()` in the life cycle. So, we're guaranteed that all `Update()` methods have been called at the time of this method's execution. This is useful if, for example, we're pressing the Spacebar to make our player jump in the Update function of another script. There won't be any interference with the trigger collider, because the functions are called at different times!

So what does the code do? It checks if the game is over, and if so, it replaces the character's idle sprite with a dead character sprite. Pretty simple! Your game should obey this if you test with the play button.

It might also be good to disable player control once the bird dies. To do that, just change the `if` statement in the `Update()` function of your `PlayerController` script:

```csharp
if (!GameController.gameOver && Input.GetKeyDown (KeyCode.Space)) {
	// other code
}
```

That way, the Spacebar will only be counted as long as gameOver is false.
