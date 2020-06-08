using System;
using System.Collections.Generic;
using System.IO;
using System.Text.RegularExpressions;
using System.Windows.Forms;

namespace VAR.Toolbox.UI.Tools.WorkLog
{
    public class VARTextWorkLogImporter : IWorkLogImporter
    {
        public string Name { get { return "VARText"; } }

        public bool Export(List<WorkLogItem> items, Form form)
        {
            throw new System.NotImplementedException();
        }

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
                    if (strDateParts.Length >= 3 && strDateParts[0].Length == 4 && strDateParts[1].Length == 2 && strDateParts[2].Length == 2)
                    {
                        currentDateStart = new DateTime(Convert.ToInt32(strDateParts[0]), Convert.ToInt32(strDateParts[1]), Convert.ToInt32(strDateParts[2]), 0, 0, 0);
                    }
                    else
                    {
                        continue;
                    }
                }
                else if (lineAux.StartsWith("--"))
                {
                    continue;
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
                        dateTime.AddMonths(1);
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
