// See https://aka.ms/new-console-template for more information

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
#pragma warning disable CA1416

Bitmap image = (Bitmap)Image.FromFile(args[0]);
string line = "#include \"stm32l4xx_hal.h\" \n\n uint8_t " + args[1] + "[] = \n{\n";
var color = GeneratePixel(image.GetPixel(0, 0));
line += " 0x" + color[0].ToString("x2") + ",0x" + color[1].ToString("x2");
File.AppendAllTextAsync(args[1] + ".h",line);
for (int j = 0; j < image.Height; j++)
{
    line = "";
    for (int i = 0; i < image.Width; i++)
    {
        if (i == 0 && j == 0)
            continue;
        color = GeneratePixel(image.GetPixel(i, j));
        line += ",0x" + color[0].ToString("x2") + ",0x" + color[1].ToString("x2");
    }

    line += '\n';
    if(j == image.Height-1)
        line += "\n\n};";
    File.AppendAllTextAsync(args[1] + ".h",line);
}

line = "";
File.AppendAllTextAsync(args[1] + ".h", line);

waitsomething();
Console.WriteLine("Converting worked!");

#pragma warning restore CA1416

async void waitsomething()
{
    await Task.Delay(2000);
}


byte[] GeneratePixel(Color color)
{
    byte r = (byte)(color.R >> 3);
    byte g = (byte)(color.G >> 2);
    byte b = (byte)(color.B >> 3);
    byte[] pixel = new byte[2];
    pixel[0] = (byte)((r << 3) + (g >> 3));
    pixel[1] = (byte)((g << 5) + (b));
    return pixel;
}