﻿using System.Drawing;
using ThreatsManager.Interfaces;
using ThreatsManager.Interfaces.ObjectModel;
using ThreatsManager.Interfaces.ObjectModel.Diagrams;
using ThreatsManager.Interfaces.ObjectModel.Entities;
using ThreatsManager.Interfaces.ObjectModel.ThreatsMitigations;
using ThreatsManager.Icons;

namespace ThreatsManager.Utilities
{
    /// <summary>
    /// Image Extensions.
    /// </summary>
    public static class ImageExtensions
    {
        /// <summary>
        /// Obtains the image related to the Identity passed as argument.
        /// </summary>
        /// <param name = "identity" > Identity for which the image should be determined.</param>
        /// <param name = "size" > Size of the image.</param>
        /// <returns>Requested image.</returns>
        public static Bitmap GetImage(this IIdentity identity, ImageSize size)
        {
            Bitmap result = null;

            if (identity is IEntity entity)
            {
                switch (size)
                {
                    case ImageSize.Big when entity.BigImage != null:
                        result = entity.BigImage;
                        break;
                    case ImageSize.Medium when entity.Image != null:
                        result = entity.Image;
                        break;
                    case ImageSize.Small when entity.SmallImage != null:
                        result = entity.SmallImage;
                        break;
                    default:
                        result = GetStandardEntityImage(entity, size);
                        break;
                }
            }
            else if (identity is IDataFlow)
            {
                result = GetFlowImage(size);
            }
            else if (identity is ITrustBoundary)
            {
                result = GetTrustBoundaryImage(size);
            }
            else if (identity is IEntityTemplate template)
            {
                result = GetEntityTemplateImage(template, size);
            }
            else if (identity is IFlowTemplate)
            {
                result = GetFlowImage(size);
            }
            else if (identity is ITrustBoundaryTemplate)
            {
                result = GetTrustBoundaryImage(size);
            }
            else if (identity is IThreatModel)
            {
                result = GetThreatModelImage(size);
            }
            else if (identity is IDiagram)
            {
                result = GetDiagramImage(size);
            }
            else if (identity is IThreatEvent)
            {
                result = GetThreatEventImage(size);
            }
            else if (identity is IThreatEventMitigation || identity is IMitigation || 
                identity is IVulnerabilityMitigation || identity is IThreatTypeMitigation ||
                identity is IWeaknessMitigation)
            {
                result = GetMitigationImage(size);
            }
            else if (identity is IThreatType)
            {
                result = GetThreatTypeImage(size);
            }
            else if (identity is IThreatEventScenario)
            {
                result = GetScenarioImage(size);
            }
            else if (identity is IThreatActor)
            {
                result = GetThreatActorImage(size);
            }
            else if (identity is IWeakness || identity is IThreatTypeWeakness)
            {
                result = GetWeaknessImage(size);
            }
            else if (identity is IVulnerability)
            {
                result = GetVulnerabilityImage(size);
            }

            if (result == null)
                result = GetDefaultImage(size);

            return Normalize(result, size);
        }

        /// <summary>
        /// Get the standard image for the Entity given its type.
        /// </summary>
        /// <param name="entityType">Type of Entity.</param>
        /// <param name="size">Size of the Image.</param>
        /// <returns>Requested image.</returns>
        public static Bitmap GetEntityImage(this EntityType entityType, ImageSize size)
        {
            Bitmap result = null;

            switch (entityType)
            {
                case EntityType.ExternalInteractor:
                    result = GetExternalInteractorImage(size);
                    break;
                case EntityType.Process:
                    result = GetProcessImage(size);
                    break;
                case EntityType.DataStore:
                    result = GetDataStoreImage(size);
                    break;
                default:
                    result = GetDefaultImage(size);
                    break;
            }

            return result;
        }

        private static Bitmap Normalize(Bitmap bitmap, ImageSize size)
        {
            Bitmap result;

            int heightWidth = 0;
            switch (size)
            {
                case ImageSize.Small:
                    heightWidth = 16;
                    break;
                case ImageSize.Medium:
                    heightWidth = 32;
                    break;
                case ImageSize.Big:
                    heightWidth = 64;
                    break;
            }

            if (heightWidth > 0 && (bitmap.Width != heightWidth || bitmap.Height != heightWidth))
                result = new Bitmap(bitmap, heightWidth, heightWidth);
            else
                result = bitmap;

            return result;
        }

