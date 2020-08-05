using DevExpress.Mvvm.POCO;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using TS2MP4_Video_Converter.Models;

namespace TS2MP4_Video_Converter.ViewModels
{
    public class MainWindowViewModel
    {
        public static MainWindowViewModel Create()
            => ViewModelSource.Create(() => new MainWindowViewModel());

        public virtual string Log { get; set; } = "";
        public virtual bool IsDefault { get; set; } = true;
        public virtual bool ConvertEnabled { get; set; }
        public virtual bool IsDelete { get; set; }
        public virtual string CustomArgs { get; set; } = "-i $InputFile -y -c:v libx264 -c:a copy $OutputFile";
        public virtual string OutputDirectory { get; set; } = "$Videos\\TS2MP4";
        public virtual string FFmpegHelp { get; set; } = Properties.Resources.FFmpegHelp;
        public virtual TsFilesCollectionItem SelectedTsFile { get; set; }
        public ObservableCollection<TsFilesCollectionItem> TsFilesCollection { get; } = new ObservableCollection<TsFilesCollectionItem>();

        Process currentProcess = null;


        public void Browse()
        {
            var dialog = new FolderBrowserDialog();
            if (dialog.ShowDialog() == DialogResult.OK)
            {
                OutputDirectory = dialog.SelectedPath;
            }
        }

        public void Add()
        {
            var dialog = new OpenFileDialog();
            dialog.Multiselect = true;
            dialog.Filter = "ts files (*.ts)|*.ts";
            if (dialog.ShowDialog() == DialogResult.OK)
            {
                ConvertEnabled = true;
                // ToDo: Continue
                var files = dialog.FileNames;
                foreach (var item in files)
                {
                    var file = TsFilesCollectionItem.Create(Delete);
                    file.FileName = item;
                    file.Size = new FileInfo(item).Length.SizeFormat();
                    file.Status = ConvertStatus.Waiting;
                    TsFilesCollection.Add(file);
                }

            }
        }

        public void Delete(TsFilesCollectionItem item)
        {
            TsFilesCollection.Remove(item);
        }

        public async void Convert()
        {
            ConvertEnabled = false;
            await Task.Run(() =>
            {
                Log += "===================================\n";
                Log += "Starting new Process\n";
                Log += "===================================\n";


                var currentDirectory = Path.GetDirectoryName(typeof(MainWindowViewModel).Assembly.Location);
                var resourcesDirectory = $"{currentDirectory}\\Resources";
                var medialnfoFileName = $"{resourcesDirectory}\\Medialnfo.exe";
                var ffmpegFileName = $"{resourcesDirectory}\\ffmpeg.exe";
                string videosPath = Environment.GetFolderPath(Environment.SpecialFolder.MyVideos);

                var item = TsFilesCollection.FirstOrDefault(x => !x.Processed);
                while (item != null)
                {
                    item.Status = ConvertStatus.InProgress;
                    var medialnfoArguments = $"\"{item.FileName}\"";
                    var infoProcess = StartProcess(medialnfoFileName, medialnfoArguments);
                    JObject mediaInfo = JObject.Parse(JsonConvert.SerializeObject(new MediaInfo()));
                    var s = infoProcess.ExitCode.ToString(); //-532459699
                    if (infoProcess.ExitCode == 0)
                    {
                        var res = infoProcess.StandardOutput.ReadToEnd();
                        var err = infoProcess.StandardError.ReadToEnd();
                        mediaInfo = JObject.Parse(res);
                    }
                    Log += $"converting file {item.FileName}\n";
                    try
                    {
                        var outputPath = $"{OutputDirectory}\\{Path.ChangeExtension(Path.GetFileName(item.FileName), "mp4")}".Replace("$Videos", videosPath);
                        if (!Directory.Exists(Path.GetDirectoryName(outputPath)))
                            Directory.CreateDirectory(Path.GetDirectoryName(outputPath));

                        string ffmpegArguments;
                        if (IsDefault)
                            ffmpegArguments = $"-i \"{item.FileName}\" -y -c:v libx264 -c:a copy  \"{outputPath}\"";
                        else
                        {
                            ffmpegArguments = CustomArgs.Replace("$InputFile", $"\"{item.FileName}\"");
                            ffmpegArguments = ffmpegArguments.Replace("$VideoBitrate", (mediaInfo["VideoInfo"]["Bitrate"].Value<int>() / 1000).ToString("0k"));
                            ffmpegArguments = ffmpegArguments.Replace("$AudioSamplingRateHz", "44100");
                            ffmpegArguments = ffmpegArguments.Replace("$OutputFile", outputPath);
                            ffmpegArguments = ffmpegArguments.Replace("$Videos", videosPath);
                        }

                        currentProcess = StartProcess(ffmpegFileName, ffmpegArguments, false, false, false);
                        currentProcess.WaitForExit();
                        if (IsDelete)
                            System.IO.File.Delete(item.FileName);
                        item.Status = ConvertStatus.Success;
                        Log += $"converting file {item.FileName} DONE!\n";
                    }
                    catch (Exception ex)
                    {
                        Log += $"converting file {item.FileName} ERROR!\n";
                        Log += $"Exception\n{ex.ToString()}\n";
                        item.Status = ConvertStatus.Failed;
                    }
                    item.Processed = true;
                    item = TsFilesCollection.FirstOrDefault(x => !x.Processed);
                }
                Log += "===================================\n";
                Log += "Process Completed\n\n\n";
                ConvertEnabled = true;
            });
        }

        public void Exit()
        {
            try
            {
                if (currentProcess != null)
                    currentProcess.Kill();
            }
            catch { }
            Environment.Exit(0);
        }

        Process StartProcess(string fileName, string args, bool wait = true, bool redirectStandardOutput = true, bool redirectStandardError = true)
        {
            var process = new Process()
            {
                StartInfo = new ProcessStartInfo
                {
                    FileName = fileName,
                    Arguments = args,
                    RedirectStandardError = redirectStandardError,
                    RedirectStandardOutput = redirectStandardOutput,
                    UseShellExecute = false,
                    CreateNoWindow = true,
                }
            };
            process.Start();
            if (wait)
                process.WaitForExit();
            return process;
        }
    }
}
