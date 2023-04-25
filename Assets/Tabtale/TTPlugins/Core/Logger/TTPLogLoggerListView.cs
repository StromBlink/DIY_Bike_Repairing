using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

namespace Tabtale.TTPlugins
{
	public class TTPLogLoggerListView : TTPLogMessageList<TTPLoggerViewHolder>
	{
		public RectTransform itemPrefab;

		public List<string> Data { get; private set; }

		LayoutElement _PrefabLayoutElement;

		Dictionary<RectTransform, TTPLoggerViewHolder> _MapRootToViewsHolder =
			new Dictionary<RectTransform, TTPLoggerViewHolder>();


		protected override void Awake()
		{
			base.Awake();

			Data = new List<string>();
			_PrefabLayoutElement = itemPrefab.GetComponent<LayoutElement>();
		}

		protected override void Start()
		{
			base.Start();
			Data.AddRange(TTPLogger.GetLogs());
			TTPLogger.onAddMessage = msg =>
			{
				Data.Add(msg);
				InsertItems(Data.Count - 1, 1);
			};
			ResetItems(Data.Count);
		}

		protected override TTPLoggerViewHolder CreateViewsHolder(int itemIndex)
		{
			var instance = new TTPLoggerViewHolder();
			instance.Init(itemPrefab, itemIndex);
			_MapRootToViewsHolder[instance.root] = instance;

			return instance;
		}

		protected override void UpdateViewsHolder(TTPLoggerViewHolder vh)
		{
			vh.UpdateViews(Data[vh.ItemIndex]);
		}

	}
}
