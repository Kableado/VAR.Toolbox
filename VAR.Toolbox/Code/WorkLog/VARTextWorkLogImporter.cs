using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Windows.Forms;

namespace VAR.Toolbox.Code.WorkLog
{
    public class VARTextWorkLogImporter : IWorkLogImporter
    {
        public string Name => "VARText";

        //TODO: VARTextWorkLogImporter: Export WorkLigItem.Tags
        public bool Export(List<WorkLogItem> items, Form form)
        {
            SaveFileDialog saveFileDialog = new SaveFileDialog();
            DialogResult dialogResult = saveFileDialog.ShowDialog(form);
            if (dialogResult != DialogResult.OK) { return false; }

            if (File.Exists(saveFileDialog.FileName) == false)
            {
                File.Delete(saveFileDialog.FileName);
            }

            using (StreamWriter streamWriter = new StreamWriter(saveFileDialog.FileName))
            {
                DateTime lastDateTime = DateTime.MinValue;
                int lastWeekOfYear = -1;
                List<WorkLogItem> itemsOrdered = items.OrderBy(x => x.DateStart).ToList();
                foreach (WorkLogItem item in itemsOrdered)
                {
                    int weekOfYear = CultureInfo.InvariantCulture.Calendar.GetWeekOfYear(item.DateStart,
                        CalendarWeekRule.FirstFourDayWeek, DayOfWeek.Monday);
                    if (weekOfYear != lastWeekOfYear)
                    {
                        streamWriter.WriteLine();
                        streamWriter.WriteLine();
                        streamWriter.WriteLine("--- {0:0000}-{1:00}-{2:00}", item.DateStart.Year, item.DateStart.Month,
                            item.DateStart.Day);
                        lastWeekOfYear = weekOfYear;
                    }

                    if (lastDateTime.Day != item.DateStart.Day)
                    {
                        streamWriter.WriteLine();
                        lastDateTime = item.DateStart;
                    }

                    streamWriter.WriteLine("{0:00}_{1:00}_{2:00} - {3:00}_{4:00}_{5:00} {6}", item.DateStart.Day,
                        item.DateStart.Hour, item.DateStart.Minute, item.DateEnd.Day, item.DateEnd.Hour,
                        item.DateEnd.Minute, item.Activity);
                    if (string.IsNullOrEmpty(item.Description) == false)
                    {
                        streamWriter.WriteLine($"                    {item.Description}");
                    }
                }
            }

            return true;
        }

        //TODO: VARTextWorkLogImporter: Import WorkLigItem.Tags
        public List<WorkLogItem> Import(Form form)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            DialogResult dialogResult = openFileDialog.ShowDialog(form);
            if (dialogResult != DialogResult.OK) { return null; }

            if (File.Exists(openFileDialog.FileName) == false) { return null; }

            string[] allLines = File.ReadAllLines(openFileDialog.FileName);
            Regex regex = new Regex(@"(\d\d)_(\d\d)_(\d\d)\s-\s(\d\d)_(\d\d)_(\d\d)\s(.*)");

            DateTime currentDateStart = DateTime.MinValue;
            WorkLogItem currentWorkLog = null;
            List<WorkLogItem> workLog = new List<WorkLogItem>();

            foreach (string line in allLines)
            {
                string lineAux = line.Trim();
                if (string.IsNullOrEmpty(lineAux)) { continue; }

                if (lineAux.StartsWith("--- "))
                {
                    string strDate = lineAux.Substring(4);
                    string[] strDateParts = strDate.Split('-');
                    if (strDateParts.Length >= 3 && strDateParts[0].Length == 4 && strDateParts[1].Length == 2 &&
                        strDateParts[2].Length == 2)
                    {
                        currentDateStart = new DateTime(Convert.ToInt32(strDateParts[0]),
                            Convert.ToInt32(strDateParts[1]), Convert.ToInt32(strDateParts[2]), 0, 0, 0);
                    }
                    else { }
                }
                else if (lineAux.StartsWith("--"))
                {
                    if (currentWorkLog != null)
                    {
                        currentWorkLog.Description += lineAux + "\n";
                    }
                }
                else
                {
                    Match match = regex.Match(lineAux);
                    if (match.Groups.Count < 7)
                    {
                        if (currentWorkLog != null)
                        {
                            currentWorkLog.Description += lineAux + "\n";
                        }

                        continue;
                    }

                    int startDay = Convert.ToInt32(match.Groups[1].Value);
                    int startHour = Convert.ToInt32(match.Groups[2].Value);
                    int startMinute = Convert.ToInt32(match.Groups[3].Value);
                    int endDay = Convert.ToInt32(match.Groups[4].Value);
                    int endHour = Convert.ToInt32(match.Groups[5].Value);
                    int endMinute = Convert.ToInt32(match.Groups[6].Value);
                    string activity = match.Groups[7].Value;

                    DateTime dateTime = currentDateStart;
                    if (dateTime.Day > startDay)
                    {
                        dateTime = dateTime.AddMonths(1);
                    }

                    currentWorkLog = new WorkLogItem
                    {
                        DateStart = new DateTime(dateTime.Year, dateTime.Month, startDay, startHour, startMinute, 0),
                        DateEnd = new DateTime(dateTime.Year, dateTime.Month, endDay, endHour, endMinute, 0),
                        Activity = activity,
                        Description = string.Empty,
                    };
                    workLog.Add(currentWorkLog);
                }
            }

            return workLog;
        }
    }
}