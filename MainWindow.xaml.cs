using System;
using System.Collections.Generic;
using System.Data;
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
using System.Windows.Navigation;
using System.Windows.Shapes;


namespace MO_project
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        List<TextBox> CollectionBoxItem = new List<TextBox>();

        TextBox boxBasis = new TextBox();

        public MainWindow()
        {
            InitializeComponent();
            //Стартовая чистка файла
            dataPrint print = new dataPrint();
            print.Clear();
        }
        //Рисование таблицы для заполнения данных
        //variables  NumberOfVariables.SelectedIndex + 2
        //limitations NumberOfVariablesRestrictions.SelectedIndex +4
        private void Table(int variables, int limitations)
        { 

            List<TextBox> CollectionBox = new List<TextBox>();

            //column
            String name = "";
            String nameRow = "x";
            Boolean ColourFlag=true;
            for (int i = 1; i <= limitations + 3; i++)
            {
                switch (i) {
                    case 2:
                        name = "f(x̅)" + i;
                        nameRow = "";
                        ColourFlag = false;
                        break;
                    case 3:
                        name = "";
                        nameRow = "a";
                        ColourFlag = true;
                        break;
                }
                if (i > 3)
                {
                    name = "f"+(i-3)+"(x̅)";
                    nameRow = "";
                    ColourFlag = false;
                }
                //row data i
                CollectionBox =Row(CollectionBox, nameRow, i*22, ColourFlag, variables);
                //create column
                TextBox column = new TextBox();
                column.FontSize = 12;
                column.Width = 32;
                column.Height = 22;
                column.VerticalAlignment = VerticalAlignment.Top;
                column.HorizontalAlignment = HorizontalAlignment.Left;
                column.TextAlignment = TextAlignment.Center;
                column.TextWrapping = TextWrapping.Wrap;
                column.Background = Brushes.LightGray;
                column.Margin = new Thickness(0, i*22, 0, 0);
                column.Text = name ;
                TableGrid.Children.Add(column);
            }
            //новая коллекция textBox
            SetCollection(CollectionBox);
        }
        //Заполнение 0 столбца таблицы
        private List<TextBox> Row(List<TextBox> CollectionBox, String name, int range, Boolean data , int variables)
        {
            for (int i = 1; i <=variables; i++)
            {
                TextBox row = new TextBox();
                row.FontSize = 12;
                row.Width = 32;
                row.Height = 22;
                row.VerticalAlignment = VerticalAlignment.Top;
                row.HorizontalAlignment = HorizontalAlignment.Left;
                row.TextAlignment = TextAlignment.Center;
                row.TextWrapping = TextWrapping.Wrap;
                if (data) { row.Background = Brushes.LightGray; }
                else { row.Background = Brushes.White; }
                row.Margin = new Thickness(i * 32, range, 0, 0);
                //if (i == NumberOfVariables.SelectedIndex + 2 && range==22) row.Text = "c";
                //if (i == NumberOfVariables.SelectedIndex + 2 && range ==66) row.Text = "b";
                if (i == variables && range==22) row.Text = "c";
                if (i == variables && range ==66) row.Text = "b";
                if (range == 44 || range > 66)
                {
                    row.Text = name;
                    CollectionBox.Add(row);
                }
                else
                {
                    if (i != variables) row.Text = name + i.ToString();
                }
                TableGrid.Children.Add(row);
            }
            return CollectionBox;
        }
        //храним коллекцию textBox
        private List<TextBox> GetCollection()
        {
            return CollectionBoxItem;
        }
        private void SetCollection(List<TextBox> NewCollectionBox)
        {
           CollectionBoxItem = NewCollectionBox;
        }
        //Удаление элементов на TableGrid
        private void ClearGrid()
        {
            TableGrid.Children.Clear();
        }

        //Нажатие кнопки создания таблицы
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            ClearGrid();
            Table(NumberOfVariables.SelectedIndex + 2, NumberOfVariablesRestrictions.SelectedIndex + 1);
        }

        

        //Запись данных в файл
        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            dataPrint print = new dataPrint();
            List<TextBox> CollectionBox = GetCollection();
            List<int> array = new List<int>();
            List<Fraction> arrayFr = new List<Fraction>();
            //условия задачи !!!Будут дополнены!!!!
            String text = (NumberOfVariables.SelectedIndex + 1).ToString();
            text = text + " " + (NumberOfVariablesRestrictions.SelectedIndex + 1).ToString();
            //Min <-> Max
            if(CheckMin.IsChecked == true)  text = text + " " + 1;
            else text = text + " " + 0;
            //Обыкновенная <-> Десятичная
            if (OrdinaryFraction.IsChecked == true) text = text + " " + 1;
            else text = text + " " + 0;
            //данные таблицы

            print.flagFr = false;
            var ch = new[] { '/' };

            for (int i = 0; i < CollectionBox.Count; i++)
            {
                String str = CollectionBox[i].Text;
                //проверка
                if (str == "") MessageBox.Show("Таблица не заполнена");
                else
                {
                    if(OrdinaryFraction.IsChecked == true)
                    {
                        print.flagFr = true;
                        if (ch.Any(str.Contains))
                        {
                            string[] num_denom = str.Split('/');
                            arrayFr.Add(new Fraction(int.Parse(num_denom[0]), int.Parse(num_denom[1])));
                        }
                        else
                        {
                            arrayFr.Add(new Fraction(int.Parse(str), 1));
                        }
                       
                    }
                    else array.Add(int.Parse(str));
                }
            }
            print.ArrayDataFr = arrayFr ;
            print.Write(text, array, NumberOfVariables.SelectedIndex + 2);
        }

        //загрузить задачу из файла
        private void Button_Click_2(object sender, RoutedEventArgs e)
        {
            dataPrint print = new dataPrint();
            List<int> array = new List<int>();
            List<Fraction> arrayFr = new List<Fraction>();

            array = print.Read();

            arrayFr = print.arrayFrac;
            Console.WriteLine("arrayFr");
            Console.WriteLine();
            for (int i = 0; i < arrayFr.Count(); i++)
            {
                Console.WriteLine(arrayFr[i].GetNumerator()+'/'+ arrayFr[i].GetDenominatror());
            }

            //проверка пустоты файла
            if (array.Count() == 0) MessageBox.Show("Файл с задачей пуст");
            else
            {
                //Создаём таблицу
                ClearGrid();
                Table(array[0]+1, array[1]);
                //Берём созданные TextBox
                List<TextBox> CollectionBox = GetCollection();
                //Переносим данные из файла на форму
                if(array[3] == 1)
                {
                    for (int i = 0; i < arrayFr.Count(); i++)
                    {
                        if (arrayFr[i].GetDenominatror() == 1)
                        {
                            CollectionBox[i].Text = arrayFr[i].GetNumerator().ToString();
                        }
                        else CollectionBox[i].Text = arrayFr[i].GetNumerator()+"/" + arrayFr[i].GetDenominatror();
                    }
                }
                else
                {
                    for (int i = 4; i < array.Count(); i++)
                    {
                        CollectionBox[i - 4].Text = array[i].ToString();
                    }
                }
               

                NumberOfVariables.SelectedIndex = array[0] - 1;
                NumberOfVariablesRestrictions.SelectedIndex = array[1] - 1;

                //
                if (array[2] == 1) { OrdinaryFraction.IsChecked = true; NoFract.IsChecked = false; }
                else
                {
                    OrdinaryFraction.IsChecked = false;
                    NoFract.IsChecked = true;
                }
                //
                if (array[3] == 1) { CheckMax.IsChecked = false; CheckMin.IsChecked = true; }
                else { CheckMax.IsChecked = true; CheckMin.IsChecked = false; }

                Console.WriteLine(" NumberOfVariables.SelectedIndex " + NumberOfVariables.SelectedIndex);
                Console.WriteLine(" NumberOfVariablesRestrictions.SelectedIndex " + NumberOfVariablesRestrictions.SelectedIndex);

                SetCollection(CollectionBox);
            }
        }

        private void Start_Click(object sender, RoutedEventArgs e)
        {
            //проверка что таблица с данными заполнена и перенос данных

           
            int column = NumberOfVariables.SelectedIndex + 2;
            int row = NumberOfVariablesRestrictions.SelectedIndex + 1;
            double[,] arrayData = new double[row+1, column];

            double[] arrayBase = new double[column];

            //Берём созданные TextBox
            List<TextBox> CollectionBox = GetCollection();

            //переносим данные из textblock в массив
            int count = 0;

            //создади отдельный случай когда дроби обыкновенные
            //переменные для дробей 
            Fraction[] arrayBaseFr = new Fraction[column];
            Fraction[,] arrayDataFr = new Fraction[row + 1, column];
            var ch = new[] { '/' };

            if (OrdinaryFraction.IsChecked == true)
            {
                for (int i = 0; i < column; i++, count++)
                {
                    //если в строке есть / делим на числитель и знаменатель
                    if (ch.Any(CollectionBox[count].Text.Contains))
                    {
                        //делим строку на числитель и знаменатель
                        string[] num_denom = CollectionBox[count].Text.Split('/');
                        //заносим данные 
                        arrayBaseFr[i] = new Fraction(int.Parse(num_denom[0]), int.Parse(num_denom[1]));
                    }
                    else
                    {
                        arrayBaseFr[i] = new Fraction(int.Parse(CollectionBox[count].Text), 1);
                    }
                }

            }
            else
            {
                for (int i = 0; i < column; i++, count++)
                {
                    arrayBase[i] = double.Parse(CollectionBox[count].Text);
                }
            }

            //MIN <-> MAX
            if (CheckMax.IsChecked == true)
            {
                //если необходимо едти к MAX, то домножаем на -1 целевую функцию
                if(OrdinaryFraction.IsChecked == true)
                {
                    for (int j = 0; j < arrayBaseFr.Count(); j++)
                    {
                        arrayBaseFr[j]= arrayBaseFr[j].Mul(new Fraction(-1,1));
                    }
                }
                else
                {
                    for (int j = 0; j < arrayBase.Count(); j++)
                    {
                        arrayBase[j] *= -1;
                    }
                }
            }

            for (int i = 0; i < row; i++)
            {
                for (int j = 0; j < column; j++)
                {
                    //проверка на заполненноыть
                    if (CollectionBox[count].Text == "") MessageBox.Show("Таблица не заполнена");
                    else
                    {
                        if (OrdinaryFraction.IsChecked == true)
                        {
                            //если в строке есть / делим на числитель и знаменатель
                            if (ch.Any(CollectionBox[count].Text.Contains))
                            {
                                //делим строку на числитель и знаменатель
                                string[] num_denom = CollectionBox[count].Text.Split('/');
                                //заносим данные 
                                arrayDataFr[i, j] = new Fraction(int.Parse(num_denom[0]), int.Parse(num_denom[1]));

                                //Если в последнем столбце отр число умножаем на -1
                                if(j == (column - 1))
                                {
                                    if(arrayDataFr[i, j].GetNumerator() < 0)
                                    {
                                        for (int k = 0; k < column; k++)
                                        {
                                            arrayDataFr[i, k] = arrayDataFr[i,k].Mul(new Fraction(-1,1));
                                        }
                                    }
                                }
                            }
                            else
                            {
                                arrayDataFr[i, j] = new Fraction(int.Parse(CollectionBox[count].Text), 1);

                                if (j == (column - 1))
                                {
                                    if (arrayDataFr[i, j].GetNumerator() < 0)
                                    {
                                        for (int k = 0; k < column; k++)
                                        {
                                            arrayDataFr[i, k] = arrayDataFr[i, k].Mul(new Fraction(-1, 1));
                                        }
                                    }
                                }
                            }
                        }
                        else
                        {
                            arrayData[i, j] = double.Parse(CollectionBox[count].Text);

                            if (j == (column - 1))
                            {
                                if (arrayData[i, j] < 0)
                                {
                                    for (int k = 0; k < column; k++)
                                    {
                                        arrayData[i, k] *= -1;
                                    }
                                }
                            }
                        }
                        count += 1;
                    }
                }
            }

            double sum = 0;
            for (int i = 0; i < column; i++)
            {
                sum = 0;
                Fraction sumFr = new Fraction(0 , 1);
                for (int j = 0; j < row; j++)
                {
                    if (OrdinaryFraction.IsChecked == true)
                    {
                        //получаем последнюю строку для массива
                        sumFr = arrayDataFr[j, i].Sum(sumFr);
                    }
                    else sum += arrayData[j, i];
                }
                arrayData[arrayData.GetLength(0)-1 , i]=-1*sum;

                sumFr.SetNumerator(sumFr.GetNumerator() * -1);
                arrayDataFr[arrayDataFr.GetLength(0) - 1, i] = sumFr;
            }
            //определение обозначений для индексов строки и столбца
            int[] intRow = new int[row+1];
            int[] intColumn = new int[column];
            for (int i = 0; i < column-1; i++)
            {
                intColumn[i] = i+1;
            }
            for (int i = 0; i < row; i++)
            {
                intRow[i] = i + column;
            }

            DrowTable drow = new DrowTable();
            //выбран метод искуственного базиса 
            if (ArtificialBasis.IsChecked == true)
            {

                if(OrdinaryFraction.IsChecked == true)
                {
                    drow.SetFlagFr(true);
                    drow.SetArrayBaseFr(arrayBaseFr);
                    drow.SetArrayDataFr(arrayDataFr);
                }
                else drow.SetFlagFr(false);
                drow.SetBaseRow(column-1);
                drow.Table(row, 44, column, 0, arrayData, intRow, intColumn);
                drow.setSizeX(intRow.Count()+intColumn.Count() - 2);
                drow.setTargetFunction(arrayBase);

                drow.Show();
            }
            //выбран симплекс метод
            if (Simplex.IsChecked == true)
            {
                double[,] matrix2 = new double[1,1];
                Fraction[,] matrix2Fr = new Fraction[1,1];
                //нам нужна матрица без последней дополнитейльной строки
                if (OrdinaryFraction.IsChecked == true)
                {
                    matrix2Fr = new Fraction[arrayDataFr.GetLength(0) - 1, arrayDataFr.GetLength(1)];

                    for (int i = 0; i < matrix2Fr.GetLength(0); i++)
                    {
                        for (int j = 0; j < matrix2Fr.GetLength(1); j++)
                        {
                            matrix2Fr[i, j] = arrayDataFr[i, j];
                        }
                    }
                }
                else
                {
                    matrix2 = new double[arrayData.GetLength(0) - 1, arrayData.GetLength(1)];

                    for (int i = 0; i < matrix2.GetLength(0); i++)
                    {
                        for (int j = 0; j < matrix2.GetLength(1); j++)
                        {
                            matrix2[i, j] = arrayData[i, j];
                        }
                    }
                }

                ArtificialBasis simplecs = new ArtificialBasis();
                //сначала решаем систему с выбранными базисными элементами
                Gause gause = new Gause();
                //проверка на заполненность 
                if (boxBasis.Text == "") { MessageBox.Show("Элементы базиов не запонены"); }
                else 
                {
                    int[] bases = intBase(boxBasis.Text);
                    int[] bac = intBase(boxBasis.Text);

                    for (int j = 0; j < bases.Count(); j++)
                    {
                        Console.Write(" " + bases[j]); 
                    }

                    //проверка на количество базисных элементов
                    if (bases.Count() > (NumberOfVariablesRestrictions.SelectedIndex + 1)) MessageBox.Show("Количество базисных элементов больше чем количество ограничений");
                    else
                    {
                        if(OrdinaryFraction.IsChecked == true)
                        {

                            gause.SetFr(matrix2Fr);
                            gause.SetBaseFun(arrayBaseFr);

                            matrix2 = gause.Method(matrix2, bases, true);

                            matrix2Fr = gause.GetFr();

                            matrix2 = gause.Transformation(arrayBase, matrix2, bac, true);

                            matrix2Fr = gause.GetFr();

                        }
                        else
                        {
                            matrix2 = gause.Method(matrix2, bases, false);
                            matrix2 = gause.Transformation(arrayBase, matrix2, bases, false);
                        }

                        //смена индексов - по row - базисы  - column - не базисы
                        intRow = bases;
                        int[] ColumnInt = new int[NumberOfVariables.SelectedIndex + 1];
                        //все остальный элементы кроме базисных  
                        Console.WriteLine("Количество переменных "+ (NumberOfVariables.SelectedIndex + 1));
                        for (int j = 0, k = 0; j < NumberOfVariables.SelectedIndex + 1; j++)
                        {
                            if (!bases.Contains(j + 1)){ ColumnInt[k] = j + 1; k++;}
                        }
                       
                        simplecs.SetBaseRow(column - 1);
                        simplecs.setSizeX(intRow.Count() + intColumn.Count() - 2);

                        if (OrdinaryFraction.IsChecked == true)
                        {
                            simplecs.SetFlagFr(true);
                            simplecs.SetFr(matrix2Fr);
                            simplecs.Table(matrix2Fr.GetLength(0) - 1, 44, matrix2Fr.GetLength(1), 0, matrix2, intRow, ColumnInt);
                        }
                        else simplecs.Table(matrix2.GetLength(0) - 1, 44, matrix2.GetLength(1), 0, matrix2, intRow, ColumnInt);

                        //simplecs.setTargetFunction(arrayBase);
                        
                        simplecs.Show();
                    }
                }
            }

            //выбран графический метод решения
            if (Graph.IsChecked == true)
            {

            }
        }

        private void Simplex_Click(object sender, RoutedEventArgs e)
        {
            //нарисуем ввод базисных элементов по цифрам
            TextBlock block_basis = new TextBlock() ;

            block_basis.Margin = new Thickness(10, 170, 0, 0);
            block_basis.Width = 99;
            block_basis.Height = 40;
            block_basis.VerticalAlignment = VerticalAlignment.Top;
            block_basis.HorizontalAlignment = HorizontalAlignment.Left;
            block_basis.TextAlignment = TextAlignment.Center;
            block_basis.TextWrapping = TextWrapping.Wrap;
            block_basis.Text = "Номер базисного элемента";

            myGrid.Children.Add(block_basis);

            TextBox box_basis = new TextBox();

            box_basis.Margin = new Thickness(132, 169, 0, 0);
            box_basis.Width = 120;
            box_basis.Height = 23;
            box_basis.VerticalAlignment = VerticalAlignment.Top;
            box_basis.HorizontalAlignment = HorizontalAlignment.Left;
            box_basis.TextAlignment = TextAlignment.Center;
            box_basis.TextWrapping = TextWrapping.Wrap;

            boxBasis = box_basis;

            myGrid.Children.Add(box_basis);

        }

        //определяем массив с базисными переменными - упорядочены по возрастанию
        private int[] intBase(string str)
        {

            string[] new_str = str.Split(' ');
            int[] array = new int[new_str.Count()];
            for (int i = 0; i < new_str.Count(); i++)
            {
                array[i] = int.Parse(new_str[i]);
            }

            return array;
        }
    }
    
}