        private static Bitmap GetStandardEntityImage(IEntity entity, ImageSize size)
        {
            Bitmap bitmapOverride = null;
            var template = entity.Template;
            if (template != null)
            {
                switch (size)
                {
                    case ImageSize.Small:
                        bitmapOverride = template.SmallImage;
                        break;
                    case ImageSize.Medium:
                        bitmapOverride = template.Image;
                        break;
                    case ImageSize.Big:
                        bitmapOverride = template.BigImage;
                        break;
                }
            }

            return bitmapOverride ?? (entity is IExternalInteractor ? GetExternalInteractorImage(size) :
                entity is IProcess ? GetProcessImage(size) :
                entity is IDataStore ? GetDataStoreImage(size) : null);
        }

        private static Bitmap GetExternalInteractorImage(ImageSize size)
        {
            Bitmap result;

            switch (size)
            {
                case ImageSize.Small:
                    result = Resources.external_small;
                    break;
                case ImageSize.Medium:
                    result = Resources.external;
                    break;
                case ImageSize.Big:
                    result = Resources.external_big;
                    break;
                default:
                    result = null;
                    break;
            }

            return result;
        }

        private static Bitmap GetProcessImage(ImageSize size)
        {
            Bitmap result;

            switch (size)
            {
                case ImageSize.Small:
                    result = Resources.process_small;
                    break;
                case ImageSize.Medium:
                    result = Resources.process;
                    break;
                case ImageSize.Big:
                    result = Resources.process_big;
                    break;
                default:
                    result = null;
                    break;
            }

            return result;
        }

        private static Bitmap GetDataStoreImage(ImageSize size)
        {
            Bitmap result;

            switch (size)
            {
                case ImageSize.Small:
                    result = Resources.storage_small;
                    break;
                case ImageSize.Medium:
                    result = Resources.storage;
                    break;
                case ImageSize.Big:
                    result = Resources.storage_big;
                    break;
                default:
                    result = null;
                    break;
            }

            return result;
        }

        /// <summary>
        /// Get the standard image for the Flows.
        /// </summary>
        /// <param name="size">Size of the Image.</param>
        /// <returns>Requested image.</returns>
        public static Bitmap GetFlowImage(ImageSize size)
        {
            Bitmap result;

            switch (size)
            {
                case ImageSize.Small:
                    result = Resources.flow_small;
                    break;
                case ImageSize.Medium:
                    result = Resources.flow;
                    break;
                case ImageSize.Big:
                    result = Resources.flow_big;
                    break;
                default:
                    result = null;
                    break;
            }

            return result;
        }

        /// <summary>
        /// Get the standard image for the Trust Boundaries.
        /// </summary>
        /// <param name="size">Size of the Image.</param>
        /// <returns>Requested image.</returns>
        public static Bitmap GetTrustBoundaryImage(ImageSize size)
        {
            Bitmap result;

            switch (size)
            {
                case ImageSize.Small:
                    result = Resources.trust_boundary_small;
                    break;
                case ImageSize.Medium:
                    result = Resources.trust_boundary;
                    break;
                case ImageSize.Big:
                    result = Resources.trust_boundary_big;
                    break;
                default:
                    result = null;
                    break;
            }

            return result;
        }

        private static Bitmap GetEntityTemplateImage(IEntityTemplate template, ImageSize size)
        {
            Bitmap result;

            switch (size)
            {
                case ImageSize.Small:
                    result = template.SmallImage;
                    break;
                case ImageSize.Medium:
                    result = template.Image;
                    break;
                case ImageSize.Big:
                    result = template.BigImage;
                    break;
                default:
                    result = null;
                    break;
            }

            return result;
        }

        /// <summary>
        /// Get the standard image for the Threat Model.
        /// </summary>
        /// <param name="size">Size of the Image.</param>
        /// <returns>Requested image.</returns>
        public static Bitmap GetThreatModelImage(ImageSize size)
        {
            Bitmap result;

            switch (size)
            {
                case ImageSize.Small:
                    result = Resources.threat_model_small;
                    break;
                case ImageSize.Medium:
                    result = Resources.threat_model;
                    break;
                case ImageSize.Big:
                    result = Resources.threat_model_big;
                    break;
                default:
                    result = null;
                    break;
            }

            return result;
        }

        /// <summary>
        /// Get the standard image for the Diagrams.
        /// </summary>
        /// <param name="size">Size of the Image.</param>
        /// <returns>Requested image.</returns>
        public static Bitmap GetDiagramImage(ImageSize size)
        {
            Bitmap result;

            switch (size)
            {
                case ImageSize.Small:
                    result = Resources.model_small;
                    break;
                case ImageSize.Medium:
                    result = Resources.model;
                    break;
                case ImageSize.Big:
                    result = Resources.model_big;
                    break;
                default:
                    result = null;
                    break;
            }

            return result;
        }

