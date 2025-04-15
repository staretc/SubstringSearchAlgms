using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SubstringSearchLib
{
    public class RabinCarpAlgorithm : ISubstringSearch
    {
        private const int CAPACITY = 65536;
        private const int MODULE = 3301;

        /// <summary>
        /// Поиск подстрок алгоритмом Рабина-Карпа
        /// </summary>
        /// <param name="text">Образ-текст для поиска подстрок</param>
        /// <param name="pattern">Искомая подстрока</param>
        /// <returns>Список индексов, с которых начинается искомая подстрока в образ-тексте (-1 если таких нет)</returns>
        public List<int> Search(string text, string pattern)
        {
            // проверяем на некорректные случаи входных параметров
            if (pattern == null || text == null ||
                pattern.Length > text.Length)
            {
                return new List<int>() { -1 };
            }

            // Алгоритм
            // Вместо работы со строками, будем работать с числами, используя модульную арифметику
            // (1) Вычисляем хэш-функции паттерна и текущего окна образа-текста (такой же длины как и паттерн) и берём остаток от деления
            // (2) Если хэши совпали, сравниваем посимвольно паттерн и окно образа-текста. Если совпали - нашли подстроку
            // (3) Далее, при помощи формулы Горнера пересчитываем хэш для следующего окна образа-текста (со сдвигом на 1)
            // Повторяем (2) и (3) пока не дойдём до конца строки

            List<int> indexesOfSubstring = new List<int>(); // сюда заносим индексы, с которых начинается подстрока (-1 если ничего не нашли)

            int pointer;    // объявляем указатели для прохода по образу-тексту
            int subPointer; // и для сравнения паттерна с окном текста

            var patternHash = GetBinaryHash(pattern); // хэш паттерна, его будем сравнивать с хэшем текущего окна текста
            var textWindowHash = GetBinaryHash(text.Substring(0, pattern.Length)); // хэш первого окна текста [0..pattern.Length - 1]
            var power = 1; // значение (CAPACITY**(pattern-1)) % MODULE
            // считаем power
            for (pointer = 0; pointer < pattern.Length - 1; pointer++)
            {
                power = (power * CAPACITY) % MODULE;
            }

            // проходим по всем символам начиная с [pattern.Length]
            for (pointer = 0; pointer < text.Length - pattern.Length; pointer++)
            {
                // если хэши оказались равны
                if (patternHash == textWindowHash)
                {
                    // проверяем посимвольно
                    for (subPointer = 0; subPointer < pattern.Length; subPointer++)
                    {
                        if (text[pointer + subPointer] != pattern[subPointer])
                        {
                            break;
                        }
                    }
                    // если прошли по всему паттерну, то нашли искомую подстроку, добавляем в список
                    if (subPointer == pattern.Length)
                    {
                        indexesOfSubstring.Add(pointer);
                    }
                }
                // пересчитываем хэш для нового окна образа-текста
                // для этого воспользуемся формулой Горнера
                // удаляем первую цифру, добавляем в конец следующую и всё по модулю
                if (pointer < text.Length - pattern.Length)
                {
                    textWindowHash = (CAPACITY * (textWindowHash - (int)text[pointer] * power) + text[pointer + pattern.Length]) % MODULE;
                    // если получили отрицательный textWindowHash, приводим по модулю MODULE
                    if (textWindowHash < 0)
                    {
                        textWindowHash += MODULE;
                    }
                }
            }

            return indexesOfSubstring.Count == 0 ? new List<int>() { -1 } : indexesOfSubstring;
        }
        /// <summary>
        /// Получение полиноминального хэша строки
        /// </summary>
        /// <param name="text">Строка, по которой получаем хэш</param>
        /// <returns>Хэш строки</returns>
        private int GetBinaryHash(string text)
        {
            var hash = 0;
            for(int indx = 0; indx < text.Length; indx++)
            {
                hash = ((int)text[indx] + hash * CAPACITY) % MODULE;
            }
            return hash;
        }
    }
}
