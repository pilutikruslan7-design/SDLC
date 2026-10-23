namespace MorseDecoder.Model;

/// <summary>Алфавит, по которому расшифровываются буквенные коды.</summary>
public enum MorseAlphabet
{
    Russian,
    Latin
}

public static class MorseAlphabetExtensions
{
    public static string ToDisplayName(this MorseAlphabet alphabet) => alphabet switch
    {
        MorseAlphabet.Russian => "русский (кириллица)",
        MorseAlphabet.Latin => "латинский",
        _ => alphabet.ToString()
    };
}