        /// <summary>
        /// Get the standard image for the Threat Events.
        /// </summary>
        /// <param name="size">Size of the Image.</param>
        /// <returns>Requested image.</returns>        
        public static Bitmap GetThreatEventImage(ImageSize size)
        {
            Bitmap result;

            switch (size)
            {
                case ImageSize.Small:
                    result = Resources.threat_event_small;
                    break;
                case ImageSize.Medium:
                    result = Resources.threat_event;
                    break;
                case ImageSize.Big:
                    result = Resources.threat_event_big;
                    break;
                default:
                    result = null;
                    break;
            }

            return result;
        }

        /// <summary>
        /// Get the standard image for the Mitigations.
        /// </summary>
        /// <param name="size">Size of the Image.</param>
        /// <returns>Requested image.</returns>
        public static Bitmap GetMitigationImage(ImageSize size)
        {
            Bitmap result;

            switch (size)
            {
                case ImageSize.Small:
                    result = Resources.mitigations_small;
                    break;
                case ImageSize.Medium:
                    result = Resources.mitigations;
                    break;
                case ImageSize.Big:
                    result = Resources.mitigations_big;
                    break;
                default:
                    result = null;
                    break;
            }

            return result;
        }

        /// <summary>
        /// Get the standard image for the Threat Types.
        /// </summary>
        /// <param name="size">Size of the Image.</param>
        /// <returns>Requested image.</returns>
        public static Bitmap GetThreatTypeImage(ImageSize size)
        {
            Bitmap result;

            switch (size)
            {
                case ImageSize.Small:
                    result = Resources.threat_type_small;
                    break;
                case ImageSize.Medium:
                    result = Resources.threat_type;
                    break;
                case ImageSize.Big:
                    result = Resources.threat_type_big;
                    break;
                default:
                    result = null;
                    break;
            }

            return result;
        }

        /// <summary>
        /// Get the standard image for the Scenarios.
        /// </summary>
        /// <param name="size">Size of the Image.</param>
        /// <returns>Requested image.</returns>        
        public static Bitmap GetScenarioImage(ImageSize size)
        {
            Bitmap result;

            switch (size)
            {
                case ImageSize.Small:
                    result = Resources.scenario_small;
                    break;
                case ImageSize.Medium:
                    result = Resources.scenario;
                    break;
                case ImageSize.Big:
                    result = Resources.scenario_big;
                    break;
                default:
                    result = null;
                    break;
            }

            return result;
        }

        /// <summary>
        /// Get the standard image for the Threat Actors.
        /// </summary>
        /// <param name="size">Size of the Image.</param>
        /// <returns>Requested image.</returns>        
        public static Bitmap GetThreatActorImage(ImageSize size)
        {
            Bitmap result;

            switch (size)
            {
                case ImageSize.Small:
                    result = Resources.actor_small;
                    break;
                case ImageSize.Medium:
                    result = Resources.actor;
                    break;
                case ImageSize.Big:
                    result = Resources.actor_big;
                    break;
                default:
                    result = null;
                    break;
            }

            return result;
        }

        /// <summary>
        /// Get the standard image for the Weaknesses.
        /// </summary>
        /// <param name="size">Size of the Image.</param>
        /// <returns>Requested image.</returns>        
        public static Bitmap GetWeaknessImage(ImageSize size)
        {
            Bitmap result;

            switch (size)
            {
                case ImageSize.Small:
                    result = Resources.weakness_small;
                    break;
                case ImageSize.Medium:
                    result = Resources.weakness;
                    break;
                case ImageSize.Big:
                    result = Resources.weakness_big;
                    break;
                default:
                    result = null;
                    break;
            }

            return result;
        }

        /// <summary>
        /// Get the standard image for the Vulnerabilities.
        /// </summary>
        /// <param name="size">Size of the Image.</param>
        /// <returns>Requested image.</returns>        
        public static Bitmap GetVulnerabilityImage(ImageSize size)
        {
            Bitmap result;

            switch (size)
            {
                case ImageSize.Small:
                    result = Resources.vulnerability_small;
                    break;
                case ImageSize.Medium:
                    result = Resources.vulnerability;
                    break;
                case ImageSize.Big:
                    result = Resources.vulnerability_big;
                    break;
                default:
                    result = null;
                    break;
            }

            return result;
        }

        private static Bitmap GetDefaultImage(ImageSize size)
        {
            Bitmap result;

            switch (size)
            {
                case ImageSize.Small:
                    result = Resources.undefined_small;
                    break;
                case ImageSize.Medium:
                    result = Resources.undefined;
                    break;
                case ImageSize.Big:
                    result = Resources.undefined_big;
                    break;
                default:
                    result = null;
                    break;
            }

            return result;
        }
    }
}
