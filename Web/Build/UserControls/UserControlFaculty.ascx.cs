using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.Registers;
using SouthNests.Phoenix.CrewManagement;
using Telerik.Web.UI;
public partial class UserControlFaculty : System.Web.UI.UserControl
{
	public event EventHandler TextChangedEvent;
	private bool _appenddatabounditems;
	private int _selectedValue = -1;
	int courseid;
	protected void Page_Load(object sender, EventArgs e)
	{
		if (!IsPostBack)
		{
			bind();
		}
	}
	public string CourseId
	{
		get
		{
			return courseid.ToString();
		}
		set
		{
			courseid = Int32.Parse(value);
		}
	}
	public DataTable FacultyList
	{
		set
		{
			ddlFaculty.DataSource = value;
			ddlFaculty.DataBind();
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

	public string CssClass
	{
		set
		{
			ddlFaculty.CssClass = value;
		}
	}

	public string AutoPostBack
	{
		set
		{
			if (value.ToUpper().Equals("TRUE"))
				ddlFaculty.AutoPostBack = true;
		}
	}
	public bool Enabled
	{
		set
		{
			ddlFaculty.Enabled = value;
		}
	}
	protected void OnTextChangedEvent(EventArgs e)
	{
		if (TextChangedEvent != null)
			TextChangedEvent(this, e);
	}

	protected void ddlFaculty_TextChanged(object sender, EventArgs e)
	{
		OnTextChangedEvent(e);
	}

	public string SelectedFaculty
	{
		get
		{
			return ddlFaculty.SelectedValue;
		}
		set
		{
			if (value == string.Empty)
			{
				ddlFaculty.SelectedIndex = -1;
                ddlFaculty.ClearSelection();
                ddlFaculty.Text = string.Empty;
                return;
			}
			_selectedValue = Int32.Parse(value);
			ddlFaculty.SelectedIndex = -1;
			foreach (RadComboBoxItem item in ddlFaculty.Items)
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
		ddlFaculty.DataSource = PhoenixRegistersFaculty.ListFaculty(Convert.ToInt32(CourseId));
		ddlFaculty.DataBind();
		foreach (RadComboBoxItem item in ddlFaculty.Items)
		{
			if (item.Value == _selectedValue.ToString())
			{
				item.Selected = true;
				break;
			}
		}
	}
	protected void ddlFaculty_DataBound(object sender, EventArgs e)
	{
		if (_appenddatabounditems)
			ddlFaculty.Items.Insert(0, new RadComboBoxItem("--Select--", "Dummy"));
	}
}
