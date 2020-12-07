# VRCursorAttackExperiment
1. V1.0 notes
- Teleport: use your *left* controller to point at a teleporting area before a screen, and press the controller dpad to teleport there.
- Click a button: use your *right* controller to point at a button on the canvas and press the trigger button to click the button. 
- Malicious cursor manipulation: currently, only the canvas on the *left* of your spawn area is active.
- Button layouts: **Origin** is where you move your cursor to activate an attack (you should see a ball spawned and destroyed after you successfully activate the attack). **Bait** is where the user wants to click after clicking the **Origin** button. **Attack** (or attack target) is the button that the malicious developer wants to steal the clicks for. **Adjust** is where the user moves their cursor after their cursor reaches the **Attack** button in order for the displayed cursor to smooth back to the real cursor (however, you can simply move around randomly to reach the same effect).
- Display/hide real cursor: click this button to display/hide the real cursor.
- Note: the buttons above may vary depending on your VR system and your SteamVR input settings.