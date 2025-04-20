using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SubstringSearchLib
{
    public class KnuthMorrisPrattAlgorithm : ISubstringSearch
    {
        /// <summary>
        /// Поиск подстрок алгоритмом Кнута-Морриса-Пратта
        /// </summary>
        /// <param name="text">Образ-текст для поиска подстрок</param>
        /// <param name="pattern">Искомая подстрока</param>
        /// <returns>Список индексов, с которых начинается искомая подстрока в образ-тексте (-1 если таких нет)</returns>
        public List<int> Search(string text, string pattern)
        {
            // проверяем на некорректные случаи входных параметров
            if (string.IsNullOrEmpty(pattern) || string.IsNullOrEmpty(text) || pattern.Length > text.Length)
            {
                return new List<int>() { -1 };
            }

            // логика работы:
            // (1) Считаем префикс-функцию для подстроки, которую ищем в тексте
            // (2) Далее, во время поиска подстроки, если во время проверки на совпадение подстроки терпим неудачу,
            // подсчитанная префикс-функция позволяет пропустить некоторое количество символов образа-текста без потери очередной подстроки,
            // позволяя тем самым обогнать по скорости алгоритм грубой силы

            List<int> indexesOfSubstring = new List<int>(); // сюда заносим индексы, с которых начинается подстрока (-1 если ничего не нашли)
            var prefixFunc = CalculatePrefixFunction(pattern); // префикс-функция паттерна, который ищем в тексте

            // указатели для прохода по тексту и паттерну
            var textPointer = 0;
            var patternPointer = 0;

            // идём по всей длине образа-текста
            while (textPointer < text.Length)
            {
                // если символы совпали, передвигаемся на следующие
                if (text[textPointer] == pattern[patternPointer])
                {
                    textPointer++;
                    patternPointer++;
                    // если закончили проход по паттерну, то нашли искомую подстроку
                    if (patternPointer == pattern.Length)
                    {
                        // записываем индекс начала подстроки
                        indexesOfSubstring.Add(textPointer - patternPointer);
                        // сдвигаем указатель на несколько позиций с учётом префикс функции
                        // таким образом пропустим ненужные сравнения, но не упустим появления ещё одной пододящей подстроки
                        patternPointer = prefixFunc[patternPointer - 1];
                    }
                    continue;
                }
                // если символы не совпали, то начинаем поиск подстроки заново
                // если указатель паттерна не нулевой, то сдвигаем на несколько позиций с учётом префикс функции
                if (patternPointer != 0)
                {
                    patternPointer = prefixFunc[patternPointer - 1];
                    continue;
                }
                textPointer++;
            }

            return indexesOfSubstring.Count == 0 ? new List<int>() { -1 } : indexesOfSubstring;
        }
        /// <summary>
        /// Подсчёт префикс-функции для строки
        /// </summary>
        /// <param name="text">Строка, дла которой считаем префикс-функцию</param>
        /// <returns>Префикс функция</returns>
        private int[] CalculatePrefixFunction(string text)
        {
            var prefixFunc = new int[text.Length];
            var longestPrefixSuffix = 0; // длина наибольшего префикса, который также является и суффиксом для предыдущего index
            var index = 1; // текущий индекс (начинаем с 1 тк префикс функция для [0] индекса строки всегда ноль)

            // идём по всей длине строки
            // рассматриваем посимвольно строку в двух местах
            // получается так, что условно рассматриваем перфикс и суффикс у текущей подстроки,
            // которая начинается с [0] и заканчивается на [index]
            while (index < text.Length)
            {
                // если символы совпадают, увеличиваем longestPrefixSuffix и index, и в значение префикс-функции записываем longestPrefixSuffix
                // таким образом, если префикс и суффикс с некоторой позиции совпадают несколько раз для идущих подряд подстрок,
                // то мы не пересчитываем префикс-функцию с нуля, а используем полученные с предыдущих итераций значения
                if (text[index] == text[longestPrefixSuffix])
                {
                    longestPrefixSuffix++;
                    prefixFunc[index] = longestPrefixSuffix;
                    index++;
                    continue;
                }
                // если символы не совпали, то имеем 2 случая:
                // (1) Если longestPrefixSuffix равен нулю, то нет никаких совпадающий префиксов и суффиксов.
                // Записываем в значение префикс-функции 0 и идём рассматривать следующую подстроку
                // (2) Если longestPrefixSuffix НЕ равен нулю, то пытаемся рассмотреть префиксы и суффиксы мЕньшей длины
                // чтобы получить нужную позицию для просмотра, присваиваем longestPrefixSuffix значчение префикс функции по индексу longestPrefixSuffix -1
                // (проще говоря) Если в префиксе, есть подстрока, начинающаяся с [0] индекса,
                // которая совпадает с подстрокой суффикса, заканчивающаяся index, то мы начинаем рассматривать именно такой префикс
                if (longestPrefixSuffix == 0)
                {
                    prefixFunc[index] = 0;
                    index++;
                    continue;
                }
                longestPrefixSuffix = prefixFunc[longestPrefixSuffix - 1];
            }

            return prefixFunc;
        }
    }
}
