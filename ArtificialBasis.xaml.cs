using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace MO_project
{
    /// <summary>
    /// Interaction logic for ArtificialBasis.xaml
    /// </summary>
    public partial class ArtificialBasis : Window
    {
        //храним все данные о этапах
        List<object> dataBox = new List<object>();

        //временное хранение кнопок
        List<Button> CollectionButton = new List<Button>();
        //временное хранение полей
        List<TextBox> CollectionTextBox = new List<TextBox>();
        //хранение индексов
        List<int[]> CollectionIndexColumn = new List<int[]>();


        List<int[]> testColRow = new List<int[]>();
        List<int[]> testColCol = new List<int[]>();

        // List<RowInt> Collection = new List<RowInt>();

        List<int[]> CollectionRow = new List<int[]>();
        //хранение индексов стобцов и строк
        private int[] intRow;
        private int[] intColumn;
        private int step;
        private int range;
        private int sizeX;

        private int minRow;
        private int minColumn;

        private int baseRow;

        private bool checkEnd = false;

        Fraction[,] arrayDataFr;
        bool flagFra;

        //хранение целевой функции
        private double[] targetFunction;

        public ArtificialBasis()
        {
            InitializeComponent();
        }

        public int GetBaseRow() { return baseRow; }
        public void SetBaseRow(int newBaseRow)
        {
            Console.WriteLine(newBaseRow);
            baseRow = newBaseRow;
        }

        private int[] basEl;
        private int[] basElFr;

        public void SetFr(Object[,] newFr) { arrayDataFr = (Fraction[,])newFr; }
        public void SetFlagFr(bool newFlag) { flagFra = newFlag; }

        private int[] GetCollectionIntRow() { return CollectionRow[CollectionRow.Count() - 1]; }
        private void SetCollectionIntRow(int[] newIntRow) { CollectionRow.Add(newIntRow); }

        private int[] GetCollectionIntColumn() { return CollectionIndexColumn[CollectionIndexColumn.Count() - 1]; }
        private void SetCollectionIntColumn(int[] newIntColumn){ CollectionIndexColumn.Add(newIntColumn);}
        //get и set для CollectionButton
        private List<Button> GetCollectionButton() { return CollectionButton; }
        private void SetCollectionButton(List<Button> newCollectionButton) { CollectionButton = newCollectionButton; }

        //get и set для CollectionTextBox
        private List<TextBox> GetCollectionTextBox() { return CollectionTextBox; }
        private void SetCollectionTextBox(List<TextBox> newCollectioTextBox) { CollectionTextBox = newCollectioTextBox; }

        //get и set для intRow intColumn
        private int[] getIntRow() { return intRow; }
        private void setIntRow(int[] newIntRow) { intRow = newIntRow; }
        private int[] getIntColumn() { return intColumn; }
        private void setIntColumn(int[] newIntColumn) { intColumn = newIntColumn; }

        //get и set step и range
        private int getStep() { return step; }
        private void setStep(int newStep) { step = newStep; }
        private int getRange() { return range; }
        private void setRange(int newRange) { range = newRange; }

        //get и set для целевой функции
        public double[] getTargetFunction() { return targetFunction; }
        public void setTargetFunction(double[] newTargetFunction) { targetFunction = newTargetFunction; }
        public int getSizeX() { return sizeX; }
        public void setSizeX(int size) { sizeX = size; }

        //рисование новой таблицы
        public void Table(int row, int location, int column, int step, double[,] arrayData, int[] intRow, int[] intColumn)
        {
            //сохраняем в данные нашу таблицу
            if (flagFra) dataBox.Add(arrayDataFr);
            else dataBox.Add(arrayData);
            //наилучший опроный жлемент


            //исправление
            BasickElem(arrayData);

            setIntRow(intRow);
            setIntColumn(intColumn);
            setStep(step);


            SetCollectionIntRow(intRow);

            SetCollectionIntColumn(intColumn);

            //CollectionIndexRow.Add(intRow);
            //CollectionIndexColumn.Add(intColumn);

            //1 строчка с обозначениями
            Row(-2, column, location, intRow, intColumn, arrayData, row);
            //i кол-во строк столбцов

            MinElent(arrayData);

            for (int i = 0; i < row; i++)
            {
                Row(column + i, column, 22 * (i + 1) + location, intRow, intColumn, arrayData, row);
            }
            //i+1 строка
            Row(-1, column, 22 * (row+1) + location, intRow, intColumn, arrayData, row);
            setRange(22 * (row + 1) + location);

            dataBox.Add(GetCollectionButton());
            dataBox.Add(GetCollectionTextBox());
        }
        // range 41 начало
        //рисование строчки / номер шага, количество переменных, размещение строчки, что поместиим на строчке
        private void Row(int step, int count, int range, int[] intRow, int[] intColumn, double[,] arrayData, int sizeRow)
        {
            List<TextBox> Collection = GetCollectionTextBox();
            List<Button> CollectionButtons = GetCollectionButton();


            for (int i = 0; i < count + 1; i++)
            {
                TextBox row = new TextBox();
                row.FontSize = 12;
                row.Width = 32;
                row.Height = 22;
                row.VerticalAlignment = VerticalAlignment.Top;
                row.HorizontalAlignment = HorizontalAlignment.Left;
                row.TextAlignment = TextAlignment.Center;
                row.TextWrapping = TextWrapping.Wrap;
                row.Background = Brushes.LightGray;
                row.Margin = new Thickness(5 + i * 32, range, 0, 0);

                Button but = new Button();

                //оформляем заполнение данных
                if (step == -1)
                {
                    if (i == 0) row.Text = "";
                    else
                    {
                        if (flagFra)
                        {
                            if (arrayDataFr[arrayDataFr.GetLength(0) - 1, i - 1].GetDenominatror() == 1) row.Text = arrayDataFr[arrayDataFr.GetLength(0) - 1, i - 1].GetNumerator().ToString();
                            else row.Text = arrayDataFr[arrayDataFr.GetLength(0) - 1, i - 1].GetNumerator().ToString() + '/' + arrayDataFr[arrayDataFr.GetLength(0) - 1, i - 1].GetDenominatror().ToString();
                        }
                        else row.Text = arrayData[arrayData.GetLength(0) - 1, i - 1].ToString();
                    }
                    //добавление бокса
                    GridArtificzlBasis.Children.Add(row);
                    Collection.Add(row);
                }
                else if (step == -2)
                {
                    if (i == 0)
                    {
                        row.Text = "x‾" + getStep().ToString();
                    }
                    else if (i == count)
                    {
                        row.Text = "";
                    }
                    else
                    {
                        row.Text = "x" + intColumn[i - 1].ToString();
                    }
                    //добавление бокса
                    GridArtificzlBasis.Children.Add(row);
                    Collection.Add(row);
                }
                else
                {
                    if (i == 0)
                    {

                        row.Text = "x" + intRow[step - count].ToString();
                        //добавление бокса
                        GridArtificzlBasis.Children.Add(row);
                        Collection.Add(row);
                    }
                    else if (i == count)
                    {
                        if (flagFra)
                        {
                            if (arrayDataFr[step - count, i - 1].GetDenominatror() == 1) row.Text = arrayDataFr[step - count, i - 1].GetNumerator().ToString();
                            else row.Text = arrayDataFr[step - count, i - 1].GetNumerator().ToString() + '/' + arrayDataFr[step - count, i - 1].GetDenominatror().ToString();
                        }
                        else row.Text = arrayData[step - count, i - 1].ToString();
                        //добавление бокса
                        GridArtificzlBasis.Children.Add(row);
                        Collection.Add(row);
                    }
                    else
                    {
                        but.FontSize = 12;
                        but.Width = 32;
                        but.Height = 22;
                        but.VerticalAlignment = VerticalAlignment.Top;
                        but.HorizontalAlignment = HorizontalAlignment.Left;
                        but.Name = "test_" + ((step - count) * 10 + i - 1);

                        //поменяем цвет (что то типо светло зелёного для обычного и синий для лучшего выбор
                        //if (arrayData[step - count, i - 1] == max) but.Background = Brushes.LightBlue;
                        but.Background = Brushes.LightGreen;
                        if ((step - count) == minRow && (i - 1) == minColumn) but.Background = Brushes.LightBlue;

                        but.Margin = new Thickness(5 + i * 32, range, 0, 0);
                        //myGrid.Children.Add(but);
                        if (flagFra)
                        {
                            if (arrayDataFr[step - count, i - 1].GetDenominatror() == 1) but.Content = arrayDataFr[step - count, i - 1].GetNumerator().ToString();
                            else but.Content = arrayDataFr[step - count, i - 1].GetNumerator().ToString() + '/' + arrayDataFr[step - count, i - 1].GetDenominatror().ToString();
                        }
                        else but.Content = arrayData[step - count, i - 1].ToString();

                        //если решение уже есть на 0 шаге
                        //пробное изменение

                        ArtificialBasisMethod steps = new ArtificialBasisMethod();
                        steps.SetArray(arrayDataFr);
                        int switchCheck = 0;
                        if (flagFra)
                            switchCheck = steps.Ending(arrayData, true, true);
                        else
                            switchCheck = steps.Ending(arrayData, true, false);

                        switch (switchCheck)
                        {
                            case -1:
                                Rezolt.Text = "Решение не найдено \n";
                                Rezolt.Background = Brushes.Red;
                                break;
                            //case 0:
                            //   Rezolt.Text = "Продолжаем решать";
                            //   break;
                            case 1:
                                //ответ
                                double[] answer = steps.Answer(arrayData, GetCollectionIntRow(), getSizeX(), flagFra);

                                //проверка
                                if (flagFra)
                                {
                                    Fraction[] answers = steps.GetAns();
                                    checkEnd = true;
                                    //выводим ответ
                                    string str = "";
                                    for (int j = 0; j < answers.Count(); j++)
                                    {
                                        string number = answers[j].GetNumerator().ToString();
                                        if (answers[j].GetDenominatror() != 1) number = number + '/' + answers[j].GetDenominatror();
                                        str = str + "x" + (j + 1) + "=" + number + " ";
                                    }
                                    Fraction finish_sum = arrayDataFr[arrayDataFr.GetLength(0) - 1, arrayDataFr.GetLength(1) - 1].Mul(new Fraction(-1, 1));
                                    Rezolt.Text = "Ответ получен \n" + str + "\n f = " + finish_sum.GetNumerator() + '/' + finish_sum.GetDenominatror();
                                    Rezolt.Background = Brushes.LightGreen;
                                    break;
                                }
                                else
                                {
                                    checkEnd = true;
                                   
                                    //выводим ответ
                                    string str = "";
                                    for (int j = 0; j < answer.Count(); j++)
                                    {
                                        str = str + "x" + (j + 1) + "=" + answer[j] + " ";
                                    }
                                    double finish_sum = (-1) * arrayData[arrayData.GetLength(0) - 1, arrayData.GetLength(1) - 1];
                                    Rezolt.Text = "Ответ получен \n" + str + "\n f = " + finish_sum;
                                    Rezolt.Background = Brushes.LightGreen;
                                    break;
                                }
                        }

                        //добавление кнопки
                        //если число положительное то мы делаем кнопку для возможного выбора
                        if (flagFra)
                        {
                            if (arrayDataFr[step - count, i - 1].GetNumerator() > 0 && arrayDataFr[arrayDataFr.GetLength(0)-1, i - 1].GetNumerator()<0 && basElFr[i - 1] == step - count)
                            {

                                //добавляем кнопку в коллекию
                                CollectionButtons.Add(but);

                                int temp = CollectionButtons.Count() - 1;

                                but.MouseRightButtonDown +=
                               (sender, args) => SomeDoButton_PreviewMouseRightButtonDown(temp);

                                GridArtificzlBasis.Children.Add(but);
                            }
                            else
                            {
                                if (arrayDataFr[step - count, i - 1].GetDenominatror() == 1) row.Text = arrayDataFr[step - count, i - 1].GetNumerator().ToString();
                                else row.Text = arrayDataFr[step - count, i - 1].GetNumerator().ToString() + '/' + arrayDataFr[step - count, i - 1].GetDenominatror().ToString();
                                //добавление бокса
                                GridArtificzlBasis.Children.Add(row);
                                Collection.Add(row);
                            }
                        }
                        else
                        {
                            //пробное исправление по выбору элемента
                            if (arrayData[step - count, i - 1] > 0 && arrayData[arrayData.GetLength(0)-1, i - 1]< 0  && basEl[i - 1] == step - count)
                            {
                                //добавляем кнопку в коллекию
                                CollectionButtons.Add(but);

                                int temp = CollectionButtons.Count() - 1;

                                but.MouseRightButtonDown +=
                               (sender, args) => SomeDoButton_PreviewMouseRightButtonDown(temp);

                                GridArtificzlBasis.Children.Add(but);
                            }

                            else
                            {
                                row.Text = arrayData[step - count, i - 1].ToString();
                                //добавление бокса
                                GridArtificzlBasis.Children.Add(row);
                                Collection.Add(row);
                            }
                        }
                        
                    }

                }
            }

            SetCollectionButton(CollectionButtons);
            SetCollectionTextBox(Collection);
        }

        private void BasickElem(double[,] matrix)
        {
            /*
             * Нам необходимо взять каждый столбец разделить элемент на элемент в той же строке последнем столбце
             * И выбрать из них максимальный
             * Сохраняем номер строке в int массиве, где номер элемент равняется номеру в массиве
            */

            Console.WriteLine("BasickElem");
            if (flagFra)
            {//для дробей

                int[] basFr = new int[arrayDataFr.GetLength(1) - 1];

                Fraction minEl = new Fraction(10000, 1); int rowBas = 0;
                for (int i = 0; i < arrayDataFr.GetLength(1) - 1; i++)
                {
                    for (int j = 0; j < arrayDataFr.GetLength(0) - 1; j++)
                    {
                        //переменныйе
                        Fraction number = arrayDataFr[j, i].ABS();
                        Fraction basNumber = arrayDataFr[j, arrayDataFr.GetLength(1) - 1].ABS();

                        //дополнительно число должно для симплекс быть не отрицательным
                        if (arrayDataFr[j, i].GetNumerator() > 0)
                        {
                            Console.WriteLine("number " + number.GetNumerator() + '/' + number.GetDenominatror());
                            Console.WriteLine("basNumber " + basNumber.GetNumerator() + '/' + basNumber.GetDenominatror());
                            Console.WriteLine("del " + basNumber.Del(number).GetNumerator() + '/' + basNumber.Del(number).GetDenominatror());

                            if (basNumber.Del(number).Srav(minEl) == -1) { minEl = basNumber.Del(number); rowBas = j; }


                            Console.WriteLine("Временная minEl " + minEl.GetNumerator() + '/' + minEl.GetDenominatror());
                        }
                    }
                    Console.WriteLine();
                    Console.WriteLine("minEl " + minEl.GetNumerator() + '/' + minEl.GetDenominatror());
                    Console.WriteLine("rowBas " + rowBas);
                    Console.WriteLine();
                    basFr[i] = rowBas;
                    rowBas = 0;
                    minEl = new Fraction(10000, 1);
                }


                basElFr = basFr;

                Console.WriteLine("rezoltBasickElem");
                //
                for (int i = 0; i < basFr.Count(); i++)
                {
                    Console.Write(" " + basFr[i]);
                }
                Console.WriteLine("rezoltBasickElem");
            }
            else
            {
                Console.WriteLine("Standart");
                int[] bas = new int[matrix.GetLength(1) - 1];

                int rowBas = 0;
                double minEl = 10000;
                for (int i = 0; i < matrix.GetLength(1) - 1; i++)
                {
                    for (int j = 0; j < matrix.GetLength(0) - 1; j++)
                    {
                        double number = Math.Abs(matrix[j, i]);
                        double numberDel = Math.Abs(matrix[j, matrix.GetLength(1) - 1]);

                        if(matrix[j, i] > 0)
                        {
                            Console.WriteLine("number" + number);
                            Console.WriteLine("numberDel" + numberDel);

                            Console.WriteLine("Del" + numberDel / number);

                            if (minEl > (numberDel / number)) { minEl = numberDel / number; rowBas = j; }
                        }
                      
                    }
                    Console.WriteLine();
                    Console.WriteLine("minEl " + minEl);
                    Console.WriteLine("rowBas " + rowBas);
                    Console.WriteLine();
                    bas[i] = rowBas;
                    rowBas = 0;
                    minEl = 10000;
                }

                basEl = bas;

                Console.WriteLine("rezoltBasickElem");
                //
                for (int i = 0; i < bas.Count(); i++)
                {
                    Console.Write(" " + bas[i]);
                }
                Console.WriteLine("rezoltBasickElem");
            }

        }

        //необходимо определить максимальный элемент основной таблицы для наилучшего опроного элемента
        //вернём значение этого элемента
        private void MinElent(double[,] arrayData)
        {
            //необходимо выбрать наилучший элемент
            double min = 10000;
            //плохо конечно ну ладно
            Fraction minFr = new Fraction(10000,1);
            if (flagFra)
            {
                for (int i = 0; i < arrayDataFr.GetLength(0) - 1; i++)
                {
                    
                    Fraction B = arrayDataFr[i, arrayDataFr.GetLength(1) - 1];
                    for (int j = 0; j < arrayDataFr.GetLength(1) - 1; j++)
                    {
                        //если последняя строка отрицательна то не берём её
                        if (arrayDataFr[arrayDataFr.GetLength(0) - 1, j].GetNumerator() < 0)
                        {
                            if (arrayDataFr[i, j].GetNumerator() > 0)
                            {
                                if (B.Del(arrayDataFr[i, j]).Srav(minFr) == -1)
                                {
                                    minFr = B.Del(arrayDataFr[i, j]);
                                    minRow = i;
                                    minColumn = j;
                                }
                            }
                        }
                    }
                }
            }
            else
            {
                for (int i = 0; i < arrayData.GetLength(0) - 1; i++)
                {
                    double B = arrayData[i, arrayData.GetLength(1) - 1];
                    for (int j = 0; j < arrayData.GetLength(1) - 1; j++)
                    {
                        if (arrayData[i, j] > 0)
                        {
                            if ((B / arrayData[i, j]) < min)
                            {
                                min = (B / arrayData[i, j]);
                                minRow = i;
                                minColumn = j;
                            }
                        }
                    }
                }
            }
            

        }


        //на нажатие правой кнопки мыши вернёт позиции button в collectionButton
        private void SomeDoButton_PreviewMouseRightButtonDown(int i)
        {

            List<Button> CollectionButton = (List<Button>)dataBox[dataBox.Count() - 2];
            List<TextBox> col = (List<TextBox>)dataBox[dataBox.Count() - 1]; ;
            SetCollectionButton(new List<Button>());
            SetCollectionTextBox(new List<TextBox>());

            string[] words = CollectionButton[i].Name.Split('_');

            int column = int.Parse(words[1]) % 10;
            int row = int.Parse(words[1]) / 10;

            //меняем места обозначения индексов строки и столбца
            //int[] supportIntRow = GetCollectionIntRow();
            //int[] swapIntRow = new int[supportIntRow.Count()];

            //Проверка
            Console.WriteLine("CollectionRow 1");
            for (int k = 0; k < CollectionRow.Count(); k++)
            {
                int[] Col = CollectionRow[k];
                Console.WriteLine("Col i " + k);
                for (int j = 0; j < Col.Count(); j++)
                {
                    Console.WriteLine();
                    Console.Write(" " + int.Parse(Col[j].ToString()));
                }
            }


            int[] supportIntRow = GetCollectionIntRow();
            int[] supportColRow = GetCollectionIntColumn();
            int[] swapIntRow = new int[supportIntRow.Count()];
            int[] swapIntCol = new int[supportColRow.Count()];


            for (int j = 0; j < supportIntRow.Count(); j++)
            {
                swapIntRow[j] = supportIntRow[j];
            }

            for (int j = 0; j < supportColRow.Count(); j++)
            {
                swapIntCol[j] = supportColRow[j];
            }

            int swapColumn = swapIntCol[column];
            bool flagDelete = true;

            //базисные переменные
            flagDelete = false;
            swapIntCol[column] = swapIntRow[row];
            swapIntRow[row] = swapColumn;


            ArtificialBasisMethod step = new ArtificialBasisMethod();

           double[,] newTable;
            if (flagFra)
            {
                step.SetArray(arrayDataFr);
                newTable = step.Step(flagDelete, new double[1, 1], row, column, flagFra);
                arrayDataFr = step.getFr();

                Table(arrayDataFr.GetLength(0) - 1, getRange() + 44, arrayDataFr.GetLength(1), getStep() + 1, newTable, swapIntRow, swapIntCol);

            }
            else
            {
                newTable = step.Step(flagDelete, (double[,])(dataBox[dataBox.Count() - 3]), row, column, flagFra);
                //новая таблица
                Table(newTable.GetLength(0) - 1, getRange() + 44, newTable.GetLength(1), getStep() + 1, newTable, swapIntRow, swapIntCol);
            }

            //отправим наш массив на проверки конца решения
            step.SetArray(arrayDataFr);
            int switchCheck = 0;
            if (flagFra)
                switchCheck = step.Ending(newTable, true, true);
            else
                switchCheck = step.Ending(newTable, true, false);

            switch (switchCheck)
            {
                case -1:
                    Rezolt.Text = "Решение не найдено \n";
                    Rezolt.Background = Brushes.Red;
                    break;
                //case 0:
                //    Rezolt.Text = "Продолжаем решать";
                //    break;
                case 1:
                    //ответ
                    double[] answer = step.Answer(newTable, swapIntRow, getSizeX(), flagFra);
                    //проверка
                    if (flagFra)
                    {
                        Fraction[] answers = step.GetAns();
                        Console.WriteLine("Ответ внутри функции");
                        //выводим ответ
                        string str = "";
                        for (int j = 0; j < answers.Count(); j++)
                        {
                            string number = answers[j].GetNumerator().ToString();
                            if (answers[j].GetDenominatror() != 1) number  = number +'/'+answers[j].GetDenominatror();
                            str = str + "x" + (j + 1) + "=" + number + " ";
                        }
                        Fraction finish_sum = arrayDataFr[arrayDataFr.GetLength(0) - 1, arrayDataFr.GetLength(1) - 1].Mul(new Fraction(-1, 1));
                        Rezolt.Text = "Ответ получен \n" + str + "\n f = " + finish_sum.GetNumerator()+'/'+finish_sum.GetDenominatror();
                        Rezolt.Background = Brushes.LightGreen;
                        break;
                    }
                    else
                    {                      
                        //выводим ответ
                        string str = "";
                        for (int j = 0; j < answer.Count(); j++)
                        {
                            str = str + "x" + (j + 1) + "=" + answer[j] + " ";
                        }

                        double finish_sum = (-1) * newTable[newTable.GetLength(0) - 1, newTable.GetLength(1) - 1];

                        Rezolt.Text = "Ответ получен \n" + str + "\n" + finish_sum;
                        Rezolt.Background = Brushes.LightGreen;
                        break;
                    }
            }
        }
        private List<int[]> GetColRowAll() { return CollectionRow;  }
        //Шаг назад
        private void StepBack_Click(object sender, RoutedEventArgs e)
        {
            //Необходимо удалить последние кнопки и текст блоки
            List<TextBox> CollectionTextBoxs = (List<TextBox>)dataBox[dataBox.Count() - 1];
            List<Button> CollectionButtons = (List<Button>)dataBox[dataBox.Count() - 2];

            setStep(getStep() - 1);

            if (flagFra) setRange(getRange() - 44 - 22 * ((Fraction[,])dataBox[dataBox.Count() - 3]).GetLength(0));
            else setRange(getRange() - 44 - 22 * ((double[,])dataBox[dataBox.Count() - 3]).GetLength(0));

            CollectionIndexColumn.RemoveAt(CollectionIndexColumn.Count() - 1);
            CollectionRow.RemoveAt(CollectionRow.Count() - 1);

            //удаляем из памяти
            dataBox.RemoveAt(dataBox.Count() - 1);
            dataBox.RemoveAt(dataBox.Count() - 1);
            dataBox.RemoveAt(dataBox.Count() - 1);

            checkEnd = false;

            //может не работать
            if (flagFra) arrayDataFr = (Fraction[,])dataBox[dataBox.Count() - 3];
            //удаляем сами кнопки
            foreach (var button in CollectionButtons)
            {
                GridArtificzlBasis.Children.Remove(button);
            }

            foreach (var box in CollectionTextBoxs)
            {
                GridArtificzlBasis.Children.Remove(box);
            }

            //очищаем надпись
            Rezolt.Text = "";
            Rezolt.Background = Brushes.White;

            ///TableGrid.Children.Remove(CollectionBox[0]);
            /// TableGrid.Children.Remove(CollectionBox[1]);
        }

        //автоматический рассчёт
        private void Button_Click(object sender, RoutedEventArgs e)
        {

            //опередляем максимальный элемент
            ArtificialBasisMethod steps = new ArtificialBasisMethod();
            double[,] dataTable = new double[1,1];
            Fraction[,] dataTableFr = new Fraction[1,1];
            if (flagFra)
            {
               dataTableFr = (Fraction[,])dataBox[dataBox.Count() - 3];
            }
            else { dataTable = (double[,])dataBox[dataBox.Count() - 3]; }

            int row = 0;
            int column = 0;
            int[] swapIntRow = new int[1];
            int[] swapIntColumn;
            int[] swapIntCol = new int[1];

            steps.SetArray(dataTableFr);
            int switchCheck = 0;
            if (flagFra)
                switchCheck = steps.Ending(dataTable, false, true);
            else
                switchCheck = steps.Ending(dataTable, false, false);

            while (switchCheck == 0)
            {

                if (checkEnd) { checkEnd = false; return; }

                SetCollectionButton(new List<Button>());
                SetCollectionTextBox(new List<TextBox>());

                int[] supportIntRow = GetCollectionIntRow();
                swapIntRow = new int[supportIntRow.Count()];

                int[] supportIntCol = GetCollectionIntColumn();
                swapIntColumn = new int[supportIntCol.Count()]; 

                for (int j = 0; j < supportIntRow.Count(); j++)
                {
                    swapIntRow[j] = supportIntRow[j];
                }

                for (int j = 0; j < supportIntCol.Count(); j++)
                {
                    swapIntColumn[j] = supportIntCol[j];
                }


                bool flagDelete = true;

                if (flagFra)
                    MinElent(new double[1,1]);
                else
                    MinElent((double[,])dataBox[dataBox.Count() - 3]);

                //проходим по выбранному стоблцу и берём первый не нулевой элемент
                row = minRow;
                column = minColumn;

                //меняем места обозначения индексов строки и столбца
                //swapIntRow = CollectionRow[CollectionRow.Count() - 1];

                int swapColumn = swapIntColumn[column];

                //базисные переменные
                flagDelete = false;
                swapIntColumn[column] = swapIntRow[row];
                swapIntRow[row] = swapColumn;
                //новые рассчёты
                if (flagFra)
                {
                  
                    steps.SetArray((Fraction[,])dataBox[dataBox.Count() - 3]);
                    dataTable = steps.Step(flagDelete, new double[1, 1], row, column, flagFra);
                    arrayDataFr = steps.getFr();
                    Table(arrayDataFr.GetLength(0) - 1, getRange() + 44, arrayDataFr.GetLength(1), getStep() + 1, dataTable, swapIntRow, swapIntColumn);
                }
                else
                {
                    dataTable = steps.Step(flagDelete, (double[,])(dataBox[dataBox.Count() - 3]), row, column, flagFra);
                    Table(dataTable.GetLength(0) - 1, getRange() + 44, dataTable.GetLength(1), getStep() + 1, dataTable, swapIntRow, swapIntColumn);
                }

            }

            steps.SetArray(arrayDataFr);
            switchCheck = 0;
            if (flagFra)
                switchCheck = steps.Ending(dataTable, false, true);
            else
                switchCheck = steps.Ending(dataTable, false, false);
            //отправим наш массив на проверки конца решения
            switch (switchCheck)
            {
                case -1:
                    Rezolt.Text = "Решение не найдено \n";
                    Rezolt.Background = Brushes.Red;
                    break;
                case 1:
                    //ответ
                    double[] answer = steps.Answer(dataTable, swapIntRow, getSizeX(), flagFra);
                    //выводим ответ
                    if (flagFra)
                    {
                        Fraction[] answers = steps.GetAns();

                        //выводим ответ
                        string str = "";
                        for (int j = 0; j < answers.Count(); j++)
                        {
                            string number = answers[j].GetNumerator().ToString();
                            if (answers[j].GetDenominatror() != 1) number = number + '/' + answers[j].GetDenominatror();
                            str = str + "x" + (j + 1) + "=" + number + " ";
                        }
                        Fraction finish_sum = arrayDataFr[arrayDataFr.GetLength(0) - 1, arrayDataFr.GetLength(1) - 1].Mul(new Fraction(-1, 1));
                        Rezolt.Text = "Ответ получен \n" + str + "\n f = " + finish_sum.GetNumerator() + '/' + finish_sum.GetDenominatror();
                        Rezolt.Background = Brushes.LightGreen;
                        break;
                    }
                    else
                    {
                        //выводим ответ
                        string str = "";
                        for (int j = 0; j < answer.Count(); j++)
                        {
                            str = str + "x" + (j + 1) + "=" + answer[j] + " ";
                        }

                        double finish_sum = (-1) * dataTable[dataTable.GetLength(0) - 1, dataTable.GetLength(1) - 1];

                        Rezolt.Text = "Ответ получен \n" + str + "\n" + finish_sum;
                        Rezolt.Background = Brushes.LightGreen;
                        break;
                    }

            }

            //если нет решения вернуться назад взять дургое (если дошли до самого конца решений нет)
            //если получен ответ то закончить
        }
    }
}
