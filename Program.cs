using System;
using System.IO;

using SixLabors.ImageSharp;
using SixLabors.ImageSharp.PixelFormats;

// dotnet add package SixLabors.ImageSharp --version 1.0.3
// <PackageReference Include="SixLabors.ImageSharp" Version="1.0.3" />

namespace imageChannelCombiner {
    class Program {
        static void Main(string[] paths) {
            if(paths.Length == 3){
                GenerateImage(paths[0], paths[1], paths[2]);
            }else {
                Console.WriteLine("Provide image paths for each channel [R, G, B]");
                Console.Write("Red Channel Image: ");
                string readPath  = Console.ReadLine();

                Console.Write("Green Channel Image: ");
                string greenPath = Console.ReadLine();

                Console.Write("Blue Channel Image: ");
                string bluePath  = Console.ReadLine();

                Console.Write("Image final name: ");
                string finalName = Console.ReadLine();

                GenerateImage(readPath, greenPath, bluePath, finalName);
            }
        }

        static void GenerateImage(string rPath, string gPath, string bPath, string finalName = "final"){

            Image<Rgba32> rImg;
            Image<Rgba32> bImg;
            Image<Rgba32> gImg;

            try {
                rImg = Image.Load<Rgba32>(rPath);
                bImg = Image.Load<Rgba32>(gPath);
                gImg = Image.Load<Rgba32>(bPath);
            }catch(Exception err){
                Console.WriteLine($"{err.GetType().Name}: Could not load images");
                return; // throw;
            }

            // IImageFormat imgFormat = rImg.DetectFormat(byte[] data);
            string rFileExtension = Path.GetExtension(rPath);
            string gFileExtension = Path.GetExtension(gPath);
            string bFileExtension = Path.GetExtension(bPath);

            // TODO: try converting file extensions
            if(rFileExtension != bFileExtension || rFileExtension != gFileExtension){
                Console.WriteLine("All files must be the same format");
                return;
            }

            if(rImg.Width != bImg.Width || rImg.Width != gImg.Width 
                || rImg.Height != bImg.Height || rImg.Height != gImg.Height){
                    
                Console.WriteLine("All images must be the same size");
                return;
            }

            using Image<Rgba32> final = new Image<Rgba32>(rImg.Width, rImg.Height);

            for(int y = 0; y < rImg.Height; y++){
                for(int x = 0; x < rImg.Width; x++){
                    // Could use any channel because images should be gray scl
                    byte redChannel   = rImg[x, y].R;
                    byte greenChannel = bImg[x, y].G;
                    byte blueChannel  = gImg[x, y].B;

                    final[x, y] = new Rgba32(redChannel, greenChannel, blueChannel, 255);
                }
            }

            final.Save(finalName + rFileExtension); 
        }
    }
}
