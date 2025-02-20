using System;
using System.Collections.Generic;
using System.Linq;

namespace MedivacSolution
{
    class Program
    {
        static void Main(string[] args)
        {
           

            // 1. Четем капацитета на медивака
            int medivacCapacity = int.Parse(Console.ReadLine());

            // 2. Четем данни за единиците (unit, capacity, urgencyRating) до Launch
            //    Ще съхраняваме всичко в List<(int unit, int cap, int rating)>
            var units = new List<(int unit, int cap, int rating)>();

            while (true)
            {
                // Прочитаме ред
                string input = Console.ReadLine();
                if (input == null) 
                {
                    // Ако случайно няма повече вход
                    break;
                }
                
                if (input.Trim().ToLower() == "launch")
                {
                    // Край на въвеждането
                    break;
                }

                string[] tokens = input.Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
                if (tokens.Length < 3)
                {
                    // Ред, който не отговаря на очаквания формат, прескачаме или логваме
                    Console.WriteLine("[WARN]: Невалиден ред, пропускам: " + input);
                    continue;
                }

                // Опитваме се да парснем
                int unitId;
                int capacityNeeded;
                int urgencyRating;
                bool parseUnit = int.TryParse(tokens[0], out unitId);
                bool parseCap = int.TryParse(tokens[1], out capacityNeeded);
                bool parseRating = int.TryParse(tokens[2], out urgencyRating);

                if (!parseUnit || !parseCap || !parseRating)
                {
                    Console.WriteLine("[WARN]: Невалидни данни, пропускам: " + input);
                    continue;
                }

                // Добавяме към списъка
                units.Add((unitId, capacityNeeded, urgencyRating));
            }

            // 3. Ако нямаме никакви единици, резултатът е 0 capacity used, 0 urgency, без единици
            if (units.Count == 0)
            {
                Console.WriteLine(0); // capacity used
                Console.WriteLine(0); // total urgency
                // без единици
                return;
            }

            // 4. Прилагаме 0/1 Knapsack алгоритъм
            // Ще използваме двуизмерен масив dp, където dp[i, c] = максималната спешност, 
            // ако разгледаме първите i единици и разполагаме с капацитет c.
            int n = units.Count;
            int[,] dp = new int[n + 1, medivacCapacity + 1];

            // За да реконструираме, пазим и "родителска" информация (кое решение сме взели).
            // parent[i, c] = true, ако сме взели i-тата единица (units[i-1]) за да достигнем dp[i,c].
            bool[,] parent = new bool[n + 1, medivacCapacity + 1];

            // 5. Инициализиране на dp: по подразбиране е 0, което е правилно (без елементи - 0 рейтинг).

            // 6. Основен цикъл на попълване
            for (int i = 1; i <= n; i++)
            {
                // Текуща единица (под i имаме units[i-1], защото dp е с индекс от 1..n)
                var (unitId, capNeeded, urgency) = units[i - 1];

                for (int currentCap = 0; currentCap <= medivacCapacity; currentCap++)
                {
                    // Първо взимаме опцията да НЕ вземем текущата единица
                    dp[i, currentCap] = dp[i - 1, currentCap];

                    // Проверяваме дали може да вземем текущата единица
                    if (capNeeded <= currentCap)
                    {
                        // Ако можем да я вместим, проверяваме дали така не получаваме по-добра спешност
                        int candidate = dp[i - 1, currentCap - capNeeded] + urgency;

                        if (candidate > dp[i, currentCap])
                        {
                            dp[i, currentCap] = candidate;
                            parent[i, currentCap] = true;
                        }
                    }
                    // Ако capNeeded == 0, тогава можем да добавим елемента без да намаляме currentCap
                    // но тъй като условието е 0/1, можем да вземем всеки елемент само веднъж. 
                    // В горната проверка capNeeded <= currentCap ще хване и capNeeded=0, 
                    // и ще сравни dp[i-1, currentCap] + urgency. 
                    // Ако urgency е по-добро, ще го вземе.
                }
            }

            // 7. Максималната спешност ще бъде dp[n, c], където c е <= medivacCapacity.
            // Но не знаем точно кой c да вземем, защото може да не използваме пълния капацитет, 
            // стига да имаме максимална спешност. 
            // Затова търсим c, за който dp[n,c] е максимално.
            int bestCapacityUsed = 0;
            int bestUrgency = 0;

            for (int c = 0; c <= medivacCapacity; c++)
            {
                if (dp[n, c] > bestUrgency)
                {
                    bestUrgency = dp[n, c];
                    bestCapacityUsed = c;
                }
            }

            // 8. Реконструкция на използваните единици
            var chosenUnits = new List<int>();
            // Тръгваме от (n, bestCapacityUsed) и вървим назад
            int remainingCap = bestCapacityUsed;
            for (int i = n; i > 0; i--)
            {
                if (parent[i, remainingCap])
                {
                    // Взели сме i-тата единица
                    var (unitId, capNeeded, urgency) = units[i - 1];
                    chosenUnits.Add(unitId);
                    // Връщаме се с capNeeded назад
                    remainingCap -= capNeeded;
                }
            }

            // 9. Обръщаме списъка, защото сме го запълвали отзад напред, 
            //    но за условието няма значение редът, стига после да ги подредим възходящо
            //    Така или иначе после ще ги сортираме.
            // chosenUnits.Reverse(); -- не е задължително
            // 10. Сортираме във възходящ ред
            chosenUnits.Sort();

            // 11. Принтираме резултата
            //    - capacity used (от bestCapacityUsed)
            //    - urgency rating (от bestUrgency)
            //    - списък на unit-ите в ascending order

            Console.WriteLine(bestCapacityUsed);
            Console.WriteLine(bestUrgency);

            foreach (var unitId in chosenUnits)
            {
                Console.WriteLine(unitId);
            }

         
        }
    }
}
