﻿using System.Collections;
using System.Collections.Generic;
using Hugula.Utils;
using UnityEngine;
using UnityEngine.UI;

namespace Hugula.UIComponents {
    public class LoopItemSelect : MonoBehaviour, ILoopSelectStyle {
        public Button clickBtn;

        public GameObject[] selected;

        public Animation selectedAnimation;

        private LoopItem m_LoopItem;
        public LoopItem loopItem {
            get {
                return m_LoopItem;
            }
        }
        ILoopSelect loopScrollBase;

        public void OnSelect () {
            loopScrollBase?.OnSelect (this);
        }

        public void SelectedStyle () {
            SetActiveSelectes (true);
            if (selectedAnimation != null)
                LuaHelper.PlayAnimation (selectedAnimation, "", AnimationDirection.Forward);
        }

        public void CancelStyle () {
            SetActiveSelectes (false);

            if (selectedAnimation != null)
                LuaHelper.PlayAnimation (selectedAnimation, "", AnimationDirection.Reverse);
        }

        public void InitSytle (LoopItem loopItem, ILoopSelect loopScrollBase) {
            this.m_LoopItem = loopItem;
            this.loopScrollBase = loopScrollBase;
        }

        void SetActiveSelectes (bool active) {
            foreach (var j in selected)
                j.SetActive (active);
        }

        void Start () {
            if (clickBtn == null) clickBtn = GetComponent<Button> ();
            clickBtn?.onClick.AddListener (OnSelect);
        }

        void OnDestroy () {
            if (clickBtn)
                clickBtn.onClick.RemoveListener (OnSelect);
            selected = null;
            clickBtn = null;
            m_LoopItem = null;
            loopScrollBase = null;
        }
    }

    public class LoopItem {
        public int templateType = 0; //当前项的模板类型
        public int index = -1; //对应data的索引
        public Component item; //clone的模板项
        public RectTransform transform; //
        public Rect rect; //当前位置

    }

    public class LoopVerticalItem : LoopItem {
        public Vector2 bound; //高度范围
        public void SetPos (float y) {
            bound.x = y;
        }

        public bool isDirty { set; get; }

        public void SetHeight (float height) {
            bound.y = height;
            isDirty = true;
        }

        public float yMin {
            get {
                return bound.x;
            }
        }

        public float yMax {
            get {
                return bound.x + bound.y;
            }
        }
    }

    public interface ILoopSelect {
        void OnSelect (ILoopSelectStyle loopItem);
    }

    public interface ILoopSelectStyle {

        void InitSytle (LoopItem loopItem, ILoopSelect loopScrollBase);

        LoopItem loopItem { get; }

        void SelectedStyle ();
        void CancelStyle ();
    }
}