using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MO_project
{
    class dataPrint
    {

        public bool flagFr;
        public  List<Fraction> ArrayDataFr;
        //запись в файл
        public void Write(String ConditionsText, List<int> arrayData, int height)
        {

            string path = @"..\..\data.txt";

            try
            {
                StreamWriter sw = new StreamWriter(path, false);
                //число переменных и число ограничений
                sw.WriteLine(ConditionsText);
                //!!!НЕОБХОДИМО СОХРАНЯТЬ И ДОПОЛНИТЕЛЬНЫЕ УСЛОВИЯ К ЗАДАЧЕ!!!!!
                //запись данных из таблицы
                String text = "";
                //Основная таблица
                Console.WriteLine("flagFr " + flagFr);
                if (flagFr)
                {
                    int counts = 0;
                    for (int i = 0; i < ArrayDataFr.Count(); i++)
                    {
                        text = text + ArrayDataFr[i].GetNumerator()+'/'+ ArrayDataFr[i].GetDenominatror()+ " ";
                        counts++;
                        if (counts == height)
                        {
                            counts = 0;
                            sw.WriteLine(text.TrimEnd());
                            text = "";
                        }
                    }
                    sw.Close();
                }
                else
                {
                    int count = 0;
                    for (int i = 0; i < arrayData.Count(); i++)
                    {
                        text = text + arrayData[i] + " ";
                        count++;
                        if (count == height)
                        {
                            count = 0;
                            sw.WriteLine(text.TrimEnd());
                            text = "";
                        }
                    }
                    sw.Close();
                }
                
            }
            catch (Exception e)
            {
                Console.WriteLine("Exeption" + e.Message);
            }
        }

        //чистка файла
        public void Clear()
        {
            string path = @"..\..\data.txt";

            try
            {
                StreamWriter sw = new StreamWriter(path, false);
                sw.Close();
            }
            catch (Exception e)
            {
                Console.WriteLine("Exeption" + e.Message);
            }
        }

        public List<Fraction> arrayFrac = new List<Fraction>();

        //чтение из файла
        public List<int> Read()
        {
            string path = @"..\..\data.txt";
            List<int> array = new List<int>();

            //вернуть массив с данными первые n Элементов это условия задачи
            //Сейчас n=2
            //если файл пуст то вывести ошибку
            Console.WriteLine("!!Чтение!!");
            try
            {
                StreamReader sr = new StreamReader(path);
                int flagORd = 0;
                //Условия задачи
                String[] line = sr.ReadLine().Split(' ');
                Console.WriteLine("Чтение из файла!!");
                if (line==null) return array;
                Console.WriteLine(" Условия задачи!!");
                for (int i = 0; i < line.Count(); i++)
                {
                    Console.WriteLine(line[i]);
                    array.Add(int.Parse(line[i]));

                    if (i == 3)
                    {
                        if (int.Parse(line[i]) == 1) flagORd = 1;
                        else flagORd = 0;
                    }
                }
                //заносим условия
                Console.WriteLine(" Данные в таблице!!");

                while (line != null)
                {
                    Console.WriteLine(" while");

                    line = sr.ReadLine().Split(' ');
                    Console.WriteLine("line ciount "+line.Count());
                    for (int i = 0; i < line.Count(); i++)
                    {
                        Console.WriteLine("int.Parse(line[3]) " + flagORd);
                        if (flagORd == 1)
                        {

                            Console.WriteLine(line[i]);
                            string[] num_denom = line[i].Split('/');
                            arrayFrac.Add(new Fraction(int.Parse(num_denom[0]), int.Parse(num_denom[1])));
                            Console.WriteLine(arrayFrac.Last().GetNumerator()+"/"+ arrayFrac.Last().GetDenominatror());
                        }
                        else
                        {
                            Console.WriteLine(line[i]);
                            array.Add(int.Parse(line[i]));
                        }
                    }

                    
                }
                sr.Close();
                Console.ReadLine();
            }
            catch (Exception e)
            {
                Console.WriteLine("Exception: " + e.Message);
            }

            return array;
        }
    }
}
