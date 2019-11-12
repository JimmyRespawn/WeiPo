using System;
using System.Diagnostics;
using System.Threading.Tasks;
using Microsoft.Toolkit.Uwp.Helpers;
using WeiPo.Common;
using WeiPo.Services;
using WeiPo.Services.Models;

namespace WeiPo.ViewModels
{
    public class NotificationViewModel : ViewModelBase
    {
        private readonly int _duration = 60 * 1000;
        private bool _isLoginCompleted = false;
        private Task? _task;

        private NotificationViewModel()
        {
            Singleton<BroadcastCenter>.Instance.Subscribe("login_completed", delegate
            {
                _isLoginCompleted = true;

                _task = Task.Run(async () =>
                {
                    while (true)
                    {
                        if (_isLoginCompleted)
                        {
                            try
                            {
                                await FetchUnread();
                            }
                            catch (Exception e)
                            {
                                Console.WriteLine(e);
                                Debug.WriteLine(e.Message);
                                Debug.WriteLine(e.StackTrace);
                            }
                        }

                        await Task.Delay(_duration);
                    }
                });
            });
            Singleton<BroadcastCenter>.Instance.Subscribe("notification_clear_fans", delegate
            {
                DispatcherHelper.ExecuteOnUIThreadAsync(() =>
                {
                    if (Unread == null)
                    {
                        return;
                    }
                    Unread.Follower = 0;
                    OnPropertyChanged(nameof(Unread));
                });
            });
            Singleton<BroadcastCenter>.Instance.Subscribe("notification_clear_mention_at", delegate
            {
                DispatcherHelper.ExecuteOnUIThreadAsync(() =>
                {
                    if (Unread == null)
                    {
                        return;
                    }
                    Unread.MentionStatus = 0;
                    OnPropertyChanged(nameof(Unread));
                });
            });

            Singleton<BroadcastCenter>.Instance.Subscribe("notification_clear_mention_comment", delegate
            {
                DispatcherHelper.ExecuteOnUIThreadAsync(() =>
                {
                    if (Unread == null)
                    {
                        return;
                    }
                    Unread.MentionCmt = 0;
                    OnPropertyChanged(nameof(Unread));
                });
            });

            Singleton<BroadcastCenter>.Instance.Subscribe("notification_clear_comment", delegate
            {
                DispatcherHelper.ExecuteOnUIThreadAsync(() =>
                {
                    if (Unread == null)
                    {
                        return;
                    }
                    Unread.Cmt = 0;
                    OnPropertyChanged(nameof(Unread));
                });
            });

            Singleton<BroadcastCenter>.Instance.Subscribe("notification_clear_dm", delegate
            {
                DispatcherHelper.ExecuteOnUIThreadAsync(() =>
                {
                    if (Unread == null)
                    {
                        return;
                    }
                    Unread.Dm = 0;
                    OnPropertyChanged(nameof(Unread));
                });
            });
        }

        public static NotificationViewModel Instance { get; } = new NotificationViewModel();
        public UnreadModel? Unread { get; set; }

        private async Task FetchUnread()
        {
            Debug.WriteLine("fetching notification...");
            var result = await Singleton<Api>.Instance.Unread();
            if (result == null)
            {
                return;
            }

            await DispatcherHelper.ExecuteOnUIThreadAsync(() =>
            {
                if (Unread == null)
                {
                    return;
                }
                SendToastNotification(result, Unread);
                Unread = result;
            });
            Debug.WriteLine("fetching complete!");
        }

        private void SendToastNotification(UnreadModel newValue, UnreadModel oldValue)
        {
            if (newValue.Follower != 0 && newValue.Follower != oldValue?.Follower)
            {
                ToastNotificationSender.SendText(Localization.Format("FollowerCount", newValue.Follower ?? 0));
            }

            if (newValue.MentionStatus != 0 && newValue.MentionStatus != oldValue?.MentionStatus)
            {
                ToastNotificationSender.SendText(Localization.Format("MentionStatusCount", newValue.MentionStatus ?? 0));
            }

            if (newValue.MentionCmt != 0 && newValue.MentionCmt != oldValue?.MentionCmt)
            {
                ToastNotificationSender.SendText(Localization.Format("MentionCmtCount", newValue.MentionCmt ?? 0));
            }

            if (newValue.Cmt != 0 && newValue.Cmt != oldValue?.Cmt)
            {
                ToastNotificationSender.SendText(Localization.Format("CmtCount", newValue.Cmt ?? 0));
            }

            if (newValue.Dm != 0 && newValue.Dm != oldValue?.Dm)
            {
                ToastNotificationSender.SendText(Localization.Format("DmCount", newValue.Dm ?? 0));
            }
        }
    }
}