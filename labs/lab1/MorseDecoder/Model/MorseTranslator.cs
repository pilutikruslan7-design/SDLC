using System.Text;
using System.Text.RegularExpressions;

namespace MorseDecoder.Model;

/// <summary>Результат декодирования.</summary>
public sealed record DecodeResult(string Text, int SymbolCount, int WordCount);

/// <summary>
/// Декодер азбуки Морзе.
/// Формат ввода: точка «.» и тире «-», знаки разделяются пробелом,
/// слова — косой чертой «/» (или «|», переводом строки, двумя и более пробелами).
/// </summary>
public static class MorseTranslator
{
    private static readonly Regex WordSeparator = new(@"\s*/\s*| {2,}", RegexOptions.Compiled);

    public static DecodeResult Decode(string? signals, MorseAlphabet alphabet)
    {
        if (string.IsNullOrWhiteSpace(signals))
            throw new MorseDecodeException("Строка сигналов пуста.\nВведите код азбуки Морзе, например: ... --- ...");

        string normalized = Normalize(signals).Trim();

        var words = new List<string>();
        int symbolCount = 0;

        foreach (string word in WordSeparator.Split(normalized))
        {
            string[] codes = word.Split(' ', StringSplitOptions.RemoveEmptyEntries);
            if (codes.Length == 0)
                continue;

            var decodedWord = new StringBuilder(codes.Length);
            foreach (string code in codes)
            {
                if (!MorseTable.TryGetSymbol(code, alphabet, out char symbol))
                {
                    throw new MorseDecodeException(
                        $"Неизвестная комбинация «{code}» (слово {words.Count + 1}, знак {decodedWord.Length + 1}).\n" +
                        $"Проверьте код и выбранный алфавит ({alphabet.ToDisplayName()}).");
                }

                decodedWord.Append(symbol);
            }

            symbolCount += decodedWord.Length;
            words.Add(decodedWord.ToString());
        }

        if (words.Count == 0)
            throw new MorseDecodeException("Не найдено ни одного знака: введены только разделители.");

        return new DecodeResult(string.Join(' ', words), symbolCount, words.Count);
    }

    /// <summary>
    /// Приводит альтернативные обозначения точки/тире к «.» и «-», а разделители слов — к «/».
    /// Любой другой символ считается ошибкой ввода.
    /// </summary>
    private static string Normalize(string signals)
    {
        var result = new StringBuilder(signals.Length);

        for (int i = 0; i < signals.Length; i++)
        {
            char c = signals[i];
            switch (c)
            {
                case '.' or '·' or '•':
                    result.Append('.');
                    break;
                case '-' or '−' or '–' or '—' or '_':
                    result.Append('-');
                    break;
                case ' ' or '\t' or ' ':
                    result.Append(' ');
                    break;
                case '/' or '|' or '\n':
                    result.Append('/');
                    break;
                case '\r':
                    break;
                default:
                    throw new MorseDecodeException(
                        $"Недопустимый символ «{c}» в позиции {i + 1}.\n" +
                        "Разрешены только точка (.), тире (-), пробел и косая черта (/).");
            }
        }

        return result.ToString();
    }
}
