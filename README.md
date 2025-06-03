# COMP3180 Final Project

<h2>Portfolio Piece 2 Demonstration</h2>
https://github.com/user-attachments/assets/c2ded82f-e203-49e6-bf5a-242eef7c43d9
Also available at Documentation\PortfolioPieceVideo.mp4

## Week 1 - Selecting a Project

```
Select a project area you want to explore.
Physics

A 1-2 sentence description of your project idea.
A 3D physics based platformer focused on designing controls and equipment that contribute to a 'manic' but precise gameplay experience. This might include real-time physics manipulation, frictionless velocity driven platforming, dynamic grappling or rhythm based platforming.

Please advise of any special hardware you would like to work with (including any of your own equipment you intend to bring):
N/A
```
## Week 2 - Narrowing Focus & Tool Familiarisation

You need to have accepted this assignment repo and made a post here identifying a resource relevant to your topic.

## Week 3 - Research (Reading & Prototyping)

## Week 4 - Research (Reading & Prototyping)
Game References:
Neon White, Pistol Whip, Beat Saber, Grapple Tournament, Cyber Hook

Grappling Hook:
Wire Physics, joints on collisions. 

Physics Hand Collisions:

Shooting and projectile mechanics:

Projectile Physics: 

Visually-Induced Motion Sickness (VIMS): VIMS occurs when there is lag between the actual experience of motion and the lag of the vr display itself.
*Real life FOV, Movement indicators, smoothed edges, 

https://www.frontiersin.org/journals/virtual-reality/articles/10.3389/frvir.2020.582108/full
https://arxiv.org/pdf/2205.07041
https://www.youtube.com/watch?v=LsAzSq749Iw
https://www.youtube.com/watch?v=DR8QVy2LIj4
https://web.stanford.edu/class/ee267/Spring2016/report_becker_ngo.pdf

Anchors: Stationary objects  people can focus on to provide a sense of external consistancy


Rhythm Gameify (Maybe)


## Week 5 - Presentations (& research/rescoping)

## Week 6 - Presentations (& research/rescoping)

## Week 7 - Research Report Finalisation

## Week 8 - Project Development
Completed single arm grappling scene.
  Includes joystick movement, acceleration and counterinput deceleration.
  Added grappling logic with predictive aiming.
Experimented with post processing effects, visual anchoring and observed whether they caused me motion sickness.
Tested scenes with glowing surfaces, rows of objects to aid in perceiving depth, and placing too many objects to see if visual overstimulation would take place and cause eye strain. 

## Mid-semster break 1 - Continued R&D
<b><h3>DONE:</b></h3> 
- Look into 27 slicing (9 slicing for 3d objects) https://assetstore.unity.com/packages/tools/utilities/27-slicer-204453
  - Need it for designing scalable 3d objects (DONE)
  - IDEA: Independant left and right eye 3d objects. (too disorienting and extremely motion sickness inducing)
- Movement Based on camera position (DONE)
- Design Player Acceleration (DONE)
  - Movement state detection (DONE)
    - Also need a falling state (DONE)
    - ![alt text](README_Images\image-1.png)
  - Deceleration on counter-input (DONE)
  - Detection for which hand is grappling (DONE)
  - Add custom gravity (remove inbuilt physics gravity) (DONE)
    - More gravity based vertical speed (DONE)
  - Deceleration when no input is read (DONE)
  - Create physics objects for floor and wall layers (DONE) 
  - Monitor controller velocity for 'tug' input (DONE)
  - Tune the grapple so more in line with reference game (DONE)
  - Abstract left and right controllers (DONE)
- Add Jump Input (DONE)
  - Maybe right hand analog stick can control jump height. Like a gliding wingsuit (DONE)
  - Double jump (lighter than original jump) (DONE)
