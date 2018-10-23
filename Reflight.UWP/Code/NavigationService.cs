using System;
using System.Collections.Generic;
using Windows.UI.Xaml.Controls;

namespace ParrotDiscoReflight.Code
{
    class NavigationService : INavigationService
    {
        private readonly Frame frame;
        private readonly IDictionary<Type, Type> mapping = new Dictionary<Type, Type>();

        public NavigationService(Frame frame)
        {
            this.frame = frame;
        }

        public void Navigate<T>(T viewModel)
        {
            var pageType = mapping[typeof(T)];
            frame.Navigate(pageType, viewModel);
        }

        public void Register<TViewModel, TPage>()
        {
            mapping.Add(typeof(TViewModel), typeof(TPage));
        }
    }
}