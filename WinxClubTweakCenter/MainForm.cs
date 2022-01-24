using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Security.Principal;
using System.Text;
using System.Windows.Forms;

namespace WinxClubTweakCenter
{
    using static DisplayModesHelper;

    public partial class MainForm : Form
    {
        private string helpText;
        private string errorCaption = "Error";
        private string warningCaption = "Warning";
        private string infoCaption = "Info";
        private string folderErrorMessage = "Selected folder is not valid Winx Club The Game folder";
        private string dataInconsistencyWarningMessage = "Data inconsistency detected. If you proceed, data in chosen folder may be corrupted";
        private string resolutionWarningMessage = "Unsupported resolution detected. Resolution will be set to default";
        private string versionInfoMessage = "Old tweaks detected. They will be updated";
        private string doneMessage = "Done!";

        private Resolution vanillaResolution = new Resolution(new Size(1024, 768));

        private FileInfo executableInfo;
        private FileInfo winxIniInfo;
        private FileInfo playerSptInfo;
        private FileInfo star01SptInfo;
        private FileInfo star02SptInfo;
        private FileInfo star03SptInfo;

        private readonly string mode;
        private string gameFolder;
        private bool[] tweaks, currentFolderTweaks;
        private int resolutionIndex = -1, currentFolderResolutionIndex = -1;
        private readonly Size passedResolutionSize;

        public MainForm(string mode, string gameFolder, bool[] tweaks, Size resolutionSize)
        {
            this.mode = mode;
            UpdateGameFolder(gameFolder);
            this.tweaks = tweaks;
            passedResolutionSize = resolutionSize;
            InitializeComponent();
        }

        private void UpdateGameFolder(string path)
        {
            if (Directory.Exists(path))
            {
                gameFolder = path;
                executableInfo = new FileInfo($@"{gameFolder}\WinxClub.exe");
                winxIniInfo = new FileInfo($@"{gameFolder}\winx.ini");
                playerSptInfo = new FileInfo($@"{gameFolder}\Media\Characters\Bloom\Player.spt");
                var challengesPath = $@"{gameFolder}\Media\Levels\Challenges";
                star01SptInfo = new FileInfo($@"{challengesPath}\star_01.spt");
                star02SptInfo = new FileInfo($@"{challengesPath}\star_02.spt");
                star03SptInfo = new FileInfo($@"{challengesPath}\star_03.spt");
            }
        }

        private void UpdateInputResolution()
        {
            if (inputResolution.Items.Contains(vanillaResolution))
                inputResolution.Items.Remove(vanillaResolution);
            if (checkBoxResolution.Checked)
            {
                inputResolution.Enabled = true;
                inputResolution.SelectedIndex = resolutionIndex;
            }
            else
            {
                inputResolution.Enabled = false;
                resolutionIndex = inputResolution.SelectedIndex;
                inputResolution.SelectedIndex = inputResolution.Items.Add(vanillaResolution);
            }
        }

        private bool EnsureAuthorizedAccess(FileAccess access, string modeForRestart)
        {
            try
            {
                executableInfo.Open(FileMode.Open, access).Dispose();
            }
            catch (UnauthorizedAccessException uaex)
            {
                if (WindowsIdentity.GetCurrent().Owner.IsWellKnown(WellKnownSidType.BuiltinAdministratorsSid))
                    throw uaex;

                var resolutionString = "-";
                if (inputResolution.SelectedIndex > -1)
                {
                    var resolution = (Resolution)inputResolution.SelectedItem;
                    resolutionString = $"{resolution.Size.Width}x{resolution.Size.Height}";
                }
                var processStartInfo = new ProcessStartInfo(System.Reflection.Assembly.GetExecutingAssembly().GetName().Name)
                {
                    Verb = "runas",
                    Arguments = $"{modeForRestart} " +
                    $"\"{gameFolder}\" " +
                    $"{Convert.ToByte(checkBoxResolution.Checked)}" +
                    $"{Convert.ToByte(checkBoxFirstPersonMovement.Checked)}" +
                    $"{Convert.ToByte(checkBoxFirstPersonAnywhere.Checked)}" +
                    $"{Convert.ToByte(checkBoxLookingUp.Checked)}" +
                    $"{Convert.ToByte(checkBoxSkipLogos.Checked)}" +
                    $"{Convert.ToByte(checkBoxStarChallenges.Checked)}" +
                    $"{Convert.ToByte(checkBoxFlight.Checked)}" +
                    $" {resolutionString}"
                };
                try
                {
                    Process.Start(processStartInfo);
                    Application.Exit();
                }
                catch (Win32Exception w32ex)
                {
                    if (w32ex.ErrorCode != unchecked((int)0x80004005))
                        throw w32ex;
                }
                return false;
            }
            return true;
        }

