using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MO_project
{
    class Gause
    {
        private Fraction[,] arrayDataFr;

        public void SetFr(Fraction[,] newFr) { arrayDataFr = newFr; }
        public Fraction[,] GetFr() { return arrayDataFr; }

        public double[,] Method(double[,] matrix, int[] baseElems, bool flagFr)
        {
            if (flagFr)
            {

                //прямой ход 
                int row1 = 0;
                for (int i = 0; i < arrayDataFr.GetLength(1); i++)
                {
                    //если столбец не базисный пропускаем этот шаг
                    if (baseElems.Contains(i + 1))
                    {
                        //поднимаем нужную строчку наверх
                        int Max_row = MaxRow(matrix, i, row1, flagFr);
                        //проверяем на ошибку
                        if (Max_row == -1)
                        {
                            Console.WriteLine("ERROR!!");
                        }
                        matrix = Transfer(matrix, Max_row, row1, flagFr);
                        //делим row строку на первый элемент
                        Fraction del = arrayDataFr[row1, i];
                        Console.WriteLine("del " + del.GetNumerator() + '/' + del.GetDenominatror());
                        for (int j = 0; j < arrayDataFr.GetLength(1); j++)
                        {
                            if (j == i) arrayDataFr[row1, j] = new Fraction(1, 1);
                            else
                            {
                                Console.WriteLine(" arrayDataFr[row1, j] до " + arrayDataFr[row1, j].GetNumerator() + '/' + arrayDataFr[row1, j].GetDenominatror());
                                arrayDataFr[row1, j] = arrayDataFr[row1, j].Del(del);
                                arrayDataFr[row1, j].StartNod();
                                Console.WriteLine(" arrayDataFr[row1, j] после" + arrayDataFr[row1, j].GetNumerator() + '/' + arrayDataFr[row1, j].GetDenominatror());
                            }
                        }
                        //все строки под нашей проходим и каждый элемент j = j - row*j
                        for (int k = row1 + 1; k < arrayDataFr.GetLength(0); k++)
                        {
                            del = arrayDataFr[k, i];
                            for (int j = 0; j < arrayDataFr.GetLength(1); j++)
                            {
                                arrayDataFr[k, j] = arrayDataFr[k, j].Sub(del.Mul(arrayDataFr[row1, j]));
                                arrayDataFr[k, j].StartNod();
                            }
                        }
                        row1++;

                    }
                }

                row1--;
                //обратный ход 
                //идём с конца и ищем первый базисный элемент
                for (int i = arrayDataFr.GetLength(1) - 2; i > 0; i--)
                {
                    //если элемент является базисным

                    if (baseElems.Contains(i + 1))
                    {

                        Fraction del = arrayDataFr[row1, i];
                        //делим всю нашу строку
                        for (int j = 0; j < arrayDataFr.GetLength(1); j++)
                        {
                            arrayDataFr[row1, j] = arrayDataFr[row1, j].Del(del);
                            arrayDataFr[row1, j].StartNod();
                        }
                        //проходим все строки над нашей и каждый элемент j = j - row *j
                        for (int j = row1 - 1; j >= 0; j--)
                        {
                            del = arrayDataFr[j, i];

                            for (int k = arrayDataFr.GetLength(1) - 1; k >= 0; k--)
                            {
                                arrayDataFr[j, k] = arrayDataFr[j, k].Sub(del.Mul(arrayDataFr[row1, k]));
                                arrayDataFr[j, k].StartNod();
                            }
                        }

                        row1--;
                    }
                }

            }
            else
            {
                //прямой ход 
                int row = 0;
                for (int i = 0; i < matrix.GetLength(1); i++)
                {
                    //если столбец не базисный пропускаем этот шаг
                    if (baseElems.Contains(i + 1))
                    {
                        //поднимаем нужную строчку наверх
                        int Max_row = MaxRow(matrix, i, row, flagFr);
                        //проверяем на ошибку
                        if (Max_row == -1)
                        {
                            Console.WriteLine("ERROR!!");
                        }
                        matrix = Transfer(matrix, Max_row, row, flagFr);
                        //делим row строку на первый элемент
                        double del = matrix[row, i];
                        for (int j = 0; j < matrix.GetLength(1); j++)
                        {
                            if (j == i) matrix[row, j] = 1;
                            else matrix[row, j] /= del;
                        }
                        //все строки под нашей проходим и каждый элемент j = j - row*j
                        for (int k = row + 1; k < matrix.GetLength(0); k++)
                        {
                            del = matrix[k, i];
                            for (int j = 0; j < matrix.GetLength(1); j++)
                            {
                                matrix[k, j] = matrix[k, j] - del * matrix[row, j];
                            }
                        }
                        row++;

                    }
                }

                row--;
                //обратный ход 
                //идём с конца и ищем первый базисный элемент
                for (int i = matrix.GetLength(1) - 2; i > 0; i--)
                {
                    //если элемент является базисным

                    if (baseElems.Contains(i + 1))
                    {

                        double del = matrix[row, i];
                        //делим всю нашу строку
                        for (int j = 0; j < matrix.GetLength(1); j++)
                        {
                            matrix[row, j] /= del;
                        }
                        //проходим все строки над нашей и каждый элемент j = j - row *j
                        for (int j = row - 1; j >= 0; j--)
                        {
                            del = matrix[j, i];

                            for (int k = matrix.GetLength(1) - 1; k >= 0; k--)
                            {
                                matrix[j, k] = matrix[j, k] - del * matrix[row, k];
                            }
                        }

                        row--;
                    }
                }
                return matrix;
            }
            return matrix;
        }

        //определем строку с максимальным элементов
        private int MaxRow(double[,] matrix, int column, int startRow, bool flagFr)
        {
            if (flagFr)
            {
                Fraction max1 = new Fraction(-1, 1);
                int rowIndex1 = 0;
                for (int i = startRow; i < arrayDataFr.GetLength(0); i++)
                {
                    if (arrayDataFr[i, column].ABS().Srav(max1) == 1)
                    {
                        max1 = arrayDataFr[i, column].ABS();
                        rowIndex1 = i;
                    }
                }
                return rowIndex1;
            }
            else
            {
                double max = -1;
                int rowIndex = 0;
                for (int i = startRow; i < matrix.GetLength(0); i++)
                {
                    if (Math.Abs(matrix[i, column]) > max)
                    {
                        max = Math.Abs(matrix[i, column]);
                        rowIndex = i;
                    }
                }
                //если максимальный элемент по модулю 0 то решения нет
                if (max == 0) return -1;
                return rowIndex;
            }
        }

        //переносим строчку  с максиамальным элементов наверх массива
        private double[,] Transfer(double[,] matrix, int RowMax, int top, bool flagFr)
        {

            if (flagFr)
            {
                //переносим строчку наверх
                for (int i = RowMax; i > top; i--)
                {
                    //меняем местами соседние строчки 
                    for (int j = 0; j < arrayDataFr.GetLength(1); j++)
                    {
                        //проходим по всем элементам строки и меняем их местами с соседней верхней строкой
                        Fraction supporting = arrayDataFr[i, j];
                        arrayDataFr[i, j] = arrayDataFr[i - 1, j];
                        arrayDataFr[i - 1, j] = supporting;
                    }
                }

            }
            else
            {
                //переносим строчку наверх
                for (int i = RowMax; i > top; i--)
                {
                    //меняем местами соседние строчки 
                    for (int j = 0; j < matrix.GetLength(1); j++)
                    {
                        //проходим по всем элементам строки и меняем их местами с соседней верхней строкой
                        double supporting = matrix[i, j];
                        matrix[i, j] = matrix[i - 1, j];
                        matrix[i - 1, j] = supporting;
                    }
                }
                return matrix;
            }
            return matrix;

        }
        //преобразуем нашу матрицу 
        /*
         *  Необходимо, чтобы в столбцах расополагались коэффициенты не базисных элементов
         *  1) Преобразуем целевую функцию
         *  2) Преобразуем матрицу в соответсвии с полученными коэффициентами
         */
        private Fraction[] base_funFr;
        public void SetBaseFun(Object[] newBase) { base_funFr = (Fraction[])newBase; }
        public Fraction[] getBasesFun() { return base_funFr; }

        public double[,] Transformation(double[] base_function, double[,] matrix, int[] baseElems, bool flagFr)
        {
            if (flagFr)
            {

                matrix = DeleteColumn(matrix, flagFr);

                Fraction[,] matrix_fi = new Fraction[arrayDataFr.GetLength(0) + 1, arrayDataFr.GetLength(1)];

                for (int i = 0; i < arrayDataFr.GetLength(0); i++)
                {
                    for (int j = 0; j < arrayDataFr.GetLength(1); j++)
                    {
                        matrix_fi[i, j] = arrayDataFr[i, j];
                    }
                }

                //изменяем целевую функцию
                TargetFunction(base_function, matrix, baseElems, true);

                //добавляем к нашей матрице последнюю функцию
                Fraction[,] new_matrixs = new Fraction[arrayDataFr.GetLength(0) + 1, arrayDataFr.GetLength(1)];

                for (int i = 0; i < matrix_fi.GetLength(0); i++)
                {
                    for (int j = 0; j < matrix_fi.GetLength(1); j++)
                    {
                        new_matrixs[i, j] = matrix_fi[i, j];
                    }
                }

                for (int i = 0; i < new_matrixs.GetLength(1); i++)
                {
                    new_matrixs[new_matrixs.GetLength(0) - 1, i] = base_funFr[i];
                }

                arrayDataFr = new_matrixs;

            }
            else
            {
                //удалим из нашей матрицы базисные столбцы
                matrix = DeleteColumn(matrix, flagFr);

                double[,] matrix_fix = new double[matrix.GetLength(0) + 1, matrix.GetLength(1)];

                for (int i = 0; i < matrix.GetLength(0); i++)
                {
                    for (int j = 0; j < matrix.GetLength(1); j++)
                    {
                        matrix_fix[i, j] = matrix[i, j];
                    }
                }

                //изменяем целевую функцию
                double[] function = TargetFunction(base_function, matrix, baseElems, false);

                //добавляем к нашей матрице последнюю функцию
                double[,] new_matrix = new double[matrix.GetLength(0) + 1, matrix.GetLength(1)];

                for (int i = 0; i < matrix_fix.GetLength(0); i++)
                {
                    for (int j = 0; j < matrix_fix.GetLength(1); j++)
                    {
                        new_matrix[i, j] = matrix_fix[i, j];
                    }
                }

                for (int i = 0; i < new_matrix.GetLength(1); i++)
                {
                    new_matrix[new_matrix.GetLength(0) - 1, i] = function[i];
                }

                return new_matrix;
            }
            return new double[1,1];
        }

        //Удаление столбца
        private double[,] DeleteColumn(double[,] matrix, bool flagFr)
        {
            if (flagFr)
            {
                int col = 0;


                for (int i = 0; i < arrayDataFr.GetLength(1);)
                {
                    if (BasesDelete(matrix, i, true))
                    {
                        col++;
                        for (int k = i; k < arrayDataFr.GetLength(1) - 1; k++)
                        {
                            for (int j = 0; j < arrayDataFr.GetLength(0); j++)
                            {
                                arrayDataFr[j, k] = arrayDataFr[j, k + 1];
                            }
                        }
                        //пробная 
                        if ((col + i) == (arrayDataFr.GetLength(1)-1)) break;
                    }
                    else i++;
                }

                Fraction[,] new_matrix = new Fraction[arrayDataFr.GetLength(0), arrayDataFr.GetLength(1) - col];
                //создаём новую матрицу
                for (int i = 0; i < new_matrix.GetLength(0); i++)
                {
                    for (int j = 0; j < new_matrix.GetLength(1); j++)
                    {
                        new_matrix[i, j] = arrayDataFr[i, j];
                    }
                }

                arrayDataFr = new_matrix;

            }
            else
            {
                int column = 0;

                for (int i = 0; i < matrix.GetLength(1);)
                {
                    if (BasesDelete(matrix, i, false))
                    {
                        column++;
                        for (int k = i; k < matrix.GetLength(1) - 1; k++)
                        {
                            for (int j = 0; j < matrix.GetLength(0); j++)
                            {
                                matrix[j, k] = matrix[j, k + 1];
                            }
                        }
                    }
                    else i++;
                }

                double[,] new_matrix = new double[matrix.GetLength(0), matrix.GetLength(1) - column];
                //создаём новую матрицу
                for (int i = 0; i < new_matrix.GetLength(0); i++)
                {
                    for (int j = 0; j < new_matrix.GetLength(1); j++)
                    {
                        new_matrix[i, j] = matrix[i, j];
                    }
                }

                return new_matrix;
            }
            return new double[1,1];
        }

        //проверка на базис
        private bool BasesDelete(double[,] matrix, int column, bool flagFr)
        {
            if (flagFr)
            {
                Fraction sum = new Fraction(0, 1);

                for (int i = 0; i < arrayDataFr.GetLength(0); i++)
                {
                    sum = sum.Sum(arrayDataFr[i, column]);
                    if (arrayDataFr[i, column].Srav(new Fraction(1,1)) != 0)
                    {
                        if (arrayDataFr[i, column].Srav(new Fraction(0, 1)) != 0)
                        {
                            return false;
                        }
                    }
                    if (arrayDataFr[i, column].Srav(new Fraction(0,1)) != 0)
                    {
                        if (arrayDataFr[i, column].Srav(new Fraction(1, 1)) != 0)
                        {
                            return false;
                        }
                    }
                    
                }

                //доп проверка с суммой
                if (sum.Srav(new Fraction(1, 1)) != 0)
                {
                    return false;
                }

                return true;
            }
            else
            {
                double sum = 0;
                for (int i = 0; i < matrix.GetLength(0); i++)
                {
                    sum += matrix[i, column];
                    if (matrix[i, column] != 1)
                        if (matrix[i, column] != 0) return false;
                    if (matrix[i, column] != 0)
                        if (matrix[i, column] != 1) return false;
                }
                if (sum == 1) return true;
                return false;
            }
        }

        //изменение целевой функции
        public double[] TargetFunction(double[] function, double[,] matrix, int[] bases , bool flagFr)
        {
            List<double[]> arrays = new List<double[]>();

            if (flagFr)
            {
                Fraction[] new_funFr = new Fraction[function.Count()];
                Fraction[] finish_funcFr = new Fraction[function.Count()];

                for (int i = 0; i < arrayDataFr.GetLength(0); i++)
                {
                    //Console.WriteLine("element "+ element);
                    Fraction element = base_funFr[bases[i] - 1];

                    //домножаем его на все элементы в столбце i
                    for (int j = 0; j < arrayDataFr.GetLength(1); j++)
                    {
                        if (j != arrayDataFr.GetLength(1) - 1) arrayDataFr[i, j] =arrayDataFr[i, j].Mul(new Fraction(-1,1));
                        arrayDataFr[i, j] = arrayDataFr[i, j].Mul(element);
                    }

                }

                //в целевой функции нужно создать массив только с коээффициентами не базисных элементов
                for (int i = 0; i < base_funFr.Count(); i++)
                {
                    if (!bases.Contains(i + 1)) new_funFr[i] = base_funFr[i];
                }

                //теперь собираем все коэффициенты в одном массиве новой целевой функции
                for (int i = 0; i < arrayDataFr.GetLength(1); i++)
                {
                    Fraction sum = new Fraction(0,1);
                    for (int j = 0; j < arrayDataFr.GetLength(0); j++)
                    {
                        sum = sum.Sum(arrayDataFr[j, i]);
                    }
                    finish_funcFr[i] = sum;
                }

                int bass = 0;
                for (int i = 0; i < new_funFr.Count(); i++)
                {
                    if (!bases.Contains(i + 1))
                    {

                        if (i == new_funFr.Count() - 1)
                        {
                            new_funFr[bass] =new_funFr[bass].Sum(finish_funcFr[bass]);
                            new_funFr[bass] =new_funFr[bass].Mul(new Fraction(-1,1));
                            new_funFr[i] = new Fraction(0,1);
                        }
                        else new_funFr[bass] =new_funFr[bass].Sum(finish_funcFr[bass]);
                        bass++;
                    }
                    else
                    {
                        //смещаем строки влево
                        for (int j = bass; j < new_funFr.Count() - 1; j++)
                        {
                            new_funFr[j] = new_funFr[j + 1];
                        }

                    }
                }

                base_funFr = new_funFr;

            }
            else
            {
                double[] new_function = new double[function.Count()];
                double[] finish_function = new double[function.Count()];

                for (int i = 0; i < matrix.GetLength(0); i++)
                {
                    //коэффициент у элемента в целевой функции
                    double element = function[bases[i] - 1];

                    //домножаем его на все элементы в столбце i
                    for (int j = 0; j < matrix.GetLength(1); j++)
                    {
                        if (j != matrix.GetLength(1) - 1) matrix[i, j] *= -1;
                        matrix[i, j] *= element;
                    }
                }

                //в целевой функции нужно создать массив только с коээффициентами не базисных элементов
                for (int i = 0; i < function.Count(); i++)
                {
                    if (!bases.Contains(i + 1)) new_function[i] = function[i];
                }

                //теперь собираем все коэффициенты в одном массиве новой целевой функции
                for (int i = 0; i < matrix.GetLength(1); i++)
                {
                    double sum = 0;
                    for (int j = 0; j < matrix.GetLength(0); j++)
                    {
                        sum += matrix[j, i];
                    }
                    finish_function[i] = sum;
                }

                int bas = 0;
                for (int i = 0; i < new_function.Count(); i++)
                {
                    if (!bases.Contains(i + 1))
                    {

                        if (i == new_function.Count() - 1)
                        {
                            new_function[bas] += finish_function[bas];
                            new_function[bas] *= -1;
                            new_function[i] = 0;
                        }
                        else new_function[bas] += finish_function[bas];
                        bas++;
                    }
                    else
                    {
                        //смещаем строки влево
                        for (int j = bas; j < new_function.Count() - 1; j++)
                        {
                            new_function[j] = new_function[j + 1];
                        }
                    }
                }

                return new_function;
            }
            return new double[1];
        }
    }
}
