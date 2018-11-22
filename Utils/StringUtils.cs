using System.Collections;
using static System.Console;
namespace EfPostgre.Utils
{
    public static class StringUtils
    {
        /// <summary>
        /// Обрамить входную строку в двойные кавычки
        /// </summary>
        /// <param name="param">исходная строка</param>
        /// <returns>строка, обрамленная в ""</returns>
        public static string QuoteParam(this string param)
        {
            return "\"" + param + "\"";
        }

        /// <summary>
        /// Проверка строки на пустоту или Null
        /// </summary>
        /// <param name="param">строка для проверки</param>
        /// <returns>true - если строка пуста или Null</returns>
        public static bool IsEmpty(this string param)
        {
            return string.IsNullOrEmpty(param);
        }

        public static void PrintToConsole<T>(T param) where T : IEnumerable
        {
            WriteLine("--------------------------------------");
            foreach (var item in param)
                WriteLine($"Item name: {item}");
        }
    }
}