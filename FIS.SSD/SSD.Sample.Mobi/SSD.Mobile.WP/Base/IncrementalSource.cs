using SSD.Mobile.Share;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Core;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Data;
using SSD.Mobile.Common.Para;

namespace SSD.Mobile.WP
{
    public class IncrementalSource<T> : ObservableCollection<T>, ISupportIncrementalLoading
        where T : PagedBaseModel
    {
        //private int VirtualCount { get; set; }
        IPageSource<T> _Source;
        ListFullPara _Para;
        public IncrementalSource(IPageSource<T> source)
        {
            _Source = source;
            HasMoreItems = true;
        }
        public IncrementalSource(IPageSource<T> source, ListFullPara para)
        {
            _Source = source;
            _Para = para;
            HasMoreItems = true;
        }
        
        #region ISupportIncrementalLoading

        public bool HasMoreItems
        {
            get;
            protected set;
        }

        public IAsyncOperation<LoadMoreItemsResult> LoadMoreItemsAsync(uint count)
        {
            return InnerLoadMoreItemsAsync(count).AsAsyncOperation();

            //return AsyncInfo.Run(async c => {
            //    long currrentOnID = 0;
            //    ObservableCollection<T> result = new ObservableCollection<T>();
            //    if (this.Count > 0)
            //        currrentOnID = this.First().OnID;
            //    if (_Para != null)
            //    {
            //        PagedPara para = new PagedPara() { ListStore = _Para.ListStore, StartDate = _Para.StartDate, EndDate = _Para.EndDate, CurrentOnID = currrentOnID, StartIndex = this.Count };
            //        result = await _Source.GetPagedOlder(para);
            //    }
            //    else
            //    {
            //        result = await _Source.GetPagedOlder(currrentOnID, this.Count);
            //    }
            //    HasMoreItems = result.Any();

            //    foreach (T item in result)
            //        this.Add(item);

            //    return new LoadMoreItemsResult() { Count = (uint)result.Count() };
            //});

            //CoreDispatcher dispatcher = Window.Current.Dispatcher;

            //return Task.Run<LoadMoreItemsResult>(
            //    async () =>
            //    {
            //        long currrentOnID=0;
            //        ObservableCollection<T> result = new ObservableCollection<T>();
            //        if(this.Count>0)
            //            currrentOnID = this.First().OnID;
            //        if(_Para!=null)
            //        {
            //            PagedPara para = new PagedPara(){ListStore = _Para.ListStore,StartDate = _Para.StartDate, EndDate = _Para.EndDate,CurrentOnID = currrentOnID, StartIndex=this.Count};
            //            result = await _Source.GetPagedOlder(para);
            //        }
            //        else
            //        {
            //            result = await _Source.GetPagedOlder(currrentOnID,this.Count);
            //        }
            //        CurrentCount = result.Count;

            //        foreach (T item in result)
            //            this.Add(item);
            //        //await dispatcher.RunAsync(
            //        //    CoreDispatcherPriority.Normal,
            //        //    () =>
            //        //    {
            //        //        foreach (T item in result)
            //        //            this.Add(item);
            //        //    });

            //        return new LoadMoreItemsResult() { Count = (uint)result.Count() };

            //    }).AsAsyncOperation<LoadMoreItemsResult>();
        }
        private async Task<LoadMoreItemsResult> InnerLoadMoreItemsAsync(uint expectedCount)
        {
            long currrentOnID = 0;
            ObservableCollection<T> result = new ObservableCollection<T>();
            if (this.Count > 0)
                currrentOnID = this.First().OnID;
            if (_Para != null)
            {
                PagedPara para = new PagedPara() { ListStore = _Para.ListStore, StartDate = _Para.StartDate, EndDate = _Para.EndDate, CurrentOnID = currrentOnID, StartIndex = this.Count };
                result = await _Source.GetPagedOlder(para);
            }
            else
            {
                result = await _Source.GetPagedOlder(currrentOnID, this.Count);
            }
            HasMoreItems = result.Any();

            foreach (T item in result)
                this.Add(item);

            return new LoadMoreItemsResult() { Count = (uint)result.Count() };

        }
        #endregion
    }
}