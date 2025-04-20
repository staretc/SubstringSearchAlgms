using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SubstringSearchLib
{
    /// <summary>
    /// Реализация алгоритма грубой силы для поиска подстроки
    /// </summary>
    public class BruteForceAlgorithm : ISubstringSearch
    {
        public long ComparisonCount { get; private set; } // Счетчик сравнений


        /// <summary>
        /// Поиск подстрок алгоритмом грубой силы
        /// </summary>
        /// <param name="text">Образ-текст для поиска подстрок</param>
        /// <param name="pattern">Искомая подстрока</param>
        /// <returns>Список индексов, с которых начинается искомая подстрока в образ-тексте (-1 если таких нет)</returns>
        public List<int> Search(string text, string pattern)
        {
            ComparisonCount = 0;
            // Проверяем на некорректные входные параметры
            if (string.IsNullOrEmpty(pattern) || string.IsNullOrEmpty(text) || pattern.Length > text.Length)
            {
                return new List<int> { -1 };
            }

            // Логика работы:
            // (1) Проходим по всем возможным позициям в тексте, где может начинаться подстрока
            // (2) Для каждой позиции сравниваем символы подстроки с соответствующими символами текста
            // (3) Если все символы совпали, добавляем индекс начала в результат
            // (4) Если встретилось несовпадение, переходим к следующей позиции

            List<int> indexesOfSubstring = new List<int>(); // Список индексов вхождений подстроки
            int textLength = text.Length;
            int patternLength = pattern.Length;

            // Проходим по тексту до последней возможной позиции начала подстроки
            for (int i = 0; i <= textLength - patternLength; i++)
            {
                bool match = true; // Флаг совпадения подстроки
                // Сравниваем символы подстроки с текстом
                for (int j = 0; j < patternLength; j++)
                {
                    ComparisonCount++;
                    if (text[i + j] != pattern[j])
                    {
                        match = false; // Несовпадение символов
                        break; // Прерываем внутренний цикл
                    }
                }

                // Если все символы совпали, добавляем индекс
                if (match)
                {
                    indexesOfSubstring.Add(i);
                }
            }

            // Возвращаем -1, если вхождений не найдено
            return indexesOfSubstring.Count == 0 ? new List<int> { -1 } : indexesOfSubstring;
        }
    }
}
