# PNG to RBG565/BW Converter

This code was only quickly created to convert a png to the RBG565 for a stm controller.

For RBG565 also color should work.
It also now has a black white algorithmus which combines 8 pixels to one byte.
Means ff = 8 white pixels

algorithmus is:
- 565
- bw
- BW

# Usage
```
PNGtoBMPArray <png file> <arrayname and filename for header> <algorithmus>
```

```
PNGtoBMPARRAY -f <folder> <output filename> <algorithmus>
```
