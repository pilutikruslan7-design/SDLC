namespace MorseDecoder.Model;

/// <summary>
/// Наблюдатель активной модели. Модель сама оповещает подписчиков об изменении
/// своего состояния, после чего они читают из неё актуальные данные.
/// </summary>
public interface IMorseModelObserver
{
    void ModelChanged();
}
