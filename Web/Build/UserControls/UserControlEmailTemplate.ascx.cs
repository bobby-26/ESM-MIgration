using System;
using System.Data;
using System.Web.UI.WebControls;
using SouthNests.Phoenix.Framework;
using Telerik.Web.UI;
public partial class UserControlEmailTemplate : System.Web.UI.UserControl
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

    public DataSet TemplateList
    {
        set
        {
            ddlEmailTemplate.Items.Clear();
            ddlEmailTemplate.DataSource = value;
            ddlEmailTemplate.DataBind();
        }
    }

    public int? EmailType
    {
        get;
        set;
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
            ddlEmailTemplate.CssClass = value;
        }
    }

    public bool Enabled
    {
        set
        {
            ddlEmailTemplate.Enabled = value;
        }
    }

    public bool AutoPostBack
    {
        set
        {
            ddlEmailTemplate.AutoPostBack = value;
        }
    }

    protected void OnTextChangedEvent(EventArgs e)
    {
        if (TextChangedEvent != null)
            TextChangedEvent(this, e);
    }

    protected void ddlEmailTemplate_TextChanged(object sender, EventArgs e)
    {
        OnTextChangedEvent(e);
    }

    public string SelectedTemplate
    {
        get
        {
            return ddlEmailTemplate.SelectedValue;
        }
        set
        {
            if (value == string.Empty)
            {
                ddlEmailTemplate.SelectedIndex = -1;
                ddlEmailTemplate.ClearSelection();
                ddlEmailTemplate.Text = string.Empty;
                return;
            }
            _selectedValue = Int32.Parse(value);
            ddlEmailTemplate.SelectedIndex = -1;
            foreach (RadComboBoxItem item in ddlEmailTemplate.Items)
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
        ddlEmailTemplate.DataSource = PhoenixCommonEmailTemplate.ListEmailTemplate(EmailType, null);
        ddlEmailTemplate.DataBind();
        foreach (RadComboBoxItem item in ddlEmailTemplate.Items)
        {
            if (item.Value == _selectedValue.ToString())
            {
                item.Selected = true;
                break;
            }
        }
    }
    protected void ddlEmailTemplate_DataBound(object sender, EventArgs e) 
    {
        if(_appenddatabounditems)
            ddlEmailTemplate.Items.Insert(0, new RadComboBoxItem("--Select--", ""));
    } 
}
