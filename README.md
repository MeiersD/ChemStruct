# Chemstruct
ChemStruct is a project I made for my Extended Reality (XR) Class at Bucknell University.

Project Demo:
[![ChemStruct demo](https://img.youtube.com/vi/7Rspczc9kWk/maxresdefault.jpg)](https://www.youtube.com/watch?v=7Rspczc9kWk)

## Overview
This is a sandbox game where you are assigned molecule names, and must build the structure in VR. The more structures you build, the more points you get. Molecules include 16 amino acids, ethane through nonane, formate-butyrate, several small molecules like formaldehyde, O2, and nitrous oxide. 

### How to play
Your headset will have a quest bar in a static location. This quest bar will have the name of the molecule you must build. Once you build the molecule, your score will increase by 1, and a new molecule will appear in the quest bar. Black atoms are carbon, blue atoms are nitrogen, and red atoms are oxygen. No hydrogen is needed to build a structure for the sake of simplicity. In addition, the validation is not stereoselective, meaning that you can build either L or D amino acids and get the structure correct.

ChemStruct was built with the XR Hands package, meaning that to play, you can interact with atoms by pinching them either once your rays have alligned with an atom, or by pinching the atom directly. From there, drag the atom until it overlays on a socket, attatched to another atom. You will know the atom you are holding has overlayed with a socket when a dark blue sphere pops up on screen, signifying the socket location.

Similarly, to toggle a bond between single and double bond, you can either pinch once your ray is targeting a bond, or by pinching the bond directly. Bonds are not provided among your interactable objects, as they are automatically built upon being socketed.

### Algorithm Overview and Bugs
There are a few limitations with the validation algorithm. As mentioned previously, the amino acids can be either D or L and you could get the structure correct. The structure of each atom also is not always regioselective; the validation algorithm has no way of detecting the difference between isoleucine and leucine, meaning you can build one rather than the other and still get the structure correct. This is because each molecule's structure is stored in a json whose value is a simplified form of the MOL notation (not SMILES) where each molecule is defined by a sum total of its bonds and atoms, rather than the atom coordinates in 3d space.

SMILES would be more accurate, but it turns out it is quite difficult to convert a collection of bonds and atoms into SMILES notation for comparing.

Furthermore, Oxygen is implemented with SP2 orbital hybridization and has 2 sockets despite all structures only requiring at most 1 or less socket on oxygen (there are no ethers in the list of molecules). Carbon and nitrogen have sockets preset to SP3 hybridization, meaning that carboxylates and imines (if there were any imines) are technically be structurally inaccurate, as they ought to have SP2 bond angles. This is why Tyrosine, Phenylalanine, Tryptophan, and Histadine are excluded from the molecule list. Lastly, both Carbon and nitrogen only have 3 sockets, meaning that you cannot make 4 coordinate carbon, which is not required for any of the molecules on the list.

## Requirements
<ins>Tested Hardware</ins>
Meta Quest 3 Headset

<ins>Software</ins>
Unity (On Meta Quest 3)
Unity (on laptop)

## How to run ChemStruct on your machine:
The project can be run through the following way:
1) Clone this repository with
```git clone https://github.com/MeiersD/ChemStruct.git```
2) Open Unity Hub and create a project from Add -> Add project from disk and select this repository.
3) Open the project, connect your XR Headset with a USB-C USB-C connector to your laptop, and open Build Settings, and select your headset from buildsettings. Make sure that the required packages are installed, namely
- Newtonsoft.js
- XR Interaction Toolkit
- XR Hands
- OpenXR Plugin
- TextMeshPro

4) Select Build and Run
5) Once finished building, the project should load onto your headset automatically.




