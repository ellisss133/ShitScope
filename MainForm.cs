using System;
using System.IO;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;
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

    private const string SettingsFile = "user_settings.json";
    private string selectedZodiac = "";

    public MainForm()
    {
      InitializeComponent();
      SetupCustomUI();
      LoadSettings();

      if (!string.IsNullOrEmpty(selectedZodiac))
      {
        ShowDailyInfo();
      }
      else
      {
        ShowZodiacSelection();
      }

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

    private void CenterLoadingLabel()
    {
      loadingLabel.Location = new Point(
        (this.ClientSize.Width - loadingLabel.Width) / 2,
        (this.ClientSize.Height - loadingLabel.Height) / 2
      );
    }

    private async void ShowDailyInfo()
    {
      dotCount = 0;
      loadingLabel.Text = "загружаю твой дерьмоскоп";
      loadingLabel.Visible = true;
      loadingDotsTimer.Start();

      infoLabel.Visible = false;
      catPicture.Image = null;
      catPanel.Visible = false;

      var data = await FetchData();

      loadingDotsTimer.Stop();
      loadingLabel.Visible = false;

      infoLabel.Text = $"🔥 гороскоп дня: {selectedZodiac}\n\n{data.Text}";
      infoLabel.Visible = true;

      if (!string.IsNullOrEmpty(data.CatUrl))
      {
        try
        {
          var request = System.Net.WebRequest.Create(data.CatUrl);
          using var response = await request.GetResponseAsync();
          using var stream = response.GetResponseStream();
          catPicture.Image = Image.FromStream(stream);
          catPanel.Visible = true;
        }
        catch
        {
          // не смог загрузить кота — ну и хрен с ним
        }
      }
    }

    private void LoadSettings()
    {
      if (File.Exists(SettingsFile))
      {
        var json = File.ReadAllText(SettingsFile);
        var obj = JsonSerializer.Deserialize<UserSettings>(json);
        selectedZodiac = obj?.ZodiacSign ?? "";
      }
    }

    private void SaveSettings()
    {
      var obj = new UserSettings { ZodiacSign = selectedZodiac };
      var json = JsonSerializer.Serialize(obj);
      File.WriteAllText(SettingsFile, json);
    }

    private string TranslateZodiacToEnglish(string russianZodiac)
    {
      return russianZodiac switch
      {
        "Овен" => "aries",
        "Телец" => "taurus",
        "Близнецы" => "gemini",
        "Рак" => "cancer",
        "Лев" => "leo",
        "Дева" => "virgo",
        "Весы" => "libra",
        "Скорпион" => "scorpio",
        "Стрелец" => "sagittarius",
        "Козерог" => "capricorn",
        "Водолей" => "aquarius",
        "Рыбы" => "pisces",
        _ => "aries"
      };
    }

    private void ShowZodiacSelection()
    {
      var zodiacForm = new ZodiacSelectionForm();
      if (zodiacForm.ShowDialog() == DialogResult.OK)
      {
        selectedZodiac = zodiacForm.SelectedZodiac;
        SaveSettings();
        ShowDailyInfo();
      }
      else
      {
        Application.Exit();
      }
    }

    private async Task<DailyInfo> FetchData()
    {
      using var client = new HttpClient();

      try
      {
        string result = "";

        var zodiacForApi = TranslateZodiacToEnglish(selectedZodiac);
        var horoscopeJson = await client.GetStringAsync($"https://ohmanda.com/api/horoscope/{zodiacForApi}");
        using var horoscopeDoc = JsonDocument.Parse(horoscopeJson);
        var horoscopeText = horoscopeDoc.RootElement.GetProperty("horoscope").GetString();
        var horoscopeTranslated = await TranslateToRussian(horoscopeText);
        result += $"🌙 Гороскоп:\n{horoscopeTranslated}\n\n";

        var adviceJson = await client.GetStringAsync("https://api.adviceslip.com/advice");
        using var adviceDoc = JsonDocument.Parse(adviceJson);
        var adviceText = adviceDoc.RootElement.GetProperty("slip").GetProperty("advice").GetString();
        var adviceTranslated = await TranslateToRussian(adviceText);
        result += $"📌 Твой совет на день:\n{adviceTranslated}\n\n";

        var jokeJson = await client.GetStringAsync("https://v2.jokeapi.dev/joke/Any?type=single");
        using var jokeDoc = JsonDocument.Parse(jokeJson);
        var jokeText = jokeDoc.RootElement.GetProperty("joke").GetString();
        var jokeTranslated = await TranslateToRussian(jokeText);
        result += $"😂 Сегодня расскажи этот анекдот другу!\n{jokeTranslated}\n\n";

        var catJson = await client.GetStringAsync("https://api.thecatapi.com/v1/images/search");
        using var catDoc = JsonDocument.Parse(catJson);
        var catUrl = catDoc.RootElement[0].GetProperty("url").GetString();

        return new DailyInfo { Text = result, CatUrl = catUrl };
      }
      catch (Exception ex)
      {
        return new DailyInfo { Text = $"🔥 Ошибка при получении данных: {ex.Message}", CatUrl = null };
      }
    }

    private async Task<string> TranslateToRussian(string text)
    {
      try
      {
        using var client = new HttpClient();
        string url = "https://translate.googleapis.com/translate_a/single?client=gtx&sl=en&tl=ru&dt=t&q=" + Uri.EscapeDataString(text);
        var response = await client.GetAsync(url);
        if (!response.IsSuccessStatusCode)
          return text;

        var json = await response.Content.ReadAsStringAsync();
        using var doc = JsonDocument.Parse(json);

        var sentences = doc.RootElement[0];
        var sb = new System.Text.StringBuilder();

        foreach (var segment in sentences.EnumerateArray())
        {
          var translatedPart = segment[0].GetString();
          if (!string.IsNullOrEmpty(translatedPart))
            sb.Append(translatedPart);
        }

        return sb.ToString();
      }
      catch
      {
        return text;
      }
    }

    public class UserSettings
    {
      public string ZodiacSign { get; set; }
    }

    public class DailyInfo
    {
      public string Text { get; set; }
      public string CatUrl { get; set; }
    }

    private void MainForm_Load(object sender, EventArgs e)
    {
    }
  }
}
