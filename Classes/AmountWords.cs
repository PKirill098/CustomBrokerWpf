using System;
using System.Text;

namespace KirillPolyanskiy.CustomBrokerWpf
{
    internal class CurrAmountWords
    {
        internal enum CurrencyName
        {
            RUB = 0,
            USD = 1,
            EUR = 2
        }
        internal enum GenderWord
        {
            Masculine,
            Feminine,
            Neuter
        }
        internal enum SingularPlural
        {
            One,Some,Many
        }

        internal string AmountCurrWords(decimal summ, CurrencyName curr)
        {
            int countNumeral;
            string strsumm;
            SingularPlural sp;
            StringBuilder amountWords = new StringBuilder();
            if (summ.CompareTo(999999999999M) > 0) throw new Exception("Превышен максимальный диапазон преобразования!");
            strsumm = decimal.Truncate(summ).ToString();
            countNumeral = strsumm.Length;
            amountWords.Append(AmountWords(strsumm, out sp, GenderWord.Masculine, true));
            switch (curr)
            {
                case CurrencyName.RUB:
                    if ((strsumm.Length > 1 && strsumm[countNumeral - 2] == '1') | strsumm[countNumeral - 1] == '0' | strsumm[countNumeral - 1] > '4')
                        amountWords.Append(" рублей ");
                    else
                        if (strsumm[countNumeral - 1] == '1') amountWords.Append(" рубль "); else amountWords.Append(" рубля ");
                    break;
                case CurrencyName.USD:
                    if ((strsumm.Length > 1 && strsumm[countNumeral - 2] == '1') | strsumm[countNumeral - 1] == '0' | strsumm[countNumeral - 1] > '4')
                        amountWords.Append(" долларов США ");
                    else
                        if (strsumm[countNumeral - 1] == '1') amountWords.Append(" доллар США "); else amountWords.Append(" доллара США ");
                    break;
                case CurrencyName.EUR:
                    amountWords.Append(" евро ");
                    break;
                default:
                    break;
            }
            strsumm = decimal.Truncate(decimal.Multiply(decimal.Subtract(decimal.Round(summ, 2), decimal.Truncate(summ)), 100)).ToString().PadLeft(2, '0');
            if (strsumm != "00")
            {
                amountWords.Append(strsumm);
                switch (curr)
                {
                    case CurrencyName.RUB:
                        if (strsumm[0] == '1' | strsumm[1] == '0' | strsumm[1] > '4')
                            amountWords.Append(" копеек");
                        else
                            if (strsumm[1] == '1') amountWords.Append(" копейка"); else amountWords.Append(" копейки");
                        break;
                    case CurrencyName.USD:
                    case CurrencyName.EUR:
                        if (strsumm[0] == '1' | strsumm[1] == '0' | strsumm[1] > '4')
                            amountWords.Append(" центов");
                        else
                            if (strsumm[1] == '1') amountWords.Append(" цент"); else amountWords.Append(" цента");
                        break;
                    default:
                        break;
                }
            }
            return amountWords.ToString();
        }
        internal string AmountWords(string summ,out SingularPlural endwordform, GenderWord gender = GenderWord.Masculine, bool isstart = true)
        {
            endwordform = SingularPlural.Many;
            SingularPlural sp;
            StringBuilder amountWords = new StringBuilder();
            int countNumeral;
            countNumeral = summ.Length;
            char[] inverse = new char[countNumeral];
            for (int i = 1; i < countNumeral + 1; i++)
            {
                inverse[i - 1] = summ[countNumeral - i];
            }
            if (countNumeral > 1)
            {
                switch (inverse[1])
                {
                    case '0':
                        break;
                    case '1':
                        switch (inverse[0])
                        {
                            case '0':
                                amountWords.Append("десять");
                                break;
                            case '1':
                                amountWords.Append("одиннадцать");
                                break;
                            case '2':
                                amountWords.Append("двенадцать");
                                break;
                            case '3':
                                amountWords.Append("тринадцать");
                                break;
                            case '4':
                                amountWords.Append("четырнадцать");
                                break;
                            case '5':
                                amountWords.Append("пятнадцать");
                                break;
                            case '6':
                                amountWords.Append("шестнадцать");
                                break;
                            case '7':
                                amountWords.Append("семнадцать");
                                break;
                            case '8':
                                amountWords.Append("восемнадцать");
                                break;
                            case '9':
                                amountWords.Append("девятнадцать");
                                break;
                        }
                        break;
                    case '2':
                        amountWords.Append("двадцать");
                        break;
                    case '3':
                        amountWords.Append("тридцать");
                        break;
                    case '4':
                        amountWords.Append("сорок");
                        break;
                    case '5':
                        amountWords.Append("пятьдесят");
                        break;
                    case '6':
                        amountWords.Append("шестьдесят");
                        break;
                    case '7':
                        amountWords.Append("семьдесят");
                        break;
                    case '8':
                        amountWords.Append("восемьдесят");
                        break;
                    case '9':
                        amountWords.Append("девяносто");
                        break;
                }
                if (inverse[1] != '1')
                {
                    switch (inverse[0])
                    {
                        case '0':
                            break;
                        case '1':
                            if (gender==GenderWord.Masculine) amountWords.Append(" один"); else if(gender==GenderWord.Feminine) amountWords.Append(" одна"); else amountWords.Append(" одно");
                            endwordform = SingularPlural.One;
                            break;
                        case '2':
                            if (gender == GenderWord.Feminine) amountWords.Append(" двe"); else amountWords.Append(" два");
                            endwordform = SingularPlural.Some;
                            break;
                        case '3':
                            amountWords.Append(" три");
                            endwordform = SingularPlural.Some;
                            break;
                        case '4':
                            amountWords.Append(" четыре");
                            endwordform = SingularPlural.Some;
                            break;
                        case '5':
                            amountWords.Append(" пять");
                            break;
                        case '6':
                            amountWords.Append(" шесть");
                            break;
                        case '7':
                            amountWords.Append(" семь");
                            break;
                        case '8':
                            amountWords.Append(" восемь");
                            break;
                        case '9':
                            amountWords.Append(" девять");
                            break;
                    }
                }
                if (countNumeral > 2)
                {
                    switch (inverse[2])
                    {
                        case '0':
                            break;
                        case '1':
                            amountWords.Insert(0, "сто ");
                            break;
                        case '2':
                            amountWords.Insert(0, "двести ");
                            break;
                        case '3':
                            amountWords.Insert(0, "триста ");
                            break;
                        case '4':
                            amountWords.Insert(0, "четыреста ");
                            break;
                        case '5':
                            amountWords.Insert(0, "пятьсот ");
                            break;
                        case '6':
                            amountWords.Insert(0, "шестьсот ");
                            break;
                        case '7':
                            amountWords.Insert(0, "семьсот ");
                            break;
                        case '8':
                            amountWords.Insert(0, "восемьсот ");
                            break;
                        case '9':
                            amountWords.Insert(0, "девятьсот ");
                            break;
                    }
                }
                string thousandsWord;
                StringBuilder thousandstr = new StringBuilder(3);
                if (countNumeral > 3)
                {
                    for (int i = 3; i < countNumeral & i < 6; i++)
                    {
                        thousandstr.Insert(0, inverse[i]);
                    }
                    thousandsWord = AmountWords(thousandstr.ToString(),out sp, GenderWord.Feminine, false);
                    if (thousandsWord.Length > 0)
                    {
                        if (sp==SingularPlural.Many)
                        {
                            amountWords.Insert(0, " тысяч ");
                        }
                        else
                        {
                            if (sp == SingularPlural.One) amountWords.Insert(0, " тысяча "); else amountWords.Insert(0, " тысячи ");
                        }
                        amountWords.Insert(0, thousandsWord);
                    }
                }
                if (countNumeral > 6)
                {
                    thousandstr.Clear();
                    for (int i = 6; i < countNumeral & i < 9; i++)
                    {
                        thousandstr.Insert(0, inverse[i]);
                    }
                    thousandsWord = AmountWords(thousandstr.ToString(),out sp, GenderWord.Masculine, false);
                    if (thousandsWord.Length > 0)
                    {
                        if (sp == SingularPlural.Many)
                        {
                            amountWords.Insert(0, " миллионов ");
                        }
                        else
                        {
                            if (sp == SingularPlural.One) amountWords.Insert(0, " миллион "); else amountWords.Insert(0, " миллиона ");
                        }
                        amountWords.Insert(0, thousandsWord);
                    }
                }
                if (countNumeral > 9)
                {
                    thousandstr.Clear();
                    for (int i = 9; i < countNumeral & i < 12; i++)
                    {
                        thousandstr.Insert(0, inverse[i]);
                    }
                    thousandsWord = AmountWords(thousandstr.ToString(),out sp, GenderWord.Masculine, false);
                    if (thousandsWord.Length > 0)
                    {
                        if (sp == SingularPlural.Many)
                        {
                            amountWords.Insert(0, " миллиардов ");
                        }
                        else
                        {
                            if (sp == SingularPlural.One) amountWords.Insert(0, " миллиард "); else amountWords.Insert(0, " миллиарда ");
                        }
                        amountWords.Insert(0, thousandsWord);
                    }
                }
            }
            else
            {
                switch (inverse[0])
                {
                    case '0':
                        if (isstart) amountWords.Append("ноль");
                        break;
                    case '1':
                        if (gender == GenderWord.Masculine) amountWords.Append(" один"); else if (gender == GenderWord.Feminine) amountWords.Append(" одна"); else amountWords.Append(" одно");
                        endwordform = SingularPlural.One;
                        break;
                    case '2':
                        if (gender == GenderWord.Feminine) amountWords.Append(" двe"); else amountWords.Append(" два");
                        endwordform = SingularPlural.Some;
                        break;
                    case '3':
                        amountWords.Append("три");
                        endwordform = SingularPlural.Some;
                        break;
                    case '4':
                        amountWords.Append("четыре");
                        endwordform = SingularPlural.Some;
                        break;
                    case '5':
                        amountWords.Append("пять");
                        break;
                    case '6':
                        amountWords.Append("шесть");
                        break;
                    case '7':
                        amountWords.Append("семь");
                        break;
                    case '8':
                        amountWords.Append("восемь");
                        break;
                    case '9':
                        amountWords.Append("девять");
                        break;
                }
            }
            return amountWords.ToString();
        }
    }
}
