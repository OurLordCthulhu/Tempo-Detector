# Tempo-Detector
A simple, minimalist tempo tap detector for musicians and music enjoyers. This application allows a user to tap a tempo using either spacebar or a mouse click, it then calculates the tempo in beats per minute (BPM), and can play a metronome sound at that tempo.

Features:
* **Tap Tempo Calculation:** Precisely calculates BPM from user input. (up to 0.00)
* **Dual Input:** Supports tempo tapping via both keyboard (spacebar) and mouse click.
* **Metronome:** Plays a metronome click at the detected tempo.
* **Volume Control:** A slider allows the user to adjust the metronome's volume.
* **Start/Stop:** A button to start and stop the metronome sound.

Installing requiments:
* **Windows 10 or higher:** The application is built using the Windows Presentation Foundation (WPF) framework.
* **.NET 8 Runtime:** You will need the .NET 8 runtime installed to run the application.


Installing instructions: 
1.  **Clone the Repository:** Download or clone this project to your local machine.
2.  **Open in Visual Studio:** Open the solution file (.sln) in Visual Studio.
3.  **Install NAudio:** The project uses the NAudio library. In Visual Studio, go to **Tools -> NuGet Package Manager -> Manage NuGet Packages for Solution...** and search for and install `NAudio`.
4.  **Add Sound File:** (A basic sound should already come installed) If need add a short audio file (e.g., a `.wav` file of a metronome click) to your project and name it `click.wav`. In the file's properties, set **"Copy to Output Directory"** to **"Copy if newer"**. 
5.  **Run:** Build and run the project by pressing **F5** in Visual Studio.

