
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

    
  }
}
