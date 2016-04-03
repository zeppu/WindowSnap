using System;
using System.Drawing;
using System.Linq.Expressions;
using System.Reflection;
using System.Windows.Forms;
using ReactiveUI;

namespace Snapinator.Core.SystemTray
{
    class SystemTrayService : ISystemTrayService, IDisposable
    {
        private static readonly Expression<Action<IMessageBus>> SendMessageActionExpression =
            mb => mb.SendMessage(default(object), default(string));

        private static readonly MethodInfo SendMessageMethod;

        private readonly IServiceProvider _serviceProvider;

        private readonly NotifyIcon _systemTrayIcon = new NotifyIcon()
        {
            ContextMenuStrip = new ContextMenuStrip()
        };

        #region ISystemTrayNotificationService
        public void ShowInformation(string message, string caption)
        {
            ShowSystrayTooltip(ToolTipIcon.Info, message, caption);
        }

        private void ShowSystrayTooltip(ToolTipIcon toolTipIcon, string message, string caption)
        {
            _systemTrayIcon.BalloonTipIcon = toolTipIcon;
            _systemTrayIcon.BalloonTipText = message;
            _systemTrayIcon.BalloonTipTitle = caption;
            _systemTrayIcon.ShowBalloonTip(10 * 1000);
        }

        public void ShowWarning(string message, string caption)
        {
            ShowSystrayTooltip(ToolTipIcon.Warning, message, caption);
        }

        public void ShowError(string message, string caption)
        {
            ShowSystrayTooltip(ToolTipIcon.Error, message, caption);
        }

        public bool AskConfirmation(string message, string caption)
        {
            throw new NotImplementedException();
        }
        #endregion

        static SystemTrayService()
        {
            SendMessageMethod = ((MethodCallExpression)SendMessageActionExpression.Body).Method.GetGenericMethodDefinition();
        }

        public SystemTrayService(IServiceProvider serviceProvider)
        {

            _serviceProvider = serviceProvider;
            _systemTrayIcon.ContextMenuStrip.ItemClicked += (sender, args) =>
            {
                var metadata = (MenuItemCommandData)args.ClickedItem.Tag;

                if (metadata == null)
                    return;

                var message = _serviceProvider.Get(metadata.MessageType);
                var action = SendMessageMethod.MakeGenericMethod(metadata.MessageType);
                action.Invoke(MessageBus.Current, new object[] { message, null });
            };
        }

        public void Show()
        {
            _systemTrayIcon.Visible = true;
        }

        public void Hide()
        {
            _systemTrayIcon.Visible = false;
        }

        public void SetIcon(Uri packUri)
        {
            if (packUri == null) throw new ArgumentNullException(nameof(packUri));

            var streamResourceInfo = System.Windows.Application.GetResourceStream(packUri);
            if (streamResourceInfo != null)
            {
                using (var stream = streamResourceInfo.Stream)
                {
                    _systemTrayIcon.Icon = new Icon(stream);
                }

            }
            else
            {
                throw new InvalidOperationException("Invalid icon path");
            }
        }


        class MenuItemCommandData
        {
            public MenuItemCommandData(Type messageType)
            {
                MessageType = messageType;
            }

            public Type MessageType { get; }
        }

        public void AddMenuItem<TMessage>(string caption, Uri iconUri)
            where TMessage : class, new()
        {
            var item = new ToolStripMenuItem(caption)
            {
                Tag = new MenuItemCommandData(typeof(TMessage))
            };

            if (iconUri != null)
            {
                var streamResourceInfo = System.Windows.Application.GetResourceStream(iconUri);
                using (var stream = streamResourceInfo.Stream)
                {
                    var image = Image.FromStream(stream);
                    item.Image = image;
                }
            }

            _systemTrayIcon.ContextMenuStrip.Items.Add(item);
        }

        public void AddSeperator()
        {
            _systemTrayIcon.ContextMenuStrip.Items.Add(new ToolStripSeparator());
        }

        public void Dispose()
        {
            _systemTrayIcon.ContextMenuStrip.Dispose();

            _systemTrayIcon.Visible = false;
            _systemTrayIcon.Dispose();
        }
    }
}