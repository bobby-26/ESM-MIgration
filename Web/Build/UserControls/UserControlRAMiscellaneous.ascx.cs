using System;
using System.Data;
using System.Web.UI.WebControls;
using SouthNests.Phoenix.Inspection;
using SouthNests.Phoenix.Framework;
using Telerik.Web.UI;

public partial class UserControlRAMiscellaneous : System.Web.UI.UserControl
{
	private bool _appenddatabounditems;
	private int _selectedValue = -1;
	string _typeid;
    private bool? _includeothers = null;
	protected void Page_Load(object sender, EventArgs e)
	{
		if (!IsPostBack)
			bind();
	}

	public DataSet MiscellaneousList
	{
		set
		{
			ddlMiscellaneous.Items.Clear();
			ddlMiscellaneous.DataSource = value;
			ddlMiscellaneous.DataBind();
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

    public string IncludeOthers
    {
        set
		{
			if (value.ToUpper().Equals("TRUE"))
				_includeothers = true;
			else
				_includeothers = false;
		}
    }

	public string CssClass
	{
		set
		{
			ddlMiscellaneous.CssClass = value;
		}
	}
	public bool Enabled
	{
		set
		{
			ddlMiscellaneous.Enabled = value;
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

	public string SelectedMiscellaneous
	{
		get
		{
			return ddlMiscellaneous.SelectedValue;
		}
		set
		{
			if (value == string.Empty)
			{
				ddlMiscellaneous.SelectedIndex = -1;
                ddlMiscellaneous.ClearSelection();
                ddlMiscellaneous.Text = string.Empty;
                return;
			}
			_selectedValue = Int32.Parse(value);
			ddlMiscellaneous.SelectedIndex = -1;
            foreach (RadComboBoxItem item in ddlMiscellaneous.Items)
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
			return (int.Parse(ddlMiscellaneous.SelectedValue));
		}
		set
		{
			_selectedValue = value;
			ddlMiscellaneous.SelectedIndex = -1;
            foreach (RadComboBoxItem item in ddlMiscellaneous.Items)
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
		ddlMiscellaneous.DataSource = PhoenixInspectionRiskAssessmentMiscellaneous.ListRiskAssessmentMiscellaneous(General.GetNullableInteger(_typeid)
            , General.GetNullableInteger((_includeothers != null && _includeothers == true) ? "1" : "0"));
		ddlMiscellaneous.DataBind();
        foreach (RadComboBoxItem item in ddlMiscellaneous.Items)
		{
			if (item.Value == _selectedValue.ToString())
			{
				item.Selected = true;
				break;
			}
		}
	}
	protected void ddlMiscellaneous_DataBound(object sender, EventArgs e)
	{
		if (_appenddatabounditems)
            ddlMiscellaneous.Items.Insert(0, new RadComboBoxItem("--Select--", "Dummy"));
	}

}
