namespace MorseDecoder.Model;

/// <summary>
/// Активная модель MVC. Хранит последние введённые данные и результат декодирования
/// и при каждом изменении состояния сама уведомляет зарегистрированных наблюдателей
/// (паттерн «Наблюдатель»). О конкретных классах представления модель не знает.
/// </summary>
public sealed class MorseModel
{
    private readonly List<IMorseModelObserver> _observers = new();

    /// <summary>Последняя успешно декодированная строка сигналов.</summary>
    public string Signals { get; private set; } = string.Empty;

    public MorseAlphabet Alphabet { get; private set; } = MorseAlphabet.Russian;

    public string DecodedText { get; private set; } = string.Empty;

    public int SymbolCount { get; private set; }

    public int WordCount { get; private set; }

    public bool HasResult { get; private set; }

    /// <summary>Последние введённые данные — для восстановления в окне ввода.</summary>
    public MorseInput LastInput => new(Signals, Alphabet);

    public void Attach(IMorseModelObserver observer)
    {
        if (!_observers.Contains(observer))
            _observers.Add(observer);
    }

    public void Detach(IMorseModelObserver observer) => _observers.Remove(observer);

    /// <summary>
    /// Декодирует сигналы и обновляет состояние модели.
    /// При некорректных данных бросает <see cref="MorseDecodeException"/>, состояние не меняется.
    /// </summary>
    public void Decode(MorseInput input)
    {
        DecodeResult result = MorseTranslator.Decode(input.Signals, input.Alphabet);

        Signals = input.Signals.Trim();
        Alphabet = input.Alphabet;
        DecodedText = result.Text;
        SymbolCount = result.SymbolCount;
        WordCount = result.WordCount;
        HasResult = true;

        NotifyObservers();
    }

    private void NotifyObservers()
    {
        foreach (IMorseModelObserver observer in _observers.ToArray())
            observer.ModelChanged();
    }
}
