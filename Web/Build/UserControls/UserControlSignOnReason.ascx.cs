using System;
using System.Data;
using System.Web.UI.WebControls;
using SouthNests.Phoenix.Registers;
using Telerik.Web.UI;

public partial class UserControlSignOnReason : System.Web.UI.UserControl
{
    public event EventHandler TextChangedEvent;
    private bool _appenddatabounditems;
    private int _selectedValue = -1;

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            bind();
        }
    }

    public DataSet SignOnReasonList
    {
        set
        {
            ddlSignOnReason.Items.Clear();
            ddlSignOnReason.DataSource = value;
            ddlSignOnReason.DataBind();
        }
    }

    public string CssClass
    {
        set
        {
            ddlSignOnReason.CssClass = value;
        }
    }
	public bool Enabled
	{
		set
		{
			ddlSignOnReason.Enabled = value;
		}
	}
    public bool AutoPostBack
    {
        set
        {
            ddlSignOnReason.AutoPostBack = value;
        }
    }

    public bool AppendDataBoundItems
    {
        set
        {
            _appenddatabounditems = value;
        }
    }

    public string SelectedSignOnReason
    {
        get
        {

            return ddlSignOnReason.SelectedValue;
        }
        set
        {
            ddlSignOnReason.SelectedIndex = -1;
            ddlSignOnReason.Text = "";
            ddlSignOnReason.ClearSelection();
            if (value == string.Empty)
            {                
                ddlSignOnReason.Text = string.Empty;
                return;
            }
            _selectedValue = Int32.Parse(value);            
            foreach (RadComboBoxItem item in ddlSignOnReason.Items)
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
            return (int.Parse(ddlSignOnReason.SelectedValue));
        }
        set
        {
            _selectedValue = value;
            ddlSignOnReason.SelectedIndex = -1;
            foreach (RadComboBoxItem item in ddlSignOnReason.Items)
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
        ddlSignOnReason.DataSource = PhoenixRegistersreasonssignon.Listreasonssignon();
        ddlSignOnReason.DataBind();
        foreach (RadComboBoxItem item in ddlSignOnReason.Items)
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

    protected void ddlSignOnReason_TextChanged(object sender, EventArgs e)
    {
        OnTextChangedEvent(e);
    }

    protected void ddlSignOnReason_DataBound(object sender, EventArgs e)
    {
        if (_appenddatabounditems)
            ddlSignOnReason.Items.Insert(0, new RadComboBoxItem("--Select--", "Dummy"));
    }
    public string Width
    {
        get
        {
            return ddlSignOnReason.Width.ToString();
        }
        set
        {
            ddlSignOnReason.Width = Unit.Parse(value);
        }
    }
}
