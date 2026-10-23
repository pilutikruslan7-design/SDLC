namespace MorseDecoder.Model;

/// <summary>Данные, вводимые пользователем: строка сигналов и выбранный алфавит.</summary>
public sealed record MorseInput(string Signals, MorseAlphabet Alphabet);
