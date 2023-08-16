// See https://aka.ms/new-console-template for more information

using System.Drawing;

Console.WriteLine("Hello to the png to bmp converter");
string array_name, algo;
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

string[] files;
string folder = "", filename;
if (args[0] == "-f")
{
    folder = args[1];
    files = Directory.GetFiles(args[1]);
    for (int i = 0; i < files.Length; i++)
        files[i] = Path.GetFileNameWithoutExtension(files[i]);
    algo = args[3];
    array_name = args[2];
    filename = array_name;
}
else
{
    files = new[] { args[0] };
    algo = args[2];
    array_name = args[1];
    filename = array_name;
}

for (int f = 0; f < files.Length; f++)
{
    if (files.Length > 1)
    {
        array_name = filename + "_" + files[f];
        files[f] = folder + "\\" + files[f] + ".png";
    }
    Bitmap image = (Bitmap)Image.FromFile(files[f]);
    string line = "uint8_t " + array_name + "_width = " + image.Width + ";\nuint8_t " + array_name + "_width_byte = " +
                  image.Width / 8 + ";\nuint8_t " +
                  array_name + "_height = " + image.Height + "; \n\nuint8_t " + array_name + "[] = \n{\n";
    byte[] color;
    if (algo == "565")
    {
        color = GeneratePixel565(image.GetPixel(0, 0));
        line += " 0x" + color[0].ToString("x2") + ",0x" + color[1].ToString("x2");
        File.AppendAllTextAsync(filename + ".h", line);
    }
    else if (algo == "bw" || algo == "BW")
    {
        line += "0x" + GeneratePixelBWFullByte(image, 0, 0).ToString("x2");
        File.AppendAllText(filename + ".h", line);
    }

    for (int j = 0; j < image.Height; j++)
    {
        line = "";
        if (algo == "565")
            for (int i = 0; i < image.Width; i++)
            {
                if (i == 0 && j == 0)
                    continue;
                color = GeneratePixel565(image.GetPixel(i, j));
                line += ",0x" + color[0].ToString("x2") + ",0x" + color[1].ToString("x2");
            }
        else if (algo == "bw" || algo == "BW")
            for (int i = 0; i < image.Width; i += 8)
            {
                if (i == 0 && j == 0)
                    continue;
                line += ",0x" + GeneratePixelBWFullByte(image, i, j).ToString("x2");
            }

        line += '\n';
        if (j == image.Height - 1)
            line += "\n\n};";
        File.AppendAllText(filename + ".h", line);
    }

    line = "\n\n";
    File.AppendAllText(filename + ".h", line);
}

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
    return (color.R > 240 && color.G > 240 && color.B > 240) ? (byte)1 : (byte)0;
}

byte GeneratePixelBWFullByte(Bitmap image, int x, int y)
{
    byte pixel = 0;
    for (int i = x; i < image.Width && i < x + 8; i++)
    {
        pixel = (byte)(pixel << 1);
        pixel += GeneratePixelBW(image.GetPixel(i, y));
    }

    return pixel;
}