// Largely based on the TransitioningContentControl found at:
// https://github.com/jenspettersson/WPF-Controls
// (c) Copyright Microsoft Corporation.
// This source is subject to the Microsoft Public License (Ms-PL).
// Please see http://go.microsoft.com/fwlink/?LinkID=131993 for details.
// All other rights reserved.

using Muses.Wpf.Extensions;
using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Animation;

namespace Muses.Wpf.Controls
{
    public class TransitioningContentControl : ContentControl
    {
        #region Template part names
        /// <summary>
        /// The name of the control that will display the previous content.
        /// </summary>
        internal const string PreviousContentPresentationSitePartName = "PreviousContentPresentationSite";

        /// <summary>
        /// The name of the control that will display the current content.
        /// </summary>
        internal const string CurrentContentPresentationSitePartName = "CurrentContentPresentationSite";

        #endregion Template part names

        #region Construction
        /// <summary>
        /// Initializes a new instance of the <see cref="TransitioningContentControl"/> class.
        /// </summary>
        public TransitioningContentControl()
        {
            DefaultStyleKey = typeof(TransitioningContentControl);
        }
        #endregion

        #region TemplateParts
        /// <summary>
        /// Gets or sets the current content presentation site.
        /// </summary>
        /// <value>The current content presentation site.</value>
        private ContentPresenter CurrentContentPresentationSite { get; set; }

        /// <summary>
        /// Gets or sets the previous content presentation site.
        /// </summary>
        /// <value>The previous content presentation site.</value>
        private ContentPresenter PreviousContentPresentationSite { get; set; }
        #endregion

        #region IsTransitioning dependency property.
        /// <summary>
        /// Identifies the IsTransitioning dependency property.
        /// </summary>
        public static readonly DependencyProperty IsTransitioningProperty = DependencyProperty.Register("IsTransitioning", typeof(bool), typeof(TransitioningContentControl), new PropertyMetadata(OnIsTransitioningPropertyChanged));

        /// <summary>
        /// Indicates whether the control allows writing IsTransitioning.
        /// </summary>
        private bool _allowIsTransitioningWrite;

        /// <summary>
        /// Gets a value indicating whether this instance is currently performing
        /// a transition.
        /// </summary>
        public bool IsTransitioning
        {
            get => (bool)GetValue(IsTransitioningProperty); 
            private set
            {
                _allowIsTransitioningWrite = true;
                SetValue(IsTransitioningProperty, value);
                _allowIsTransitioningWrite = false;
            }
        }


        /// <summary>
        /// IsTransitioningProperty property changed handler.
        /// </summary>
        /// <param name="d"><see cref="TransitioningContentControl"/> that changed its <see cref="IsTransitioning"/>.</param>
        /// <param name="e">Event arguments.</param>
        private static void OnIsTransitioningPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            TransitioningContentControl source = (TransitioningContentControl)d;

            if (!source._allowIsTransitioningWrite)
            {
                source.IsTransitioning = (bool)e.OldValue;
                throw new InvalidOperationException();
            }
        }
        #endregion

        #region CurrentTransition property.
        /// <summary>
        /// The storyboard that is used to transition old and new content.
        /// </summary>
        private Storyboard _currentTransition;

        /// <summary>
        /// Gets or sets the storyboard that is used to transition old and new content.
        /// </summary>
        private Storyboard CurrentTransition
        {
            get => _currentTransition;
            set
            {
                // decouple event
                if (_currentTransition != null)
                {
                    _currentTransition.Completed -= OnTransitionCompleted;
                }

                _currentTransition = value;

                if (_currentTransition != null)
                {
                    _currentTransition.Completed += OnTransitionCompleted;
                }
            }
        }
        #endregion

        #region Transition dependency property.
        /// <summary>
        /// Identifies the Transition dependency property.
        /// </summary>
        public static readonly DependencyProperty TransitionProperty = DependencyProperty.Register("Transition", typeof(TransitionType), typeof(TransitioningContentControl), new PropertyMetadata(TransitionType.Fade, OnTransitionPropertyChanged));

