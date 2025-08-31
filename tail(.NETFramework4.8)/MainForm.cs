using System.Diagnostics;
using System.Windows.Forms;

namespace tail_.NETFramework4._8_
{
    public partial class MainForm : Form
    {
        private const string POWERSHELL = "powershell.exe";

        public MainForm()
        {
            InitializeComponent();
        }

        private void MainForm_DragDrop(object sender, DragEventArgs e)
        {
            string[] files = GetDroppedFiles(e);

            foreach (string file in files)
            {
                OpenPowerShell(file);
            }
        }

        private void MainForm_DragEnter(object sender, DragEventArgs e)
        {
            if (e.Data == null) return;
            if (e.Data.GetDataPresent(DataFormats.FileDrop))
            {
                e.Effect = DragDropEffects.Copy;
            }
            else
            {
                e.Effect = DragDropEffects.None;
            }
        }

        /// <summary>
        /// DragDropよりDATAの配列を返します
        /// </summary>
        /// <param name="e"></param>
        /// <returns></returns>
        private static string[] GetDroppedFiles(DragEventArgs e)
        {
            if (e.Data == null) return null;
            return e.Data.GetData(DataFormats.FileDrop) as string[] ?? null;
        }

        /// <summary>
        /// shell
        /// </summary>
        /// <param name="filePath"></param>
        private static void OpenPowerShell(string filePath)
        {
            if (string.IsNullOrEmpty(filePath)) return;

            string title = "ログ監視中" + filePath;
            string command = $"$host.UI.RawUI.WindowTitle = '{title}'; Get-Content -Path '{filePath}' -Wait -Tail 0 -Encoding Default";

            Process p = new Process();
            p.StartInfo.FileName = POWERSHELL;
            p.StartInfo.Arguments = $"-NoExit -Command \"{command}\"";
            p.StartInfo.UseShellExecute = true;
            p.Start();
        }
    }
}
