using Cirrious.CrossCore.IoC;
using Cirrious.MvvmCross.Views;
using System;

namespace SSD.Mobile.Share
{
    //public class App : Cirrious.MvvmCross.ViewModels.MvxApplication
    //{
    //    public override void Initialize()
    //    {
    //        CreatableTypes()
    //            .EndingWith("Service")
    //            .AsInterfaces()
    //            .RegisterAsLazySingleton();

    //        RegisterAppStart<ViewModels.FirstViewModel>();
    //    }
    //}
    public class App : Cirrious.MvvmCross.ViewModels.MvxApplication
    {
        public override void Initialize()
        {
            //Mvx.RegisterSingleton<IJiraClient>(App.JiraClient);

            //CreatableTypes()
            //    .EndingWith("Biz")
            //    .AsInterfaces()
            //    .RegisterAsLazySingleton();

            RegisterAppStart(new CustomAppStart());
        }

        public class CustomAppStart : Cirrious.MvvmCross.ViewModels.MvxNavigatingObject, Cirrious.MvvmCross.ViewModels.IMvxAppStart
        {
            public void Start(object hint = null)
            {
                //INetworkService NService = Mvx.Resolve<INetworkService>();
                //IUserBiz userBiz = Mvx.Resolve<IUserBiz>();
                //var modelUser = userBiz.GetLoginDetails();
                //if (modelUser == null || string.IsNullOrWhiteSpace(modelUser.UserName))
                //{
                ShowViewModel<LoginViewModel>();
                //}
                //else
                //{
                //    ICommonBiz commonBiz = Mvx.Resolve<ICommonBiz>();
                //    await commonBiz.GetMenu();
                //    var user = await userBiz.GetDetails(modelUser.UserName);
                //    if (user != null)
                //        ShowViewModel<HomeViewModel>();
                //    else ShowViewModel<LoginViewModel>();
                //}
            }
        }
    }

    public interface IHostableView
    {
        bool Show(IMvxView view);
    }

    public interface IHostableViewPresenter
    {
        void Register(Type type, IHostableView parentView);
    }
}