using System;
using System.Collections.Generic;
using Exceptionless;
using Microsoft.WindowsAPICodePack.Taskbar;
using PostSharp.Patterns.Contracts;

namespace ThreatsManager
{
    public class JumpListManager
    {
        private readonly JumpList _jumpList = JumpList.CreateJumpList();
        private readonly Dictionary<string, JumpListCustomCategory> _categories = new Dictionary<string, JumpListCustomCategory>();

        public JumpListManager()
        {
            try
            {
                _jumpList.ClearAllUserTasks();
                _jumpList.Refresh();
            }
            catch
            {
            }
        }

        #region Public members.
        public bool AutoRefresh { get; set; }

        public void AddLink([Required] string title, [Required] string path, 
            string arguments = null, string iconPath = null, int iconNumber = 0,
            string categoryName = null)
        {
            var task = CreateJumpListLink(title, path, arguments, CreateIconReference(iconPath, iconNumber));
            if (!string.IsNullOrWhiteSpace(categoryName))
            {
                if (!_categories.TryGetValue(categoryName, out var category))
                {
                    category = new JumpListCustomCategory(categoryName);
                    _categories.Add(categoryName, category);
                    _jumpList.AddCustomCategories(category);
                }
                category.AddJumpListItems(task);
            }
            else
            {
                _jumpList.AddUserTasks(task);
            }

            if (AutoRefresh)
                _jumpList.Refresh();
        }

        public void Refresh()
        {
            try
            {
                _jumpList.Refresh();
            }
            catch (UnauthorizedAccessException)
            {
            }
            catch (Exception e)
            {
                e.ToExceptionless().AddTags("Handled").Submit();
            }
        }

        public bool ShowRecentFiles
        {
            get => _jumpList.KnownCategoryToDisplay == JumpListKnownCategoryType.Recent;
            set => _jumpList.KnownCategoryToDisplay = value ? JumpListKnownCategoryType.Recent : JumpListKnownCategoryType.Neither;
        }

        public bool ShowFrequentFiles
        {
            get => _jumpList.KnownCategoryToDisplay == JumpListKnownCategoryType.Frequent;
            set => _jumpList.KnownCategoryToDisplay = value ? JumpListKnownCategoryType.Frequent : JumpListKnownCategoryType.Neither;
        }
        #endregion

        #region Private auxiliary member functions.
        private Microsoft.WindowsAPICodePack.Shell.IconReference? CreateIconReference(string iconPath, int iconNumber)
        {
            Microsoft.WindowsAPICodePack.Shell.IconReference? icon = null;
            if (!string.IsNullOrEmpty(iconPath))
                icon = new Microsoft.WindowsAPICodePack.Shell.IconReference(iconPath, iconNumber);

            return icon;
        }

        private static JumpListLink CreateJumpListLink(string title, string path, string arguments, 
            Microsoft.WindowsAPICodePack.Shell.IconReference? icon)
        {
            var task = new JumpListLink(path, title);
            if (icon.HasValue)
                task.IconReference = icon.Value;
            task.Arguments = arguments;

            return task;
        }
        #endregion
    }
}
