// Engine.md (1)
// Temperature is stored in Kelvin, based on wawe spectrum of light
// Violet - 0 degrees, Red is everything 200 degrees and above

Objects
    - all objects have unique Name used for object identification
    - all objects has width of 1 mm
    - 1 cell is 1 mm long and 1 mm wide


Calculations:
All objects will be divided in group
Group count depends on count of my processor threads
Each group will be calculated in separate thread
One object with all other objects


1) Radiation  calculation
The rate of heat transfer by emitted radiation is determined by the Stefan-Boltzmann law of radiation: 
Q/t=σeAT4 Q t = σ e A T 4 , where σ = 5.67 × 10−8 J/s · m2 · K4 is the Stefan-Boltzmann constant, 
A is the surface area of the object, and T is its absolute temperature in kelvin.

Radiation absorbed by object is calculated by formula:
The net rate of heat transfer by radiation (absorption minus emission) is related to both the temperature of the object and the temperature of its surroundings. Assuming that an object with a temperature T1 is surrounded by an environment with uniform temperature T2, the net rate of heat transfer by radiation is

Qnet/t=σe(T^4_2−T^4_1) where e is the emissivity of the object alone. In other words, 
it does not matter whether the surroundings are white, gray, or black; 
the balance of radiation into and out of the object depends on how well it emits and absorbs radiation.