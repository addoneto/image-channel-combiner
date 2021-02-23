using System;

using SixLabors.ImageSharp;
using SixLabors.ImageSharp.PixelFormats;
using SixLabors.ImageSharp.Processing;

// dotnet add package SixLabors.ImageSharp --version 1.0.3
// <PackageReference Include="SixLabors.ImageSharp" Version="1.0.3" />

namespace imageChannelCombiner {
    class Program {
        static void Main(string[] paths) {
            if(paths.Length == 3){
                GenerateImage(paths[0], paths[1], paths[2]);
            }else {
                Console.WriteLine("Provide image paths for each channel [R, G, B]");
            }
        }

        static void GenerateImage(string rPath, string gPath, string bPath){

            try {
                using Image<Rgba32> rImg = Image.Load<Rgba32>(rPath);
                using Image<Rgba32> bImg = Image.Load<Rgba32>(gPath);
                using Image<Rgba32> gImg = Image.Load<Rgba32>(bPath);

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

                final.Save("test.png"); 
            }catch(Exception err){
                Console.WriteLine($"{err.GetType().Name}: Could not load images");
                return; // throw;
            }
        }
    }
}
