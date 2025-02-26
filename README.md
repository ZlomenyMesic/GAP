# Generative Art Program

a platform for creating generative art

## Moddability
1. create new library project
2. import the [GapCore](https://www.nuget.org/packages/GapCore) core library (see
    [Example Mod](https://github.com/KryKomDev/ExampleGapMod) repo for more info)
3. implement `IMod`
4. create new generator, that implements `IImageGenerator<>` or `IImageTransformer<>`
5. compile to .dll
6. put the library to the mod directory
7. enjoy 👍👍👍
