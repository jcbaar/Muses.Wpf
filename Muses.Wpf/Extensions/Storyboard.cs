using System;
using System.Diagnostics;
using System.Windows;
using System.Windows.Media;
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
        /// <param name="from">The <see cref="Thickness"/> to animate from.</param>
        /// <param name="to">The <see cref="Thickness"/> to animate to.</param>
        /// <param name="propertyPath">The property path to animate.</param>
        /// <param name="target">The target to run the animation on.</param>
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
        /// <param name="target">The target to run the animation on.</param>
        public static void AddObjectCollapseAnimation(this Storyboard sb, TimeSpan duration, DependencyObject target)
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
        /// <param name="from">The value to animate from.</param>
        /// <param name="to">The value to animate to.</param>
        /// <param name="propertyPath">The property path to animate.</param>
        /// <param name="target">The target to run the animation on.</param>
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

        #region ScaleTransfrom animation
        /// <summary>
        /// Adds a <see cref="ScaleTransform"/> to the <paramref name="target"/> and sets it's
        /// origin to the middle. Then it adds two double animations to the <see cref="Storyboard"/> which
        /// will scale the size of the <paramref name="target"/> from<paramref name="from"/> to <paramref name="to"/>.
        /// </summary>
        /// <param name="sb">The <see cref="Storyboard"/> to add the animation to.</param>
        /// <param name="duration">The duration of the animation.</param>
        /// <param name="from">The value to animate from.</param>
        /// <param name="to">The value to animate to.</param>
        /// <param name="target">The target to run the animation on.</param>
        public static void AddScaleTransform(this Storyboard sb, TimeSpan duration, double from, double to, UIElement target)
        {
            if (target == null) return;

            ScaleTransform scale = new ScaleTransform(1.0, 1.0);
            target.RenderTransformOrigin = new Point(0.5, 0.5);
            target.RenderTransform = scale;

            sb.AddDoubleAnimation(duration, from, to, "RenderTransform.ScaleX", target);
            sb.AddDoubleAnimation(duration, from, to, "RenderTransform.ScaleY", target);
        }
        #endregion
    }
}
