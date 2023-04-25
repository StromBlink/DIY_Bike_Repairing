using UnityEngine;
using UnityEngine.UI;

public abstract class TTPAbstractViewsHolder
{
    public RectTransform root;
    public int ItemIndex { get; set; }
    public void Init(RectTransform rootPrefab, int itemIndex, bool activateRootGameObject = true, bool callCollectViews = true)
    {
        Init(rootPrefab.gameObject, itemIndex, activateRootGameObject, callCollectViews);
    }

    public void Init(GameObject rootPrefabGO, int itemIndex, bool activateRootGameObject = true,
        bool callCollectViews = true)
    {
        root = (GameObject.Instantiate(rootPrefabGO) as GameObject).transform as RectTransform;
        if (activateRootGameObject)
            root.gameObject.SetActive(true);
        this.ItemIndex = itemIndex;

        if (callCollectViews)
            CollectViews();
    }

    public virtual void CollectViews()
    {
    }

    public void MarkForRebuild()
    {
        if (root) LayoutRebuilder.MarkLayoutForRebuild(root);
    }
}
