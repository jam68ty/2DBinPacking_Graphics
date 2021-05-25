# 2DBinPacking_Graphics

## Using C# Graphics to implement and Perform BinPacking Process

### Open `2DBinPacking_Graphics.sln` with Visual Studio
### Run 
### Click `Start` to cut a rectangle
### Click `Random Packing` to see how will the pieces place

> You Can control these variables to show different situation
> public static int W = 500;  (Big rectangle's width)
> public static int H = 500;  (Big rectangle's height)
> public static int CuttingTimes = 3;  (How much times you want to cut)

## This alogrithm of cutting a complete rectangle
1. Random direction and coordinate to cut the rectangel into two pieces.
2. Do step 1. to evey rectangular pieces

## The alogrithm of placing pieces
I was used "Bottom-Left" to implement.
