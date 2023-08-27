using System;

namespace GIF_TEST
{
    class Program
    {
        static void Main(string[] args)
        {
            string _jpg1 = @"D:\Family\20161226_133654.jpg";
            string _jpg2 = @"D:\Family\20161226_133655.jpg";
            string _result_GIF = @"D:\park_20161226_1336_.gif";

            GIF_Factory gif_Factory = new GIF_Factory();
            gif_Factory.JPG_To_Image(_jpg1, _jpg2, _result_GIF);
            Console.ReadLine();
        }
    }
}
