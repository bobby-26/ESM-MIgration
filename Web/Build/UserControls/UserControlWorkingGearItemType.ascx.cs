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
public partial class UserControlWorkingGearItemType : System.Web.UI.UserControl
{
    public event EventHandler TextChangedEvent;
    private bool _appenddatabounditems;
    //private int _selectedValue = -1;
    private string _selectedValue = string.Empty;
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
            ddlGearItemType.Items.Clear();
            ddlGearItemType.DataSource = value;
            ddlGearItemType.DataBind();
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
            ddlGearItemType.CssClass = value;
        }
    }

    public string AutoPostBack
    {
        set
        {
            if (value.ToUpper().Equals("TRUE"))
                ddlGearItemType.AutoPostBack = true;
        }
    }

    protected void OnTextChangedEvent(EventArgs e)
    {
        if (TextChangedEvent != null)
            TextChangedEvent(this, e);
    }

    protected void ddlGearItemType_TextChanged(object sender, EventArgs e)
    {
        OnTextChangedEvent(e);
    }

    public string SelectedGearType
    {
        get
        {
            return ddlGearItemType.SelectedValue;
        }
        set
        {
            if (value == string.Empty)
            {
                ddlGearItemType.SelectedIndex = -1;
                ddlGearItemType.ClearSelection();
                ddlGearItemType.Text = string.Empty;
                return;
            }
            _selectedValue = value.ToString();
            ddlGearItemType.SelectedIndex = -1;
            foreach (RadComboBoxItem item in ddlGearItemType.Items)
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
        ddlGearItemType.DataSource = PhoenixRegistersWorkingGearItemType.ListWorkingGearType(null);
        ddlGearItemType.DataBind();
        foreach (RadComboBoxItem item in ddlGearItemType.Items)
        {
            if (item.Value == _selectedValue.ToString())
            {
                item.Selected = true;
                break;
            }
        }
    }
    protected void ddlGearItemType_DataBound(object sender, EventArgs e)
    {
        if (_appenddatabounditems)
            ddlGearItemType.Items.Insert(0, new RadComboBoxItem("--Select--", "Dummy"));
    } 
}
