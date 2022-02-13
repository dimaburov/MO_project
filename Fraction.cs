using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MO_project
{
    //класс обыкновенных дробей
    class Fraction
    {
        private int numerator;
        private int denominatror;

        public Fraction(int numerator, int denominatror)
        {
            //добавить сокращение дроби
            this.numerator = numerator;
            this.denominatror = denominatror;

            StartNod();
        }

        public int GetNumerator() { return numerator; }
        public void SetNumerator(int newNum) { numerator = newNum; }

        public int GetDenominatror() { return denominatror; }
        public void SetDenominatror(int newDen) { denominatror = newDen; }


        //сложение
        public Fraction Sum(Fraction number2)
        {
            return new Fraction(numerator * number2.GetDenominatror() + denominatror * number2.GetNumerator(), denominatror * number2.GetDenominatror());
        }
        //вычитание
        public Fraction Sub(Fraction number2)
        {
            return new Fraction(numerator * number2.GetDenominatror() - denominatror * number2.GetNumerator(), denominatror * number2.GetDenominatror());
        }
        //умножение
        public Fraction Mul(Fraction number2)
        {
            return new Fraction(numerator * number2.GetNumerator(), denominatror * number2.GetDenominatror());
        }
        //деление
        public Fraction Del(Fraction number2)
        {
            int num1 = numerator;
            int num2 = number2.GetNumerator();
            if (numerator < 0 && number2.GetNumerator() < 0)
            {
                num1 = numerator * -1;
                num2 = number2.GetNumerator() * -1;
            }
            if (numerator > 0 && number2.GetNumerator() < 0)
            {
                num1 = numerator * -1;
                num2 = number2.GetNumerator() * -1;
            }
            return new Fraction(num1 * number2.GetDenominatror(), num2 * denominatror);
        }

        //сравнение чисел
        //1 больше 2 -> 1
        //2 больше 1 -> -1
        //1 равен 2 -> 0
        public int Srav(Fraction number2)
        {
            if (AbsSub(number2).GetNumerator() > 0) return 1;
            if (AbsSub(number2).GetNumerator() < 0) return -1;
            return 0;
        }

        //вычитание по модулю
        public Fraction AbsSub(Fraction number2)
        {
            return new Fraction(Math.Abs(numerator * number2.GetDenominatror()) - Math.Abs(denominatror * number2.GetNumerator()), denominatror * number2.GetDenominatror());
        }

        //наибольший общий делитель
        private int GCD(int a, int b)
        {
            while (a != 0 && b != 0)
                if (a > b) a %= b;
                else b %= a;
            return a + b;
        }

        public Fraction ABS()
        {
            return new Fraction(Math.Abs(numerator), denominatror);
        }
        public void StartNod()
        {
            int nod = 0;

            if (numerator == 1 || denominatror == 1) nod = 1;
            if (numerator == 0 && denominatror == 1) nod = 1;
            if (nod == 0) nod = Nod(numerator, denominatror);

            numerator /= nod;
            denominatror /= nod;
        }
        public int  Nod(int a, int b)
        {
            a = Math.Abs(a);
            b = Math.Abs(b);
            if (a == 0)
            {
                return b;
            }
            else
            {
                while (b != 0)
                {
                    if (a > b)
                    {
                        a -= b;
                    }
                    else
                    {
                        b -= a;
                    }
                }

                return a;
            }
        }
    }
}