- Debugging & Bug Fixing
  - FPS Counter (DONE)
  - Line preview disabled upon dropping grappling gun (DONE)
  - Grappling prioritizes front collision instead of back (DONE)
  - No-input and counter-input deceleration should only happen onGround, not midair (DONE)
  - Assign XRGrabInteractable events in prefab instantiation (no idea how to do this) (Doesn't really matter but i'd like to know how to do this. Maybe ask.) 
- UI
  - Speed counter (DONE)
  - Green: Slow speed (DONE)
  - Yellow: Medium speed (DONE)
  - Red: Fast speed (DONE)
  - ![alt text](README_Images\image.png)
- Sound (DONE)
  - SFX Manager for inputting sfx on events (DONE)
  - BGM Manager for inputting looping bgm on events (DONE)
  - Increase/Decrease volume based on player velocity (DONE)
- Sound FX
  - Find sounds for SFX Library (DONE)
    - Gameplay SFX: (DONE)
      - Grappling (DONE)
        - PickedUpGrapple (DONE)
        - DroppedGrapple (DONE)
        - Grapple successful (DONE)
        - Ungrapple (DONE)
        - Regular grapple missed (DONE)
        - Pull input successful (DONE)
        - Both grapple's successfully latched (DONE)
        - Double pull performed (DONE)
      - Running - WIND (based on rb.velocity) (DONE)
      - Footsteps if OnGround (louder based on velocity) (DONE)
      - Counter-input/no input deceleration WIND (Loud wind -> no wind) (DONE)
      - Vertical Velocity (DONE)
        - Jump (DONE)
        - Double Jump (Maybe a higher db, higher pitch jump?) (DONE)
        - Falling (soft, whistling sound) WIND (higher pitch) (DONE)
          - Landed onGround (DONE)
        - Rising (deeper, rushing whoosh) WIND (lower pitch) (DONE)
          - Apex of y-velocity (none) WIND (no wind) (DONE)
    - UI SFX (DONE)
      - Level Complete (DONE)
      - Star time elapsed (DONE)
        - 3 star (DONE)
          - Timer counting down to '3 star elapsed' (DONE)
        - 2 star (DONE)
          - Timer counting down to '2 star elapsed' (DONE)
  - Input sounds into SFX Library (DONE)
  - Hook up the Gameplay SFX (DONE)
- Haptic Events (DONE)
  - Grappling, tug and dual tug (DONE)
