using System;

namespace Miralissa.Shared
{
	public static class StringExtensions
	{
        unsafe public static string[] FastSplit(this string str)
        {
            if (str == null)
                throw new ArgumentNullException(nameof(str));

            if (str.Length > 255)
                throw new ArgumentOutOfRangeException("Длина строки не должна превышать 255 символов");

            int len = str.Length;

            // Буфер для индексов начала слов в строке (максимальный размер)
            byte* wordIndexes = stackalloc byte[(len / 2) + 1];

            // Буфер исправленной строки (без пробелов и знаков пунктуации)
            char* buffer = stackalloc char[len + 1];
            buffer[len] = '\0'; // окончание последнего слова в строке. Иначе в конце последнего слова может попадать мусор

            char tmpChar; // Хранит очередной символ строки
            bool wordIndexFound = false; // Признак того, что в строке найдено начало слова
            int wordCount = 0; // Счётчик слов в строке

            // Создаём буферы с информацией о начале слов в строке,
            // а также избавляемся от пробелов и точек в строке, нормализуем строку
            for (byte i = 0; i < len; ++i)
            {
                tmpChar = str[i];

                switch (tmpChar)
                {
                    case ' ':
                    case '.':
                    case ',':
                    case ':':
                    case ';':
                    case '\n':
                    case '\t':
                    case '\v':
                        if (wordIndexFound)
                        {
                            wordIndexFound = false;
                        }
                        break;

                    default:
                        if (!wordIndexFound) // нашли начало слова в строке
                        {
                            wordIndexFound = true;
                            wordIndexes[wordCount++] = i;
                        }

                        buffer[i] = char.ToLowerInvariant(tmpChar);
                        break;
                }
            }

            string[] words = new string[wordCount];
            for (int w = 0; w < wordCount; ++w)
            {
                words[w] = new string((char*)(buffer + wordIndexes[w]));
            }

            return words;
        }

        /// <summary>
        /// Возращает строку заданную в параметр, если вызывающая строка пуста или null
        /// </summary>
        /// <param name="this">Инспектируемая строка</param>
        /// <param name="default">Значение для возрата если инспектируемая строка пуста или null</param>
        /// <returns>Параметр @default</returns>
        public static string OnNullOrEmpty(this string @this, string @default)
			=> string.IsNullOrEmpty(@this) ? @default : @this;
	}
}
