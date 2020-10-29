using System.Drawing;
using ThreatsManager.Icons;
using ThreatsManager.Interfaces;

namespace ThreatsManager.Utilities.Help
{
    /// <summary>
    /// Training specific extensions.
    /// </summary>
    public static class HelpExtensions
    {
        /// <summary>
        /// Calculate the image for the Level.
        /// </summary>
        /// <param name="level">Level for which the image must be returned.</param>
        /// <param name="size">Size of the image.</param>
        /// <returns>Bitmap with the image corresponding to the training level.</returns>
        public static Bitmap GetLevelImage(this LearningLevel level, ImageSize size)
        {
            Bitmap result = null;

            switch (level)
            {
                case LearningLevel.Introductive:
                    switch (size)
                    {
                        case ImageSize.Small:
                            result = Resources.teacher_small;
                            break;
                        case ImageSize.Medium:
                            result = Resources.teacher;
                            break;
                        case ImageSize.Big:
                            result = Resources.teacher_big;
                            break;
                    }
                    break;
                case LearningLevel.Advanced:
                    switch (size)
                    {
                        case ImageSize.Small:
                            result = Resources.graduate_small;
                            break;
                        case ImageSize.Medium:
                            result = Resources.graduate;
                            break;
                        case ImageSize.Big:
                            result = Resources.graduate_big;
                            break;
                    }
                    break;
                case LearningLevel.Expert:
                    switch (size)
                    {
                        case ImageSize.Small:
                            result = Resources.genius_small;
                            break;
                        case ImageSize.Medium:
                            result = Resources.genius;
                            break;
                        case ImageSize.Big:
                            result = Resources.genius_big;
                            break;
                    }
                    break;
            }

            return result;
        }
    }
}
