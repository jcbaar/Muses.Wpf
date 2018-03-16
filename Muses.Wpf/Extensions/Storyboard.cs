using System;
using System.Diagnostics;
using System.Windows;
using System.Windows.Media.Animation;

namespace Muses.Wpf.Extensions
{
    public static class StoryboardExtensions
    {
        #region Slide animations
        public static void AddSlideOutToAbove(this Storyboard sb, TimeSpan duration, double pixels, double deceleration, DependencyObject target)
        {
            AddSlideAnimation(ref sb, duration, new Thickness(0), new Thickness(0, -pixels, 0, pixels), deceleration, target);
        }

        public static void AddSlideInFromAbove(this Storyboard sb, TimeSpan duration, double pixels, double deceleration, DependencyObject target)
        {
            AddSlideAnimation(ref sb, duration, new Thickness(0, -pixels, 0, pixels), new Thickness(0), deceleration, target);
        }

        public static void AddSlideOutToLeft(this Storyboard sb, TimeSpan duration, double pixels, double deceleration, DependencyObject target)
        {
            AddSlideAnimation(ref sb, duration, new Thickness(0), new Thickness(-pixels, 0, pixels, 0), deceleration, target);
        }

        public static void AddSlideInFromLeft(this Storyboard sb, TimeSpan duration, double pixels, double deceleration, DependencyObject target)
        {
            AddSlideAnimation(ref sb, duration, new Thickness(-pixels, 0, pixels, 0), new Thickness(0), deceleration, target);
        }

        public static void AddSlideOutToBelow(this Storyboard sb, TimeSpan duration, double pixels, double deceleration, DependencyObject target)
        {
            AddSlideAnimation(ref sb, duration, new Thickness(0), new Thickness(0, pixels, 0, -pixels), deceleration, target);
        }

        public static void AddSlideInFromBelow(this Storyboard sb, TimeSpan duration, double pixels, double deceleration, DependencyObject target)
        {
            AddSlideAnimation(ref sb, duration, new Thickness(0, pixels, 0, -pixels), new Thickness(0), deceleration, target);
        }

        public static void AddSlideOutToRight(this Storyboard sb, TimeSpan duration, double pixels, double deceleration, DependencyObject target)
        {
            AddSlideAnimation(ref sb, duration, new Thickness(0), new Thickness(pixels, 0, -pixels, 0), deceleration, target);
        }

        public static void AddSlideInFromRight(this Storyboard sb, TimeSpan duration, double pixels, double deceleration, DependencyObject target)
        {
            AddSlideAnimation(ref sb, duration, new Thickness(pixels, 0, -pixels, 0), new Thickness(0), deceleration, target);
        }

        private static void AddSlideAnimation(ref Storyboard sb, TimeSpan duration, Thickness from, Thickness to, double deceleration, DependencyObject target)
        {
            var animation = new ThicknessAnimation
            {
                Duration = duration,
                From = from,
                To = to,
                DecelerationRatio = deceleration

            };

            Storyboard.SetTargetProperty(animation, new PropertyPath("Margin"));
            if (target != null) Storyboard.SetTarget(animation, target);
            sb.Children.Add(animation);
        }
        #endregion

        #region Fade animations
        public static void AddFadeInAnimation(this Storyboard sb, TimeSpan duration, DependencyObject target)
        {
            Debug.Assert(sb != null, "The Storyboard should not be null.");

            var animation = new DoubleAnimation
            {
                Duration = duration,
                From = 0.0,
                To = 1.0
            };

            Storyboard.SetTargetProperty(animation, new PropertyPath("Opacity"));
            if (target != null) Storyboard.SetTarget(animation, target);
            sb.Children.Add(animation);
        }

        public static void AddFadeOutAnimation(this Storyboard sb, TimeSpan duration, DependencyObject target)
        {
            Debug.Assert(sb != null, "The Storyboard should not be null.");

            var animation = new DoubleAnimation
            {
                Duration = duration,
                From = 1.0,
                To = 0.0
            };

            Storyboard.SetTargetProperty(animation, new PropertyPath("Opacity"));
            if (target != null) Storyboard.SetTarget(animation, target);
            sb.Children.Add(animation);
        }
        #endregion

        public static void AddObjectCollapseAnimation(this Storyboard sb, TimeSpan duration, DependencyObject target)
        {
            Debug.Assert(sb != null, "The Storyboard should not be null.");

            var animation = new ObjectAnimationUsingKeyFrames
            {
                Duration = duration
            };

            var kf = new DiscreteObjectKeyFrame
            {
                KeyTime = TimeSpan.FromSeconds(0),
                Value = Visibility.Collapsed
            };

            animation.KeyFrames.Add(kf);

            Storyboard.SetTargetProperty(animation, new PropertyPath("Visibility"));
            if (target != null) Storyboard.SetTarget(animation, target);
            sb.Children.Add(animation);
        }

    }
}
