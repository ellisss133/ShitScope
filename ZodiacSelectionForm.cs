using System;
using System.Drawing;
using System.Windows.Forms;

namespace Shitscope
{
  public class ZodiacSelectionForm : Form
  {
    private ListBox zodiacListBox;
    private Button selectButton;

    public string SelectedZodiac { get; private set; }

    public ZodiacSelectionForm()
    {
      InitializeComponent();
    }

    private void InitializeComponent()
    {
      this.Text = "☾ выбери свой знак, звезда";
      this.BackColor = Color.FromArgb(30, 30, 40);
      this.ForeColor = Color.White;
      this.Size = new Size(220, 420);
      this.FormBorderStyle = FormBorderStyle.FixedSingle;
      this.MaximizeBox = false;
      this.Font = new Font("Segoe UI", 12F, FontStyle.Bold);

      zodiacListBox = new ListBox
      {
        Dock = DockStyle.Fill,
        Font = new Font("Segoe UI", 14F, FontStyle.Regular),
        BackColor = Color.FromArgb(40, 40, 50),
        ForeColor = Color.FromArgb(220, 220, 255),
        BorderStyle = BorderStyle.None,
        ItemHeight = 40,
        IntegralHeight = false,
        SelectionMode = SelectionMode.One
      };
      zodiacListBox.Items.AddRange(new string[]
      {
        "Овен",
        "Телец",
        "Близнецы",
        "Рак",
        "Лев",
        "Дева",
        "Весы",
        "Скорпион",
        "Стрелец",
        "Козерог",
        "Водолей",
        "Рыбы"
      });
      this.Controls.Add(zodiacListBox);

      selectButton = new Button
      {
        Text = "☄ выбрать",
        Dock = DockStyle.Bottom,
        Height = 50,
        FlatStyle = FlatStyle.Flat,
        Font = new Font("Segoe UI", 13F, FontStyle.Bold),
        BackColor = Color.FromArgb(70, 70, 90),
        ForeColor = Color.FromArgb(220, 220, 255),
        Cursor = Cursors.Hand,
        Padding = new Padding(0)
      };
      selectButton.FlatAppearance.BorderSize = 0;
      selectButton.MouseEnter += (s, e) => selectButton.BackColor = Color.FromArgb(110, 110, 160);
      selectButton.MouseLeave += (s, e) => selectButton.BackColor = Color.FromArgb(70, 70, 90);
      selectButton.Click += SelectButton_Click;
      this.Controls.Add(selectButton);
    }

    private void SelectButton_Click(object sender, EventArgs e)
    {
      if (zodiacListBox.SelectedItem != null)
      {
        SelectedZodiac = zodiacListBox.SelectedItem.ToString();
        this.DialogResult = DialogResult.OK;
        this.Close();
      }
      else
      {
        MessageBox.Show("ну выбери хоть что-то, а?");
      }
    }
  }
}