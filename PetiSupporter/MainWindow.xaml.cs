using System;
using System.Text;
using System.Windows;
using System.IO;

namespace PetiSupporter {
    /// <summary>
    /// MainWindow.xaml の相互作用ロジック
    /// </summary>
    public partial class MainWindow : Window {
        public MainWindow() {
            InitializeComponent();
        }

        private void Window_DragEnter(object sender, DragEventArgs e) {
            e.Effects = e.Data.GetDataPresent(DataFormats.FileDrop) ? DragDropEffects.Copy : DragDropEffects.None;
        }

        private void Window_Drop(object sender, DragEventArgs e) {
            foreach (var fileName in (string[])e.Data.GetData(DataFormats.FileDrop)) {
                if (Path.GetExtension(fileName) == ".txt") convert2lrc(fileName);
            }
            System.Media.SystemSounds.Asterisk.Play();
        }

        private void convert2lrc(string fileName) {
            string temp = fileName.Replace(".txt", " ###temp.txt");
            StringBuilder sb = new StringBuilder();
            foreach (var line in File.ReadLines(fileName, Encoding.UTF8)) {
                try { sb.AppendLine(line.Remove(12, 1).Remove(0, 4).Insert(8, "]").Insert(0, "[")); }
                catch { }
            }
            string content = sb.ToString();
            //最終行の改行を削除
            content = content.Remove(content.LastIndexOf(Environment.NewLine));
            File.WriteAllText(temp, content, Encoding.UTF8);
            File.Move(temp, Path.ChangeExtension(fileName, ".lrc"));
        }
    }
}
