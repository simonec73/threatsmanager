using System;
using System.Linq;
using Newtonsoft.Json;
using PostSharp.Aspects;
using PostSharp.Aspects.Advices;
using PostSharp.Reflection;
using PostSharp.Serialization;
using ThreatsManager.Interfaces.ObjectModel;

namespace ThreatsManager.Utilities.Aspects.Engine
{
    //#region Additional placeholders required.
    //protected Guid _id { get; set; }
    //private string _lockingKey { get; set; }
    //#endregion

    //#region Additional optional placeholders.
    //private void CascadeLock() {}
    //private void CascadeUnlock() {}
    //#endregion

    [PSerializable]
    public class LockableAspect : InstanceLevelAspect
    {
        #region Imports from the extended class.
        [ImportMember("Identity", IsRequired=true)]
        public Property<IIdentity> Identity;

        [ImportMember("CascadeLock", IsRequired=false)]
        public Action CascadeLock;

        [ImportMember("CascadeUnlock", IsRequired=false)]
        public Action CascadeUnlock;
        #endregion

        #region Extra elements to be added.
        [IntroduceMember(OverrideAction = MemberOverrideAction.OverrideOrFail, 
            LinesOfCodeAvoided = 1, Visibility = Visibility.Private)]
        [CopyCustomAttributes(typeof(JsonPropertyAttribute), 
            OverrideAction = CustomAttributeOverrideAction.MergeReplaceProperty)]
        [JsonProperty("id")]
        public Guid _id { get; set; }

        [IntroduceMember(OverrideAction = MemberOverrideAction.OverrideOrFail, 
            LinesOfCodeAvoided = 1, Visibility = Visibility.Private)]
        [CopyCustomAttributes(typeof(JsonPropertyAttribute), 
            OverrideAction = CustomAttributeOverrideAction.MergeReplaceProperty)]
        [JsonProperty("lockKey")]
        public string _lockingKey { get; set; }
        #endregion

        #region Implementation of interface ILocked.
        private Action<object> _objectLocked;
        [IntroduceMember(OverrideAction = MemberOverrideAction.OverrideOrFail, LinesOfCodeAvoided = 6)]
        public event Action<object> ObjectLocked
        {
            add
            {
                if (_objectLocked == null || !_objectLocked.GetInvocationList().Contains(value))
                {
                    _objectLocked += value;
                }
            }
            remove
            {
                _objectLocked -= value;
            }
        }

        private Action<object> _objectUnlocked;
        [IntroduceMember(OverrideAction = MemberOverrideAction.OverrideOrFail, LinesOfCodeAvoided = 6)]
        public event Action<object> ObjectUnlocked
        {
            add
            {
                if (_objectUnlocked == null || !_objectUnlocked.GetInvocationList().Contains(value))
                {
                    _objectUnlocked += value;
                }
            }
            remove
            {
                _objectUnlocked -= value;
            }
        }

        [IntroduceMember(OverrideAction = MemberOverrideAction.OverrideOrFail, LinesOfCodeAvoided = 1)]
        [CopyCustomAttributes(typeof(JsonPropertyAttribute),
            OverrideAction = CustomAttributeOverrideAction.MergeReplaceProperty)]
        public bool Locked => !string.IsNullOrWhiteSpace(_lockingKey);

        public bool Lock(string key)
        {
            if (string.IsNullOrWhiteSpace(key))
                throw new ArgumentNullException(nameof(key));

            bool result = false;

            if (string.IsNullOrWhiteSpace(_lockingKey) || string.CompareOrdinal(key, _lockingKey) == 0)
            {
                _lockingKey = key;
                result = true;
                try
                {
                    CascadeLock?.Invoke();
                }
                catch
                {
                    // TODO: verify if hiding exceptions is the right thing to do, here.
                }
                finally
                {
                    var identity = Identity?.Get();
                    if (identity != null)
                        _objectLocked?.Invoke(identity);
                }
            }

            return result;
        }

        public bool Unlock(string key)
        {
            if (string.IsNullOrWhiteSpace(key))
                throw new ArgumentNullException(nameof(key));
            bool result = false;

            if (string.IsNullOrWhiteSpace(_lockingKey) || string.CompareOrdinal(key, _lockingKey) == 0)
            {
                _lockingKey = null;
                result = true;
                try
                {
                    CascadeUnlock?.Invoke();
                }
                finally
                {
                    var identity = Identity?.Get();
                    if (identity != null)
                        _objectUnlocked?.Invoke(identity);
                }
            }

            return result;
        }
        #endregion
    }
}
