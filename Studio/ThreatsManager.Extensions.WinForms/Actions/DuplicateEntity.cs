using PostSharp.Patterns.Contracts;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using Forms = System.Windows.Forms;
using ThreatsManager.Extensions.Dialogs;
using ThreatsManager.Interfaces;
using ThreatsManager.Interfaces.Extensions;
using ThreatsManager.Interfaces.Extensions.Actions;
using ThreatsManager.Interfaces.ObjectModel;
using ThreatsManager.Interfaces.ObjectModel.Diagrams;
using ThreatsManager.Interfaces.ObjectModel.Entities;
using ThreatsManager.Utilities;

namespace ThreatsManager.Extensions.Actions
{
    [Extension("F5EDBF05-5A70-4844-BF52-C3B6193D0386", "Duplicate and Convert Context Aware Action", 35, ExecutionMode.Simplified)]
    public class DuplicateEntity: IShapeContextAwareAction, IIdentityContextAwareAction
    {
        public Scope Scope => Scope.Entity;
        public string Label => "Duplicate and Convert";
        public string Group => "ItemActions";
        public Bitmap Icon => Properties.Resources.objects_cube_to_cone_big;
        public Bitmap SmallIcon => Properties.Resources.objects_cube_to_cone;
        public Shortcut Shortcut => Shortcut.None;

        public bool Execute([NotNull] object item)
        {
            return false;
        }

        public bool IsVisible(object item)
        {
            return true;
        }

        public bool Execute([NotNull] IShape shape)
        {
            var result = false;

            if (shape is IEntityShape entityShape)
            {
                var identity = entityShape.Identity;
                if (identity != null)
                {
                    result = Execute(identity);
                }
            }

            return result;
        }

        public bool Execute([NotNull] IIdentity identity)
        {
            var result = false;

            if (identity is IEntity entity)
            {
                var dialog = new DuplicateConvertDialog();
                dialog.Initialize(entity);
                if (dialog.ShowDialog(Forms.Form.ActiveForm) == Forms.DialogResult.OK)
                {
                    IEntity newEntity;

                    using (var scope = UndoRedoManager.OpenScope("Duplicate Entity"))
                    {
                        if (dialog.Convert)
                        {
                            newEntity = entity.CopyAndConvert(dialog.EntityType, dialog.EntityTemplate);
                        }
                        else
                        {
                            newEntity = entity.CopyAndConvert();
                        }

                        if (newEntity != null)
                        {
                            if (!string.IsNullOrWhiteSpace(dialog.NewEntityName))
                                newEntity.Name = dialog.NewEntityName;

                            result = true;
                            scope?.Complete();
                        }
                    }
                }
            }

            return result;
        }
    }
}
