using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Layout;
using VAR.Toolbox.Code;

namespace VAR.Toolbox.UI.Tools
{
    public class FrmTestWebService : Window, IToolForm
    {
        public string ToolName => "TestWebService";
        public bool HasIcon => false;

        private readonly TextBox _txtUrlSoap, _txtNamespaceUrlSoap, _txtMethodSoap, _txtParametersSoap, _txtResultSoap;
        private readonly TextBox _txtUrlRest, _txtUrlApiMethodRest, _txtParametersRest, _txtBodyRest, _txtResultRest;

        public FrmTestWebService()
        {
            Title = "TestWebService";
            Width = 682;
            Height = 521;

            // SOAP tab
            _txtUrlSoap = new TextBox { Classes = { "mono", }, };
            _txtNamespaceUrlSoap = new TextBox { Classes = { "mono", }, };
            _txtMethodSoap = new TextBox { Classes = { "mono", }, };
            _txtParametersSoap = new TextBox { Classes = { "mono", }, AcceptsReturn = true, Height = 100, };
            _txtResultSoap = new TextBox { Classes = { "mono", }, AcceptsReturn = true, IsReadOnly = true, };

            Button btnTestSoap = new() { Content = "Test", };
            btnTestSoap.Click += BtnTestSoap_Click;

            StackPanel soapPanel = new() { Spacing = 4, Margin = new Thickness(6), };
            soapPanel.Children.Add(MakeRow("URL", _txtUrlSoap));
            soapPanel.Children.Add(MakeRow("NamespaceUrl", _txtNamespaceUrlSoap));
            soapPanel.Children.Add(MakeRow("Method", _txtMethodSoap));
            soapPanel.Children.Add(MakeRow("Parameters", _txtParametersSoap));
            soapPanel.Children.Add(btnTestSoap);
            soapPanel.Children.Add(new TextBlock { Text = "Result", });
            soapPanel.Children.Add(_txtResultSoap);

            TabItem soapTab = new() { Header = "SoapService", Content = new ScrollViewer { Content = soapPanel, }, };

            // REST tab
            _txtUrlRest = new TextBox { Classes = { "mono", }, };
            _txtUrlApiMethodRest = new TextBox { Classes = { "mono", }, };
            _txtParametersRest = new TextBox { Classes = { "mono", }, AcceptsReturn = true, Height = 50, };
            _txtBodyRest = new TextBox { Classes = { "mono", }, AcceptsReturn = true, Height = 60, };
            _txtResultRest = new TextBox { Classes = { "mono", }, AcceptsReturn = true, IsReadOnly = true, };

            Button btnTestRest = new() { Content = "Test", };
            btnTestRest.Click += BtnTestRest_Click;

            StackPanel restPanel = new() { Spacing = 4, Margin = new Thickness(6), };
            restPanel.Children.Add(MakeRow("URL", _txtUrlRest));
            restPanel.Children.Add(MakeRow("UrlApiMethod", _txtUrlApiMethodRest));
            restPanel.Children.Add(MakeRow("Parameters", _txtParametersRest));
            restPanel.Children.Add(MakeRow("Body", _txtBodyRest));
            restPanel.Children.Add(btnTestRest);
            restPanel.Children.Add(new TextBlock { Text = "Result", });
            restPanel.Children.Add(_txtResultRest);

            TabItem restTab = new() { Header = "RestService", Content = new ScrollViewer { Content = restPanel, }, };

            TabControl tabControl = new();
            tabControl.Items.Add(soapTab);
            tabControl.Items.Add(restTab);

            Content = tabControl;
        }

        private static Control MakeRow(string label, Control control)
        {
            DockPanel row = new();
            TextBlock lbl = new() { Text = label, Width = 100, VerticalAlignment = VerticalAlignment.Center, };
            DockPanel.SetDock(lbl, Dock.Left);
            row.Children.Add(lbl);
            row.Children.Add(control);
            return row;
        }

        private void BtnTestSoap_Click(object? sender, Avalonia.Interactivity.RoutedEventArgs e)
        {
            try
            {
                string url = _txtUrlSoap.Text ?? string.Empty;
                string namespaceUrl = _txtNamespaceUrlSoap.Text ?? string.Empty;
                string method = _txtMethodSoap.Text ?? string.Empty;
                Dictionary<string, object> parameters = StringToDictionary(_txtParametersSoap.Text ?? string.Empty)
                    .ToDictionary(p => p.Key, p => (object)p.Value);

                string result = WebServicesUtils.CallSoapMethod(url, method, parameters, namespaceUrl);
                _txtResultSoap.Text = result;
            }
            catch (Exception ex)
            {
                StringBuilder sbException = new();
                Exception? exAux = ex;
                while (exAux != null)
                {
                    sbException.Append($"{exAux.Message}\r\n{exAux.StackTrace ?? string.Empty}\r\n\r\n");
                    exAux = exAux.InnerException;
                }
                _txtResultSoap.Text = sbException.ToString();
            }
        }

        private void BtnTestRest_Click(object? sender, Avalonia.Interactivity.RoutedEventArgs e)
        {
            try
            {
                string url = _txtUrlRest.Text ?? string.Empty;
                string urlApiMethod = _txtUrlApiMethodRest.Text ?? string.Empty;
                Dictionary<string, string> parameters = StringToDictionary(_txtParametersRest.Text ?? string.Empty);
                string body = _txtBodyRest.Text ?? string.Empty;

                string result = WebServicesUtils.CallApi(url, urlApiMethod, parameters, null, stringContent: body);
                _txtResultRest.Text = result;
            }
            catch (Exception ex)
            {
                StringBuilder sbException = new();
                Exception? exAux = ex;
                while (exAux != null)
                {
                    sbException.Append($"{exAux.Message}\r\n{exAux.StackTrace ?? string.Empty}\r\n\r\n");
                    exAux = exAux.InnerException;
                }
                _txtResultRest.Text = sbException.ToString();
            }
        }

        private static Dictionary<string, string> StringToDictionary(string str)
        {
            Dictionary<string, string> dic = new();
            List<string> pairs = SplitUnescaped(str, ',');
            foreach (string pair in pairs)
            {
                List<string> values = SplitUnescaped(pair, ':');
                if (values.Count < 2) continue;
                string key = values[0].Replace("\\:", ":").Replace("\\,", ",");
                string val = values[1].Replace("\\:", ":").Replace("\\,", ",");
                dic.Add(key, val);
            }
            return dic;
        }

        private static List<string> SplitUnescaped(string str, char splitter)
        {
            List<string> strings = new();
            int j, i;
            int n = str.Length;
            for (j = 0, i = 0; i < n; i++)
            {
                if (str[i] == '\\') i++;
                else if (str[i] == splitter)
                {
                    strings.Add(str.Substring(j, i - j));
                    j = i + 1;
                }
            }
            if (i >= j) strings.Add(str.Substring(j, n - j));

            return strings;
        }
    }
}