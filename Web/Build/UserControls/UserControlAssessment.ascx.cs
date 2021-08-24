using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.Registers;
using Telerik.Web.UI;
public partial class UserControlAssessment : System.Web.UI.UserControl
{
	private bool _appenddatabounditems;
	private int _selectedValue = -1;

	protected void Page_Load(object sender, EventArgs e)
	{
		if (!IsPostBack)
			bind();
	}

	public DataSet AssessmentList
	{
		set
		{
			ddlAssessment.Items.Clear();
			ddlAssessment.DataSource = value;
			ddlAssessment.DataBind();
		}
	}
	public string Country
	{
		get;
		set;
	}
	public string CssClass
	{
		set
		{
			ddlAssessment.CssClass = value;
		}
	}
	public bool Enabled
	{
		set
		{
			ddlAssessment.Enabled = value;
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

	public string SelectedAssessment
	{
		get
		{
			return ddlAssessment.SelectedValue;
		}
		set
		{
			if (value == string.Empty)
			{
				ddlAssessment.SelectedIndex = -1;
                ddlAssessment.ClearSelection();
                ddlAssessment.Text = string.Empty;
                return;
			}
			_selectedValue = Int32.Parse(value);
			ddlAssessment.SelectedIndex = -1;
			foreach (RadComboBoxItem item in ddlAssessment.Items)
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
			return (int.Parse(ddlAssessment.SelectedValue));
		}
		set
		{
			_selectedValue = value;
			ddlAssessment.SelectedIndex = -1;
			foreach (RadComboBoxItem item in ddlAssessment.Items)
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
		ddlAssessment.DataSource = PhoenixRegistersAssessment.ListAssessment();
		ddlAssessment.DataBind();
		foreach (RadComboBoxItem item in ddlAssessment.Items)
		{
			if (item.Value == _selectedValue.ToString())
			{
				item.Selected = true;
				break;
			}
		}
	}
	protected void ddlAssessment_DataBound(object sender, EventArgs e)
	{
		if (_appenddatabounditems)
			ddlAssessment.Items.Insert(0, new RadComboBoxItem("--Select--","Dummy"));
	}

}
