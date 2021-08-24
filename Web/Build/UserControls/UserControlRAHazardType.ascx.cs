using System;
using System.Data;
using System.Web.UI.WebControls;
using SouthNests.Phoenix.Inspection;
using SouthNests.Phoenix.Framework;
using Telerik.Web.UI;

public partial class UserControlRAHazardType : System.Web.UI.UserControl
{
	public event EventHandler TextChangedEvent;
	private bool _appenddatabounditems;
	private int _selectedValue = -1;
	string _typeid;
	protected void Page_Load(object sender, EventArgs e)
	{
		if (!IsPostBack)
			bind();
	}

	public DataSet HazardTypeList
	{
		set
		{
			ddlHazardType.Items.Clear();
			ddlHazardType.DataSource = value;
			ddlHazardType.DataBind();
		}
	}
	public string Type
	{
		get
		{
			return _typeid.ToString();
		}
		set
		{
			_typeid = value;
		}
	}

	public string CssClass
	{
		set
		{
			ddlHazardType.CssClass = value;
		}
	}
	public bool Enabled
	{
		set
		{
			ddlHazardType.Enabled = value;
		}
	}

	public bool AutoPostBack
	{
		set
		{
			ddlHazardType.AutoPostBack = value;
		}
	}

	public string AppendDataBoundItems
	{
		set
		{
			if (value.ToUpper().Equals("TRUE"))
				_appenddatabounditems = true;
			else
				_appenddatabounditems = false;

		}
	}

	public string SelectedHazardType
	{
		get
		{
			return ddlHazardType.SelectedValue;
		}
		set
		{
			if (value == string.Empty)
			{
				ddlHazardType.SelectedIndex = -1;
                ddlHazardType.ClearSelection();
                ddlHazardType.Text = string.Empty;
                return;
			}
			_selectedValue = Int32.Parse(value);
			ddlHazardType.SelectedIndex = -1;
            foreach (RadComboBoxItem item in ddlHazardType.Items)
			{
				if (item.Value == _selectedValue.ToString())
				{
					item.Selected = true;
					break;
				}
			}
		}
	}

	public int SelectedValue
	{
		get
		{
			return (int.Parse(ddlHazardType.SelectedValue));
		}
		set
		{
			_selectedValue = value;
			ddlHazardType.SelectedIndex = -1;
            foreach (RadComboBoxItem item in ddlHazardType.Items)
			{
				if (item.Value == _selectedValue.ToString())
				{
					item.Selected = true;
					break;
				}
			}
		}
	}
	private void bind()
	{
		ddlHazardType.DataSource = PhoenixInspectionRiskAssessmentHazard.ListRiskAssessmentHazard(Convert.ToInt32(_typeid),0);
		ddlHazardType.DataBind();
        foreach (RadComboBoxItem item in ddlHazardType.Items)
		{
			if (item.Value == _selectedValue.ToString())
			{
				item.Selected = true;
				break;
			}
		}
	}
	protected void ddlHazardType_DataBound(object sender, EventArgs e)
	{
		if (_appenddatabounditems)
            ddlHazardType.Items.Insert(0, new RadComboBoxItem("--Select--", "Dummy"));
	}
	protected void OnTextChangedEvent(EventArgs e)
	{
		if (TextChangedEvent != null)
			TextChangedEvent(this, e);
	}

	protected void ddlHazardType_TextChanged(object sender, EventArgs e)
	{
		OnTextChangedEvent(e);
	}

    public string Width
    {
        get
        {
            return ddlHazardType.Width.ToString();
        }
        set
        {
            ddlHazardType.Width = Unit.Parse(value);
        }
    }

}
