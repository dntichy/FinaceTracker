using System;
using System.ComponentModel;
using System.Reflection;
using System.Windows;
using Microsoft.Extensions.DependencyInjection;

namespace TinyLittleMvvm
{
    /// <summary>
    /// Provides methods to get a view instance for a given view model.
    /// </summary>
    public class ViewLocator {
        private readonly IServiceProvider _serviceProvider;
        private readonly ViewLocatorOptions _options;

        /// <summary>
        /// Creates a new <see cref="ViewLocator"/> instance.
        /// </summary>
        /// <param name="serviceProvider">The service provider.</param>
        /// <param name="options">The options for ViewModel discovery.</param>
        public ViewLocator(IServiceProvider serviceProvider, ViewLocatorOptions options ) {
            _serviceProvider = serviceProvider;
            _options = options;
        }

        /// <summary>
        /// Gets the view for the passed view model.
        /// </summary>
        /// <param name="serviceProvider">The optional service provider. If <see langname="null"/>, the instance passed in the
        /// constructor will be used.</param>
        /// <typeparam name="TViewModel"></typeparam>
        /// <returns>The view matching the view model.</returns>
        /// <exception cref="InvalidOperationException">Thrown when the view cannot be found in the IoC container.</exception>
        /// <remarks>
        /// <para>
        /// To get the correct view type of the given <typeparamref name="TViewModel"/>, this method will
        /// call <see cref="ViewLocatorOptions.GetViewTypeFromViewModelType"/>.
        /// </para>
        /// <para>
        /// If <typeparamref name="TViewModel"/> implements <see cref="IOnLoadedHandler"/> or <see cref="IOnClosingHandler"/>,
        /// this method will register the view's corresponding events and call <see cref="IOnLoadedHandler.OnLoadedAsync"/>
        /// and <see cref="IOnClosingHandler.OnClosing"/> respectively when those events are raised.
        /// </para>
        /// <para>
        /// Don't call <strong>InitializeComponent()</strong> in your view's constructor yourself! If your view contains
        /// a method called InitializeComponent, this method will call it automatically via reflection.
        /// This allows the user of the library to remove the code-behind of her/his XAML files.
        /// </para>
        /// </remarks>
        public object GetViewForViewModel<TViewModel>(IServiceProvider serviceProvider = null) {
            var viewModel = (serviceProvider ?? _serviceProvider).GetRequiredService<TViewModel>();
            return GetViewForViewModel(viewModel, serviceProvider);
        }

        /// <summary>
        /// Gets the view for the passed view model.
        /// </summary>
        /// <param name="viewModel">The view model for which a view should be returned.</param>
        /// <param name="serviceProvider">The optional service provider. If <see langname="null"/>, the instance passed in the
        /// constructor will be used.</param>
        /// <returns>The view matching the view model.</returns>
        /// <exception cref="InvalidOperationException">Thrown when the view cannot be found in the IoC container.</exception>
        /// <remarks>
        /// <para>
        /// To get the correct view type of the passed <paramref name="viewModel"/>, this method will
        /// call <see cref="ViewLocatorOptions.GetViewTypeFromViewModelType"/>.
        /// </para>
        /// <para>
        /// If the <paramref name="viewModel"/> implements <see cref="IOnLoadedHandler"/> or <see cref="IOnClosingHandler"/>,
        /// this method will register the view's corresponding events and call <see cref="IOnLoadedHandler.OnLoadedAsync"/>
        /// and <see cref="IOnClosingHandler.OnClosing"/> respectively when those events are raised.
        /// </para>
        /// <para>
        /// Don't call <strong>InitializeComponent()</strong> in your view's constructor yourself! If your view contains
        /// a method called InitializeComponent, this method will call it automatically via reflection.
        /// This allows the user of the library to remove the code-behind of her/his XAML files.
        /// </para>
        /// </remarks>
        public object GetViewForViewModel(object viewModel, IServiceProvider serviceProvider = null) {
            var viewType = _options.GetViewTypeFromViewModelType(viewModel.GetType());
            if (viewType == null) {
                throw new InvalidOperationException("No View found for ViewModel of type " + viewModel.GetType());
            }

            var view = _serviceProvider.GetRequiredService(viewType);

            if (serviceProvider != null && view is DependencyObject dependencyObject) {
                ServiceProviderPropertyExtension.SetServiceProvider(dependencyObject, serviceProvider);
            }

            if (view is FrameworkElement frameworkElement) {
                AttachHandler(frameworkElement, viewModel);
                frameworkElement.DataContext = viewModel;
            }

            InitializeComponent(view);

            return view;
        }

        private static void AttachHandler(FrameworkElement view, object viewModel) {
            if (viewModel is IOnLoadedHandler onLoadedHandler) {
                RoutedEventHandler handler = null;
                handler = async (sender, args) => {
                    view.Loaded -= handler;
                    await onLoadedHandler.OnLoadedAsync();
                };
                view.Loaded += handler;
            }

            if (viewModel is IOnClosingHandler onClosingHandler) {
                if (view is Window window) {
                    CancelEventHandler handler = null;
                    handler = (sender, args) => {
                        window.Closing -= handler;
                        onClosingHandler.OnClosing();
                    };
                    window.Closing += handler;
                } else {
                    RoutedEventHandler handler = null;
                    handler = (sender, args) => {
                        view.Unloaded -= handler;
                        onClosingHandler.OnClosing();
                    };
                    view.Unloaded += handler;
                }
            }

            if (viewModel is ICancelableOnClosingHandler cancelableOnClosingHandler) {
                if (!(view is Window window)) {
                    throw new ArgumentException("If a view model implements ICancelableOnClosingHandler, the corresponding view must be a window.");
                }
                CancelEventHandler closingHandler = null;
                closingHandler = (sender, args) => {
                    args.Cancel = cancelableOnClosingHandler.OnClosing();
                };
                window.Closing += closingHandler;
                EventHandler closedHandler = null;
                closedHandler = (sender, args) => {
                    window.Closing -= closingHandler;
                    window.Closed -= closedHandler;
                };
                window.Closed += closedHandler;
            }
        }

        private static void InitializeComponent(object element) {
            var method = element.GetType().GetMethod("InitializeComponent", BindingFlags.Instance | BindingFlags.Public);
            method?.Invoke(element, null);
        }
    }
}