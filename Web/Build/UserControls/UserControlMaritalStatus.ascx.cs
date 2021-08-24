using System;
using System.Collections;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Xml.Linq;
using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.Registers;
using Telerik.Web.UI;
public partial class UserControlMaritalStatus : System.Web.UI.UserControl
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

    public DataSet MaritalStatusList
    {
        set
        {
            ddlMaritalStatus.DataSource = value;
            ddlMaritalStatus.DataBind();
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
            ddlMaritalStatus.CssClass = value;
        }
    }


    public string AutoPostBack
    {
        set
        {
            if (value.ToUpper().Equals("TRUE"))
                ddlMaritalStatus.AutoPostBack = true;
        }
    }

    protected void OnTextChangedEvent(EventArgs e)
    {
        if (TextChangedEvent != null)
            TextChangedEvent(this, e);
    }

    protected void ddlMaritalStatus_TextChanged(object sender, EventArgs e)
    {
        OnTextChangedEvent(e);
    }

    public string SelectedMaritalStatus
    {
        get
        {
            return ddlMaritalStatus.SelectedValue;
        }
        set
        {
            if (value == string.Empty)
            {
                ddlMaritalStatus.SelectedIndex = -1;
                ddlMaritalStatus.ClearSelection();
                ddlMaritalStatus.Text = string.Empty;
                return;
            }
            _selectedValue = Int32.Parse(value);
            ddlMaritalStatus.SelectedIndex = -1;
            foreach (RadComboBoxItem item in ddlMaritalStatus.Items)
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
        ddlMaritalStatus.DataSource = PhoenixRegistersMaritalStatus.ListMaritalStatus();
        ddlMaritalStatus.DataBind();
        foreach (RadComboBoxItem item in ddlMaritalStatus.Items)
        {
            if (item.Value == _selectedValue.ToString())
            {
                item.Selected = true;
                break;
            }
        }
    }
    public string SelectedMaritalStatusName
    {
        get
        {
            return ddlMaritalStatus.SelectedItem.Text;
        }
    }

    protected void ddlMaritalStatus_DataBound(object sender, EventArgs e)
    {
        if (_appenddatabounditems)
            ddlMaritalStatus.Items.Insert(0, new RadComboBoxItem("--Select--", "Dummy"));
    }

    public string Width
    {
        get
        {
            return ddlMaritalStatus.Width.ToString();
        }
        set
        {
            ddlMaritalStatus.Width = Unit.Parse(value);
        }
    }
}
