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

if (args.Length < 3)
{
    Console.WriteLine("Output missing: 565 or bw");
}
#pragma warning disable CA1416

Bitmap image = (Bitmap)Image.FromFile(args[0]);
string line = "uint8_t " + args[1] + "_width = " + image.Width + ";\nuint8_t " + args[1] + "_width_bygte = " + image.Width / 8 + ";\nuint8_t " +
              args[1] + "_height = " + image.Height + "; \n\nuint8_t " + args[1] + "[] = \n{\n";
byte[] color;
if (args[2] == "565")
{
    color = GeneratePixel565(image.GetPixel(0, 0));
    line += " 0x" + color[0].ToString("x2") + ",0x" + color[1].ToString("x2");
    File.AppendAllTextAsync(args[1] + ".h", line);
}
else if (args[2] == "bw" || args[2] == "BW")
{
    line += ",0x" + GeneratePixelBWFullByte(image, 0, 0).ToString("x2");
    File.AppendAllTextAsync(args[1] + ".h", line);
}

for (int j = 0; j < image.Height; j++)
{
    line = "";
    if (args[2] == "565")
        for (int i = 0; i < image.Width; i++)
        {
            if (i == 0 && j == 0)
                continue;
            color = GeneratePixel565(image.GetPixel(i, j));
            line += ",0x" + color[0].ToString("x2") + ",0x" + color[1].ToString("x2");
        }
    else if (args[2] == "bw" || args[2] == "BW")
        for (int i = 0; i < image.Width; i = i + 8)
        {
            line += ",0x" + GeneratePixelBWFullByte(image, i, j).ToString("x2");
        }

    line += '\n';
    if (j == image.Height - 1)
        line += "\n\n};";
    File.AppendAllTextAsync(args[1] + ".h", line);
}

line = "";
File.AppendAllTextAsync(args[1] + ".h", line);

Console.WriteLine("Converting worked!");

#pragma warning restore CA1416

byte[] GeneratePixel565(Color color)
{
    byte r = (byte)(color.R >> 3);
    byte g = (byte)(color.G >> 2);
    byte b = (byte)(color.B >> 3);
    byte[] pixel = new byte[2];
    pixel[0] = (byte)((r << 3) + (g >> 3));
    pixel[1] = (byte)((g << 5) + (b));
    return pixel;
}

byte GeneratePixelBW(Color color)
{
    return (color.R > 0 || color.G > 0 || color.B > 0) ? (byte)0 : (byte)1;
}

byte GeneratePixelBWFullByte(Bitmap image, int x, int y)
{
    byte pixel = 0;
    for (int i = x; i < image.Width || i < x + 8; i++)
    {
        pixel = (byte)(pixel << 1);
        pixel += GeneratePixelBW(image.GetPixel(x, y));
    }

    return pixel;
}