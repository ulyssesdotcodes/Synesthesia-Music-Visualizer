Oculus Synesthesia
==========

Oculus Synesthesia is a music visualizer for the Oculus Rift. This GitHub repo contains the scripts for that visualizer.

Feel free to fork it and make it your own! If you add something that's useful across the board, please submit a pull request and I'll merge it in.

If you use this code elsewhere, please include my name somewhere visible.

What's that? You want to support the code author? Soon these scripts will be available on the Unity Asset store, and you can support me by buying them for a small price (probably ~$5-$10).

* * *

Basic Usage
------

1. Clone this repo into your Scripts folder

2. Create a "Visualization Controller" object and add an Audio Source and the "Audio Visualizer" script

3. Create another game object and put one of the receivers on there. These are the different types of receivers, note that some of them require various components
	- Basic Object Listener scales the object based on the volume
	- Lights Listener changes the color of the light based on spectrum data
	- Particle System Listener changes the speed and color of a particle system's emissions
	- Particle System Mover Listener mods a Particle System Listener by moving the base transform based on volume
	- Particle System Object Listener mods a Particle System Listener by scaling the base transform based on volume
	- Trail Renderer Listener moves the base transform along a path based on volume
	- Wave Listener is based on the awesome Jason Whitehouse's visualizer package (free in the Unity Asset Store)

4. Add the listeners to the array of listeners in the Audio Visualizer object's inspector window.

5. ???

6. Profit!

Implementation
-----

Oculus Synesthesia is designed to be an object oriented audio system based on audio events and audio listeners. The main controller takes advantage of the two methods Unity provides for interpreting an audio source - GetOutputData and GetSpectrumData. GetOutputData provides average decible level over a number of samples. GetSpectrumData provides the levels of all split into a number of buckets.

AudioVisualizer is the controller of the audio events. It gets the data from the Unity methods, calculates the root mean square value (rmsvalue) multiplied by the provided volume, and calculates the color. Then it passes all of this into each listener in its array of listeners using an AudioEvent object.

AudioListener is the base of all of the audio listeners. Extend this class to add a new listener. The functions in this class are designed to be overridden, check the comments in the code for descriptions on what each method is meant to do and how you might override it.

Thanks
-----
Thank you to Jason Whitehouse [Visualizer Package](http://forum.unity3d.com/threads/139776-Music-Visualizer), DimasTheDriver's excellent [blog post](http://www.41post.com/4776/programming/unity-making-a-simple-audio-visualization), and aldonaletto's [answer on Stack Overflow](http://answers.unity3d.com/questions/157940/getoutputdata-and-getspectrumdata-they-represent-t.html).


* * *

2014 Ulysses Popple http://upopple.com/