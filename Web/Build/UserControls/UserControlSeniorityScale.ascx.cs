using System;
using System.Data;
using System.Web.UI.WebControls;
using SouthNests.Phoenix.Registers;
using Telerik.Web.UI;
using SouthNests.Phoenix.Framework;
public partial class UserControlSeniorityScale : System.Web.UI.UserControl
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

    public DataTable SeniorityScaleList
    {
        set
        {
            ddlSeniorityScale.Items.Clear();
            ddlSeniorityScale.DataSource = value;
            ddlSeniorityScale.DataBind();
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
        get
        {
            return ddlSeniorityScale.CssClass.ToString();
        }
        set
        {
            ddlSeniorityScale.CssClass = value;
        }
    }


    public bool AutoPostBack
    {
        set
        {
            ddlSeniorityScale.AutoPostBack = value;
        }
    }


    public string SelectedSeniorityScale
    {
        get
        {
            return ddlSeniorityScale.SelectedValue;
        }
        set
        {
            ddlSeniorityScale.SelectedIndex = -1;
            if (value == string.Empty)
            {
                ddlSeniorityScale.ClearSelection();
                ddlSeniorityScale.Text = string.Empty;
                return;
            }
            _selectedValue = Int32.Parse(value);
            ddlSeniorityScale.SelectedIndex = -1;
            foreach (RadComboBoxItem item in ddlSeniorityScale.Items)
            {
                if (item.Value == _selectedValue.ToString())
                {
                    item.Selected = true;
                    break;
                }
            }
        }
    }

    public void bind()
    {
        ddlSeniorityScale.DataSource = PhoenixRegistersSeniorityScale.ListSeniorityWageScale(string.Empty);
        ddlSeniorityScale.DataBind();
        foreach (RadComboBoxItem item in ddlSeniorityScale.Items)
        {
            if (item.Value == _selectedValue.ToString())
            {
                item.Selected = true;
                break;
            }
        }
    }

    public bool Enabled
    {
        set
        {
            ddlSeniorityScale.Enabled = value;
        }
    }

    protected void OnTextChangedEvent(EventArgs e)
    {
        if (TextChangedEvent != null)
            TextChangedEvent(this, e);
    }

    protected void ddlSeniorityScale_TextChanged(object sender, EventArgs e)
    {
        OnTextChangedEvent(e);
    }

    protected void ddlSeniorityScale_DataBound(object sender, EventArgs e)
    {
        if (_appenddatabounditems)
            ddlSeniorityScale.Items.Insert(0, new RadComboBoxItem("--Select--", ""));
    }
    public Unit Width
    {
        get
        {
            return ddlSeniorityScale.Width;
        }
        set
        {
            ddlSeniorityScale.Width = value;
        }
    }
}
