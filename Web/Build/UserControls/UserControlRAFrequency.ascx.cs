using System;
using System.Data;
using System.Web.UI.WebControls;
using SouthNests.Phoenix.Inspection;
using SouthNests.Phoenix.Framework;
using Telerik.Web.UI;

public partial class UserControlRAFrequency : System.Web.UI.UserControl
{
	private bool _appenddatabounditems;
	private int _selectedValue = -1;
	string _typeid;
	protected void Page_Load(object sender, EventArgs e)
	{
		if (!IsPostBack)
			bind();
	}

	public DataSet FrequencyList
	{
		set
		{
			ddlFrequency.Items.Clear();
			ddlFrequency.DataSource = value;
			ddlFrequency.DataBind();
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
			ddlFrequency.CssClass = value;
		}
	}
	public bool Enabled
	{
		set
		{
			ddlFrequency.Enabled = value;
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

	public string SelectedFrequency
	{
		get
		{
			return ddlFrequency.SelectedValue;
		}
		set
		{
			if (value == string.Empty)
			{
				ddlFrequency.SelectedIndex = -1;
                ddlFrequency.ClearSelection();
                ddlFrequency.Text = string.Empty;
                return;
			}
			_selectedValue = Int32.Parse(value);
			ddlFrequency.SelectedIndex = -1;
            foreach (RadComboBoxItem item in ddlFrequency.Items)
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
			return (int.Parse(ddlFrequency.SelectedValue));
		}
		set
		{
			_selectedValue = value;
			ddlFrequency.SelectedIndex = -1;
            foreach (RadComboBoxItem item in ddlFrequency.Items)
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
		ddlFrequency.DataSource = PhoenixInspectionRiskAssessmentFrequency.ListRiskAssessmentFrequency(Convert.ToInt32(_typeid));
		ddlFrequency.DataBind();
        foreach (RadComboBoxItem item in ddlFrequency.Items)
		{
			if (item.Value == _selectedValue.ToString())
			{
				item.Selected = true;
				break;
			}
		}
	}
	protected void ddlFrequency_DataBound(object sender, EventArgs e)
	{
		if (_appenddatabounditems)
            ddlFrequency.Items.Insert(0, new RadComboBoxItem("--Select--", "Dummy"));
	}

}
