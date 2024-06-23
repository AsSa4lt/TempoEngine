# Heat Transfer Calculations

## Overview
This project involves detailed calculations of heat transfer through radiation. The objects involved in this simulation are assumed to have unique properties such as names for identification and uniform dimensions (width of 1 mm and each cell measuring 1 mm by 1 mm).

## Multi-threaded Calculation Strategy
- **Grouping**: Objects are divided into groups based on the number of processor threads available.
- **Parallel Processing**: Each group is processed in a separate thread to optimize performance and computation speed.

## Radiation Heat Transfer

### Emission Calculation
The rate of heat transfer by radiation emitted from an object is governed by the Stefan-Boltzmann law of radiation:

\[ Q/t = \sigma \cdot e \cdot A \cdot T^4 \]

Where:
- \( \sigma = 5.67 \times 10^{-8} \) J/s · m² · K⁴ (Stefan-Boltzmann constant),
- \( A \) is the surface area of the object,
- \( T \) is the absolute temperature of the object in kelvin,
- \( e \) is the emissivity of the material.

### Absorption Calculation
The net rate of heat transfer by radiation (considering both absorption and emission) for an object at temperature \( T_1 \) surrounded by an environment at temperature \( T_2 \) is given by:

\[ Q_{\text{net}}/t = \sigma \cdot e \cdot (T_2^4 - T_1^4) \]

This calculation assumes that the surroundings have a uniform temperature and that the object's emissivity is the only factor affecting its radiation interaction.

### Radiative Heat Exchange Between Two Objects
For two objects exchanging heat through radiation, assuming no intervening medium absorbs the radiation, the heat transfer can be modeled using the following equation for gray bodies:

\[ Q = \sigma \cdot A \cdot \frac{e_1 \cdot e_2}{e_1 + e_2 - e_1 \cdot e_2} \cdot (T_2^4 - T_1^4) \]

Where:
- \( e_1 \) and \( e_2 \) are the emissivities of the two objects,
- \( T_1 \) and \( T_2 \) are their respective temperatures.

## Temperature Representation
- Temperatures are represented using color codes:
  - **Violet**: 0 degrees (as a baseline temperature).
  - **Red**: Represents temperatures of 200 degrees and above.

These calculations form the core of the thermal analysis in this project, aiming to provide accurate predictions of heat interactions under various conditions.
