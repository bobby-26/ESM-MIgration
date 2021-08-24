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
public partial class UserControlWorkingGearType : System.Web.UI.UserControl
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

    public DataSet TypeList
    {
        set
        {
            ddlGearType.Items.Clear();
            ddlGearType.DataSource = value;
            ddlGearType.DataBind();
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
            ddlGearType.CssClass = value;
        }
    }

    public string AutoPostBack
    {
        set
        {
            if (value.ToUpper().Equals("TRUE"))
                ddlGearType.AutoPostBack = true;
        }
    }

    protected void OnTextChangedEvent(EventArgs e)
    {
        if (TextChangedEvent != null)
            TextChangedEvent(this, e);
    }

    protected void ddlGearType_TextChanged(object sender, EventArgs e)
    {
        OnTextChangedEvent(e);
    }

    public string SelectedGearType
    {
        get
        {
            return ddlGearType.SelectedValue;
        }
        set
        {
            if (value == string.Empty)
            {
                ddlGearType.SelectedIndex = -1;
                ddlGearType.ClearSelection();
                ddlGearType.Text = string.Empty;
                return;
            }
            _selectedValue = Int32.Parse(value);
            ddlGearType.SelectedIndex = -1;
            foreach (RadComboBoxItem item in ddlGearType.Items)
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
        ddlGearType.DataSource = PhoenixRegistersWorkingGearType.ListWorkingGearType(null);
        ddlGearType.DataBind();
        foreach (RadComboBoxItem item in ddlGearType.Items)
        {
            if (item.Value == _selectedValue.ToString())
            {
                item.Selected = true;
                break;
            }
        }
    }
    protected void ddlGearType_DataBound(object sender, EventArgs e)
    {
        if (_appenddatabounditems)
            ddlGearType.Items.Insert(0, new RadComboBoxItem("--Select--", "Dummy"));
    } 
}
