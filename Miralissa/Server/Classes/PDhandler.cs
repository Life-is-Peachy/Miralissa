using System.Collections.Generic;
using Miralissa.Server.Repository.PersonalData;
using Miralissa.Shared;

namespace Miralissa.Server.Classes
{
    public static class PDhandler
    {
        public static IEnumerable<PDelement> GetFulltextSearchResult(PDmodel pd)
        {
            string[] nameFragments = SplitFullName(pd.FIO);
            IEnumerable<PDelement> pdList
                = PersonalDataRawRepository.SearchPersonalData(nameFragments[0], nameFragments[1], nameFragments[2]);

            return GetApproximateDifferences(pdList, pd);
        }

        private static IEnumerable<PDelement> GetApproximateDifferences(IEnumerable<PDelement> pdList, PDmodel original)
        {
            string fio = original.FIO;
            string address = original.Address;

            foreach (var element in pdList)
            {
                var Rank = 1000;

                double tanimotoRank = ApproximateDifference(element.FIO, fio) * 50.0;
                Rank += (int)tanimotoRank;

                if (element.DateBirth == null)
                    Rank -= 100;

                if (string.IsNullOrWhiteSpace(element.PlaceBirth))
                    Rank -= 100;

                if (element.DateBirth == null && string.IsNullOrWhiteSpace(element.PlaceBirth))
                    Rank = 0;

                if (string.IsNullOrWhiteSpace(element.SNILS))
                    Rank -= 50;

                if (string.IsNullOrWhiteSpace(element.INN))
                    Rank -= 50;

                if (string.IsNullOrWhiteSpace(element.PassSeria))
                    Rank -= 10;

                if (string.IsNullOrWhiteSpace(element.PassNumber))
                    Rank -= 10;

                if (string.IsNullOrWhiteSpace(element.PassIssueAddr))
                    Rank -= 10;

                if (element.PassIssueDate == null)
                    Rank -= 10;

                if (string.IsNullOrWhiteSpace(element.Address))
                {
                    Rank -= 100;
                }
                else
                {
                    if (!string.IsNullOrWhiteSpace(address))
                    {
                        tanimotoRank = ApproximateDifference(element.Address, original.Address) * 50.0;

                        Rank += (int)tanimotoRank;
                    }
                }

                element.Rank = Rank;
                yield return element;
            }
        }

		/// <summary>
		/// Получает число, которое показывает степень похожести
		/// двух строк между собой. Чем больше это число, тем более похожи строки
		/// </summary>
		/// <param name="words1">Слова из основной строки</param>
		/// <param name="words2">Слова из сравниваемой строки</param>
		/// <returns>Степень похожести</returns>
		unsafe public static double ApproximateDifference(string[] words1, string[] words2)
        {
            if (words1.Length == 0 || words2.Length == 0)
                return 0.0;

            int len1 = words1.Length;
            int len2 = words2.Length;
            int totalLen = len1 * len2;
            int wordIndex;

            double max;
            double resultRank = 0.0;

            // Массив рангов похожести для каждой пары слов
            double* ranks = stackalloc double[totalLen];
            for (int i = 0; i < totalLen; ++i)
                ranks[i] = 0.0;

            // Определяем коэффициенты для каждой пары слов
            for (int i = 0; i < len2; ++i)
            {
                for (int j = 0; j < len1; ++j)
                {
                    ranks[i * len1 + j] = GetModifiedTanimotoRank(words2[i], words1[j]);
                }
            }

            // массив признаков того, что слово уже имеет максимальное совпадение
            // с одним из заданных слов.
            bool* usedWords = stackalloc bool[len1];
            for (int i = 0; i < len1; ++i)
                usedWords[i] = false;

            // Ищем максимумы с учётом того, что если слово уже имеет максимальное
            // совпадение с одним из заданных слов, то его нельзя повторно учитывать,
            // даже если оно так же имеет максимальное совпадение с другим заданным словом.
            for (int i = 0; i < len2; ++i)
            {
                max = 0.0;
                wordIndex = 0;
                for (int k = 0; k < len2; ++k)
                {
                    for (int m = 0; m < len1; ++m)
                    {
                        if (ranks[k * len1 + m] > max && !usedWords[m])
                        {
                            max = ranks[k * len1 + m];
                            wordIndex = m;
                        }
                    }
                }

                usedWords[wordIndex] = true;
                resultRank += max;

                // Для слова, которое имеет максимальную похожесть:
                // если первые буквы слов совпадают, то даём дополнительные 0.1 балла
                if (words2[i][0] == words1[wordIndex][0])
                    resultRank += 0.1;
            }

            return resultRank;
        }

        unsafe public static double GetModifiedTanimotoRank(string firstToken, string secondToken)
        {
            const int SUB_TOKEN_LENGTH = 2;
            int equalSubtokensCount = 0;
            int firstCycleLength = firstToken.Length - SUB_TOKEN_LENGTH + 1;
            int secondCycleLength = secondToken.Length - SUB_TOKEN_LENGTH + 1;

            bool* usedTokens = stackalloc bool[secondCycleLength];
            for (int i = 0; i < secondCycleLength; ++i)
                usedTokens[i] = false;

            for (int i = 0; i < firstCycleLength; ++i)
            {
                for (int j = 0; j < secondCycleLength; ++j)
                {
                    if (usedTokens[j] || firstToken[i] != secondToken[j])
                        continue;

                    // Сравнение токенов (в качестве подстрок)
                    for (int k = 1; k < SUB_TOKEN_LENGTH; ++k)
                    {
                        if (firstToken[i + k] != secondToken[j + k])
                            goto CONTINUE;
                    }

                    ++equalSubtokensCount;
                    usedTokens[j] = true;

                CONTINUE:;
                }
            }
            //      Формула Танимото
            return (double)equalSubtokensCount / (firstCycleLength + secondCycleLength - equalSubtokensCount);
        }

        public static double ApproximateDifference(string s1, string s2)
           => ApproximateDifference(s1.FastSplit(), s2.FastSplit());

        private static string[] SplitFullName(string fullName)
            => fullName.Split(' ');
    }
}
