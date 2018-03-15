﻿using System;
using System.Diagnostics;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace Muses.Wpf.Controls.Tools
{
    internal static class VisualStates
    {
        /// <summary>
        /// Translates a <see cref="TransitionType"/> to a visual state name.
        /// </summary>
        /// <param name="type">The <see cref="TransitionType"/> to translate.</param>
        /// <returns>The visual state name that corresponds to the <see cref="TransitionType"/></returns>
        public static string TransitionTypeToString(TransitionType type)
        {
            switch(type)
            {
                case TransitionType.Custom: return String.Empty;
                case TransitionType.None: return "Normal";
                case TransitionType.Down: return "DownTransition";
                case TransitionType.DownReplace: return "DownReplaceTransition";
                case TransitionType.Fade: return "DefaultTransition";
                case TransitionType.Left: return "LeftTransition";
                case TransitionType.LeftReplace: return "LeftReplaceTransition";
                case TransitionType.Right: return "RightTransition";
                case TransitionType.RightReplace: return "RightReplaceTransition";
                case TransitionType.Up: return "UpTransition";
                case TransitionType.UpReplace: return "UpReplaceTransition";
                default:
                    Debug.Assert(false, "Unknown TransitionType");
                    return "Normal";
            }
        }

        /// <summary>
        /// Gets the implementation root of the Control.
        /// </summary>
        /// <param name="dependencyObject">The DependencyObject.</param>
        /// <remarks>
        /// Implements Silverlight's corresponding internal property on Control.
        /// </remarks>
        /// <returns>Returns the implementation root or null.</returns>
        public static FrameworkElement GetImplementationRoot(DependencyObject dependencyObject)
        {
            Debug.Assert(dependencyObject != null, "DependencyObject should not be null.");
            return (1 == VisualTreeHelper.GetChildrenCount(dependencyObject)) ?
                VisualTreeHelper.GetChild(dependencyObject, 0) as FrameworkElement :
                null;
        }

        /// <summary>
        /// This method tries to get the named VisualStateGroup for the 
        /// dependency object. The provided object's ImplementationRoot will be 
        /// looked up in this call.
        /// </summary>
        /// <param name="dependencyObject">The dependency object.</param>
        /// <param name="groupName">The visual state group's name.</param>
        /// <returns>Returns null or the VisualStateGroup object.</returns>
        public static VisualStateGroup TryGetVisualStateGroup(DependencyObject dependencyObject, string groupName)
        {
            FrameworkElement root = GetImplementationRoot(dependencyObject);
            if (root == null)
            {
                return null;
            }

            return VisualStateManager.GetVisualStateGroups(root)
                .OfType<VisualStateGroup>()
                .Where(group => string.CompareOrdinal(groupName, group.Name) == 0)
                .FirstOrDefault();
        }
    }
}