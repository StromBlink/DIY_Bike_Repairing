using System.Collections;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Tabtale.TTPlugins {

    public abstract class TTPLogMessageList<TViewsHolder> : MonoBehaviour where TViewsHolder : TTPAbstractViewsHolder
    {
        public RectTransform viewport;
        public List<TViewsHolder> viewsHolders = new List<TViewsHolder>();
        public ScrollRect ScrollRectComponent { get; private set; }
        public LayoutGroup ContentLayoutGroup { get; private set; }
        public RectTransform ScrollRectRT { get; private set; }
        public bool IsHorizontal { get { return ScrollRectComponent.horizontal; } }
        public float ContentSize { get { return ScrollRectComponent.content.rect.size[_oneIfVerticalZeroIfHorizontal]; } }
        public float ViewportSize { get { return viewport.rect.size[_oneIfVerticalZeroIfHorizontal]; } }
        public RectOffset Padding { get { return ContentLayoutGroup == null ? _zeroRectOffset : ContentLayoutGroup.padding; } }
        readonly RectOffset _zeroRectOffset = new RectOffset();
        int _oneIfHorizontalZeroIfVertical, _oneIfVerticalZeroIfHorizontal;
        Coroutine _smoothScrollToCoroutine;
        public float AbstractNormalizedPosition
        {
            get
            {
                return IsHorizontal
                    ? 1f - ScrollRectComponent.horizontalNormalizedPosition
                    : ScrollRectComponent.verticalNormalizedPosition;
            }
            set
            {
                if (IsHorizontal) ScrollRectComponent.horizontalNormalizedPosition = 1f - value;
                else ScrollRectComponent.verticalNormalizedPosition = value;
            }
        }

        protected virtual void Awake()
        {
        }

        protected virtual void Start()
        {
            ScrollRectComponent = GetComponent<ScrollRect>();
            ContentLayoutGroup = ScrollRectComponent.content.GetComponent<LayoutGroup>();
            ScrollRectRT = transform as RectTransform;
            if (!viewport)
                viewport = ScrollRectRT;

            _oneIfHorizontalZeroIfVertical = ScrollRectComponent.horizontal ? 1 : 0;
            _oneIfVerticalZeroIfHorizontal = 1 - _oneIfHorizontalZeroIfVertical;
        }

        protected virtual void Update()
        {
        }

        protected virtual void OnDestroy()
        {
            if (_smoothScrollToCoroutine != null)
            {
                StopCoroutine(_smoothScrollToCoroutine);
                _smoothScrollToCoroutine = null;
            }
        }

        public void Refresh()
        {
            ChangeItemsCount(0, viewsHolders.Count);
        }

        public void ResetItems(int itemsCount, bool contentPanelEndEdgeStationary = false)
        {
            ChangeItemsCount(0, itemsCount, -1, contentPanelEndEdgeStationary);
        }

        public virtual void InsertItems(int index, int itemsCount, bool contentPanelEndEdgeStationary = false)
        {
            ChangeItemsCount(1, itemsCount, index, contentPanelEndEdgeStationary);
        }

        public void RemoveItems(int index, int itemsCount, bool contentPanelEndEdgeStationary = false)
        {
            ChangeItemsCount(2, itemsCount, index, contentPanelEndEdgeStationary);
        }

        public virtual bool SmoothScrollTo(int itemIndex, float duration = .75f,
            float normalizedOffsetFromViewportStart = 0f, float normalizedPositionOfItemPivotToUse = 0f,
            Action onDone = null, bool overrideCurrentScrollingAnimation = false)
        {
            if (ContentSize <= ViewportSize)
                return false;

            if (_smoothScrollToCoroutine != null)
            {
                if (!overrideCurrentScrollingAnimation)
                    return false;

                StopCoroutine(_smoothScrollToCoroutine);
                _smoothScrollToCoroutine = null;
            }

            duration = Mathf.Clamp(duration, 0.001f, 100f);

            var vh = viewsHolders[itemIndex];
            float itemSize = vh.root.rect.size[_oneIfVerticalZeroIfHorizontal];
            RectTransform.Edge edge;
            if (ScrollRectComponent.horizontal)
                edge = RectTransform.Edge.Left;
            else
                edge = RectTransform.Edge.Top;
            var contentRT = ScrollRectComponent.content;
            float insetFromParentStart = vh.root.GetInsetFromParentEdge(contentRT, edge);
            float initialContentInsetFromViewportStart = contentRT.GetInsetFromParentEdge(viewport, edge);
            float targetContentInsetFromViewportStart = -insetFromParentStart;
            targetContentInsetFromViewportStart += ViewportSize * normalizedOffsetFromViewportStart;
            targetContentInsetFromViewportStart -= itemSize * normalizedPositionOfItemPivotToUse;
            float scrollableArea = ContentSize - ViewportSize;
            targetContentInsetFromViewportStart = Mathf.Clamp(targetContentInsetFromViewportStart, -scrollableArea, 0f);

            _smoothScrollToCoroutine = StartCoroutine(SetContentInsetFromViewportStart(edge,
                initialContentInsetFromViewportStart, targetContentInsetFromViewportStart, duration, onDone));
            return true;
        }

        IEnumerator SetContentInsetFromViewportStart(RectTransform.Edge edge, float fromInset, float toInset, float duration, Action onDone)
        {
            float startTime = Time.time;
            float curElapsed, t01;
            bool inProgress = true;
            do
            {
                yield return null;
                if (_smoothScrollToCoroutine == null)
                    yield break;

                curElapsed = Time.time - startTime;
                t01 = curElapsed / duration;
                if (t01 >= 1f)
                {
                    t01 = 1f;

                    inProgress = false;
                }
                else
                    t01 = Mathf.Sin(t01 * Mathf.PI / 2); // normal in, sin slow out

                float curInset = Mathf.Lerp(fromInset, toInset, t01);
                ScrollRectComponent.content.SetInsetAndSizeFromParentEdgeWithCurrentAnchors(edge, curInset, ContentSize);
            } while (inProgress);

            _smoothScrollToCoroutine = null;

            if (onDone != null)
                onDone();
        }

        void ChangeItemsCount(int changeMode, int itemsCount, int indexIfAppendingOrRemoving = -1, bool contentPanelEndEdgeStationary = false)
        {
            float ctInsetBefore;
            RectTransform.Edge startEdge, endEdge;
            if (IsHorizontal)
            {
                startEdge = RectTransform.Edge.Left;
                endEdge = RectTransform.Edge.Right;
            }
            else
            {
                startEdge = RectTransform.Edge.Top;
                endEdge = RectTransform.Edge.Bottom;
            }

            RectTransform.Edge edgeToInsetFrom;
            ctInsetBefore = ScrollRectComponent.content.GetInsetFromParentEdge(viewport,
                edgeToInsetFrom = (contentPanelEndEdgeStationary ? endEdge : startEdge));

            switch (changeMode)
            {
                case 0: // reset
                    RemoveItemsAndUpdateIndices(0, viewsHolders.Count);
                    AddItemsAndUpdateIndices(0, itemsCount);
                    break;
                case 1: // insert
                    AddItemsAndUpdateIndices(indexIfAppendingOrRemoving, itemsCount);
                    break;
                case 2: // remove
                    RemoveItemsAndUpdateIndices(indexIfAppendingOrRemoving, itemsCount);
                    break;
            }

            Canvas.ForceUpdateCanvases();
            ScrollRectComponent.content.SetInsetAndSizeFromParentEdgeWithCurrentAnchors(edgeToInsetFrom, ctInsetBefore,
                ContentSize);
        }

        void RemoveItemsAndUpdateIndices(int index, int count)
        {
            int rem = count;
            while (rem-- > 0)
            {
                Destroy(viewsHolders[index].root.gameObject);
                viewsHolders.RemoveAt(index);
            }

            for (int i = index; i < viewsHolders.Count; ++i)
            {
                var vh = viewsHolders[i];
                vh.ItemIndex -= count;
                UpdateViewsHolder(vh);
            }
        }

        void AddItemsAndUpdateIndices(int index, int count)
        {
            int lastIndexExcl = index + count;
            for (int i = index; i < lastIndexExcl; ++i)
            {
                var vh = CreateViewsHolder(i);
                viewsHolders.Insert(i, vh);
                vh.root.SetParent(ScrollRectComponent.content, false);
                vh.root.SetSiblingIndex(i);
                UpdateViewsHolder(vh);
            }

            // Increase the index of next items
            for (int i = lastIndexExcl; i < viewsHolders.Count; ++i)
            {
                var vh = viewsHolders[i];
                vh.ItemIndex += count;
                UpdateViewsHolder(vh);
            }
        }


        protected abstract TViewsHolder CreateViewsHolder(int itemIndex);

        protected abstract void UpdateViewsHolder(TViewsHolder vh);
    }


    static class TTPRectTransformExtensions
    {
        static Dictionary<RectTransform.Edge, Func<RectTransform, RectTransform, float>>
            _getInsetFromParentEdgeMappedActions =
                new Dictionary<RectTransform.Edge, Func<RectTransform, RectTransform, float>>()
                {
                {RectTransform.Edge.Bottom, GetInsetFromParentBottomEdge},
                {RectTransform.Edge.Top, GetInsetFromParentTopEdge},
                {RectTransform.Edge.Left, GetInsetFromParentLeftEdge},
                {RectTransform.Edge.Right, GetInsetFromParentRightEdge}
                };

        static Dictionary<RectTransform.Edge, Action<RectTransform, RectTransform, float, float>>
            _setInsetAndSizeFromParentEdgeWithCurrentAnchorsMappedActions =
                new Dictionary<RectTransform.Edge, Action<RectTransform, RectTransform, float, float>>()
                {
                {
                    RectTransform.Edge.Bottom,
                    (child, parentHint, newInset, newSize) =>
                    {
                        var offsetChange = newInset - child.GetInsetFromParentBottomEdge(parentHint);
                        var offsetMin =
                            new Vector2(child.offsetMin.x,
                                child.offsetMin.y +
                                offsetChange); // need to store it before modifying anything, because the offsetmax will change the offsetmin and vice-versa
                        child.offsetMax = new Vector2(child.offsetMax.x,
                            child.offsetMax.y + (newSize - child.rect.height + offsetChange));
                        child.offsetMin = offsetMin;
                    }
                },
                {
                    RectTransform.Edge.Top,
                    (child, parentHint, newInset, newSize) =>
                    {
                        var offsetChange = newInset - child.GetInsetFromParentTopEdge(parentHint);
                        var offsetMax = new Vector2(child.offsetMax.x, child.offsetMax.y - offsetChange);
                        child.offsetMin = new Vector2(child.offsetMin.x,
                            child.offsetMin.y - (newSize - child.rect.height + offsetChange));
                        child.offsetMax = offsetMax;
                    }
                },
                {
                    RectTransform.Edge.Left,
                    (child, parentHint, newInset, newSize) =>
                    {
                        var offsetChange = newInset - child.GetInsetFromParentLeftEdge(parentHint);
                        var offsetMin = new Vector2(child.offsetMin.x + offsetChange, child.offsetMin.y);
                        child.offsetMax = new Vector2(child.offsetMax.x + (newSize - child.rect.width + offsetChange),
                            child.offsetMax.y);
                        child.offsetMin = offsetMin;
                    }
                },
                {
                    RectTransform.Edge.Right,
                    (child, parentHint, newInset, newSize) =>
                    {
                        var offsetChange = newInset - child.GetInsetFromParentRightEdge(parentHint);
                        var offsetMax = new Vector2(child.offsetMax.x - offsetChange, child.offsetMax.y);
                        child.offsetMin = new Vector2(child.offsetMin.x - (newSize - child.rect.width + offsetChange),
                            child.offsetMin.y);
                        child.offsetMax = offsetMax;
                    }
                }
                };

        public static float GetInsetFromParentTopEdge(this RectTransform child, RectTransform parentHint)
        {
            float parentPivotYDistToParentTop = (1f - parentHint.pivot.y) * parentHint.rect.height;
            float childLocPosY = child.localPosition.y;

            return parentPivotYDistToParentTop - child.rect.yMax - childLocPosY;
        }

        public static float GetInsetFromParentBottomEdge(this RectTransform child, RectTransform parentHint)
        {
            float parentPivotYDistToParentBottom = parentHint.pivot.y * parentHint.rect.height;
            float childLocPosY = child.localPosition.y;

            return parentPivotYDistToParentBottom + child.rect.yMin + childLocPosY;
        }

        public static float GetInsetFromParentLeftEdge(this RectTransform child, RectTransform parentHint)
        {
            float parentPivotXDistToParentLeft = parentHint.pivot.x * parentHint.rect.width;
            float childLocPosX = child.localPosition.x;

            return parentPivotXDistToParentLeft + child.rect.xMin + childLocPosX;
        }

        public static float GetInsetFromParentRightEdge(this RectTransform child, RectTransform parentHint)
        {
            float parentPivotXDistToParentRight = (1f - parentHint.pivot.x) * parentHint.rect.width;
            float childLocPosX = child.localPosition.x;

            return parentPivotXDistToParentRight - child.rect.xMax - childLocPosX;
        }

        public static float GetInsetFromParentEdge(this RectTransform child, RectTransform parentHint,
            RectTransform.Edge parentEdge)
        {
            return _getInsetFromParentEdgeMappedActions[parentEdge](child, parentHint);
        }

        public static void SetSizeFromParentEdgeWithCurrentAnchors(this RectTransform child, RectTransform.Edge fixedEdge,
            float newSize)
        {
            var par = child.parent as RectTransform;
            child.SetInsetAndSizeFromParentEdgeWithCurrentAnchors(par, fixedEdge,
                child.GetInsetFromParentEdge(par, fixedEdge), newSize);
        }

        public static void SetSizeFromParentEdgeWithCurrentAnchors(this RectTransform child, RectTransform parentHint,
            RectTransform.Edge fixedEdge, float newSize)
        {
            child.SetInsetAndSizeFromParentEdgeWithCurrentAnchors(parentHint, fixedEdge,
                child.GetInsetFromParentEdge(parentHint, fixedEdge), newSize);
        }

        public static void SetInsetAndSizeFromParentEdgeWithCurrentAnchors(this RectTransform child,
            RectTransform.Edge fixedEdge, float newInset, float newSize)
        {
            child.SetInsetAndSizeFromParentEdgeWithCurrentAnchors(child.parent as RectTransform, fixedEdge, newInset,
                newSize);
        }

        public static void SetInsetAndSizeFromParentEdgeWithCurrentAnchors(this RectTransform child,
            RectTransform parentHint, RectTransform.Edge fixedEdge, float newInset, float newSize)
        {
            _setInsetAndSizeFromParentEdgeWithCurrentAnchorsMappedActions[fixedEdge](child, parentHint, newInset, newSize);
        }
    }
}


