using System;
using System.Collections.Generic;
using System.Linq;
using System.Drawing;
using System.IO;
using System.Windows.Forms;
using System.Diagnostics;
using System.ComponentModel;
using System.Globalization;

namespace WinxClubTweakCenter
{
    public partial class MainForm : Form
    {
        private string doneMessage = "Done!";
        private string helpText;

        private readonly string folderToApply;
        private readonly bool[] tweaks;
        private readonly int resolutionIndex;
        public MainForm(string folderToApply, bool[] tweaks, int resolutionIndex)
        {
            this.folderToApply = folderToApply;
            this.tweaks = tweaks;
            this.resolutionIndex = resolutionIndex;
            InitializeComponent();
        }

        private void Apply(string gameFolder)
        {
            byte[] buffer;
            using (var bw = new BinaryWriter(new FileStream($@"{gameFolder}\WinxClub.exe", FileMode.Open)))
            {
                var resolution = new DisplayModesHelper.Resolution(new Size(1024, 768));
                if (checkBoxResolution.Checked)
                    resolution = (DisplayModesHelper.Resolution)inputResolution.SelectedItem;
                bw.Seek(0xD5E5, SeekOrigin.Begin);
                bw.Write(resolution.Size.Width);
                bw.Seek(0xD5EC, SeekOrigin.Begin);
                bw.Write(resolution.Size.Height);
                bw.Seek(0xD617, SeekOrigin.Begin);
                bw.Write(resolution.Size.Width);
                bw.Seek(0xD61E, SeekOrigin.Begin);
                bw.Write(resolution.Size.Height);
                bw.Seek(0x53BD1, SeekOrigin.Begin);
                bw.Write(resolution.Size.Width);
                bw.Seek(0x53BD8, SeekOrigin.Begin);
                bw.Write(resolution.Size.Height);
                bw.Seek(0xC2681, SeekOrigin.Begin);
                bw.Write(resolution.Size.Width);
                bw.Seek(0xC2689, SeekOrigin.Begin);
                bw.Write(resolution.Size.Height);
                bw.Seek(0xE7777, SeekOrigin.Begin);
                bw.Write(resolution.Size.Width);
                bw.Seek(0xE777F, SeekOrigin.Begin);
                bw.Write(resolution.Size.Height);
                bw.Seek(0x30ED8C, SeekOrigin.Begin);
                bw.Write($"{resolution}\0".ToCharArray());

                buffer = new byte[] { 0x89, 0x86, 0x9C, 0x02, 0x00, 0x00 };
                if (checkBoxFirstPersonMovement.Checked)
                    buffer = new byte[] { 0x90, 0x90, 0x90, 0x90, 0x90, 0x90 };
                bw.Seek(0xE0382, SeekOrigin.Begin);
                bw.Write(buffer);
                buffer = new byte[] { 0x40 };
                if (checkBoxFirstPersonMovement.Checked)
                    buffer = new byte[] { 0xC0 };
                bw.Seek(0x1C7, SeekOrigin.Begin);
                bw.Write(buffer);
                buffer = new byte[] { 0xE8, 0xA8, 0xC2, 0xFF, 0xFF };
                if (checkBoxFirstPersonMovement.Checked)
                    buffer = new byte[] { 0xE9, 0xFC, 0x69, 0x1F, 0x00 };
                bw.Seek(0xE0AD3, SeekOrigin.Begin);
                bw.Write(buffer);
                buffer = new byte[] { 0x2D, 0x13, 0x27, 0x00, 0x00 };
                if (checkBoxFirstPersonMovement.Checked)
                    buffer = new byte[] { 0xE8, 0x1F, 0x6B, 0x1F, 0x00 };
                bw.Seek(0xE09C4, SeekOrigin.Begin);
                bw.Write(buffer);
                buffer = new byte[36];
                if (checkBoxFirstPersonMovement.Checked)
                    buffer = new byte[] {
                        0xC7, 0x05, 0xC0, 0x4F, 0x6F, 0x00, 0x00, 0x00, 0xF0, 0x42,
                        0xE8, 0x9D, 0x58, 0xE0, 0xFF,
                        0xE9, 0xF0, 0x95, 0xE0, 0xFF,
                        0xC7, 0x05, 0xC0, 0x4F, 0x6F, 0x00, 0x00, 0x00, 0x16, 0x43,
                        0x2D, 0x13, 0x27, 0x00, 0x00,
                        0xC3 };
                bw.Seek(0x2D74D4, SeekOrigin.Begin);
                bw.Write(buffer);

                buffer = new byte[] { 0x84, 0xC0, 0x0F, 0x84, 0x53, 0xFF, 0xFF, 0xFF };
                if (checkBoxFirstPersonAnywhere.Checked)
                    buffer = new byte[] { 0x90, 0x90, 0x90, 0x90, 0x90, 0x90, 0x90, 0x90 };
                bw.Seek(0xE0AD8, SeekOrigin.Begin);
                bw.Write(buffer);

                buffer = new byte[] { 0x12 };
                if (checkBoxFlight.Checked)
                    buffer = new byte[] { 0x00 };
                bw.Seek(0x124FED, SeekOrigin.Begin);
                bw.Write(buffer);
            }

            using (var bw = new BinaryWriter(new FileStream($@"{gameFolder}\Media\Characters\Bloom\Player.spt", FileMode.Open)))
            {
                var elevationMinimum = 0.0f;
                if (checkBoxLookingUp.Checked)
                    elevationMinimum = -1.0f;
                bw.Seek(0x5B4, SeekOrigin.Begin);
                bw.Write(elevationMinimum);
            }

            const string testCinematicKey = "testCinematic";
            const string cinematicToTestKey = "cinematicToTest";
            var testCinematicValue = "false";
            var cinematicToTestValue = "0";
            if (checkBoxSkipLogos.Checked)
            {
                testCinematicValue = "true";
                cinematicToTestValue = "82";
            }
            var winxIniPath = $@"{gameFolder}\winx.ini";
            var winxIniLines = new List<string>(File.ReadAllLines(winxIniPath));
            var testCinematicMissing = true;
            var cinematicToTestMissing = true;
            for (int i = 0; i < winxIniLines.Count; i++)
            {
                if (winxIniLines[i].StartsWith(testCinematicKey))
                {
                    winxIniLines[i] = $"{testCinematicKey}={testCinematicValue}";
                    testCinematicMissing = false;
                }
                else if (winxIniLines[i].StartsWith(cinematicToTestKey))
                {
                    winxIniLines[i] = $"{cinematicToTestKey}={cinematicToTestValue}";
                    cinematicToTestMissing = false;
                }
            }
            if (testCinematicMissing)
                winxIniLines.Add($"{testCinematicKey}={testCinematicValue}");
            if (cinematicToTestMissing)
                winxIniLines.Add($"{cinematicToTestKey}={cinematicToTestValue}");
            File.WriteAllLines(winxIniPath, winxIniLines.ToArray());

            using (var bw = new BinaryWriter(new FileStream($@"{gameFolder}\Media\Levels\Challenges\star_01.spt", FileMode.Open)))
            {
                var time = 70000;
                if (checkBoxStarChallenges.Checked)
                    time = 135000;
                bw.Seek(0x70C, SeekOrigin.Begin);
                bw.Write(time);
            }

            using (var bw = new BinaryWriter(new FileStream($@"{gameFolder}\Media\Levels\Challenges\star_02.spt", FileMode.Open)))
            {
                var time = 75000;
                if (checkBoxStarChallenges.Checked)
                    time = 140000;
                bw.Seek(0x70C, SeekOrigin.Begin);
                bw.Write(time);
            }

            using (var bw = new BinaryWriter(new FileStream($@"{gameFolder}\Media\Levels\Challenges\star_03.spt", FileMode.Open)))
            {
                var time = 135000;
                if (checkBoxStarChallenges.Checked)
                    time = 230000;
                bw.Seek(0x70C, SeekOrigin.Begin);
                bw.Write(time);
            }

            MessageBox.Show(doneMessage);
        }

