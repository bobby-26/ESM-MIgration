using System;
using System.Data;
using System.Web.UI.WebControls;
using SouthNests.Phoenix.Registers;
using Telerik.Web.UI;

public partial class UserControlHistoryTemplate : System.Web.UI.UserControl
{
    
    public delegate void TextChangedDelegate(object o, EventArgs e);
    public event TextChangedDelegate TextChangedEvent;
    private bool _appenddatabounditems;
    private int _selectedValue = -1;

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            bind();
        }
    }

    public DataTable QuickList
    {
        set
        {
            ddlHistoryTemplate.Items.Clear();
            ddlHistoryTemplate.DataSource = value;
            ddlHistoryTemplate.DataBind();
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
            ddlHistoryTemplate.CssClass = value;
        }
    }


    public bool AutoPostBack
    {
        set
        {
            ddlHistoryTemplate.AutoPostBack = value;
        }
    }


    public string SelectedHistoryTemplate
    {
        get
        {

            return ddlHistoryTemplate.SelectedValue;
        }
        set
        {
            if (value == string.Empty)
            {
                ddlHistoryTemplate.SelectedIndex = -1;
                ddlHistoryTemplate.ClearSelection();
                ddlHistoryTemplate.Text = string.Empty;
                return;
            }
            _selectedValue = Int32.Parse(value);
            ddlHistoryTemplate.SelectedIndex = -1;
            foreach (RadComboBoxItem item in ddlHistoryTemplate.Items)
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
        ddlHistoryTemplate.DataSource = PhoenixRegistersHistoryTemplate.ListHistoryTemplate();
        ddlHistoryTemplate.DataBind();
        foreach (RadComboBoxItem item in ddlHistoryTemplate.Items)
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
            ddlHistoryTemplate.Enabled = value;
        }
    }
   
    protected void OnHistoryTemplateTextChangedEvent(EventArgs e)
    {
        if (TextChangedEvent != null)
            TextChangedEvent(this, e);
    }

    protected void ddlHistoryTemplate_TextChanged(object sender, EventArgs e)
    {
        OnHistoryTemplateTextChangedEvent(e);
    }

    protected void ddlHistoryTemplate_DataBound(object sender, EventArgs e)
    {
        if (_appenddatabounditems)
            ddlHistoryTemplate.Items.Insert(0, new RadComboBoxItem("--Select--", ""));
    } 
}