        /// <summary>
        /// Gets or sets the name of the transition to use. These correspond
        /// directly to the VisualStates inside the PresentationStates group.
        /// </summary>
        public TransitionType Transition
        {
            get => (TransitionType)GetValue(TransitionProperty);
            set => SetValue(TransitionProperty, value); 
        }

        /// <summary>
        /// TransitionProperty property changed handler.
        /// </summary>
        /// <param name="d"><see cref="TransitioningContentControl"/> that changed its <see cref="Transition"/>.</param>
        /// <param name="e">Event arguments.</param>
        private static void OnTransitionPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            TransitioningContentControl source = (TransitioningContentControl)d;
            TransitionType oldTransition = (TransitionType)e.OldValue;
            TransitionType newTransition = (TransitionType)e.NewValue;

            if (source.IsTransitioning)
            {
                source.AbortTransition();
            }

            source.CurrentTransition = source.GetStoryboard(); 
        }
        #endregion

        #region RestartTransitionOnContentChange dependency property.
        /// <summary>
        /// Identifies the RestartTransitionOnContentChange dependency property.
        /// </summary>
        public static readonly DependencyProperty RestartTransitionOnContentChangeProperty = DependencyProperty.Register("RestartTransitionOnContentChange", typeof(bool), typeof(TransitioningContentControl), new PropertyMetadata(false, OnRestartTransitionOnContentChangePropertyChanged));

        /// <summary>
        /// Gets or sets a value indicating whether the current transition
        /// will be aborted when setting new content during a transition.
        /// </summary>
        public bool RestartTransitionOnContentChange
        {
            get => (bool)GetValue(RestartTransitionOnContentChangeProperty); 
            set => SetValue(RestartTransitionOnContentChangeProperty, value); 
        }

        /// <summary>
        /// RestartTransitionOnContentChangeProperty property changed handler.
        /// </summary>
        /// <param name="d"><see cref="TransitioningContentControl"/> that changed its <see cref="RestartTransitionOnContentChange"/>.</param>
        /// <param name="e">Event arguments.</param>
        private static void OnRestartTransitionOnContentChangePropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            ((TransitioningContentControl)d).OnRestartTransitionOnContentChangeChanged((bool)e.OldValue, (bool)e.NewValue);
        }

        /// <summary>
        /// Called when the RestartTransitionOnContentChangeProperty changes.
        /// </summary>
        /// <param name="oldValue">The old value of RestartTransitionOnContentChange.</param>
        /// <param name="newValue">The new value of RestartTransitionOnContentChange.</param>
        protected virtual void OnRestartTransitionOnContentChangeChanged(bool oldValue, bool newValue)
        {

        }
        #endregion

        #region TransitionDuration dependency property
        /// <summary>
        /// The TransitionDuration dependency property.
        /// </summary>
        public static readonly DependencyProperty TransitionDurationProperty = DependencyProperty.Register("TransitionDuration", typeof(TimeSpan), typeof(TransitioningContentControl), new PropertyMetadata(new TimeSpan(0,0,0,0,200)));   

        /// <summary>
        /// Gets/sets the value of the TransitionDuration property.
        /// </summary>
        public TimeSpan TransitionDuration
        {
            get => (TimeSpan)GetValue(TransitionDurationProperty);
            set => SetValue(TransitionDurationProperty, value);
        }
        #endregion

        #region Events
        /// <summary>
        /// Occurs when the current transition has completed.
        /// </summary>
        public event RoutedEventHandler TransitionCompleted;
        #endregion Events

        #region Overrides and event handlers.
        /// <summary>
        /// Builds the visual tree for the <see cref="TransitioningContentControl"/> control 
        /// when a new template is applied.
        /// </summary>
        public override void OnApplyTemplate()
        {
            if (IsTransitioning)
            {
                AbortTransition();
            }

            base.OnApplyTemplate();

            PreviousContentPresentationSite = GetTemplateChild(PreviousContentPresentationSitePartName) as ContentPresenter;
            CurrentContentPresentationSite = GetTemplateChild(CurrentContentPresentationSitePartName) as ContentPresenter;

            if (CurrentContentPresentationSite != null)
            {
                CurrentContentPresentationSite.Content = Content;
            }

            // Hookup current transition
            void eh(object sender, RoutedEventArgs e)
            {
                CurrentTransition = GetStoryboard();
                this.Loaded -= eh;
            }

            this.Loaded += eh;
        }

        /// <summary>
        /// Called when the value of the <see cref="ContentControl.Content"/> property changes.
        /// </summary>
        /// <param name="oldContent">The old value of the <see cref="ContentControl.Content"/> property.</param>
        /// <param name="newContent">The new value of the <see cref="ContentControl.Content"/> property.</param>
        protected override void OnContentChanged(object oldContent, object newContent)
        {
            base.OnContentChanged(oldContent, newContent);
            
            StartTransition(oldContent, newContent);
        }

        /// <summary>
        /// Handles the Completed event of the transition storyboard.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        private void OnTransitionCompleted(object sender, EventArgs e)
        {
            AbortTransition();

            TransitionCompleted?.Invoke(this, new RoutedEventArgs());
        }
        #endregion

        #region Public methods.
        /// <summary>
        /// Aborts the transition and releases the previous content.
        /// </summary>
        public void AbortTransition()
        {
            // Go to normal state and release our hold on the old content.
            CurrentTransition?.Stop();
            IsTransitioning = false;
            if (PreviousContentPresentationSite != null)
            {
                PreviousContentPresentationSite.Content = null;
            }
        }
        #endregion

        #region Private methods
        /// <summary>
        /// Starts the transition.
        /// </summary>
        /// <param name="oldContent">The old content.</param>
        /// <param name="newContent">The new content.</param>
        private void StartTransition(object oldContent, object newContent)
        {
            // Both presenters must be available, otherwise a transition is useless.
            if (CurrentContentPresentationSite != null && PreviousContentPresentationSite != null)
            {
                CurrentContentPresentationSite.Content = newContent;
                PreviousContentPresentationSite.Content = oldContent;

                // And start a new transition
                if (!IsTransitioning || RestartTransitionOnContentChange)
                {
                    IsTransitioning = true;
                    CurrentTransition?.Begin(this);
                }
            }
        }

        /// <summary>
        /// Attempts to find a storyboard that matches the newTransition name.
        /// </summary>
        /// <param name="newTransition">The new transition.</param>
        /// <returns>A storyboard or null, if no storyboard was found.</returns>
        private Storyboard GetStoryboard()
        {
            switch(Transition)
            {
                case TransitionType.Fade:
                    return GetFadeTransition();
                case TransitionType.Up:
                    return GetUpTransition();
                case TransitionType.Down:
                    return GetDownTransition();
                case TransitionType.Left:
                    return GetLeftTransition();
                case TransitionType.Right:
                    return GetRightTransition();
                case TransitionType.UpReplace:
                    return GetUpReplaceTransition();
                case TransitionType.DownReplace:
                    return GetDownReplaceTransition();
                case TransitionType.LeftReplace:
                    return GetLeftReplaceTransition();
                case TransitionType.RightReplace:
                    return GetRightReplaceTransition();
                case TransitionType.LeftSlideScale:
                    return GetLeftSlideScaleTransition();
                case TransitionType.RightSlideScale:
                    return GetRightSlideScaleTransition();
                case TransitionType.UpSlideScale:
                    return GetUpSlideScaleTransition();
                case TransitionType.DownSlideScale:
                    return GetDownSlideScaleTransition();
                default:
                    return GetNoneTransition();
            }
        }

        /// <summary>
        /// Creates a <see cref="TransitionType.None"/>transition.
        /// </summary>
        /// <returns>The transition <see cref="Storyboard"/></returns>
        private Storyboard GetNoneTransition()
        {
            var sb = new Storyboard();
            sb.AddObjectCollapseAnimation(TimeSpan.FromSeconds(0), PreviousContentPresentationSite);
            return sb;
        }

        /// <summary>
        /// Creates a <see cref="TransitionType.Fade"/>transition.
        /// </summary>
        /// <returns>The transition <see cref="Storyboard"/></returns>
        private Storyboard GetFadeTransition()
        {
            var sb = new Storyboard();
            sb.AddDoubleAnimation(TransitionDuration, 0.0, 1.0, "Opacity", CurrentContentPresentationSite);
            sb.AddDoubleAnimation(TransitionDuration, 1.0, 0.0, "Opacity", PreviousContentPresentationSite);
            return sb;
        }

        /// <summary>
        /// Creates a <see cref="TransitionType.Up"/>transition.
        /// </summary>
        /// <returns>The transition <see cref="Storyboard"/></returns>
        private Storyboard GetUpTransition()
        {
            var sb = new Storyboard();
            sb.AddDoubleAnimation(TransitionDuration, 0.0, 1.0, "Opacity", CurrentContentPresentationSite);
            sb.AddDoubleAnimation(TransitionDuration, 1.0, 0.0, "Opacity", PreviousContentPresentationSite);
            sb.AddThicknessAnimation(TransitionDuration, new Thickness(0, 40, 0, -40), new Thickness(0), 0.5, "Margin", CurrentContentPresentationSite);
            sb.AddThicknessAnimation(TransitionDuration, new Thickness(0), new Thickness(0, -40, 0, 40), 0.5, "Margin", PreviousContentPresentationSite);
            return sb;
        }

        /// <summary>
        /// Creates a <see cref="TransitionType.Down"/>transition.
        /// </summary>
        /// <returns>The transition <see cref="Storyboard"/></returns>
        private Storyboard GetDownTransition()
        {
            var sb = new Storyboard();
            sb.AddDoubleAnimation(TransitionDuration, 0.0, 1.0, "Opacity", CurrentContentPresentationSite);
            sb.AddDoubleAnimation(TransitionDuration, 1.0, 0.0, "Opacity", PreviousContentPresentationSite);
            sb.AddThicknessAnimation(TransitionDuration, new Thickness(0, -40, 0, 40), new Thickness(0), 0.5, "Margin", CurrentContentPresentationSite);
            sb.AddThicknessAnimation(TransitionDuration, new Thickness(0), new Thickness(0, 40, 0, -40), 0.5, "Margin", PreviousContentPresentationSite);
            return sb;
        }

        /// <summary>
        /// Creates a <see cref="TransitionType.Left"/>transition.
        /// </summary>
        /// <returns>The transition <see cref="Storyboard"/></returns>
        private Storyboard GetLeftTransition()
        {
            var sb = new Storyboard();
            sb.AddDoubleAnimation(TransitionDuration, 0.0, 1.0, "Opacity", CurrentContentPresentationSite);
            sb.AddDoubleAnimation(TransitionDuration, 1.0, 0.0, "Opacity", PreviousContentPresentationSite);
            sb.AddThicknessAnimation(TransitionDuration, new Thickness(40, 0, -40, 0), new Thickness(0), 0.5, "Margin", CurrentContentPresentationSite);
            sb.AddThicknessAnimation(TransitionDuration, new Thickness(0), new Thickness(-40, 0, 40, 0), 0.5, "Margin", PreviousContentPresentationSite);
            return sb;
        }

        /// <summary>
        /// Creates a <see cref="TransitionType.Right"/>transition.
        /// </summary>
        /// <returns>The transition <see cref="Storyboard"/></returns>
        private Storyboard GetRightTransition()
        {
            var sb = new Storyboard();
            sb.AddDoubleAnimation(TransitionDuration, 0.0, 1.0, "Opacity", CurrentContentPresentationSite);
            sb.AddDoubleAnimation(TransitionDuration, 1.0, 0.0, "Opacity", PreviousContentPresentationSite);
            sb.AddThicknessAnimation(TransitionDuration, new Thickness(-40, 0, 40, 0), new Thickness(0), 0.5, "Margin", CurrentContentPresentationSite);
            sb.AddThicknessAnimation(TransitionDuration, new Thickness(0), new Thickness(40, 0, -40, 0), 0.5, "Margin", PreviousContentPresentationSite);
            return sb;
        }

        /// <summary>
        /// Creates a <see cref="TransitionType.UpReplace"/>transition.
        /// </summary>
        /// <returns>The transition <see cref="Storyboard"/></returns>
        private Storyboard GetUpReplaceTransition()
        {
            var sb = new Storyboard();
            sb.AddDoubleAnimation(TransitionDuration, 0.0, 1.0, "Opacity", CurrentContentPresentationSite);
            sb.AddDoubleAnimation(TransitionDuration, 1.0, 0.0, "Opacity", PreviousContentPresentationSite);
            sb.AddThicknessAnimation(TransitionDuration, new Thickness(0, 40, 0, -40), new Thickness(0), 0.5, "Margin", CurrentContentPresentationSite);
            return sb;
        }

        /// <summary>
        /// Creates a <see cref="TransitionType.DownReplace"/>transition.
        /// </summary>
        /// <returns>The transition <see cref="Storyboard"/></returns>
        private Storyboard GetDownReplaceTransition()
        {
            var sb = new Storyboard();
            sb.AddDoubleAnimation(TransitionDuration, 0.0, 1.0, "Opacity", CurrentContentPresentationSite);
            sb.AddDoubleAnimation(TransitionDuration, 1.0, 0.0, "Opacity", PreviousContentPresentationSite);
            sb.AddThicknessAnimation(TransitionDuration, new Thickness(0, -40, 0, 40), new Thickness(0), 0.5, "Margin", CurrentContentPresentationSite);
            return sb;
        }

        /// <summary>
        /// Creates a <see cref="TransitionType.LeftReplace"/>transition.
        /// </summary>
        /// <returns>The transition <see cref="Storyboard"/></returns>
        private Storyboard GetLeftReplaceTransition()
        {
            var sb = new Storyboard();
            sb.AddDoubleAnimation(TransitionDuration, 0.0, 1.0, "Opacity", CurrentContentPresentationSite);
            sb.AddDoubleAnimation(TransitionDuration, 1.0, 0.0, "Opacity", PreviousContentPresentationSite);
            sb.AddThicknessAnimation(TransitionDuration, new Thickness(40, 0, -40, 0), new Thickness(0), 0.5, "Margin", CurrentContentPresentationSite);
            return sb;
        }

        /// <summary>
        /// Creates a <see cref="TransitionType.RightReplace"/>transition.
        /// </summary>
        /// <returns>The transition <see cref="Storyboard"/></returns>
        private Storyboard GetRightReplaceTransition()
        {
            var sb = new Storyboard();
            sb.AddDoubleAnimation(TransitionDuration, 0.0, 1.0, "Opacity", CurrentContentPresentationSite);
            sb.AddDoubleAnimation(TransitionDuration, 1.0, 0.0, "Opacity", PreviousContentPresentationSite);
            sb.AddThicknessAnimation(TransitionDuration, new Thickness(-40, 0, 40, 0), new Thickness(0), 0.5, "Margin", CurrentContentPresentationSite);
            return sb;
        }

        /// <summary>
        /// Creates a <see cref="TransitionType.UpSlideScale"/>transition.
        /// </summary>
        /// <returns>The transition <see cref="Storyboard"/></returns>
        private Storyboard GetUpSlideScaleTransition()
        {
            var sb = new Storyboard();
            sb.AddDoubleAnimation(TransitionDuration, 0.0, 1.0, "Opacity", CurrentContentPresentationSite);
            sb.AddDoubleAnimation(TransitionDuration, 1.0, 0.0, "Opacity", PreviousContentPresentationSite);
            sb.AddScaleTransform(TransitionDuration, 0.8, 1.0, CurrentContentPresentationSite);
            sb.AddScaleTransform(TransitionDuration, 1.0, 0.8, PreviousContentPresentationSite);

            double size = ActualHeight;
            sb.AddThicknessAnimation(TransitionDuration, new Thickness(0, size, 0, -size), new Thickness(0), 0.5, "Margin", CurrentContentPresentationSite);
            sb.AddThicknessAnimation(TransitionDuration, new Thickness(0), new Thickness(0, -size, 0, size), 0.5, "Margin", PreviousContentPresentationSite);
            return sb;
        }

        /// <summary>
        /// Creates a <see cref="TransitionType.DownSlideScale"/>transition.
        /// </summary>
        /// <returns>The transition <see cref="Storyboard"/></returns>
        private Storyboard GetDownSlideScaleTransition()
        {
            var sb = new Storyboard();
            sb.AddDoubleAnimation(TransitionDuration, 0.0, 1.0, "Opacity", CurrentContentPresentationSite);
            sb.AddDoubleAnimation(TransitionDuration, 1.0, 0.0, "Opacity", PreviousContentPresentationSite);
            sb.AddScaleTransform(TransitionDuration, 0.8, 1.0, CurrentContentPresentationSite);
            sb.AddScaleTransform(TransitionDuration, 1.0, 0.8, PreviousContentPresentationSite);

            double size = ActualHeight;
            sb.AddThicknessAnimation(TransitionDuration, new Thickness(0, -size, 0, size), new Thickness(0), 0.5, "Margin", CurrentContentPresentationSite);
            sb.AddThicknessAnimation(TransitionDuration, new Thickness(0), new Thickness(0, size, 0, -size), 0.5, "Margin", PreviousContentPresentationSite);
            return sb;
        }

        /// <summary>
        /// Creates a <see cref="TransitionType.LeftSlideScale"/>transition.
        /// </summary>
        /// <returns>The transition <see cref="Storyboard"/></returns>
        private Storyboard GetLeftSlideScaleTransition()
        {
            var sb = new Storyboard();
            sb.AddDoubleAnimation(TransitionDuration, 0.0, 1.0, "Opacity", CurrentContentPresentationSite);
            sb.AddDoubleAnimation(TransitionDuration, 1.0, 0.0, "Opacity", PreviousContentPresentationSite);
            sb.AddScaleTransform(TransitionDuration, 0.8, 1.0, CurrentContentPresentationSite);
            sb.AddScaleTransform(TransitionDuration, 1.0, 0.8, PreviousContentPresentationSite);

            double size = ActualWidth;
            sb.AddThicknessAnimation(TransitionDuration, new Thickness(size, 0, -size, 0), new Thickness(0), 0.5, "Margin", CurrentContentPresentationSite);
            sb.AddThicknessAnimation(TransitionDuration, new Thickness(0), new Thickness(-size, 0, size, 0), 0.5, "Margin", PreviousContentPresentationSite);
            return sb;
        }

        /// <summary>
        /// Creates a <see cref="TransitionType.RightSlideScale"/>transition.
        /// </summary>
        /// <returns>The transition <see cref="Storyboard"/></returns>
        private Storyboard GetRightSlideScaleTransition()
        {
            var sb = new Storyboard();
            sb.AddDoubleAnimation(TransitionDuration, 0.0, 1.0, "Opacity", CurrentContentPresentationSite);
            sb.AddDoubleAnimation(TransitionDuration, 1.0, 0.0, "Opacity", PreviousContentPresentationSite);
            sb.AddScaleTransform(TransitionDuration, 0.8, 1.0, CurrentContentPresentationSite);
            sb.AddScaleTransform(TransitionDuration, 1.0, 0.8, PreviousContentPresentationSite);

            double size = ActualWidth;
            sb.AddThicknessAnimation(TransitionDuration, new Thickness(-size, 0, size, 0), new Thickness(0), 0.5, "Margin", CurrentContentPresentationSite);
            sb.AddThicknessAnimation(TransitionDuration, new Thickness(0), new Thickness(size, 0, -size, 0), 0.5, "Margin", PreviousContentPresentationSite);
            return sb;
        }
        #endregion
    }
}
