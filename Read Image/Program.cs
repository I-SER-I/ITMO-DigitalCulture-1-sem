using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;

namespace Read_Image
{
    class Program
    {
        static void Main()
        {
            var img = new Bitmap(Image.FromFile(@"C:\Users\Sergey\Lab2.bmp"));
            int[] array = new int[img.Width + 1];
            List<(int, int)> result = new List<(int, int)>();
            int count = 0;

            //Считывание изображения, инциализация переменных


            for (int x = 0; x < img.Width; x++)
            {
                for (int y = 0; y < img.Height; y++)
                {
                    Color bitmapColor = img.GetPixel(x, y);
                    int colorGray = (int)(bitmapColor.R * 0.299 + bitmapColor.G * 0.587 + bitmapColor.B * 0.114);
                    img.SetPixel(x, y, Color.FromArgb(colorGray, colorGray, colorGray));
                }
                array[x] = (Convert.ToInt32(img.GetPixel(x, 64  ).R) / 20) * 20;
            }

            // Перевод цветного изображения в градации серого по формуле: Y = 0,299R + 0,587G + 0,114B                                              
            // Запись центральной строки пиксилей в масссив                       
            // Квантование по формуле X = (X / 20) * 20      
            // Вывод результата конвертирования и квантования в отдельный файл

            string resultt = "";
            for (int i = 0; i < 128; i++)
                resultt += Convert.ToString(array[i]) + " ";
            File.WriteAllText("lol.txt", resultt);

            Array.Sort(array);
            for (int i = 0; i < array.Length - 1; i++)
            {
                if (array[i] == array[i + 1])
                    count++;
                else
                {
                    result.Add((array[i], count + 1));
                    count = 0;
                }
            }
            for (int i = 0; i < result.Count; i++)
                Console.WriteLine($"Значение: {result[i].Item1}  Количество: {result[i].Item2}  " +
                                  $"Вероятность: {(float)(result[i].Item2) / (float)(img.Width)}");
            Console.ReadKey();

            // Сортировка массива
            // Подсчет количества одинаковых элементов в массиве
            // Запись значения и количества этого значения в список
            // Вывод, подсчет вероятностей
        }
    }
}
