using System;
using System.Data;
using System.Web.UI.WebControls;
using SouthNests.Phoenix.Accounts;
using Telerik.Web.UI;

public partial class UserControls_UserControlProjectCode : System.Web.UI.UserControl
{
    string _projectcode;
    public event EventHandler TextChangedEvent;
    private bool _appenddatabounditems;
    private int _selectedValue = -1;
    private string _selectedText = "";

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
          //  bind(null,null);
        }
    }

    public string ProjectCode
    {
        get
        {
            return _projectcode.ToString();
        }
        set
        {
            _projectcode = (value);
        }
    }

    public DataSet ProjectCodeList
    {
        set
        {
            ddlProjectCode.Items.Clear();
            ddlProjectCode.DataSource = value;
            ddlProjectCode.DataBind();
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
            ddlProjectCode.CssClass = value;
        }
    }

    public string Width
    {
        get
        {
            return ddlProjectCode.Width.ToString();
        }
        set
        {
            ddlProjectCode.Width = Unit.Parse(value);
        }
    }

    public string AutoPostBack
    {
        set
        {
            if (value.ToUpper().Equals("TRUE"))
                ddlProjectCode.AutoPostBack = true;
        }
    }


    public string SelectedProjectCode
    {
        get
        {

            return ddlProjectCode.SelectedValue;
        }
        set
        {

            ddlProjectCode.SelectedIndex = -1;
            ddlProjectCode.Text = "";
            ddlProjectCode.ClearSelection();

            if (value == string.Empty)
            {
                ddlProjectCode.Text = string.Empty;
                return;
            }
            _selectedValue = Int32.Parse(value);
            ddlProjectCode.SelectedIndex = -1;
            foreach (RadComboBoxItem item in ddlProjectCode.Items)
            {
                if (item.Value == _selectedValue.ToString())
                {
                    item.Selected = true;
                    break;
                }
            }
        }
    }

    public void bind(int? accountid,int? budgetid)
    {
        ddlProjectCode.DataSource = PhoenixAccountProject.ListProjectCode(1, _projectcode, accountid, budgetid);
        ddlProjectCode.DataBind();
        foreach (RadComboBoxItem item in ddlProjectCode.Items)
        {
            if (item.Value == _selectedValue.ToString())
            {
                item.Selected = true;
                break;
            }
        }

        if (_selectedText != "")
        {
            foreach (RadComboBoxItem item in ddlProjectCode.Items)
            {
                if (item.Text == _selectedText)
                {
                    item.Selected = true;
                    break;
                }
            }
        }
    }

    public string SelectedValue
    {
        get
        {
            return ddlProjectCode.SelectedValue;
        }
        set
        {
            _selectedValue = Int32.Parse(value);
            ddlProjectCode.SelectedIndex = -1;
            foreach (RadComboBoxItem item in ddlProjectCode.Items)
            {
                if (item.Value == _selectedValue.ToString())
                {
                    item.Selected = true;
                    break;
                }
            }
        }
    }

    public string SelectedText
    {
        get
        {
            return ddlProjectCode.SelectedItem.Text;
        }
        set
        {
            _selectedText = value;
            ddlProjectCode.SelectedIndex = -1;
            foreach (RadComboBoxItem item in ddlProjectCode.Items)
            {
                if (item.Text == _selectedText.ToString())
                {
                    item.Selected = true;
                    break;
                }
            }
        }
    }
    public bool Enabled
    {
        set
        {
            ddlProjectCode.Enabled = value;
        }
    }

    protected void OnTextChangedEvent(EventArgs e)
    {
        if (TextChangedEvent != null)
            TextChangedEvent(this, e);
    }

    protected void ddlProjectCode_TextChanged(object sender, EventArgs e)
    {
        OnTextChangedEvent(e);
    }

    protected void ddlProjectCode_DataBound(object sender, EventArgs e)
    {
        if (_appenddatabounditems)
            ddlProjectCode.Items.Insert(0, new RadComboBoxItem("--Select--", "Dummy"));
    }
}
