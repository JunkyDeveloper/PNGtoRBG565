﻿// See https://aka.ms/new-console-template for more information

using System.Drawing;

Console.WriteLine("Hello to the png to bmp converter");
if (args.Length < 1)
{
    Console.WriteLine("Filepath for Import missing!");
    return;
}

if (args.Length < 2)
{
    Console.WriteLine("array name missing!");
}

Bitmap image = (Bitmap)Image.FromFile(args[0]);
string array = "#include \"stm32l4xx_hal.h\" \n\n uint8_t " + args[1] + "[] = \n{\n";
var ipixel = image.GetPixel(0, 0);
if (ipixel.B > 0 && ipixel.R > 0 && ipixel.R > 0)
    array += "0xff,0xff";
else
{
    array += "0x00,0x00";
}
for (int j = 0; j < image.Height; j++)
{
    for (int i = 0; i < image.Width; i++)
    {
        if (i == 0 && j == 0)
        continue;
        ipixel = image.GetPixel(i, j);
        if (ipixel.B > 0 && ipixel.R > 0 && ipixel.R > 0)
            //array += "0x" + image.GetPixel(i, j).A.ToString("x2") + ",0x" + image.GetPixel(i, j).R.ToString("x2") + ",0x" + image.GetPixel(i, j).G.ToString("x2") + ",0x" + image.GetPixel(i, j).B.ToString("x2") + ",";
            array += ",0xff,0xff";
        else
        {
            array += ",0x00,0x00";
        }
    }

    array += '\n';
}

array += "\n\n};";

File.WriteAllText(args[1] + ".h", array);

Console.WriteLine("Converting worked!");