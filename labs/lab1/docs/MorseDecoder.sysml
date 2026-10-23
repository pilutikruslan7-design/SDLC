package MorseDecoderSystem {
    doc /* Лабораторная работа №1, вариант 16: утилита декодирования текста из азбуки Морзе. */

    private import ScalarValues::*;

    // ---------- Типы данных ----------

    enum def MorseAlphabet {
        enum Russian;
        enum Latin;
    }

    attribute def MorseInput {
        attribute signals : String;
        attribute alphabet : MorseAlphabet;
    }

    attribute def DecodeResult {
        attribute text : String;
        attribute symbolCount : Integer;
        attribute wordCount : Integer;
    }

    // ---------- Порты ----------

    port def ActionPort {
        doc /* Действие пользователя «Ввести данные» (View -> Controller). */
        out item enterDataRequested;
    }

    port def DecodePort {
        doc /* Передача введённых данных в модель (Controller -> Model). */
        out item input : MorseInput;
    }

    port def NotifyPort {
        doc /* Уведомление активной модели об изменении состояния (Model -> View). */
        out item modelChanged;
    }

    // ---------- Структура ----------

    part def MorseModel {
        attribute lastInput : MorseInput;
        attribute result : DecodeResult;
        port decode : ~DecodePort;
        port notify : NotifyPort;
    }

    part def MainForm {
        port actions : ActionPort;
        port notify : ~NotifyPort;
    }

    part def InputForm;

    part def MorseController {
        port actions : ~ActionPort;
        port decode : DecodePort;
    }

    part def Application {
        part model : MorseModel;
        part mainView : MainForm;
        part inputView : InputForm;
        part controller : MorseController;

        connect mainView.actions to controller.actions;
        connect controller.decode to model.decode;
        connect model.notify to mainView.notify;
    }

    // ---------- Поведение ----------

    action def DecodeMorse {
        in input : MorseInput;
        out result : DecodeResult;

        action normalize;
        action splitWords;
        action lookupCodes;
        action notifyObservers;

        first normalize then splitWords;
        first splitWords then lookupCodes;
        first lookupCodes then notifyObservers;
    }

    // ---------- Требования варианта ----------

    requirement def DecodeRequirement {
        doc /* Программа преобразует введённые сигналы азбуки Морзе в текст. */
    }

    requirement def SeparateInputWindowRequirement {
        doc /* Данные вводятся в дополнительном окне, открываемом кнопкой «Ввести данные». */
    }

    requirement def RestoreLastInputRequirement {
        doc /* Последние введённые данные запоминаются и восстанавливаются при повторном вводе. */
    }

    requirement def ShowResultRequirement {
        doc /* Результат декодирования отображается на главной форме. */
    }

    requirement def InvalidInputRequirement {
        doc /* При вводе некорректных данных выводится сообщение об ошибке. */
    }

    requirement def ActiveModelRequirement {
        doc /* Приложение построено по схеме MVC с активной моделью. */
    }

    requirement decode : DecodeRequirement;
    requirement separateInputWindow : SeparateInputWindowRequirement;
    requirement restoreLastInput : RestoreLastInputRequirement;
    requirement showResult : ShowResultRequirement;
    requirement invalidInput : InvalidInputRequirement;
    requirement activeModel : ActiveModelRequirement;

    part variant16 : Application;

    satisfy decode by variant16;
    satisfy separateInputWindow by variant16;
    satisfy restoreLastInput by variant16;
    satisfy showResult by variant16;
    satisfy invalidInput by variant16;
    satisfy activeModel by variant16;
}
