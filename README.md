### NOTE: *This project was copy pasted into a new repository, since some members of the group did not want it to be public*

# JanC
 
This was a university project about developing a programming language. Our group chose to develop a higher level C language, that was intended to run on an Arduino and focus on concurrency. The compiler uses ANTLR4 as a parser and the compiler in itself is written in C#. The entire purpose of the language, is to give some low level concurrency structures, that will make it easier for someone to implement concurrent projects for an Arduino. 
An example of where this could be useful, could be that the Arduino needs to log data from a sensor withing x interval as well as send the data to a server every y interval.

<p align="center">
  <img width=600 src="https://user-images.githubusercontent.com/22596587/126674598-308aa471-0126-44c3-8a21-4f3e0085ea8d.png">
  <img width=350 src="https://user-images.githubusercontent.com/22596587/126674800-08792164-8d2e-4c82-a4b7-15d5c9a1521a.png">
</p>


The solution is split into a lot of smaller projects, where each of them have a respective testing project.

Also included is a VSIX installer for syntax highlighting for JanC files.
