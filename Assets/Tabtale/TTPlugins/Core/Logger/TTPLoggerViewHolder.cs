using UnityEngine.UI;

namespace Tabtale.TTPlugins
{
	public class TTPLoggerViewHolder : TTPAbstractViewsHolder
	{
		public LayoutElement layoutElement;
		public Text nameText;

		public override void CollectViews()
		{
			base.CollectViews();
			layoutElement = root.GetComponent<LayoutElement>();
			var mainPanel = root.GetChild(0);
			nameText = mainPanel.Find("LogMessagePanel/MessageText").GetComponent<Text>();
		}

		public void UpdateViews(string dataModel)
		{
			nameText.text = dataModel;
		}
	}
}


