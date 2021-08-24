using System;
using System.Data;
using System.Web.UI.WebControls;
using SouthNests.Phoenix.Registers;
using Telerik.Web.UI;
public partial class UserControlCourseCode : System.Web.UI.UserControl
{
	public event EventHandler TextChangedEvent;
	private bool _appenddatabounditems;
	private bool listcbtcourse = true;
	private int? documenttype = null;
	private int _selectedValue = -1;

	protected void Page_Load(object sender, EventArgs e)
	{
		if (!IsPostBack)
		{
			bind();
		}
	}
	public int? DocumentType
	{
		get
		{
			return documenttype;
		}
		set
		{
			documenttype = value;
		}
	}

	public bool ListCBTCourse
	{
		get
		{
			return listcbtcourse;
		}
		set
		{
			listcbtcourse = value;
		}
	}

	public DataSet CourseList
	{
		set
		{
			ddlCourse.DataSource = value;
			ddlCourse.DataBind();
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
			ddlCourse.CssClass = value;
		}
	}

	public string AutoPostBack
	{
		set
		{
			if (value.ToUpper().Equals("TRUE"))
				ddlCourse.AutoPostBack = true;
		}
	}
	public void bind()
	{
		if (listcbtcourse)
		{
			ddlCourse.DataSource = PhoenixRegistersDocumentCourse.ListDocumentCourse(documenttype);
			ddlCourse.DataBind();
		}
		else
		{
			ddlCourse.DataSource = PhoenixRegistersDocumentCourse.ListNonCBTDocumentCourse();
			ddlCourse.DataBind();
		}
		foreach (RadComboBoxItem item in ddlCourse.Items)
		{
			if (item.Value == _selectedValue.ToString())
			{
				item.Selected = true;
				break;
			}
		}
	}
	protected void OnTextChangedEvent(EventArgs e)
	{
		if (TextChangedEvent != null)
			TextChangedEvent(this, e);
	}

	protected void ddlCourse_TextChanged(object sender, EventArgs e)
	{
		OnTextChangedEvent(e);
	}

	public string SelectedCourse
	{
		get
		{
			return ddlCourse.SelectedValue;
		}
		set
		{
			if (value == string.Empty)
			{
				ddlCourse.SelectedIndex = -1;
                ddlCourse.ClearSelection();
                ddlCourse.Text = string.Empty;
                return;
			}
			_selectedValue = Int32.Parse(value);
			ddlCourse.SelectedIndex = -1;
			foreach (RadComboBoxItem item in ddlCourse.Items)
			{
				if (item.Value == _selectedValue.ToString())
				{
					item.Selected = true;
					break;
				}
			}
		}

	}

	protected void ddlCourse_DataBound(object sender, EventArgs e)
	{
		if (_appenddatabounditems)
			ddlCourse.Items.Insert(0, new RadComboBoxItem("--Select--", "Dummy"));
	}
}
