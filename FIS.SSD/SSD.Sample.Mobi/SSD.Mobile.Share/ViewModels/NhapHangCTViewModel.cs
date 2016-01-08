using Cirrious.MvvmCross.ViewModels;
using System.Windows.Input;
using System.Collections.ObjectModel;
using SSD.Mobile.Entities;
using System.Threading.Tasks;
using SSD.Mobile.BusinessLogics;
using System;
using SSD.Mobile.Common.Para;

namespace SSD.Mobile.Share
{
    public partial class NhapHangCTViewModel : ReportBaseViewModel<NhapHangModel>
	{
        NhapHangModel _model;
        public async void Init(NhapHangModel model)
        {
            IsBusy = true;
            _model = model;
            if (model != null)
            {
                var para = new IDPara() { LocationStoreID = model.LocationStoreID, ID = model.ID };
                Model = NhapHangModel.MapFull(await NhapHangBiz.Instance.Get(para));
            }
            IsBusy = false;
        }
		public override void OnCreate ()
		{
			base.OnCreate ();
		}
        private ICommand backCommand;
        public ICommand BackCommand
        {
            get
            {
                if (_model != null)
                {
                    switch (_model.ViewType)
                    {
                        case EnumViewModelNhapHangType.ThanhToan:
                            return backCommand = backCommand ?? new MvxCommand(() => ShowViewModel<ThanhToanViewModel>());
                        case EnumViewModelNhapHangType.CongNo:
                            return backCommand = backCommand ?? new MvxCommand(() => ShowViewModel<NhapHangCNViewModel>());
                        case EnumViewModelNhapHangType.Plan:
                            return backCommand = backCommand ?? new MvxCommand(() => ShowViewModel<NhapHangPlanViewModel>());
                        default:
                            return backCommand = backCommand ?? new MvxCommand(() => ShowViewModel<NhapHangListViewModel>());
                    }
                }
                return backCommand = backCommand ?? new MvxCommand(() => ShowViewModel<NhapHangListViewModel>());

            }
        }
    }
}