        private bool ExamineGameFolder()
        {
            if (!executableInfo.Exists || executableInfo.Length < 22065152 ||
                !playerSptInfo.Exists || playerSptInfo.Length < 9773 ||
                !star01SptInfo.Exists || star01SptInfo.Length < 18352 ||
                !star02SptInfo.Exists || star02SptInfo.Length < 12513 ||
                !star03SptInfo.Exists || star03SptInfo.Length < 39907)
            {
                MessageBox.Show(folderErrorMessage, errorCaption, MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }

            if (!EnsureAuthorizedAccess(FileAccess.Read, "examine"))
                return false;

            currentFolderTweaks = new bool[tweaks.Length];
            Patch.ExamineCurrentDataResult examineResult;

            using (var reader = new BinaryReader(executableInfo.OpenRead()))
            {
                if (!Patches.TryGetCurrentResolution(reader, out var resolution))
                    goto inconsistencyDetected;
                if (resolution.Size != vanillaResolution.Size)
                {
                    if (!inputResolution.Items.Contains(resolution))
                    {
                        MessageBox.Show(resolutionWarningMessage, warningCaption, MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    }
                    else
                    {
                        currentFolderTweaks[0] = true;
                        currentFolderResolutionIndex = inputResolution.Items.IndexOf(resolution);
                    }
                }

                var version = LegacyHelper.GetAllowFirstPersonMovementVersion(reader);
                if (version == LegacyHelper.Version.Unknown)
                    goto inconsistencyDetected;
                else if (version == LegacyHelper.Version.V1_0)
                    MessageBox.Show(versionInfoMessage, infoCaption, MessageBoxButtons.OK, MessageBoxIcon.Information);
                currentFolderTweaks[1] = version != LegacyHelper.Version.Vanilla;

                examineResult = Patches.AllowFirstPersonAnywhere.ExamineCurrentData(reader);
                if (examineResult == Patch.ExamineCurrentDataResult.Unknown)
                    goto inconsistencyDetected;
                currentFolderTweaks[2] = examineResult == Patch.ExamineCurrentDataResult.Tweak;

                examineResult = Patches.EnableFlight.ExamineCurrentData(reader);
                if (examineResult == Patch.ExamineCurrentDataResult.Unknown)
                    goto inconsistencyDetected;
                currentFolderTweaks[currentFolderTweaks.Length - 1] = examineResult == Patch.ExamineCurrentDataResult.Tweak;
            }

            using (var reader = new BinaryReader(playerSptInfo.OpenRead()))
            {
                examineResult = Patches.AllowLookingUp.ExamineCurrentData(reader);
                if (examineResult == Patch.ExamineCurrentDataResult.Unknown)
                    goto inconsistencyDetected;
                currentFolderTweaks[3] = examineResult == Patch.ExamineCurrentDataResult.Tweak;
            }

            currentFolderTweaks[4] = Patches.ExamineSkipLogos(winxIniInfo) == Patch.ExamineCurrentDataResult.Tweak;

            var tweakedStarChallengesCount = 0;
            using (var reader = new BinaryReader(star01SptInfo.OpenRead()))
            {
                examineResult = Patches.AddTimeInStar01Challenge.ExamineCurrentData(reader);
                if (examineResult == Patch.ExamineCurrentDataResult.Unknown)
                    goto inconsistencyDetected;
                if (examineResult == Patch.ExamineCurrentDataResult.Tweak)
                    tweakedStarChallengesCount++;
            }
            using (var reader = new BinaryReader(star02SptInfo.OpenRead()))
            {
                examineResult = Patches.AddTimeInStar02Challenge.ExamineCurrentData(reader);
                if (examineResult == Patch.ExamineCurrentDataResult.Unknown)
                    goto inconsistencyDetected;
                if (examineResult == Patch.ExamineCurrentDataResult.Tweak)
                    tweakedStarChallengesCount++;
            }
            using (var reader = new BinaryReader(star03SptInfo.OpenRead()))
            {
                examineResult = Patches.AddTimeInStar03Challenge.ExamineCurrentData(reader);
                if (examineResult == Patch.ExamineCurrentDataResult.Unknown)
                    goto inconsistencyDetected;
                if (examineResult == Patch.ExamineCurrentDataResult.Tweak)
                    tweakedStarChallengesCount++;
            }
            if (tweakedStarChallengesCount == 0)
                currentFolderTweaks[5] = false;
            else if (tweakedStarChallengesCount == 3)
                currentFolderTweaks[5] = true;
            else
                goto inconsistencyDetected;

            decorGameFolder.Text = gameFolder;
            containerTweaks.Enabled = buttonApply.Enabled = true;
            goto doneReading;
        inconsistencyDetected:
            MessageBox.Show(dataInconsistencyWarningMessage, warningCaption, MessageBoxButtons.OK, MessageBoxIcon.Warning);
        doneReading:
            return true;
        }

        private void LoadSettings()
        {
            checkBoxResolution.Checked = tweaks[0];
            if (resolutionIndex >= -1 && resolutionIndex < inputResolution.Items.Count)
                inputResolution.SelectedIndex = resolutionIndex;
            UpdateInputResolution();
            checkBoxFirstPersonMovement.Checked = tweaks[1];
            checkBoxFirstPersonAnywhere.Checked = tweaks[2];
            checkBoxLookingUp.Checked = tweaks[3];
            checkBoxSkipLogos.Checked = tweaks[4];
            checkBoxStarChallenges.Checked = tweaks[5];
            checkBoxFlight.Checked = tweaks[6];
        }

        private void PickGameFolder()
        {
            var dialog = new FolderPicker();
            if (dialog.ShowDialog() != DialogResult.OK)
                return;
            var previousGameFolder = gameFolder;
            UpdateGameFolder(dialog.SelectedPath);

            if (!ExamineGameFolder())
            {
                UpdateGameFolder(previousGameFolder);
                return;
            }

            tweaks = currentFolderTweaks;
            if (currentFolderResolutionIndex != -1)
                resolutionIndex = currentFolderResolutionIndex;
            LoadSettings();
        }

        private void Apply()
        {
            if (!EnsureAuthorizedAccess(FileAccess.Write, "apply"))
                return;

            using (var writer = new BinaryWriter(executableInfo.OpenWrite()))
            {
                var resolution = vanillaResolution;
                if (checkBoxResolution.Checked)
                    resolution = (Resolution)inputResolution.SelectedItem;
                foreach (var patch in Patches.GetResolutionPatches(resolution))
                    patch.Apply(writer);

                foreach (var patch in Patches.AllowFirstPersonMovement)
                    patch.Apply(writer, !checkBoxFirstPersonMovement.Checked);

                Patches.AllowFirstPersonAnywhere.Apply(writer, !checkBoxFirstPersonAnywhere.Checked);

                Patches.EnableFlight.Apply(writer, !checkBoxFlight.Checked);
            }

            using (var writer = new BinaryWriter(playerSptInfo.OpenWrite()))
            {
                Patches.AllowLookingUp.Apply(writer, !checkBoxLookingUp.Checked);
            }

            Patches.ApplySkipLogos(winxIniInfo, !checkBoxSkipLogos.Checked);

            using (var writer = new BinaryWriter(star01SptInfo.OpenWrite()))
            {
                Patches.AddTimeInStar01Challenge.Apply(writer, !checkBoxStarChallenges.Checked);
            }
            using (var writer = new BinaryWriter(star02SptInfo.OpenWrite()))
            {
                Patches.AddTimeInStar02Challenge.Apply(writer, !checkBoxStarChallenges.Checked);
            }
            using (var writer = new BinaryWriter(star03SptInfo.OpenWrite()))
            {
                Patches.AddTimeInStar03Challenge.Apply(writer, !checkBoxStarChallenges.Checked);
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
                buttonOpen.Text = "Открыть";
                containerTweaks.Text = "Твики";
                checkBoxResolution.Text = "Установить разрешение";
                checkBoxFirstPersonMovement.Text = "Разрешить перемещение с камерой от первого лица";
                checkBoxFirstPersonAnywhere.Text = "Разрешить камеру от первого лица везде";
                checkBoxLookingUp.Text = "Разрешить смотреть вверх";
                checkBoxSkipLogos.Text = "Пропускать логотипы при загрузке";
                checkBoxStarChallenges.Text = "Увеличить время в звёздных испытаниях";
                checkBoxFlight.Text = "Включить способность к полёту";
                buttonApply.Text = "Применить";
                buttonHelp.Text = "Помощь";
                helpText =
$@"Программа предназначена для применения твиков к игре. При нажатии кнопки ""{buttonApply.Text}"" и последующем выборе папки игры будут произведены применение выбранных твиков и откат невыбранных ранее применённых.

""{checkBoxResolution.Text}"": твик, устанавливающий разрешение и в главном меню, и в игре. Не забудьте изменить разрешение в настройках экрана вашего сохранения.

""{checkBoxFirstPersonMovement.Text}"": если вы до сих пор не знали, в игре уже есть режим просмотра от первого лица, активируемый по нажатию средней кнопки мыши. К сожалению, как только персонаж двигается, камера вновь переходит в режим от третьего лица. Данный твик призван исправить это недоразумение, и позволить игроку самому переключать режимы. v2.0: теперь Блум смотрит из глаз, а не из макушки.

""{checkBoxFirstPersonAnywhere.Text}"": в игре часто встречаются места с невращающейся камерой. С этим твиком легко узнать, что таким образом от нас скрыли разработчики, достаточно включить раннее недоступную в таких местах камеру от первого лица.

""{checkBoxLookingUp.Text}"": позволяет смотреть на небо и на потолок. Ну, и бонусом, под юбки. Правда, это вам не NieR: Automata, и смотреть там не на что.

""{checkBoxSkipLogos.Text}"": с этим твиком игра запускается намного быстрее. Да, за логотипами всё это время скрывался арт с девчонками на скутерах.

""{checkBoxStarChallenges.Text}"": какой садомазохист устанавливал таймеры в этих испытниях? Данный твик даёт достаточно времени на сбор всех звёзд.

""{checkBoxFlight.Text}"": Блум, вообще-то, фея, и ей положено летать (хотя в условиях этой игры такой твик будет скорее читом). Чтобы подняться вверх, нажмите (не зажмите!) кнопку прыжка. Находясь в воздухе, Блум будет медленно терять высоту. Чтобы ускорить падение, зажмите кнопку щита или атакуйте (в Алфее не работает). Взлетать слишком высоко не рекомендуется, так как ненароком можно застрять в небесной тверди. Анимация, к сожалению, дёрганная.";
                linkGitHub.Text = "Страница GitHub";
                doneMessage = "Готово!";
                errorCaption = "Ошибка";
                warningCaption = "Внимание";
                infoCaption = "Уведомление";
                folderErrorMessage = "Выбранная папка не является допустимой папкой игры Winx Club";
                dataInconsistencyWarningMessage = "Проверка данных прошла неудачно. При продолжении есть риск повреждения данных в выбранной папке";
                resolutionWarningMessage = "Обнаружено неподдерживаемое разрешение. Будет установлено разрешение по умолчанию";
                versionInfoMessage = "Обнаружены старые твики. Они будут обновлены";
            }

            int i = 0;
            foreach (var resolution in GetSupportedResolutions())
            {
                if ($"{resolution}".Length < Resolution.MaximumStringLength && resolution.Size != vanillaResolution.Size)
                {
                    inputResolution.Items.Add(resolution);
                    if (Screen.PrimaryScreen.Bounds.Size == resolution.Size && resolutionIndex == -1 ||
                        passedResolutionSize == resolution.Size)
                        resolutionIndex = i;
                    i++;
                }
            }
            LoadSettings();
        }

        private void MainForm_Shown(object sender, EventArgs e)
        {
            if (Directory.Exists(gameFolder))
            {
                if (!ExamineGameFolder())
                    return;
                if (mode == "apply")
                    Apply();
            }
        }

        private void ButtonOpen_Click(object sender, EventArgs e)
        {
            PickGameFolder();
        }

        private void ButtonApply_Click(object sender, EventArgs e)
        {
            Apply();
        }

        private void ButtonHelp_Click(object sender, EventArgs e)
        {
            MessageBox.Show(
$@"Winx Club Tweak Center {Application.ProductVersion}
2022, kindergal2000

{helpText}");
        }

        private void LinkGitHub_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            Process.Start("https://github.com/GlobbyBasovich/WinxClubTweakCenter");
        }

        private void CheckBoxResolution_CheckedChanged(object sender, EventArgs e)
        {
            UpdateInputResolution();
        }

        private void InputResolution_SelectedIndexChanged(object sender, EventArgs e)
        {
            buttonApply.Enabled = containerTweaks.Enabled && inputResolution.SelectedIndex != -1;
        }
    }
}
