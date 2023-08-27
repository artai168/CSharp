using ImageMagick;

namespace GIF_TEST
{
    public class GIF_Factory
    {
        //Convert 2 JPG files to an animation GIF
        public void JPG_To_Image(string in_jpg_1, string in_jpg_2, string out_Img_Result)
		{
			using (MagickImageCollection collection = new MagickImageCollection())
			{
				// Add first image and set the animation delay to 100ms
				collection.Add(in_jpg_1);
				collection[0].AnimationDelay = 100;

				// Add second image, set the animation delay to 100ms and flip the image
				collection.Add(in_jpg_2);
				collection[1].AnimationDelay = 100;
				//collection[1].Flip();

				// Optionally reduce colors
				QuantizeSettings settings = new QuantizeSettings();
				settings.Colors = 256;
				collection.Quantize(settings);

				// Optionally optimize the images (images should have the same size).
				collection.Optimize();

				// Save gif
				collection.Write(out_Img_Result);
			}
		}
    }

}
