
using System;
using System.Drawing;
using System.Windows.Forms;

namespace Shitscope
{
  public partial class MainForm : Form
  {
    private Button changeZodiacButton;
    private Label infoLabel;
    private PictureBox catPicture;

    private Label loadingLabel;
    private Timer loadingDotsTimer;
    private int dotCount = 0;

    private Label catCaption;
    private Panel catPanel;

    public MainForm()
    {
      InitializeComponent();
      SetupCustomUI();

      this.Resize += (s, e) =>
      {
        CenterLoadingLabel();
      };
    }

    private void SetupCustomUI()
    {
      this.BackColor = Color.FromArgb(30, 30, 40);
      this.ForeColor = Color.White;
      this.Font = new Font("Segoe UI Semibold", 14F, FontStyle.Bold);
      this.Text = "🔥 shitscope - гороскоп с котиками";
      this.MinimumSize = new Size(900, 600);

      infoLabel = new Label
      {
        Dock = DockStyle.Fill,
        Font = new Font("Consolas", 15F, FontStyle.Regular),
        TextAlign = ContentAlignment.TopLeft,
        Padding = new Padding(30),
        AutoSize = false,
        BackColor = Color.FromArgb(40, 40, 50),
        ForeColor = Color.FromArgb(220, 220, 255),
        Text = "",
        Visible = false
      };
      this.Controls.Add(infoLabel);

      loadingLabel = new Label
      {
        Size = new Size(400, 50),
        Font = new Font("Consolas", 16F, FontStyle.Regular),
        TextAlign = ContentAlignment.MiddleCenter,
        BackColor = Color.FromArgb(40, 40, 50),
        ForeColor = Color.FromArgb(220, 220, 255),
        Text = "загружаю твой дерьмоскоп",
        Visible = true
      };
      this.Controls.Add(loadingLabel);
      CenterLoadingLabel();

      catPanel = new Panel
      {
        Dock = DockStyle.Bottom,
        Height = 300,
        Padding = new Padding(10),
        BackColor = Color.FromArgb(40, 40, 50),
        Visible = false
      };
      this.Controls.Add(catPanel);

      catCaption = new Label
      {
        Text = "Держи котика для хорошего настроения!",
        Dock = DockStyle.Top,
        TextAlign = ContentAlignment.MiddleCenter,
        Font = new Font("Segoe UI Semibold", 16F, FontStyle.Italic | FontStyle.Bold),
        ForeColor = Color.FromArgb(200, 200, 255),
        Height = 40
      };
      catPanel.Controls.Add(catCaption);

      catPicture = new PictureBox
      {
        Dock = DockStyle.Fill,
        SizeMode = PictureBoxSizeMode.Zoom,
        BackColor = Color.FromArgb(40, 40, 50),
        Margin = new Padding(0, 10, 0, 0)
      };
      catPanel.Controls.Add(catPicture);

      catPanel.Controls.SetChildIndex(catCaption, 1);
      catPanel.Controls.SetChildIndex(catPicture, 0);

      changeZodiacButton = new Button
      {
        Text = "☾ сменить знак зодиака",
        Dock = DockStyle.Bottom,
        Height = 50,
        FlatStyle = FlatStyle.Flat,
        Font = new Font("Segoe UI", 13F, FontStyle.Bold),
        BackColor = Color.FromArgb(70, 70, 90),
        ForeColor = Color.FromArgb(220, 220, 255),
        Cursor = Cursors.Hand,
        Padding = new Padding(15, 0, 0, 0)
      };
      changeZodiacButton.FlatAppearance.BorderSize = 0;
      changeZodiacButton.MouseEnter += (s, e) => changeZodiacButton.BackColor = Color.FromArgb(110, 110, 160);
      changeZodiacButton.MouseLeave += (s, e) => changeZodiacButton.BackColor = Color.FromArgb(70, 70, 90);
      changeZodiacButton.Click += (s, e) => ShowZodiacSelection();
      this.Controls.Add(changeZodiacButton);

      loadingDotsTimer = new Timer { Interval = 500 };
      loadingDotsTimer.Tick += (s, e) =>
      {
        dotCount = (dotCount + 1) % 4;
        loadingLabel.Text = "загружаю твой дерьмоскоп" + new string('.', dotCount);
      };
    }


  }
}