        private void MainForm_Load(object sender, EventArgs e)
        {
            helpText =
$@"Application for game tweaking. After click on ""{buttonApply.Text}"" and game folder selection chosen tweaks will be applied and unchosen ones will be reverted, if were applied earlier.

""{checkBoxResolution.Text}"": sets resolution both in main menu and in game, overwrites 1024x768. Don't forget to change resolution in your save display settings.

""{checkBoxFirstPersonMovement.Text}"": you always could turn on the first person camera mode by middle mouse button. Unfortunately, it wasn't compatible with movement. This tweak fixes it. v2.0: now Bloom looks from her eyes.

""{checkBoxFirstPersonAnywhere.Text}"": there are many places in game where you can't rotate camera. This tweak allows you to turn on first person camera there, thus letting you know what developers hid from you that way.

""{checkBoxLookingUp.Text}"": allows looking at sky, at ceiling, ... up skirts. This game is not NieR: Automata, though: there is nothing to look at.

""{checkBoxSkipLogos.Text}"": really boosts starting up. Yes, logos hid this art behind them all the time.

""{checkBoxStarChallenges.Text}"": timers in these challenges are merciless! You'll have enough time for collecting all stars with this tweak.

""{checkBoxFlight.Text}"": Bloom is a fairy, she must fly! Well, with level design in Winx Club, this tweak is pretty much a cheat. To fly up, press (not hold!) jump key. In air Bloom will lose altitude slowly. To fall quickly, hold shield key or attack (does not work in Alfea). It is not recommended to fly too high, as you may stuck. Sadly, animation is glitchy.";

            if (CultureInfo.CurrentCulture.TwoLetterISOLanguageName == "ru")
            {
                buttonApply.Text = "Применить";
                buttonHelp.Text = "Помощь";
                checkBoxResolution.Text = "Установить разрешение";
                checkBoxFirstPersonMovement.Text = "Разрешить перемещение с камерой от первого лица";
                checkBoxFirstPersonAnywhere.Text = "Разрешить камеру от первого лица везде";
                checkBoxLookingUp.Text = "Разрешить смотреть вверх";
                checkBoxSkipLogos.Text = "Пропускать логотипы при загрузке";
                checkBoxStarChallenges.Text = "Увеличить время в звёздных испытаниях";
                checkBoxFlight.Text = "Включить способность к полёту";
                doneMessage = "Готово!";
                helpText =
$@"Программа предназначена для применения твиков к игре. При нажатии кнопки ""{buttonApply.Text}"" и последующем выборе папки игры будут произведены применение выбранных твиков и откат невыбранных ранее применённых.

""{checkBoxResolution.Text}"": твик, устанавливающий разрешение и в главном меню, и в игре. Не забудьте изменить разрешение в настройках экрана вашего сохранения.

""{checkBoxFirstPersonMovement.Text}"": если вы до сих пор не знали, в игре уже есть режим просмотра от первого лица, активируемый по нажатию средней кнопки мыши. К сожалению, как только персонаж двигается, камера вновь переходит в режим от третьего лица. Данный твик призван исправить это недоразумение, и позволить игроку самому переключать режимы. v2.0: теперь Блум смотрит из глаз, а не из макушки.

""{checkBoxFirstPersonAnywhere.Text}"": в игре часто встречаются места с невращающейся камерой. С этим твиком легко узнать, что таким образом от нас скрыли разработчики, достаточно включить раннее недоступную в таких местах камеру от первого лица.

""{checkBoxLookingUp.Text}"": позволяет смотреть на небо и на потолок. Ну, и бонусом, под юбки. Правда, это вам не NieR: Automata, и смотреть там не на что.

""{checkBoxSkipLogos.Text}"": с этим твиком игра запускается намного быстрее. Да, за логотипами всё это время скрывался арт с девчонками на скутерах.

""{checkBoxStarChallenges.Text}"": какой садомазохист устанавливал таймеры в этих испытниях? Данный твик даёт достаточно времени на сбор всех звёзд.

""{checkBoxFlight.Text}"": Блум, вообще-то, фея, и ей положено летать (хотя в условиях этой игры такой твик будет скорее читом). Чтобы подняться вверх, нажмите (не зажмите!) кнопку прыжка. Находясь в воздухе, Блум будет медленно терять высоту. Чтобы ускорить падение, зажмите кнопку щита или атакуйте (в Алфее не работает). Взлетать слишком высоко не рекомендуется, так как ненароком можно застрять в небесной тверди. Анимация, к сожалению, дёрганная.";
            }

            int i = 0;
            foreach (var resolution in DisplayModesHelper.GetSupportedResolutions())
            {
                if ($"{resolution}".Length < 0xC)
                {
                    inputResolution.Items.Add(resolution);
                    if (Screen.PrimaryScreen.Bounds.Size == resolution.Size)
                        inputResolution.SelectedIndex = i;
                    i++;
                }
            }

            checkBoxResolution.Checked = tweaks[0];
            if (resolutionIndex >= -1 && resolutionIndex < inputResolution.Items.Count)
                inputResolution.SelectedIndex = resolutionIndex;
            checkBoxFirstPersonMovement.Checked = tweaks[1];
            checkBoxFirstPersonAnywhere.Checked = tweaks[2];
            checkBoxLookingUp.Checked = tweaks[3];
            checkBoxSkipLogos.Checked = tweaks[4];
            checkBoxStarChallenges.Checked = tweaks[5];
            checkBoxFlight.Checked = tweaks[6];
        }

