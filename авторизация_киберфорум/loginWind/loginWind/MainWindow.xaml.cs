using agsXMPP;
using agsXMPP.Xml.Dom;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;


namespace loginWind
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private login log = new login();
        public pages.loader loader;
        public Jid iam;
        public XmppClientConnection XmppCon;

        public MainWindow()
        {
            InitializeComponent();
            XmppCon = new XmppClientConnection();
            main.Navigate(log);
            log.logBT.Click += new RoutedEventHandler(enter_Click);
        }
        private void XmppCon_OnXmppConnectionStateChanged(object sender, XmppConnectionState state)
        {
            Dispatcher.BeginInvoke(new Action(() =>
            {
                loader.stat.Text = state.ToString();
               

            }));
        }
        private void enter_Click(object sender, RoutedEventArgs e)
        {
            
            Dispatcher.BeginInvoke(new Action(() =>
            {

                iam = new Jid(log.jid.Text.Trim());
                XmppCon.Server = iam.Server;
                XmppCon.Username = iam.User;
                XmppCon.Password = log.pass.Password;
                XmppCon.Priority = 10;
                XmppCon.Port = 5222;
                XmppCon.AutoResolveConnectServer = true;
                XmppCon.UseCompression = false;
                XmppCon.Open();
                XmppCon.OnSocketError += new ErrorHandler(XmppCon_OnSocketError);
                XmppCon.OnError += new ErrorHandler(XmppCon_OnError);
                 XmppCon.OnAuthError += new XmppElementHandler(XmppCon_OnAuthError);
                 XmppCon.OnRosterStart += new ObjectHandler(XmppCon_OnRosterStart);
                XmppCon.OnXmppConnectionStateChanged += new XmppConnectionStateHandler(XmppCon_OnXmppConnectionStateChanged);
                loader = new pages.loader();
                main.Navigate(loader);
            }));


        }
        private void XmppCon_OnSocketError(object sender, Exception ex)
        {
            MessageBox.Show(ex.Message);
            main.GoBack();
        }
        private void XmppCon_OnAuthError(object sender, Element e)
        {
            Dispatcher.BeginInvoke(new Action(() =>
            {
                MessageBox.Show("Ошибка авторизации\r\nID пользователя или пароль введен не верно. Сервер " + iam.Server + " пользователь " + iam.Bare,
                    "Ошибка авторизации", MessageBoxButton.OK, MessageBoxImage.Error);
                main.GoBack();

            }));
        }


        private void XmppCon_OnError(object sender, Exception ex)
        {
            Dispatcher.BeginInvoke(new Action(() =>
            {


                MessageBox.Show("Не найдено не одного интернет соединения.\r\nПроверте настройки подключения и повторите попытку снова.");
                main.GoBack();

            }));
        }
        private void XmppCon_OnRosterStart(object sender)
        {
            Dispatcher.BeginInvoke(new Action(() =>
            {
                pages.mainf mainf = new pages.mainf();
                main.Navigate(mainf);
                

            }));
        }
    }
}
