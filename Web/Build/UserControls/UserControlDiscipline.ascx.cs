using System;
using System.Web.UI.WebControls;
using System.Data;
using SouthNests.Phoenix.Registers;
using Telerik.Web.UI;
using System.Web.UI;

[ValidationPropertyAttribute("SelectedDiscipline")]
public partial class UserControlDiscipline : System.Web.UI.UserControl
{
    private bool _appenddatabounditems;
    public event EventHandler TextChangedEvent;
    private int _selectedValue = -1;

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            bind();
        }
    }

    public DataSet DisciplineList
    {
        set
        {
            ddlDiscipline.Items.Clear();
            ddlDiscipline.DataSource = value;
            ddlDiscipline.DataBind();
        }
    }
	public bool Enabled
	{
		set
		{
			ddlDiscipline.Enabled = value;
		}
	}
    public bool AppendDataBoundItems
    {
        set
        {
            _appenddatabounditems = value;
        }
    }

    public string CssClass
    {
        set
        {
            ddlDiscipline.CssClass = value;
        }
    }

    public bool AutoPostBack
    {
        set
        {
            ddlDiscipline.AutoPostBack = value;
        }
    }

    public string SelectedDiscipline
    {
        get
        {
            return ddlDiscipline.SelectedValue;
        }
        set
        {
            if (value == string.Empty)
            {
                ddlDiscipline.SelectedIndex = -1;
                ddlDiscipline.ClearSelection();
                ddlDiscipline.Text = string.Empty;
                return;
            }
            _selectedValue = Int32.Parse(value);
            ddlDiscipline.SelectedIndex = -1;
            foreach (RadComboBoxItem item in ddlDiscipline.Items)
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

            return (int.Parse(ddlDiscipline.SelectedValue));
        }
        set
        {
            _selectedValue = value;
            ddlDiscipline.SelectedIndex = -1;
            foreach (RadComboBoxItem item in ddlDiscipline.Items)
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
        ddlDiscipline.DataSource = PhoenixRegistersDiscipline.ListDiscipline();
        ddlDiscipline.DataBind();
        foreach (RadComboBoxItem item in ddlDiscipline.Items)
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

    protected void ddlDiscipline_TextChanged(object sender, EventArgs e)
    {
        OnTextChangedEvent(e);
    }

    protected void ddlDiscipline_DataBound(object sender, EventArgs e)
    {
        if (_appenddatabounditems)
            ddlDiscipline.Items.Insert(0, new RadComboBoxItem("--Select--", "Dummy"));
    }
    public Unit Width
    {
        set
        {
            ddlDiscipline.Width = Unit.Parse(value.ToString());
        }
        get
        {
            return ddlDiscipline.Width;
        }
    }
}