        private void MainForm_Shown(object sender, EventArgs e)
        {
            if (Directory.Exists(folderToApply))
                Apply(folderToApply);
        }

        private void ButtonApply_Click(object sender, EventArgs e)
        {
            string gameFolder;
            if (Environment.OSVersion.Version.Major >= 6)
            {
                var dialog = new FolderPicker();
                if (dialog.ShowDialog() != DialogResult.OK)
                    return;
                gameFolder = dialog.SelectedPath;
            }
            else
            {
                var dialog = new FolderBrowserDialog { ShowNewFolderButton = false };
                if (dialog.ShowDialog() != DialogResult.OK)
                    return;
                gameFolder = dialog.SelectedPath;
            }

            try
            {
                using (var stream = new FileStream($@"{gameFolder}\WinxClub.exe", FileMode.Open)) { }
            }
            catch (UnauthorizedAccessException)
            {
                var processStartInfo = new ProcessStartInfo(System.Reflection.Assembly.GetExecutingAssembly().GetName().Name)
                {
                    Verb = "runas",
                    Arguments = $"\"{gameFolder}\" " +
                    $"{Convert.ToByte(checkBoxResolution.Checked)}" +
                    $"{Convert.ToByte(checkBoxFirstPersonMovement.Checked)}" +
                    $"{Convert.ToByte(checkBoxFirstPersonAnywhere.Checked)}" +
                    $"{Convert.ToByte(checkBoxLookingUp.Checked)}" +
                    $"{Convert.ToByte(checkBoxSkipLogos.Checked)}" +
                    $"{Convert.ToByte(checkBoxStarChallenges.Checked)}" +
                    $"{Convert.ToByte(checkBoxFlight.Checked)}" +
                    $" {inputResolution.SelectedIndex}"
                };
                try
                {
                    Process.Start(processStartInfo);
                    Application.Exit();
                }
                catch (Win32Exception ex)
                {
                    if (ex.ErrorCode != unchecked((int)0x80004005))
                        throw ex;
                }
                return;
            }

            Apply(gameFolder);
        }

        private void ButtonHelp_Click(object sender, EventArgs e)
        {
            MessageBox.Show(
$@"Winx Club Tweak Center 2.0
2022, kindergal2000

{helpText}");
        }

        private void CheckBoxResolution_CheckedChanged(object sender, EventArgs e)
        {
            inputResolution.Enabled = checkBoxResolution.Checked;
        }
    }
}
