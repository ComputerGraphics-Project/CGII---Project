# Ålesund Smart City Simulator

General:
1 - Navigation in the application:
    The application starts with a home screen and we can select the simulation type from a drop-down menu that will lead to the page of that particular page.
    The "Exit Button" quits the application.
    
2 - Navigation within a scene / Camera Controller:
    Navigation within a scene is done through mouse control.
      - Pan: Mouse left click hold and drag.
      - Zoom in: Mouse scroll wheel rolling backward.
      - Zoom out: Mouse scroll wheel rolling forward.
      - Rotate / Yaw: Mouse center button/ scroll wheel press and drag.

Features:
A - Sky Exposure:
    The sky exposure page is opened by selecting the sky exposure option in the simulation type dropdown menu.
    The page shows parameters to adjust, and two different visualization options (top view heatmap, sky view, and bar graphs).
    The sky visibility of any point can be computed by the following steps:
    a) Set the parameters for simulation and click the "OK" button.
    b) Right Click on points you want to compute sky exposure.
    c) Drag the mouse to get the sky view at any point (without computation).
    d) Click "Reset" to remove changes.

B - Shadow Frequency and Heatmaps / Sun Simulation
    The shadow heatmap page is opened by selecting the shadow heatmap option in the simulation type dropdown menu.
    The shadow heatmap on any area can be computed by the following steps:
    a) Set the parameters and click the "OK" button.
    b) "Right-click" on any point to compute the shadow heatmap and aggregated heatmap on that region.
    c) Click on the "Save" button and select the file to save your data.
    d) Access the data by clicking on the "Load" button and selecting the data you want to load.
    e) Click on "Reset Camera" to bring the camera to its default transform.

C - Landmark Visibility:
    The landmark visibility page is opened by selecting the landmark visibility option in the simulation type dropdown menu.
    The landmark visibility of any building can be computed by the following steps:
    a) Set the parameters of the simulation.
    b) Click on the start simulation button.
    c) Click on the building to compute its landmark visibility.
    d) Wait for results. The results would be a color encoding of buildings that can be seen from the landmark or vice versa.
    e) Stop the function by clicking on the stop computation button. (This will help to avoid starting a new computation if you click on another building).
    f) Click refresh colors to bring the colors to their original state.
    g) Re-do the entire process in sequence to perform another computation.
    On the landmark visibility page, there are two buttons, three sliders, and three text fields for results.
    Sliders:
      The sliders in the landmark visibility page control the parameters of landmark visibility computation. The sliders are for controlling:
        a) The number of circles in a level
        b) The height level of computation
        c) The angle increment for the ray directions.
    Buttons:
        a) Start Computation / Stop Computation: This toggle button is used to start the functionality and the same button is converted to a stop button once the function is active and it can be used to stop the function.
        b) Refresh Colors: This button is used to refresh the colors of the building. It is used after stopping the functionality when we have to perform a new computation.
    Text Fields:
      The text fields show the results. The three text fields are:
        a) Total number of buildings.
        b) Actual height of selected buildings.
        c) The landmark visibility as a percentage of visible buildings compared to the total buildings.
    

D - Bus Service Availability:
    The bus service availability page is opened by selecting the bus service availability option in the simulation type dropdown menu.
    a) Set the parameters and click the "OK" button.
    b) Wait for results.
    c) Click on the "Save" button and select the file to save your data.
    d) Access the data by clicking on the "Load" button and selecting the data you want to load.
