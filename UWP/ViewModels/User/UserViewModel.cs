using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Toolkit.Uwp.Extensions;
using PropertyChanged;
using WeiPo.Common;
using WeiPo.Services;
using WeiPo.Services.Models;

namespace WeiPo.ViewModels.User
{
    public class UserViewModel : ViewModelBase
    {
        public UserViewModel(UserModel user, Action updatePivot)
        {
            UpdatePivot = updatePivot;
            Init(user.Id);
        }

        public UserViewModel(string name, Action updatePivot)
        {
            UpdatePivot = updatePivot;
            Init(name);
        }

        public UserViewModel(long id, Action updatePivot)
        {
            UpdatePivot = updatePivot;
            Init(id);
        }

        public bool IsLoading { get; private set; } = false;

        public ProfileData? Profile { get; private set; }

        [DependsOn(nameof(Profile))]
        public Services.Models.Tab[] Tabs
        {
            get
            {
                return Profile?.TabsInfo?.Tabs?.Concat(new[]
                {
                    new Services.Models.Tab
                    {
                        Title = "Follow".GetLocalized(),
                        TabType = "follow",
                        Containerid = Profile.UserInfo?.Id.ToString()
                    },
                    new Services.Models.Tab
                    {
                        Title = "Fans".GetLocalized(),
                        TabType = "fans",
                        Containerid = Profile.UserInfo?.Id.ToString()
                    }
                })?.ToArray() ?? new List<Services.Models.Tab>().ToArray();
            }
        }

        [DependsOn(nameof(Profile))]
        public bool IsNotMe
        {
            get
            {
                //TODO:Not a good idea
                return Profile?.UserInfo?.Id != null && Profile?.UserInfo?.Id != DockViewModel.Instance.MyProfile?.Result?.UserInfo?.Id;
            }
        }

        public Action UpdatePivot { get; }

        public async Task UpdateFollowState()
        {
            if (IsLoading || Profile == null || Profile.UserInfo?.Following == null || Profile.UserInfo.Id == null)
            {
                return;
            }
            IsLoading = true;
            UserModel? model;
            if (Profile.UserInfo.Following.Value)
            {
                model = await Singleton<Api>.Instance.Unfollow(Profile.UserInfo.Id.Value);
            }
            else
            {
                model = await Singleton<Api>.Instance.Follow(Profile.UserInfo.Id.Value);
            }
            Profile.UserInfo.Following = !Profile.UserInfo.Following;
            OnPropertyChanged(nameof(Profile));
            IsLoading = false;
        }

        private async void Init(long? id)
        {
            if (IsLoading || id == null)
            {
                return;
            }

            IsLoading = true;
            await InitProfile(id.Value);
            IsLoading = false;
        }

        private async void Init(string name)
        {
            if (IsLoading)
            {
                return;
            }

            IsLoading = true;
            var id = await Singleton<Api>.Instance.UserId(name);
            if (id != null)
            {
                await InitProfile(id.Value);                
            }
            IsLoading = false;
        }

        private async Task InitProfile(long id)
        {
            Profile = await Singleton<Api>.Instance.Profile(id);
            UpdatePivot.Invoke();
        }
    }
}