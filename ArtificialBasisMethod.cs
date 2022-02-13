using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MO_project
{
    class ArtificialBasisMethod
    {
        //основные рассчёты для метода искуственного базиса
        //даём: массив с данными и выбранной опроной точкой
        //возвращаем: массив с выполнееным шагом
        private Fraction[,] array;
        public void SetArray(Object[,] newFr) { array = (Fraction[,])newFr; }
        public Fraction[,] getFr() { return array; }

        public double[,] Step(bool flagDelete, double[,] table, int row, int column ,bool flagFr)
        {
            double[,] new_array = new double[table.GetLength(0), table.GetLength(1)];
            if (flagFr)
            {

                Fraction[,] ArrayDataFr = getFr();

                Fraction[,] new_arrayFraction = new Fraction[ArrayDataFr.GetLength(0), ArrayDataFr.GetLength(1)];

                //работа с обыкновенными дробями
                for (int i = 0; i < ArrayDataFr.GetLength(1); i++)
                {
                    //опорная строка
                    if (i != column) new_arrayFraction[row, i] = ArrayDataFr[row, i].Del(ArrayDataFr[row, column]);
                    else new_arrayFraction[row, i] = (new Fraction(1,1)).Del(ArrayDataFr[row, column]);
                }

                //заполнение таблицы
                for (int i = 0; i < ArrayDataFr.GetLength(0); i++)
                {
                    if (i != row)
                    {
                        for (int j = 0; j < ArrayDataFr.GetLength(1); j++)
                        {
                            new_arrayFraction[i, j] = ArrayDataFr[i, j].Sub(ArrayDataFr[i, column].Mul(new_arrayFraction[row, j]));
                        }
                    }
                    //тестовое изменение
                    //если мы не удаляем столбец то надо его рассчитать
                    if (!flagDelete)
                    {
                        for (int j = 0; j < ArrayDataFr.GetLength(0); j++)
                        {
                            if (j != row)
                            {
                                new_arrayFraction[j, column] = ArrayDataFr[j, column].Mul((new Fraction(1,1).Del(ArrayDataFr[row, column])).Mul(new Fraction(-1,1)));
                            }
                        }
                        
                    }
                }

                //удаляем столбец
                if (flagDelete)
                {
                    Fraction[,] new_array2Fr = new Fraction[ArrayDataFr.GetLength(0), ArrayDataFr.GetLength(1) - 1];
                    for (int i = 0; i < ArrayDataFr.GetLength(0); i++)
                    {
                        for (int j = 0; j < ArrayDataFr.GetLength(1); j++)
                        {
                            if (j < column) new_array2Fr[i, j] = new_arrayFraction[i, j];
                            if (j > column) new_array2Fr[i, j - 1] = new_arrayFraction[i, j];
                        }
                    }

                    SetArray(new_array2Fr);
                }
                else SetArray(new_arrayFraction);
            }
            else
            {
                for (int i = 0; i < table.GetLength(1); i++)
                {
                    //опорная строка
                    Console.WriteLine("Отриц элемент table[row, column] "+ table[row, column]);
                    Console.WriteLine("table[row, i] " + table[row, i]);
                    if (i != column) new_array[row, i] = table[row, i] / table[row, column];
                    else new_array[row, i] = 1 / table[row, column];

                    Console.WriteLine("new_array[row, i]" + new_array[row, i]);
                }
                //заполнение таблицы
                for (int i = 0; i < table.GetLength(0); i++)
                {
                    if (i != row)
                    {
                        for (int j = 0; j < table.GetLength(1); j++)
                        {
                            new_array[i, j] = Math.Round(table[i, j], 4) - Math.Round((table[i, column] * new_array[row, j]), 4);
                        }
                    }
                    //тестовое изменение
                    //если мы не удаляем столбец то надо его рассчитать
                    if (!flagDelete)
                    {
                        for (int j = 0; j < table.GetLength(0); j++)
                        {
                            if (j != row)
                            {
                                new_array[j, column] = table[j, column] * (1 / table[row, column]) * (-1);
                            }
                        }
                    }
                }

                //удаляем столбец
                if (flagDelete)
                {
                    double[,] new_array2 = new double[table.GetLength(0), table.GetLength(1) - 1];
                    for (int i = 0; i < table.GetLength(0); i++)
                    {
                        for (int j = 0; j < table.GetLength(1); j++)
                        {
                            if (j < column) new_array2[i, j] = new_array[i, j];
                            if (j > column) new_array2[i, j - 1] = new_array[i, j];
                        }
                    }

                    return new_array2;
                }
                return new_array;
            }
          
            return new_array;
        }

        //проверяем достигли ли мы решения или ответа по этой линии не получить
        //-1 - не достигнуть ответа
        //0 - продолжаем
        //1 - ответ получен
        public int Ending(double[,] Table, bool Method, bool flagFr)
        {
            //Console.WriteLine("Ending");
            //for (int i = 0; i < array.GetLength(0); i++)
            //{
            //    Console.WriteLine();
            //    for (int j = 0; j < array.GetLength(1); j++)
            //    {
            //        Console.Write(" "+array[i,j].GetNumerator()+'/'+ array[i, j].GetDenominatror());
            //    }
            //}
            //Console.WriteLine("flagFr "+ flagFr);
            if (Method)
            {   
                if (EndingNegativeValue(Table, flagFr) == -2) return -1;
                if (Rezolt(Table, flagFr) == -2) return 1;
            }
            else if(EndingZero(Table, flagFr) == -2) return 1;
            //если не то и не другое продолжаем решение
            return 0;
        }

        private int Rezolt(double[,] Table, bool flagFr)
        {
            //если в последней строке все числа положительны то результат получен (кроме последнего столбца)
            if (flagFr)
            {
                for (int i = 0; i < array.GetLength(1)-1; i++)
                {
                    if (array[array.GetLength(0) - 1, i].GetNumerator()<0 || array[array.GetLength(0) - 1, i].GetDenominatror() < 0) return -1;
                }
            }
            else
            {
                for (int i = 0; i < Table.GetLength(1)-1; i++)
                {
                    if (Table[Table.GetLength(0) - 1, i] < 0) return -1;
                }
                return -2;
            }
            return -2;
        }

        private int EndingZero(double[,] Table, bool flagFr)
        {
            int response = -2;
            if (flagFr)
            {
                //если в последней строке все 0 значит ответ получен
                for (int i = 0; i < array.GetLength(1); i++)
                {
                    Console.WriteLine("(array[array.GetLength(0) - 1, i].GetNumerator() "+ (array[array.GetLength(0) - 1, i].GetNumerator()));
                    if (array[array.GetLength(0) - 1, i].GetNumerator() !=0 ) response = 0;
                }
                return response;
            }
            else
            {
                //если в последней строке все 0 значит ответ получен
                for (int i = 0; i < Table.GetLength(1); i++)
                {
                    if (Table[Table.GetLength(0) - 1, i] != 0) response = 0;
                }
                return response;
            }
        }

        private int EndingNegativeValue(double[,] Table, bool flagFr)
        {
            int response = -2;
            //если в столбце все значения отрицательны - ответ не получить
            if (flagFr)
            {
                //for (int i = 0; i < array.GetLength(1) ; i++)
                //{
                //    if (array[array.GetLength(0) - 1, i].GetNumerator()>=0 && array[array.GetLength(0) - 1, i].GetDenominatror()>= 0) response = 0;
                //}


                for (int i = 0; i < array.GetLength(1)-1; i++)
                {
                    for (int j = 0; j < array.GetLength(0); j++)
                    {
                        if (array[j, i].GetNumerator() > 0 && array[j, i].GetDenominatror() > 0) response = 0;
                    }
                    if (response != 0)  return -2; 
                    else response = -2;
                }
                return 0;
            }
            else
            {
                //for (int i = 0; i < Table.GetLength(1); i++)
                //{
                //    if (Table[Table.GetLength(0) - 1, i] >= 0) response = 0;
                //}

                for (int i = 0; i < Table.GetLength(1)-1; i++)
                {
                    for (int j = 0; j < Table.GetLength(0); j++)
                    {
                        if (Table[j, i] > 0) response = 0;
                    }
                    if (response != 0) return -2;
                    else response = -2;
                }
                return 0;
            }
        }

        private Fraction[] ans;
        public Fraction[] GetAns() { return ans; }

        //вернём массив точек x1 x2 ..
        public double[] Answer(double[,] Table, int[] intRow , int sizeX , bool flagFr)
        {
            double[] answer = new double[sizeX];
            Fraction[] answerFr = new Fraction[sizeX];
            for (int i = 0; i < answerFr.Count(); i++)
            {
                answerFr[i] = new Fraction(0,1);
            }
            if (flagFr)
            {
                for (int i = 0; i < array.GetLength(0) - 1; i++)
                {
                    Console.WriteLine("array[i, (array.GetLength(1) - 1)] "+ array[i, (array.GetLength(1) - 1)].GetNumerator()+'/'+ array[i, (array.GetLength(1) - 1)].GetDenominatror());
                    answerFr[intRow[i] - 1] = array[i, (array.GetLength(1) - 1)];
                }
                Console.WriteLine("AnswerFr test ");
                for (int i = 0; i < answerFr.Count(); i++)
                {
                    Console.Write(" " + answerFr[i].GetNumerator()+'/'+answerFr[i].GetDenominatror());
                }

                ans = answerFr;
            }
            else
            {
                for (int i = 0; i < Table.GetLength(0) - 1; i++)
                {
                    answer[intRow[i] - 1] = Table[i, (Table.GetLength(1) - 1)];
                }
               
            }
            return answer;
        }

    }
}
