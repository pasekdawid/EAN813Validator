using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EAN_Validator
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine(EANValidator("0100007", 1));

            Console.ReadKey();
        }

        static string EANValidator(string number, int eantype)
        {
            try
            {
                if (eantype < 1 || eantype > 2)
                    throw new WrongEANTypeException(string.Format($"'{eantype}' EANtype is not valid!"));

                foreach (char c in number)
                {
                    int parseOutResult;
                    if (!Int32.TryParse(c.ToString(), out parseOutResult))
                        throw new NotANumberException(string.Format($"'{number}' is not a number!"));
                }

                if (eantype == 1)
                {
                    if (number.Length < 7)
                        throw new TooShortNumberException(string.Format($"'{number}' is too short to be an EAN8 number!"));
                }
                else
                {
                    if (number.Length < 12)
                        throw new TooShortNumberException(string.Format($"'{number}' is too short to be an EAN13 number!"));
                }

                if ((eantype == 1 && number.Length == 7) || (eantype == 2 && number.Length == 12))
                {
                    number = "0" + number;
                    if (IsEANValid(number, eantype))
                        return number;
                    throw new WrongCheckSumDigitException(string.Format("'{0}' is not a valid sum control digit for this EAN number!", eantype == 1 ? number[7].ToString() : number[12].ToString()));
                }

                if (IsEANValid(number, eantype))
                    return RemoveAddon(number, eantype);

                number = "0" + number;
                if (IsEANValid(number, eantype))
                    return RemoveAddon(number, eantype);
                throw new WrongCheckSumDigitException(string.Format("'{0}' is not a valid sum control digit for this EAN number!", eantype == 1 ? number[7].ToString() : number[12].ToString()));
            }
            catch (WrongEANTypeException ex)
            {
                Console.WriteLine(ex.Message);
            }
            catch (NotANumberException ex)
            {
                Console.WriteLine(ex.Message);
            }
            catch (UnknownException ex)
            {
                Console.WriteLine(ex.Message);
            }
            catch (TooShortNumberException ex)
            {
                Console.WriteLine(ex.Message);
            }
            catch (WrongCheckSumDigitException ex)
            {
                Console.WriteLine(ex.Message);
            }
            return "";
        }

        private static string RemoveAddon(string number, int eantype)
        {
            int maxLength;
            if (eantype == 1) maxLength = 8;
            else maxLength = 13;

            return number.Substring(0, maxLength);
        }

        private static bool IsEANValid(string number, int eantype)
        {
            number = RemoveAddon(number, eantype);

            string numberWithoutCheckSumDigit = number.Substring(0, number.Length - 1);

            if (int.Parse(eantype == 1 ? number[7].ToString() : number[12].ToString()) != SumDigit(numberWithoutCheckSumDigit))
                return false;
            return true;
        }

        private static int SumDigit(string number)
        {
            bool isEven = false;
            int counter = 0;
            char[] charArray = number.ToCharArray();
            Array.Reverse(charArray);
            number = new string(charArray);
            foreach (char c in number)
            {
                if (isEven) counter += Int32.Parse(c.ToString());
                else counter += 3 * Int32.Parse(c.ToString());

                isEven = !isEven;
            }

            return 10 - (counter % 10);
        }
    }
}
