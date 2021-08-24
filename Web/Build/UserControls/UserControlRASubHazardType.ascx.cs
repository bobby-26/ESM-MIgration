using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.Registers;
using SouthNests.Phoenix.Inspection;
using Telerik.Web.UI;
public partial class UserControlRASubHazardType : System.Web.UI.UserControl
{
	public event EventHandler TextChangedEvent;
	//private bool _appenddatabounditems;
	private string _selectedValue = string.Empty;
	string mainhazardid = null;

	protected void Page_Load(object sender, EventArgs e)
	{
		if (!IsPostBack)
		{
			bind();
		}
	}

	public DataSet SubHazardList
	{
		set
		{
			cblSubHazardType.Items.Clear();
			cblSubHazardType.DataSource = value;
			cblSubHazardType.DataBind();
		}
	}

	public string CssClass
	{
		set
		{
			cblSubHazardType.CssClass = value;
		}
	}

	public string Width
	{
		get
		{
			return cblSubHazardType.Width.ToString();
		}
		set
		{
			cblSubHazardType.Width = Unit.Parse(value);
		}
	}

	public string AutoPostBack
	{
		set
		{
			if (value.ToUpper().Equals("TRUE"))
				cblSubHazardType.AutoPostBack = true;
		}
	}


	public string AppendDataBoundItems
	{
		set
		{
			//if (value.ToUpper().Equals("TRUE"))
			//	_appenddatabounditems = true;
			//else
			//	_appenddatabounditems = false;

		}
	}

	public string SelectedSubHazard
	{
		get
		{

			return cblSubHazardType.SelectedValue;
		}
		set
		{
			if (value == string.Empty)
			{
				cblSubHazardType.SelectedIndex = -1;
				return;
			}
			_selectedValue = value.ToString();
			cblSubHazardType.SelectedIndex = -1;
			foreach (RadListBoxItem item in cblSubHazardType.Items)
			{
				if (item.Value == _selectedValue.ToString())
				{
					item.Checked = true;
					break;
				}
			}
		}
	}

	public string SelectedValue
	{
		get
		{
			return cblSubHazardType.SelectedValue;
		}
		set
		{
			_selectedValue = value;
			cblSubHazardType.SelectedIndex = -1;
			foreach (RadListBoxItem item in cblSubHazardType.Items)
			{
				if (item.Value == _selectedValue.ToString())
				{
					item.Checked = true;
					break;
				}
			}
		}
	}
	private void bind()
	{
		cblSubHazardType.SelectedIndex = -1;
		cblSubHazardType.DataSource = PhoenixInspectionRiskAssessmentSubHazard.ListRiskAssessmentSubHazard(General.GetNullableInteger(mainhazardid));
		cblSubHazardType.DataBind();
		foreach (RadListBoxItem item in cblSubHazardType.Items)
		{
			if (item.Value == _selectedValue.ToString())
			{
				item.Checked = true;
				break;
			}
		}
	}
	protected void OnTextChangedEvent(EventArgs e)
	{
		if (TextChangedEvent != null)
			TextChangedEvent(this, e);
	}

	protected void cblSubHazardType_TextChanged(object sender, EventArgs e)
	{
		OnTextChangedEvent(e);
	}

	protected void cblSubHazardType_DataBound(object sender, EventArgs e)
	{
		//if (_appenddatabounditems)
		//	cblSubHazardType.Items.Insert(0, new ListItem("--Select--", "Dummy"));
	}
}
