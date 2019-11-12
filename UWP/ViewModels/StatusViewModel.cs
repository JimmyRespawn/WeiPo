﻿using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Toolkit.Collections;
using Microsoft.Toolkit.Uwp.Helpers;
using WeiPo.Common;
using WeiPo.Common.Collection;
using WeiPo.Services;
using WeiPo.Services.Models;

namespace WeiPo.ViewModels
{
    public class HotflowDataSource : IIncrementalSource<CommentModel>
    {
        private readonly long _id;
        private readonly long _mid;
        private readonly StatusModel _status;
        private long _max_id;

        public HotflowDataSource(long id, long mid, StatusModel status)
        {
            _id = id;
            _mid = mid;
            _status = status;
        }

        public async Task<IEnumerable<CommentModel>> GetPagedItemsAsync(int pageIndex, int pageSize, CancellationToken cancellationToken = new CancellationToken())
        {
            if (pageIndex == 0)
            {
                _max_id = 0;
            }

            if (pageIndex != 0 && _max_id == 0)
            {
                return new List<CommentModel>();
            }

            try
            {
                var result = await Singleton<Api>.Instance.Hotflow(_id, _mid, _max_id);
                _max_id = result?.MaxId ?? 0;
                result?.Data?.ForEach(it => it.Status = _status);
                return result?.Data ?? new List<CommentModel>();
            }
            catch (WeiboException e)
            {
                return new List<CommentModel>();
            }
        }
    }

    public class RepostTimelineDataSource : IIncrementalSource<StatusModel>
    {
        private readonly long _id;

        public RepostTimelineDataSource(long id)
        {
            _id = id;
        }

        public async Task<IEnumerable<StatusModel>> GetPagedItemsAsync(int pageIndex, int pageSize, CancellationToken cancellationToken = new CancellationToken())
        {
            try
            {
                var result = await Singleton<Api>.Instance.RepostTimeline(_id, pageIndex + 1);
                return result?.Data ?? new List<StatusModel>();
            }
            catch (WeiboException e)
            {
                return new List<StatusModel>();
            }
        }
    }

    public class StatusViewModel : ViewModelBase
    {

        public LoadingCollection<HotflowDataSource, CommentModel>? HotflowSource { get; private set; }
        public LoadingCollection<RepostTimelineDataSource, StatusModel>? RepostSource { get; private set; }

        public StatusViewModel(StatusModel status)
        {
            Status = status;
            Init();
        }

        public StatusModel? Status { get; }

        private async void Init()
        {
            if (Status == null)
            {
                return;
            }
            long.TryParse(Status.Id, out var id);
            long.TryParse(Status.Mid, out var mid);
            if (Status.IsLongText == true)
            {
                var result = await Singleton<Api>.Instance.Extend(id);
                if (result?.LongTextContent != null)
                {
                    Status.LongText = result.LongTextContent;                    
                }
                //OnPropertyChanged(nameof(Status));
            }
            
            HotflowSource = new LoadingCollection<HotflowDataSource, CommentModel>(new HotflowDataSource(id, mid, Status));
            RepostSource = new LoadingCollection<RepostTimelineDataSource, StatusModel>(new RepostTimelineDataSource(id));

        }
    }
}