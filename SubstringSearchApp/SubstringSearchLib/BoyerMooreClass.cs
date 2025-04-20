using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SubstringSearchLib
{
    /// <summary>
    /// Реализация алгоритма Бойера-Мура для поиска подстроки с использованием правил плохого символа и хорошего суффикса
    /// </summary>
    public class BoyerMooreAlgorithm : ISubstringSearch
    {
        private const int ALPHABET_SIZE = 1024; // Размер алфавита (ASCII + русский)
        /// <summary>
        /// Поиск подстрок алгоритмом Бойера-Мура
        /// </summary>
        /// <param name="text">Образ-текст для поиска подстрок</param>
        /// <param name="pattern">Искомая подстрока</param>
        /// <returns>Список индексов, с которых начинается искомая подстрока в образ-тексте (-1 если таких нет)</returns>
        public List<int> Search(string text, string pattern)
        {
            // Проверяем на некорректные входные параметры
            if (string.IsNullOrEmpty(pattern) || string.IsNullOrEmpty(text) || pattern.Length > text.Length)
            {
                return new List<int> { -1 };
            }

            // Логика работы:
            // (1) Создаем таблицы для правила плохого символа и правила хорошего суффикса
            // (2) Сравниваем подстроку с текстом справа налево
            // (3) При несовпадении выбираем максимальный сдвиг из двух эвристик, чтобы пропустить ненужные позиции
            // (4) При полном совпадении добавляем индекс вхождения и продолжаем поиск

            List<int> indexesOfSubstring = new List<int>(); // Список индексов вхождений подстроки
            var badCharTable = BuildBadCharTable(pattern); // Таблица плохого символа
            var goodSuffixShifts = BuildGoodSuffixTable(pattern); // Таблица хорошего суффикса

            int textLength = text.Length;
            int patternLength = pattern.Length;
            int shift = 0; // Текущая позиция начала подстроки в тексте

            // Проходим по тексту, пока не превысим допустимую позицию
            while (shift <= textLength - patternLength)
            {
                int j = patternLength - 1; // Начинаем сравнение с конца подстроки

                // Сравниваем символы справа налево
                while (j >= 0 && pattern[j] == text[shift + j])
                {
                    j--;
                }

                if (j < 0) // Полное совпадение подстроки
                {
                    indexesOfSubstring.Add(shift);
                    // Сдвиг по правилу хорошего суффикса (или минимальный сдвиг)
                    shift += goodSuffixShifts[0];
                }
                else // Несовпадение на позиции j
                {
                    // Правило плохого символа: определяем сдвиг на основе несовпавшего символа
                    char badChar = text[shift + j];
                    int lastOccurrence = badCharTable[badChar % ALPHABET_SIZE]; // Ограничиваем алфавит
                    int badCharShift = Math.Max(1, j - lastOccurrence);

                    // Правило хорошего суффикса: сдвиг на основе совпавшего суффикса
                    int goodSuffixShift = goodSuffixShifts[j + 1];

                    // Выбираем максимальный сдвиг из двух эвристик
                    shift += Math.Max(badCharShift, goodSuffixShift);
                }
            }

            // Возвращаем -1, если вхождений не найдено
            return indexesOfSubstring.Count == 0 ? new List<int> { -1 } : indexesOfSubstring;
        }

        /// <summary>
        /// Построение таблицы плохого символа
        /// </summary>
        /// <param name="pattern">Искомая подстрока</param>
        /// <returns>массив, где индекс — код символа, значение — индекс его последнего вхождения</returns>
        private int[] BuildBadCharTable(string pattern)
        {
            var badCharTable = new int[ALPHABET_SIZE];
            for (int i = 0; i < ALPHABET_SIZE; i++)
            {
                badCharTable[i] = -1; // По умолчанию -1 для символов, которых нет в pattern
            }

            for (int i = 0; i < pattern.Length; i++)
            {
                badCharTable[pattern[i] % ALPHABET_SIZE] = i;
            }

            return badCharTable;
        }

        /// <summary>
        /// Построение таблицы хорошего суффикса
        /// </summary>
        /// <param name="pattern">Искомая подстрока</param>
        /// <returns>Массив сдвигов для правила хорошего суффикса</returns>
        private int[] BuildGoodSuffixTable(string pattern)
        {
            int patternLength = pattern.Length;
            int[] shifts = new int[patternLength + 1]; // Сдвиги для совпавшего суффикса длиной k
            int[] borderPositions = new int[patternLength + 1]; // Позиции границ для суффиксов

            // Инициализация: заполняем shifts длиной подстроки
            for (int index = 0; index <= patternLength; index++)
            {
                shifts[index] = patternLength;
            }

            // Шаг 1: Находим позиции границ (аналог префикс-функции для суффиксов)
            int i = patternLength;
            int j = patternLength + 1;
            borderPositions[i] = j;

            while (i > 0)
            {
                while (j <= patternLength && pattern[i - 1] != pattern[j - 1])
                {
                    if (shifts[j] == patternLength)
                    {
                        shifts[j] = j - i;
                    }
                    j = borderPositions[j];
                }
                i--;
                j--;
                borderPositions[i] = j;
            }

            // Шаг 2: Устанавливаем сдвиги для префиксов, совпадающих с суффиксами
            int prefixBorder = borderPositions[0];
            for (i = 0; i <= patternLength; i++)
            {
                if (shifts[i] == patternLength)
                {
                    shifts[i] = prefixBorder;
                }
                if (i == prefixBorder)
                {
                    prefixBorder = borderPositions[prefixBorder];
                }
            }

            return shifts;
        }
    }
}
