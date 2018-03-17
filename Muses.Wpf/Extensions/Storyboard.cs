using System;
using System.Diagnostics;
using System.Windows;
using System.Windows.Media.Animation;

namespace Muses.Wpf.Extensions
{
    public static class StoryboardExtensions
    {
        #region Thickness animation
        /// <summary>
        /// Adds a animation to the <see cref="Storyboard"/> which will
        /// animate the <paramref name="propertyPath"/> of the
        /// target.
        /// </summary>
        /// <param name="sb">The <see cref="Storyboard"/> to add the animation to.</param>
        /// <param name="duration">The duration of the animation.</param>
        /// <param name="target">Optionally the target to run the animation on.</param>
        public static void AddThicknessAnimation(this Storyboard sb, TimeSpan duration, Thickness from, Thickness to, double deceleration, string propertyPath, DependencyObject target)
        {
            Debug.Assert(sb != null, "The Storyboard should not be null.");
            var animation = new ThicknessAnimation
            {
                Duration = duration,
                From = from,
                To = to,
                DecelerationRatio = deceleration

            };

            Storyboard.SetTargetProperty(animation, new PropertyPath(propertyPath));
            if (target != null) Storyboard.SetTarget(animation, target);
            sb.Children.Add(animation);
        }
        #endregion

        #region Object animations
        /// <summary>
        /// Adds a animation to the <see cref="Storyboard"/> which will
        /// collapse the <see cref="UIElement.Visibility"/> of the
        /// target.
        /// </summary>
        /// <param name="sb">The <see cref="Storyboard"/> to add the animation to.</param>
        /// <param name="duration">The duration of the animation.</param>
        /// <param name="target">Optionally the target to run the animation on.</param>
        public static void AddObjectCollapseAnimation(this Storyboard sb, TimeSpan duration, DependencyObject target = null)
        {
            Debug.Assert(sb != null, "The Storyboard should not be null.");

            var animation = new ObjectAnimationUsingKeyFrames
            {
                Duration = duration
            };

            animation.KeyFrames.Add(new DiscreteObjectKeyFrame
            {
                KeyTime = duration,
                Value = Visibility.Collapsed
            });

            Storyboard.SetTargetProperty(animation, new PropertyPath("Visibility"));
            if (target != null) Storyboard.SetTarget(animation, target);
            sb.Children.Add(animation);
        }
        #endregion

        #region Double animation
        /// <summary>
        /// Adds a animation to the <see cref="Storyboard"/> which will
        /// animate the <paramref name="propertyPath"/> of the
        /// target.
        /// </summary>
        /// <param name="sb">The <see cref="Storyboard"/> to add the animation to.</param>
        /// <param name="duration">The duration of the animation.</param>
        /// <param name="target">Optionally the target to run the animation on.</param>
        public static void AddDoubleAnimation(this Storyboard sb, TimeSpan duration, double from, double to, string propertyPath, DependencyObject target)
        {
            Debug.Assert(sb != null, "The Storyboard should not be null.");

            var animation = new DoubleAnimation
            {
                Duration = duration,
                From = from,
                To = to
            };

            Storyboard.SetTargetProperty(animation, new PropertyPath(propertyPath));
            if (target != null) Storyboard.SetTarget(animation, target);
            sb.Children.Add(animation);
        }
        #endregion

    }
}
