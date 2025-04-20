using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SubstringSearchLib;

namespace SubstringSearchExperiments
{
    internal class Program
    {
        static void Main(string[] args)
        {
            // Путь к текстовому файлу
            string filePath = "anna.txt";

            try
            {
                // Читаем текст из файла
                string text = File.ReadAllText(filePath);
                if (string.IsNullOrEmpty(text))
                {
                    Console.WriteLine("Файл пустой.");
                    return;
                }

                // Запрашиваем подстроку у пользователя
                Console.WriteLine("Введите подстроку для поиска:");
                string pattern = Console.ReadLine();

                if (string.IsNullOrEmpty(pattern))
                {
                    Console.WriteLine("Подстрока не может быть пустой.");
                    return;
                }

                // Инициализируем алгоритмы
                var bruteForce = new BruteForceAlgorithm();
                var kmp = new KnuthMorrisPrattAlgorithm();
                var boyerMoore = new BoyerMooreAlgorithm();
                var rabinCarp = new RabinCarpAlgorithm();

                // Список алгоритмов для тестирования
                var algorithms = new List<(string Name, ISubstringSearch Algorithm)>
                {
                    ("Грубая сила", bruteForce),
                    ("Кнута-Морриса-Пратта", kmp),
                    ("Бойера-Мура", boyerMoore),
                    ("Рабина-Карпа",  rabinCarp)
                };

                Console.WriteLine($"\nПоиск подстроки '{pattern}' в тексте (длина текста: {text.Length} символов)...\n");

                // Выполняем поиск и замеряем время для каждого алгоритма
                foreach (var (name, algorithm) in algorithms)
                {   if (name != "Грубая сила")
                    {
                        List<int> resultTest = algorithm.Search(text, pattern);
                    }
                    var stopwatch = Stopwatch.StartNew();
                    List<int> result = algorithm.Search(text, pattern);
                    stopwatch.Stop();

                    // Выводим результаты
                    Console.WriteLine($"Алгоритм: {name}");
                    if (result[0] == -1)
                    {
                        Console.WriteLine("Вхождений не найдено.");
                    }
                    else
                    {
                        Console.WriteLine($"Найдено вхождений: {result.Count}");
                        //Console.WriteLine($"Индексы: {string.Join(", ", result)}");
                    }
                    Console.WriteLine($"Время выполнения: {stopwatch.Elapsed.TotalSeconds:F7} сек");
                    Console.WriteLine();
                    // Выводим счетчики
                    if (algorithm is BruteForceAlgorithm bf)
                    {
                        Console.WriteLine($"Сравнений: {bf.ComparisonCount}");
                    }
                    if (algorithm is RabinCarpAlgorithm rk)
                    {
                        Console.WriteLine($"Хэширований: {rk.HashCount}, Сравнений: {rk.ComparisonCount}");
                    }
                    Console.WriteLine();
                }
            }
            catch (FileNotFoundException)
            {
                Console.WriteLine($"Ошибка: Файл '{filePath}' не найден.");
            }
            catch (IOException ex)
            {
                Console.WriteLine($"Ошибка при чтении файла: {ex.Message}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Произошла ошибка: {ex.Message}");
            }
        }
        //var text = "aabaabaaaabaabaaab";
        //var pattern = "aabaa";
        //var algms = new ISubstringSearch[]
        //{
        //    new KnuthMorrisPrattAlgorithm(),
        //    new RabinCarpAlgorithm(),
        //    new BoyerMooreAlgorithm(),
        //    new BruteForceAlgorithm()
        //};

        //Console.WriteLine("text: " + text);
        //Console.WriteLine("pattern: " + pattern);

        //foreach (var algm in algms)
        //{
        //    Console.WriteLine("Found substrings at indexes: " + String.Join(" ", algm.Search(text, pattern)));
        //}

    }
}
