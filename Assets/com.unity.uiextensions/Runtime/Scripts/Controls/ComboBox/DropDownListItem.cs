///Credit perchik
///Sourced from - http://forum.unity3d.com/threads/receive-onclick-event-and-pass-it-on-to-lower-ui-elements.293642/

using System;

namespace UnityEngine.UI.Extensions
{
    [Serializable]
    public class DropDownListItem
    {
        [SerializeField]
        private string _caption;


        public string Caption
        {
            get
            {
                return _caption;
            }
            set
            {
                _caption = value;
                if (OnUpdate != null)
                    OnUpdate();
            }
        }

        [SerializeField]
        private Sprite _image;

        public Sprite Image
        {
            get
            {
                return _image;
            }
            set
            {
                _image = value;
                if (OnUpdate != null)
                    OnUpdate();
            }
        }

        [SerializeField]
        private bool _isDisabled;

        public bool IsDisabled
        {
            get
            {
                return _isDisabled;
            }
            set
            {
                _isDisabled = value;
                if (OnUpdate != null)
                    OnUpdate();
            }
        }

        [SerializeField]
        private string _id;

        public string ID
        {
            get { return _id;  }
            set { _id = value; }
        }

        public Action OnSelect = null; 

        internal Action OnUpdate = null;   


        public DropDownListItem(string caption = "", string inId = "", Sprite image = null, bool disabled = false, Action onSelect = null)
        {
            _caption = caption;
            _image = image;
            _id = inId;
            _isDisabled = disabled;
            OnSelect = onSelect;
        }
    }
}