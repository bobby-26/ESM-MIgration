using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.Registers;
using System;
using System.Data;
using System.Web.UI.WebControls;
using Telerik.Web.UI;

public partial class UserControlQuick : System.Web.UI.UserControl
{
    int _quicktypecode;
    public event EventHandler TextChangedEvent;
    private bool _appenddatabounditems;
    private int _selectedValue = -1;
    private string _selectedText = "";

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            bind();
            lnkQuickEdit.Visible = false;
            if (PhoenixSecurityContext.CurrentSecurityContext != null && PhoenixSecurityContext.CurrentSecurityContext.UserCode == 1)
            {
                lnkQuickEdit.Visible = true;
            }
        }
    }

    public string QuickTypeCode
    {
        get
        {
            return _quicktypecode.ToString();
        }
        set
        {
            _quicktypecode = int.Parse(value);
            lnkQuickEdit.Attributes["onclick"] = "javascript:top.openNewWindow('quick','Quick','Registers/RegistersQuick.aspx?quickcodetype=" + value + "'); return false;";
        }
    }

    public DataSet QuickList
    {
        set
        {
            ddlQuick.Items.Clear();
            ddlQuick.DataSource = value;
            ddlQuick.DataBind();
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
            ddlQuick.CssClass = value;
        }
    }

    public RadComboBoxExpandDirection ExpandDirection
    {
        get
        {
            return ddlQuick.ExpandDirection;
        }
        set
        {
            ddlQuick.ExpandDirection = (RadComboBoxExpandDirection)value;
        }
    }
    public string Width
    {
        get
        {
            return ddlQuick.Width.ToString();
        }
        set
        {
            ddlQuick.Width = Unit.Parse(value);
        }
    }

    public string AutoPostBack
    {
        set
        {
            if (value.ToUpper().Equals("TRUE"))
                ddlQuick.AutoPostBack = true;
        }
    }


    public string SelectedQuick
    {
        get
        {

            return ddlQuick.SelectedValue;
        }
        set
        {
            
            ddlQuick.SelectedIndex = -1;
            ddlQuick.Text = "";
            ddlQuick.ClearSelection();

            if (value == string.Empty)
            {
                ddlQuick.Text = string.Empty;
                return;
            }
            _selectedValue = Int32.Parse(value);
            ddlQuick.SelectedIndex = -1;
            foreach (RadComboBoxItem item in ddlQuick.Items)
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
        ddlQuick.DataSource = PhoenixRegistersQuick.ListQuick(1, _quicktypecode);
        ddlQuick.DataBind();
        foreach (RadComboBoxItem item in ddlQuick.Items)
        {
            if (item.Value == _selectedValue.ToString())
            {
                item.Selected = true;
                break;
            }
        }

        if (_selectedText != "")
        {
            foreach (RadComboBoxItem item in ddlQuick.Items)
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

            return ddlQuick.SelectedValue;
        }
        set
        {
            _selectedValue = Int32.Parse(value);
            ddlQuick.SelectedIndex = -1;
            foreach (RadComboBoxItem item in ddlQuick.Items)
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
            return ddlQuick.SelectedItem.Text;
        }
        set
        {
            _selectedText = value;
            ddlQuick.SelectedIndex = -1;
            foreach (RadComboBoxItem item in ddlQuick.Items)
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
            ddlQuick.Enabled = value;
        }
    }

    protected void OnTextChangedEvent(EventArgs e)
    {
        if (TextChangedEvent != null)
            TextChangedEvent(this, e);
    }

    protected void ddlQuick_TextChanged(object sender, EventArgs e)
    {
        OnTextChangedEvent(e);
    }

    protected void ddlQuick_DataBound(object sender, EventArgs e)
    {
        if (_appenddatabounditems)
            ddlQuick.Items.Insert(0, new RadComboBoxItem("--Select--", "Dummy"));
    }

    public bool ShowToggleImage
    {
        set
        {
            ddlQuick.ShowToggleImage = value;

        }
    }

    public bool ShowDropDownOnTextboxClick
    {
        set
        {
            ddlQuick.ShowDropDownOnTextboxClick = value;
        }
    }
}
