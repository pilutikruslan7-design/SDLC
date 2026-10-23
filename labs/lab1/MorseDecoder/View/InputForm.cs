using MorseDecoder.Model;

namespace MorseDecoder.View;

/// <summary>Дополнительное окно ввода сигналов азбуки Морзе.</summary>
public sealed class InputForm : Form
{
    private const string RussianSample = ".--. .-. .. .-- . - / -- .. .-.";      // ПРИВЕТ МИР
    private const string LatinSample = ".... . .-.. .-.. --- / .-- --- .-. .-.. -.."; // HELLO WORLD

    private readonly TextBox _signalsBox;
    private readonly RadioButton _russianRadio;
    private readonly RadioButton _latinRadio;

    public InputForm(MorseInput initial)
    {
        Text = "Ввод данных";
        Font = new Font("Segoe UI", 10F);
        AutoScaleMode = AutoScaleMode.Font;
        FormBorderStyle = FormBorderStyle.FixedDialog;
        StartPosition = FormStartPosition.CenterParent;
        MaximizeBox = false;
        MinimizeBox = false;
        ShowInTaskbar = false;
        ClientSize = new Size(560, 330);

        var layout = new TableLayoutPanel
        {
            Dock = DockStyle.Fill,
            Padding = new Padding(14),
            ColumnCount = 1,
            RowCount = 4
        };
        layout.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));
        layout.RowStyles.Add(new RowStyle(SizeType.AutoSize));
        layout.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));
        layout.RowStyles.Add(new RowStyle(SizeType.AutoSize));
        layout.RowStyles.Add(new RowStyle(SizeType.AutoSize));

        var hintLabel = new Label
        {
            Text = "Введите сигналы: точка «.», тире «-». Знаки разделяйте пробелом, " +
                   "слова — косой чертой «/» или двумя пробелами.",
            AutoSize = true,
            MaximumSize = new Size(520, 0),
            Margin = new Padding(3, 0, 3, 8)
        };

        _signalsBox = new TextBox
        {
            Multiline = true,
            AcceptsReturn = false,
            ScrollBars = ScrollBars.Vertical,
            Font = new Font("Consolas", 12F),
            Dock = DockStyle.Fill,
            Text = initial.Signals
        };

        var alphabetGroup = new GroupBox
        {
            Text = "Алфавит",
            AutoSize = true,
            Dock = DockStyle.Fill,
            Padding = new Padding(8, 4, 8, 4)
        };
        var radioPanel = new FlowLayoutPanel { Dock = DockStyle.Fill, AutoSize = true, WrapContents = false };
        _russianRadio = new RadioButton { Text = "Русский (кириллица)", AutoSize = true, Margin = new Padding(3, 3, 24, 3) };
        _latinRadio = new RadioButton { Text = "Латинский", AutoSize = true };
        radioPanel.Controls.AddRange(new Control[] { _russianRadio, _latinRadio });
        alphabetGroup.Controls.Add(radioPanel);

        if (initial.Alphabet == MorseAlphabet.Latin)
            _latinRadio.Checked = true;
        else
            _russianRadio.Checked = true;

        var sampleButton = new Button { Text = "Пример", AutoSize = true, Padding = new Padding(6, 2, 6, 2) };
        sampleButton.Click += (_, _) => _signalsBox.Text = _latinRadio.Checked ? LatinSample : RussianSample;

        var okButton = new Button { Text = "Декодировать", AutoSize = true, Padding = new Padding(6, 2, 6, 2), DialogResult = DialogResult.OK };
        var cancelButton = new Button { Text = "Отмена", AutoSize = true, Padding = new Padding(6, 2, 6, 2), DialogResult = DialogResult.Cancel };

        var buttonsPanel = new FlowLayoutPanel
        {
            Dock = DockStyle.Fill,
            AutoSize = true,
            FlowDirection = FlowDirection.RightToLeft,
            Margin = new Padding(0, 8, 0, 0)
        };
        buttonsPanel.Controls.AddRange(new Control[] { cancelButton, okButton, sampleButton });

        layout.Controls.Add(hintLabel, 0, 0);
        layout.Controls.Add(_signalsBox, 0, 1);
        layout.Controls.Add(alphabetGroup, 0, 2);
        layout.Controls.Add(buttonsPanel, 0, 3);
        Controls.Add(layout);

        AcceptButton = okButton;
        CancelButton = cancelButton;

        Shown += (_, _) =>
        {
            _signalsBox.Focus();
            _signalsBox.SelectAll();
        };
    }

    public MorseInput Result =>
        new(_signalsBox.Text, _latinRadio.Checked ? MorseAlphabet.Latin : MorseAlphabet.Russian);
}