- Overlay FX (DONE)
  - Implement events that trigger vfx (or don't. Maybe just 1 speed overlay is enough) (Done)
  - Modify intensity based on player velocity (DONE)
  - Running (Done)
  - Grappling and Dual Grappling (Done)
  - Falling, Going Up (which needs a new state) (Done)
  - Vignette on sharp turns and high velocity (Done)
  - Speed lines on high velocity (DONE)
  - Implement events that trigger vfx (DONE)
  - Implement FOV changes on high velocity (No idea how to do this since VR FOV forces a consistant 100fov)
  - Screen shake on high velocity (DONE)
- BGM 
  - 1 looped track should be enough for now (DONE)
- Speedrun Timer (DONE)
  - 1. Initially stop all player movement (DONE)
  - 2. Display a 3,2,1 UI animation (DONE)
  - 3. Start a timer and display it on the left hand (DONE)
- Star Timer Manager (DONE)
  - Should hold references of the 3 and 2 star times for each level (DONE)
- Finish Line Transition to 'Finish Level UI' (DONE)
- UI (DONE)
  - Finish Level UI (should talk to the star timer manager and the timer) (DONE)
    - Design UI (DONE)
      - Should contain: (DONE)
        - Main Menu (DONE)
        - Restart (DONE) 
        - Final time (DONE)
        - Star count (DONE)
          - Based on timer script (DONE)

<b><h3>IMPORTANT TODO: Completion of 1 'Complete' level </b></h3>

- Gameplay Features
  - Wall Running
    - Ideas
      - ~~Maybe pressing the a button and close to a wall it stops y velocity~~
      - ~~Maybe when the head is tilted in the opposite direction of the face you're wall running on it counts as a wallrun (Note: Rotation tracking happens on the main camera and not the xr rig)~~
      - ~~Maybe hand proximity to wall (most intuitive i think)~~
      - ~~Velocity based trigger (least 'realistic' but good for fast paced gameplay)~~
      - Ideally I don't want the player to have to change fingers on different buttons. Potentially on the right controller joystick, inputting 'left' or 'right' will latch you onto a wall. (going with this)
        - right stick up is jump/glide
        - right stick down is slower fall 
        - right stick left is unused
          - right stick left latches onto walls on the left
        - right stick right is unused
          - right stick right latches onto walls on the right



<b><h3>TODO:</b></h3> 

- Fix restart level
- Add sfx for restart
- Game Manager
  - Holds the current Level or scene
  - Handle Scene transitions

- UI
  - Menu Scene
    - Create Menu scene
    - Design Menu UI
      - Should contain:
        - Level Select -> (hook up to Level selection UI)
        - Settings (do later)
        - Quit
      - Create UI (world space)
      - Add sfx 
    - Level Selection UI
      - Design UI
        - Should contain:
          - Level Panels
          - Return to Main Menu button
  - Pause UI
    - Design UI
      - Should contain:
        - Main Menu
        - Restart
        - CurrentTime
- Sound FX
  - UI SFX
    - Hovered over button (maybe just a SendHapticImpulse() is enough)
    - Clicked button 
      - Level Selected
      - Clicked UI Button
    - Restart

  

<br /><br />
<h3> How to make seamless sfx loops: </h3>

1. Get an audio clip  

2. Cut the clip in half.  

3. Place the first half in front of the second half  

4. Crossfade between the middle  

5. Export as .wav or .ogg (.mp3 puts junk data at the front and end making looping impossible)



## Mid-semster break 1 - Continued R&D
<b><h3>DONE:</b></h3> 
- Gameplay Features (DONE)
  - Should be faster on ground than in air (DONE)
  - Coyote time on player movement (DONE)
  - Design left/right midair player movement control (DONE)
    - More force added when input vector opposes the current velocity vector (DONE)
  - Design better tug location on tug (currently it propels you towards hit point) (DONE)
    - Blend 'current velocity' vector with 'player to hit point' vector
    - Dynamically assign how much 'influence' the current velocity has on the tug direction
      - Dot product the two normalized vectors to find the 'similarity' between the two directions. Use this to determine how much 'influence' the current velocity has on the 'player to hit point' vector
        - https://www.youtube.com/watch?v=2PrSUK1VrKA
    - ![](README_Images\image3.png)
  - Wall Running (DONE)
    - Ideas
      - ~~Maybe pressing the a button and close to a wall it stops y velocity~~
      - ~~Maybe when the head is tilted in the opposite direction of the face you're wall running on it counts as a wallrun (Note: Rotation tracking happens on the main camera and not the xr rig)~~
      - ~~Maybe hand proximity to wall (most intuitive i think)~~
      - ~~Velocity based trigger (least 'realistic' but good for fast paced gameplay)~~
      - Ideally I don't want the player to have to change fingers on different buttons. Potentially on the right controller joystick, inputting 'left' or 'right' will latch you onto a wall. (DONE)
        - right stick up is jump/glide
        - right stick down is slower fall 
        - right stick left is unused
          - right stick left latches onto walls on the left (DONE)
        - right stick right is unused
          - right stick right latches onto walls on the right (DONE)
      - ![](README_Images\image4.png)
- Restart (DONE)
  - Have ray interactor work with restart button (DONE)
  - Fix DDOL persistance (DONE)
  - Add sfx for restart (DONE)
- SFX (DONE)
  - Restart Button (DONE)
- Level Design for prototype (Should showcase all main mechanics) (DONE)
  - Length: around 30 seconds. (DONE)
    - 30 seconds for 3 stars (DONE)
    - 35 seconds for 2 stars (DONE)
    - Completion for 1 star  (DONE)
  - Section 1: Running (DONE)
  - Section 2: Platforming (DONE)
    - Should in some form include double jumping (DONE)
  - Section 3: Wallrunning (DONE)
  - Section 4: Solo grapling (DONE)
  - Section 5: Dual grappling  (DONE)
  - Section 6: Finish Line (DONE)
- UI (DONE)
  - Pause Menu (DONE)
    - Pause time (DONE)
    - Pause player interaction (DONE)
    - Pause player locomotion (DONE)
    - Enable UI interaction (DONE)
    - ![alt text](README_Images\image2.png)


<b><h3>IMPORTANT TODO:</b></h3>

- Documentation
  - Prepare VIMS questionaire and playtesting procedure

<b><h3>TODO:</b></h3> 

- Game Manager
  - Holds the current Level or scene
  - Handle Scene transitions

- UI
  - Menu Scene
    - Create Menu scene
    - Design Menu UI
      - Should contain:
        - Level Select -> (hook up to Level selection UI)
        - Settings (do later)
        - Quit
      - Create UI (world space)
      - Add sfx 
    - Level Selection UI
      - Design UI
        - Should contain:
          - Level Panels
          - Return to Main Menu button
  - Pause UI
    - Design UI
      - Should contain:
        - Main Menu
        - Restart
        - CurrentTime
- Sound FX
  - UI SFX
    - Hovered over button (maybe just a SendHapticImpulse() is enough)
    - Clicked button 
      - Level Selected
      - Clicked UI Button


## Week 9 - Project Development

<b><h3>DONE:</b></h3> 

- Documentation
  - Prepare VIMS questionaire and playtesting procedure (DONE)
- Game Manager (DONE)
  - Holds the current Level or scene (DONE)
  - Handle Scene transitions (DONE)

- UI (DONE)
  - Menu Scene (DONE)
    - Create Menu scene (DONE)
    - Design Menu UI (DONE)
      - Should contain:
        - Level Select -> (hook up to Level selection UI) (DONE)
        - Settings (do later) (DONE)
        - Quit (DONE)
      - Create UI (world space) (DONE)
      - Add sfx  (DONE)
    - Level Selection UI (DONE)
      - Design UI (DONE)
        - Should contain:
          - Level Panels (DONE)
          - Return to Main Menu button (DONE)
  - Pause UI (DONE)
    - Design UI (DONE)
      - Should contain: (DONE)
        - Main Menu (DONE)
        - Restart (DONE)
        - CurrentTime (DONE)

- UI
  - VIMS Testing Options Design (DONE)
    - Yes
      - Vignetting (DONE)
      - Screen shake and speed lines (DONE)
      - Preview lines (DONE)
      - Haptic feedback scaling (DONE)
    - Yes, but not implemented
      - Rb movement type (I think this will be the biggest factor) (DONE)
      - Anti-aliasing (DONE)
    - No
      - Post processing
      - Smaller preview line spherecasts
  - Fully hook up UI to settings (DONE)
  - Persist settings across scenes (SettingsManager instance) (DONE)
  - Create and hook up settingsData scriptable object to settings manager (DONE)
- Options/Settings Manager
  - Hook up UI Options menu to their in-game counterpart (DONE)
  - Initialize the references needed for the options menu (DONE)
    - Call that initialize method in the InitializeDDOL() (GameManager) (DONE)
- Options Menu Button
  - When pausing the game, instead of showing the next button, show an 'options' button and link that to the options prefab. (DONE)
  - Reset default settings button (DONE)
- Mechanics Tutorial Scene (DONE)
  - Acceleration System (DONE)
    - Forward movement (DONE)
    - Deceleration (DONE)
    - Faster on wall -> ground -> air (DONE)
  - Jumping (DONE)
    - Glide mechanics from holding up or down while in the air (DONE)
  - Wallrunning (DONE)
    - Up and down control on walls (DONE)
  - Grappling (DONE)
  - Tugging (DONE)
  - Dual Tugging (DONE)
  - Finish Line & Star times (DONE)
- Sound FX (Ray interactor ui on hover event sucks so not gonna do)
  - UI SFX
    - Hovered over button (maybe just a SendHapticImpulse() is enough)
    - Clicked button 
      - Level Selected
      - Clicked UI Button
- VIMS Testing Options Specifications (DONE)
  - Level Design related (DONE)
    - Anchored objects (DONE)
    - Peripheral objects (DONE)
    - Color palates (brightnes & saturation) (DONE)
    - Elevated levels (high in disorientation) (DONE)
    - Forward and backwards levels (high in disorientation) (DONE)
    - Seated and unseated levels (DONE)
    - Levels that contain sudden speed changes (extremely fast to slow and precise) (DONE)
- Level Specifications
  - 1 
  - Theme: Control Level
    - Level Design:
      - Seated
      - Anchored objects
      - Minimal peripheral objects,
      - Regular elevation
      - Minimal rotating
      - Forward movement
    - Settings: 
      - Default everything
    - Rationale: This level serves as the 'control' where all elements are designed to be as stable as possible. 'Default' settings are used as this is what I've playtested to 'feel' best.
  - 2
    - Theme: Anchored Visuals
    - Level Design:
      - Seated
      - Enclosed level
      - Muted colors
      - Many static objects within the environment (provides fixed visual references)
      - Similar to the control level with emphasis on anchoring
    - Settings: 
      - Vignetting: Off
      - Screen Shake: Off
      - Preview Lines: On
      - Haptic Feedback: On
      - Movement: Default
      - Anti-Aliasing: On
    - Rationale: This level focuses primarily on the effectiveness of anchored objects. This supposedly helps the player by visually grounding the player through various reference points. I'm aiming to evaluate the effectiveness of this design technique by designing this level around an enclosed level allowing the player to more easily deduce what their current position is relative to what's around them.
  - 3 
    - Theme: Sudden Speed Surges
    - Level Design:
      - Standing
      - Calm watercolors
      - Rapid shifts in acceleration and deceleration (sudden speed changes)
      - Forward movement (180deg range from starting position)
    - Settings: 
      - Default
    - Rationale: Sudden speed changes can disorient players and this level is meant to amplify that by incorporating these triggers with sharp turns or doors that they must slow down to turn into. 
  - 4
    - Theme: Up and down, back and forth
    - Level Design: 
      - Standing
      - Up and down movement
      - Back and forth movement
    - Settings: 
      - Movement: Slippery
      - Everything else: Default
    - Rationale: I suspect the player physically having to turn forward then backward then up then down, is highly disorienting. This level focuses on how much impact head rotations have on VIMS, particularly the 'disorientation' portion of the VRSQ (Virtual Reality Sickness Questionnaire).
  - 5 
    - Theme: Peripheral Perception
    - Level Design:
      - Standing
      - Many peripheral objects
      - Primary on ground
      - Constant fast velocity
      - High contrast, saturated colors
    - Settings: 
      - Vignetting: none
      - Screen Shake: none
      - Preview Lines: Off
      - Haptics: Off
      - Movement: Default
      - Anti-Aliasing: Off
    - Rationale: This level uses erratic elements (Level design and settings-wise) to heighten sensory input, focusing mainly on peripheral perception by placing a large amount of environmental peripheral objects. This level also tests the effectivenes of anti-aliasing (8x) and if this reduces sensory overload. 

<b><h3>IMPORTANT TODO:</b></h3>



  
- Revise questionnaires for level design related VIMS testing 

<b><h3>TODO:</b></h3> 


## Week 10 - Project Development
- Trying to do playtests (MQ headsets were broken)
## Week 11 - Project Evaluation
- Trying to do playtests (MQ headsets were broken)
## Week 12 - Project Evaluation
- Completed 12 playtests.
- Started Final Project Report
- Make minor modifications to playtesting procedure and instruments
  - Max 3 attemps
  - Linear scale now larger
## Week 13 - Project Report & Deliverables Finalisation

