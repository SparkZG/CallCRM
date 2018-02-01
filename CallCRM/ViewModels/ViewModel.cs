using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;
using System.Collections.ObjectModel;
using DevExpress.Mvvm;
using System.Data;
using System.Threading;
using System.Windows.Threading;
using CallCRM.Common;
using CallCRM.ConnFactory;
using CallCRM.DataFactory;
using System.Collections.Concurrent;

namespace CallCRM.ViewModels
{    
    public class ViewModel : ViewModelBase
    {
        public MySocket _UDPServer = null;

        Action<SocketEvent> UDPAction = null;

        public static Action HangupAction = null;

        public static List<string> CallRecord = new List<string>();


        private string hostName = "12.0.0.1";
        public string HostName
        {
            get { return hostName; }
            set
            {
                SetProperty<string>(ref hostName, value, "HostName");
            }
        }

        private int port = 9555;
        public int Port
        {
            get { return port; }
            set
            {
                SetProperty<int>(ref port, value, "Port");
            }
        }


        private ObservableCollection<string> listStatus = new ObservableCollection<string> { };
        public ObservableCollection<string> ListStatus
        {
            get { return listStatus; }
            set
            {
                SetProperty<ObservableCollection<string>>(ref listStatus, value, "ListStatus");
            }
        }

        public ViewModel()
        {
            if (Properties.Settings.Default.AccessPath == string.Empty)
                BLLCommon.GetAccessPath();
            else
            {
                if(!AccessHelper.OpenConn())
                    BLLCommon.GetAccessPath();                
            }

            HostName = Provider.getIPAddress();
            BLLCommon.GetSystemSet();
            UDPAction = (evt) =>
            {
                switch (evt.eventType)
                {
                    case SocketEventType.StartEvent:
                        AddItemsToStatus(evt.remark);
                        break;
                    case SocketEventType.ReceEvent:
                        if (evt.message == null)
                        {
                            AddItemsToStatus(evt.remark);
                        }
                        else
                        {
                            var frame = evt.message as DataFrame;
                            switch (frame.dataType)
                            {
                                case CS_R103.CallNumber:
                                    TryAddCallTime(frame);
                                    break;
                                case CS_R103.CallerNumberCheck:
                                    TryAddCallTime(frame);
                                    break;
                                case CS_R103.Hang_up:
                                    AddItemsToStatus("已挂机");
                                    if (HangupAction != null)
                                    {
                                        HangupAction();
                                    }
                                    break;
                                case CS_R103.OffHook:
                                    CreteFaultList.CloseWaitWindow();
                                    AddItemsToStatus("已摘机");
                                    break;
                                case CS_R103.RingEnd:
                                    CreteFaultList.CloseWaitWindow();
                                    break;
                                case CS_R103.Ringing:
                                    //将最新的来电号码发送过去
                                    if (CallRecord.Count > 0)
                                    {
                                        CreteFaultList.ShowWaitWindow(CallRecord[CallRecord.Count - 1]);
                                    }
                                    break;
                                case CS_R103.Unwired:
                                    AddItemsToStatus("电话未接线！");
                                    break;
                                default:
                                    break;
                            }
                        }

                        break;
                    case SocketEventType.SendEvent:

                        break;
                    case SocketEventType.StopEvent:
                        if (_UDPServer != null)
                        {
                            _UDPServer.Dispose();
                            _UDPServer = null;
                        }
                        AddItemsToStatus(evt.remark);
                        break;
                }
            };
        }

        private void TryAddCallTime(DataFrame frame)
        {
            if (!CallRecord.Contains(frame.addInfo))
            {
                CallRecord.Add(frame.addInfo);
                AddItemsToStatus(frame.addInfo + "来电");
                CreteFaultList.ShowCreateWindow(frame.addInfo, new CallTime(DateTime.Now));
            }
        }

        public string StartConnent()
        {
            if (_UDPServer == null)
            {
                SocketFactory factory = new SocketFactory();
                _UDPServer = factory.CreateSocket(ServerType.UDP, HostName, Port);

                string log = _UDPServer.Run();
                if (log != null)
                {
                    _UDPServer = null;
                    return log;
                }
                _UDPServer.SocketEventHandler += new EventHandler(UDPEventHandler);
            }
            return null;
        }

        public void StopConnent()
        {
            if (_UDPServer != null)
            {
                _UDPServer.Stop();
            }
        }


        public void UDPEventHandler(object sender, EventArgs e)
        {
            var evt = e as SocketEvent;
            if (evt == null)
            {
                return;
            }
            //执行action
            Thread NetServer = new Thread(new ThreadStart(() => InvokeAction(evt)));
            NetServer.SetApartmentState(ApartmentState.STA);
            NetServer.IsBackground = true;
            NetServer.Start();
        }

        public void InvokeAction(SocketEvent evt)
        {
            System.Windows.Application.Current.Dispatcher.Invoke(UDPAction, evt);
        }

        public bool ExportCSV(string filePath, DataTable _data)
        {
            if (!CSVFileHelper.SaveCSV(_data, filePath))
            {
                return false;
            }
            return true;
        }

        public bool ExportTxt(string filePath)
        {
            if (!BLLCommon.SaveTxt(filePath, ListStatus.ToArray()))
            {
                return false;
            }
            return true;
        }

        /// <summary>
        /// 增加状态信息到列表里
        /// </summary>
        /// <param name="isCommand"></param>
        /// <param name="strStatus"></param>
        public void AddItemsToStatus(string strStatus)
        {
            strStatus = DateTime.Now.ToString() + "：" + strStatus;

            ListStatus.Add(strStatus);
            //大于一千行就删除其中条数
            if (ListStatus.Count>1000)
            {
                int rows = ListStatus.Count - 1000;
                for (int i = 0; i < rows; i++)
                {
                    ListStatus.RemoveAt(0);
                }                
            }
            FocusLastItem();
        }
        /// <summary>
        /// 委托定义，用于控制界面元素
        /// </summary>
        public Action FocusLastItem = null;

    }
}
