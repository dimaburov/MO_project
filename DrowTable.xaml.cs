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
    /// Interaction logic for DrowTable.xaml
    /// </summary>
    public partial class DrowTable : Window
    {
        //храним все данные о этапах
        List<object> dataBox = new List<object>();

        //временное хранение кнопок
        List<Button> CollectionButton = new List<Button>();
        //временное хранение полей
        List<TextBox> CollectionTextBox = new List<TextBox>();
        //хранение индексов
        List<int[]> CollectionIndexColumn = new List<int[]>();

        Fraction[] arrayBaseFr = new Fraction[1];

        // List<RowInt> Collection = new List<RowInt>();

        List<int[]> CollectionRow = new List<int[]>();
        //хранение индексов стобцов и строк
        private int[] intRow;
        private int[] intColumn;
        private int step;
        private int range;
        private int sizeX;

        private int baseRow;

        //хранение целевой функции
        private double[] targetFunction;

        private bool FlagFr;
        private Fraction[,] arrayDataFr;

        public void SetArrayBaseFr(Object newArray) { arrayBaseFr = (Fraction[])newArray; }

        public void SetFlagFr(bool newFlag) { FlagFr = newFlag; }
        public void SetArrayDataFr(Object[,] newFraction) { arrayDataFr = (Fraction[,])newFraction; }

        //быстро и не красиво
        private int[] basEl;
        private int[] basElFr;

        public DrowTable()
        {
            InitializeComponent();

            SimplexMethod.IsEnabled = false;
        }

        public int GetBaseRow() { return baseRow; }
        public void SetBaseRow(int newBaseRow)
        {
            baseRow = newBaseRow;
        }

        private int[] GetCollectionIntRow() { return CollectionRow[CollectionRow.Count() - 1]; }
        private void SetCollectionIntRow(int[] newIntRow)
        {
            CollectionRow.Add(newIntRow);
        }

        private int[] GetCollectionIntColumn() { return CollectionIndexColumn[CollectionIndexColumn.Count()-1]; }
        private void SetCollectionIntColumn(int[] newIntColumn)
        {
            CollectionIndexColumn.Add(newIntColumn);
        }
        //get и set для CollectionButton
        private List<Button> GetCollectionButton() { return CollectionButton; }
        private void SetCollectionButton(List<Button> newCollectionButton) { CollectionButton = newCollectionButton;}

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
            if (FlagFr) dataBox.Add(arrayDataFr);
            else dataBox.Add(arrayData);

            //новое
            BasickElem(arrayData);
            //наилучший опроный жлемент
            //double max = MaxElent(arrayData, row, column);

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
            for (int i = 0; i < row; i++)
            {
                Row(column + i, column, 22 * (i+1)+location, intRow, intColumn, arrayData, row);
            }
            //i+1 строка
            Row(- 1, column, 22 * (row+1) + location, intRow, intColumn, arrayData, row);
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


            for (int i = 0; i < count+1; i++)
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
                row.Margin = new Thickness(5+i * 32, range, 0, 0);

                Button but = new Button();

                //оформляем заполнение данных
                if (step == -1)
                {
                    if (i == 0) row.Text = "";
                    else
                    {
                        if (FlagFr)
                        {
                            if (arrayDataFr[arrayDataFr.GetLength(0) - 1, i - 1].GetDenominatror() == 1) row.Text = arrayDataFr[arrayDataFr.GetLength(0) - 1, i - 1].GetNumerator().ToString();
                            else row.Text = arrayDataFr[arrayDataFr.GetLength(0) - 1, i - 1].GetNumerator().ToString() + '/' + arrayDataFr[arrayDataFr.GetLength(0) - 1, i - 1].GetDenominatror().ToString();
                        }
                        else row.Text = arrayData[arrayData.GetLength(0) - 1, i - 1].ToString();
                    }
                    //добавление бокса
                    DrowTableGrid.Children.Add(row);
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
                    DrowTableGrid.Children.Add(row);
                    Collection.Add(row);
                }
                else
                {
                    if (i == 0)
                    {

                        row.Text = "x" + intRow[step- count].ToString();
                        //добавление бокса
                        DrowTableGrid.Children.Add(row);
                        Collection.Add(row);
                    }
                    else if (i == count)
                    {
                        if (FlagFr)
                        {
                            if (arrayDataFr[step - count, i - 1].GetDenominatror() == 1) row.Text = arrayDataFr[step - count, i - 1].GetNumerator().ToString();
                            else row.Text = arrayDataFr[step - count, i - 1].GetNumerator().ToString() + '/' + arrayDataFr[step - count, i - 1].GetDenominatror().ToString();
                        }
                        else row.Text = arrayData[step - count, i - 1].ToString();
                        //добавление бокса
                        DrowTableGrid.Children.Add(row);
                        Collection.Add(row);
                    }
                    else
                    {
                        but.FontSize = 12;
                        but.Width = 32;
                        but.Height = 22;
                        but.VerticalAlignment = VerticalAlignment.Top;
                        but.HorizontalAlignment = HorizontalAlignment.Left;
                        but.Name = "test_"+ ((step - count)*10+ i - 1);

                        //поменяем цвет (что то типо светло зелёного для обычного и синий для лучшего выбор
                        //if (arrayData[step - count, i - 1] == max) but.Background = Brushes.LightBlue;
                        but.Background = Brushes.LightGreen;
                        but.Margin = new Thickness(5 + i * 32, range, 0, 0);
                        //myGrid.Children.Add(but);
                        if (FlagFr)
                        {
                            if (arrayDataFr[step - count, i - 1].GetDenominatror() == 1) but.Content = arrayDataFr[step - count, i - 1].GetNumerator().ToString();
                            else but.Content = arrayDataFr[step - count, i - 1].GetNumerator().ToString() + '/' + arrayDataFr[step - count, i - 1].GetDenominatror().ToString();
                        } 
                        else but.Content = arrayData[step - count, i - 1].ToString();
                        //добавление кнопки
                        //если число положительное то мы делаем кнопку для возможного выбора
                        if (FlagFr)
                        {
                            if (arrayDataFr[step - count, i - 1].GetNumerator() != 0 && basElFr[i - 1] == step - count && arrayDataFr[step - count, i - 1].GetNumerator() > 0)
                            {

                                //добавляем кнопку в коллекию
                                CollectionButtons.Add(but);

                                int temp = CollectionButtons.Count() - 1;

                                but.MouseRightButtonDown +=
                               (sender, args) => SomeDoButton_PreviewMouseRightButtonDown(temp);

                                DrowTableGrid.Children.Add(but);
                            }
                            else
                            {
                                if (arrayDataFr[step - count, i - 1].GetDenominatror() == 1) row.Text = arrayDataFr[step - count, i - 1].GetNumerator().ToString();
                                else row.Text = arrayDataFr[step - count, i - 1].GetNumerator().ToString()+'/'+ arrayDataFr[step - count, i - 1].GetDenominatror().ToString();
                                //добавление бокса
                                DrowTableGrid.Children.Add(row);
                                Collection.Add(row);
                            }
                        }
                        else
                        {
                            //внести изменения (в строке step-count может быть только 1 элемент для выбора )
                            if (arrayData[step - count, i - 1] != 0 && basEl[i - 1] == step - count && arrayData[step - count, i - 1] > 0)
                            {
                                //добавляем кнопку в коллекию
                                CollectionButtons.Add(but);

                                int temp = CollectionButtons.Count() - 1;

                                but.MouseRightButtonDown +=
                               (sender, args) => SomeDoButton_PreviewMouseRightButtonDown(temp);

                                DrowTableGrid.Children.Add(but);
                            }

                            else
                            {
                                row.Text = arrayData[step - count, i - 1].ToString();
                                //добавление бокса
                                DrowTableGrid.Children.Add(row);
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
            if (FlagFr)
            {//для дробей

                int[] basFr = new int[arrayDataFr.GetLength(1)-1];

                Fraction minEl = new Fraction(10000, 1); int rowBas=0;
                for (int i = 0; i < arrayDataFr.GetLength(1) -1 ; i++)
                {
                    for (int j = 0; j < arrayDataFr.GetLength(0)-1; j++)
                    {
                        //переменныйе
                        Fraction number = arrayDataFr[j, i].ABS();
                        Fraction basNumber = arrayDataFr[j, arrayDataFr.GetLength(1) - 1].ABS();
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
                    Console.Write(" "+ basFr[i]);
                }

            }
            else
            {
                Console.WriteLine("Standart");
                int[] bas = new int[matrix.GetLength(1)-1];

                int rowBas = 0;
                double minEl = 10000;
                for (int i = 0; i < matrix.GetLength(1) -1 ; i++)
                {
                    for (int j = 0; j < matrix.GetLength(0)-1; j++)
                    {
                        double number = Math.Abs(matrix[j, i]);
                        double numberDel = Math.Abs(matrix[j, matrix.GetLength(1) - 1]);

                        if (matrix[j, i] > 0)
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

        //необходимо определить максимальный элемент оmax сновной таблицы для наилучшего опроного элемента
        //вернём значение этого элемента
        private int MaxElent(int[] ColumnBasic)
        {
            for (int i = 0; i < ColumnBasic.Count(); i++)
            {
                if (ColumnBasic[i] > GetBaseRow())
                {
                    return ColumnBasic[i];
                }
            }
            return -1;
        }


        //на нажатие правой кнопки мыши вернёт позиции button в collectionButton
        private void SomeDoButton_PreviewMouseRightButtonDown(int i)
        {
            Console.WriteLine("!!Искуственный баззис!!");
            List<Button> CollectionButton = (List<Button>)dataBox[dataBox.Count()-2];
            List<TextBox> col = (List<TextBox>)dataBox[dataBox.Count() - 1]; ;
            SetCollectionButton(new List<Button>());
            SetCollectionTextBox(new List<TextBox>());

            string[] words = CollectionButton[i].Name.Split('_');

            int column = int.Parse(words[1]) % 10;
            int row = int.Parse(words[1])/10;

            //меняем места обозначения индексов строки и столбца
            int[] supportIntRow = GetCollectionIntRow();
            int[] swapIntRow = new int[supportIntRow.Count()];

            for (int j = 0; j < supportIntRow.Count(); j++)
            {
                swapIntRow[j] = supportIntRow[j];
            }

            //проверка
            Console.WriteLine("supportIntRow");
            for (int j = 0; j < supportIntRow.Count(); j++)
            {
                Console.Write(" "+ supportIntRow[j]);
            }

            //проверка
            Console.WriteLine("swapIntRow");
            for (int j = 0; j < swapIntRow.Count(); j++)
            {
                Console.Write(" " + swapIntRow[j]);
            }


            int[] swapIntColumn = GetCollectionIntColumn();
            int swapColumn = swapIntColumn[column];

            bool flagDelete = true;

            //базисные переменные
            if (swapColumn <= GetBaseRow() && swapIntRow[row] <= GetBaseRow())
            {
                flagDelete = false;
                swapIntColumn[column] = swapIntRow[row];
                swapIntRow[row] = swapColumn;
            }
            else
            {
                flagDelete = true;
                swapIntColumn = swapIntColumn.Where((val, idx) => idx != column).ToArray();
                swapIntRow[row] = swapColumn;
            }
            //проверка
            Console.WriteLine("Swap Row Column");
            Console.WriteLine("supportIntRow");
            for (int j = 0; j < supportIntRow.Count(); j++)
            {
                Console.Write(" " + supportIntRow[j]);
            }

            //проверка
            Console.WriteLine("swapIntRow");
            for (int j = 0; j < swapIntRow.Count(); j++)
            {
                Console.Write(" " + swapIntRow[j]);
            }

            //метод искуственного базиса
            ArtificialBasisMethod step = new ArtificialBasisMethod();
            double[,] newTable;
            if (FlagFr)
            {
                step.SetArray(arrayDataFr);
                newTable = step.Step(flagDelete, new double[1, 1], row, column, FlagFr);
                arrayDataFr = step.getFr();

                Table(arrayDataFr.GetLength(0) - 1, getRange() + 44, arrayDataFr.GetLength(1), getStep() + 1, newTable, swapIntRow, swapIntColumn);

            }
            else
            {
                newTable = step.Step(flagDelete, (double[,])(dataBox[dataBox.Count() - 3]), row, column, FlagFr);
                //новая таблица
                Table(newTable.GetLength(0) - 1, getRange() + 44, newTable.GetLength(1), getStep() + 1, newTable, swapIntRow, swapIntColumn);
            }
            //отправим наш массив на проверки конца решения
            step.SetArray(arrayDataFr);
            int switchCheck = 0;
            if (FlagFr)
                switchCheck = step.Ending(newTable, false, true);
            else
                switchCheck = step.Ending(newTable, false, false);

            //Console.WriteLine("switchCheck "+ switchCheck);
            //for (int j = 0; j <arrayDataFr.GetLength(0); j++)
            //{
            //    Console.WriteLine();
            //    for (int k = 0; k < arrayDataFr.GetLength(1); k++)
            //    {
            //        Console.Write(" "+arrayDataFr[j,k].GetNumerator()+"/"+ arrayDataFr[j, k].GetDenominatror());
            //    }
            //}
            
            switch (switchCheck)
            {
                case 1:
                    //ответk
                    SimplexMethod.IsEnabled = true;
                    Rezolt.Text = "Ответ получен \n";
                    Rezolt.Background = Brushes.LightGreen;
                    break;

            }
        }

        //Шаг назад
        private void StepBack_Click(object sender, RoutedEventArgs e)
        {
            //Необходимо удалить последние кнопки и текст блоки
            List<TextBox> CollectionTextBoxs = (List<TextBox>)dataBox[dataBox.Count() - 1];
            List<Button> CollectionButtons = (List<Button>)dataBox[dataBox.Count() - 2];

            setStep(getStep() - 1);
            if(FlagFr) setRange(getRange() - 44 - 22 * ((Fraction[,])dataBox[dataBox.Count() - 3]).GetLength(0));
            else setRange(getRange()- 44 - 22 *((double[,])dataBox[dataBox.Count() - 3]).GetLength(0));

            SimplexMethod.IsEnabled = false;

            CollectionIndexColumn.RemoveAt(CollectionIndexColumn.Count()-1);
            CollectionRow.RemoveAt(CollectionRow.Count()-1);

            //удаляем из памяти
            dataBox.RemoveAt(dataBox.Count() - 1);
            dataBox.RemoveAt(dataBox.Count() - 1);
            dataBox.RemoveAt(dataBox.Count() - 1);

            //может не работать
            if(FlagFr) arrayDataFr = (Fraction[,])dataBox[dataBox.Count()-3];

            //удаляем сами кнопки
            foreach (var button in CollectionButtons)
            {
                DrowTableGrid.Children.Remove(button);
            }

            foreach (var box in CollectionTextBoxs)
            {
                DrowTableGrid.Children.Remove(box);
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
            Fraction[,] dataTableFr = new Fraction[1, 1];

            if (FlagFr) { dataTableFr = (Fraction[,])(dataBox[dataBox.Count() - 3]); }
            else { dataTable = (double[,])dataBox[dataBox.Count() - 3]; }

            int row = 0;
            int column = 0;
            int[] swapIntRow = new int[1];
            int[] swapIntColumn;

            steps.SetArray(dataTableFr);
            int switchCheck = 0;
            if (FlagFr)
                switchCheck = steps.Ending(dataTable, false, true);
            else
                switchCheck = steps.Ending(dataTable, false, false);

            //Пробник
            int step = 0;
            while (switchCheck == 0)
            {
                Console.WriteLine("!!While!!");
                if (FlagFr)
                {    
                    if (dataTableFr.GetLength(1) <= 1)
                    {
                        Console.WriteLine("!!<=1");
                        break;
                    }
                    
                }
                else
                {
                    if (dataTable.GetLength(1) < 2) break;
                }
                SetCollectionButton(new List<Button>());
                SetCollectionTextBox(new List<TextBox>());

                int[] supportIntRow = GetCollectionIntRow();
                swapIntRow = new int[supportIntRow.Count()];

                for (int j = 0; j < supportIntRow.Count(); j++)
                {
                    swapIntRow[j] = supportIntRow[j];
                }

                if (step == GetCollectionIntRow().Count()-1) break;
                else step++;
               

                bool flagDelete = true;

                //int max = MaxElent(swapIntRow);

                //пробуем иначе вернём массив с возможными элементами и выберем первый из них 
                BasickElem(dataTable);


                //int index = 0;
                //for (int i = 0; i < swapIntRow.Count(); i++)
                //{
                //    if (swapIntRow[i] == max) index = i;
                //}

                //if (max == -1)
                //{
                //    Console.WriteLine("Нет такого элемента");
                //    break ;
                //}
                       
                if (FlagFr)
                {
                    ////проходим по выбранному стоблцу и берём первый не нулевой элемент
                    //for (int i = 0; i < dataTableFr.GetLength(1); i++)
                    //{
                    //    if (dataTableFr[index, i].GetNumerator() != 0)
                    //    {
                    //        row = index;
                    //        column = i;
                    //        break;
                    //    }
                    //}
                    
                    column = 0;
                    row = basElFr[0];
                }
                else
                {
                    //проходим по выбранному стоблцу и берём первый не нулевой элемент
                    //for (int i = 0; i < dataTable.GetLength(1); i++)
                    //{
                    //    if (dataTable[index, i] != 0)
                    //    {
                    //        row = index;
                    //        column = i;
                    //        break;
                    //    }
                    //}
                    column = 0;
                    row = basEl[0];
                }

                swapIntColumn = GetCollectionIntColumn();
                int swapColumn = swapIntColumn[column];

                //меняем места обозначения индексов строки и столбца
                //swapIntRow = CollectionRow[CollectionRow.Count() - 1];


                //базисные переменные
                if (swapColumn <= GetBaseRow() && swapIntRow[row] <= GetBaseRow())
                {
                    flagDelete = false;
                    swapIntColumn[column] = swapIntRow[row];
                    swapIntRow[row] = swapColumn;
                }
                else
                {
                    flagDelete = true;
                    swapIntColumn = swapIntColumn.Where((val, idx) => idx != column).ToArray();
                    swapIntRow[row] = swapColumn;
                }

                if (FlagFr)
                {
                    steps.SetArray((Fraction[,])dataBox[dataBox.Count() - 3]);
                    dataTable = steps.Step(flagDelete, new double[1,1], row, column, FlagFr);
                    arrayDataFr = steps.getFr();

                    Table(arrayDataFr.GetLength(0) - 1, getRange() + 44, arrayDataFr.GetLength(1), getStep() + 1, dataTable, swapIntRow, swapIntColumn);
                }
                else 
                {
                    dataTable = steps.Step(flagDelete, (double[,])(dataBox[dataBox.Count() - 3]), row, column, FlagFr);
                    Table(dataTable.GetLength(0) - 1, getRange() + 44, dataTable.GetLength(1), getStep() + 1, dataTable, swapIntRow, swapIntColumn);
                }

                //новые рассчёты
                //dataTable = steps.Step(flagDelete , (double[,])(dataBox[dataBox.Count() - 3]), row, column, FlagFr);
                //новая таблица
               

            }
            steps.SetArray(arrayDataFr);
            switchCheck = 0;
            if (FlagFr)
                switchCheck = steps.Ending(dataTable, false, true);
            else
                switchCheck = steps.Ending(dataTable, false, false);
            Console.WriteLine("switchCheck "+ switchCheck);
            //отправим наш массив на проверки конца решения
            switch (switchCheck)
            {
                case 1:
                    //ответ
                    double[] answer = steps.Answer(dataTable, swapIntRow, getSizeX(), FlagFr);
                    //выводим ответ
                    SimplexMethod.IsEnabled = true;
                    SimplexMethod.Background = Brushes.LightGreen;

                    Rezolt.Text = "Ответ получен \n";
                    Rezolt.Background = Brushes.LightGreen;
                    break;

            }
        }
        //переходим к симплекс методу
        private void SimplexMethod_Click(object sender, RoutedEventArgs e)
        {
            ArtificialBasis simplecs = new ArtificialBasis();
            //сначала решаем систему с выбранными базисными элементами
            Gause gause = new Gause();
            double[,] matrix2 = new double[1,1];
            Fraction[,] matrix2Fr = new Fraction[1, 1];

            if (FlagFr)
            {
                matrix2Fr = (Fraction[,])(dataBox[dataBox.Count() - 3]);
            }
            else
            {
                matrix2 = (double[,])(dataBox[dataBox.Count() - 3]);
            }

            int[] IntRowBas = GetCollectionIntRow();
            int[] IntColumnBas = GetCollectionIntColumn();
            //нам нужна матрица без последней дополнитейльной строки
            if (FlagFr == true)
            {
              

                Fraction[,] matrix3Fr = new Fraction[matrix2Fr.GetLength(0) - 1, matrix2Fr.GetLength(1)];

                for (int i = 0; i < matrix3Fr.GetLength(0); i++)
                {
                    for (int j = 0; j < matrix3Fr.GetLength(1); j++)
                    {
                        matrix3Fr[i, j] = matrix2Fr[i, j];
                    }
                }

             

                gause.SetFr(matrix3Fr);
                gause.SetBaseFun(arrayBaseFr);

                //matrix2 = gause.Method(matrix2, bases, true);

                //matrix2Fr = gause.GetFr();

                double[] support = gause.TargetFunction(getTargetFunction(), new double[1,1], IntRowBas, true);

                Fraction[] newBaeseArray = gause.getBasesFun();
                //добавляем новую целевую функцию в конец
                Fraction[,] matrixAddNewFunction = new Fraction[matrix2Fr.GetLength(0), matrix2Fr.GetLength(1)];
                for (int i = 0; i < matrixAddNewFunction.GetLength(0) - 1; i++)
                {
                    for (int j = 0; j < matrixAddNewFunction.GetLength(1); j++)
                    {
                        matrixAddNewFunction[i, j] = matrix2Fr[i, j];
                    }
                }
                for (int i = 0; i < matrixAddNewFunction.GetLength(1); i++)
                {
                    matrixAddNewFunction[matrixAddNewFunction.GetLength(0) - 1, i] = newBaeseArray[i];
                }

                simplecs.SetFlagFr(true);
                simplecs.SetFr(matrixAddNewFunction);

                simplecs.SetBaseRow(IntRowBas.Count() - 1);
                simplecs.setSizeX(intRow.Count() + intColumn.Count() - 2);
                simplecs.Table(matrixAddNewFunction.GetLength(0) - 1, 44, matrixAddNewFunction.GetLength(1), 0, matrix2, IntRowBas, IntColumnBas);
                //simplecs.setTargetFunction(arrayBase);

                simplecs.Show();
            }
            else
            {
                //удаляем в матрице посленюю строку
                double[,] matrixDelColumn = new double[matrix2.GetLength(0) - 1, matrix2.GetLength(1)];

                for (int i = 0; i < matrixDelColumn.GetLength(0); i++)
                {
                    for (int j = 0; j < matrixDelColumn.GetLength(1); j++)
                    {
                        matrixDelColumn[i, j] = matrix2[i, j];
                    }
                }

                double[] new_target = gause.TargetFunction(getTargetFunction(), matrixDelColumn, IntRowBas, false);

                //добавляем новую целевую функцию в конец
                double[,] matrixAddNewFunction = new double[matrix2.GetLength(0), matrix2.GetLength(1)];
                for (int i = 0; i < matrixAddNewFunction.GetLength(0) - 1; i++)
                {
                    for (int j = 0; j < matrixAddNewFunction.GetLength(1); j++)
                    {
                        matrixAddNewFunction[i, j] = matrix2[i, j];
                    }
                }
                for (int i = 0; i < matrixAddNewFunction.GetLength(1); i++)
                {
                    matrixAddNewFunction[matrixAddNewFunction.GetLength(0) - 1, i] = new_target[i];
                }

                simplecs.SetBaseRow(IntRowBas.Count() - 1);
                simplecs.setSizeX(intRow.Count() + intColumn.Count() - 2);
                simplecs.Table(matrix2.GetLength(0) - 1, 44, matrix2.GetLength(1), 0, matrixAddNewFunction, IntRowBas, IntColumnBas);
                //simplecs.setTargetFunction(arrayBase);

                simplecs.Show();

            }
        }
    }
}
