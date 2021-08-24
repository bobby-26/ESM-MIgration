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
using SouthNests.Phoenix.Registers;
using Telerik.Web.UI;
public partial class UserControlSetDetails : System.Web.UI.UserControl
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

    public DataSet SetDetailsList
    {
        set
        {
            ddlSetDetails.DataSource = value;
            ddlSetDetails.DataBind();
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
            ddlSetDetails.CssClass = value;
        }
    }

    public string AutoPostBack
    {
        set
        {
            if (value.ToUpper().Equals("TRUE"))
                ddlSetDetails.AutoPostBack = true;
        }
    }

    protected void OnTextChangedEvent(EventArgs e)
    {
        if (TextChangedEvent != null)
            TextChangedEvent(this, e);
    }

    protected void ddlSetDetails_TextChanged(object sender, EventArgs e)
    {
        OnTextChangedEvent(e);
    }

    public string SelectedSetDetails
    {
        get
        {
            return ddlSetDetails.SelectedValue;
        }
        set
        {
            if (value == string.Empty)
            {
                ddlSetDetails.SelectedIndex = -1;
                ddlSetDetails.ClearSelection();
                ddlSetDetails.Text = string.Empty;
                return;
            }
            _selectedValue = Int32.Parse(value);
            ddlSetDetails.SelectedIndex = -1;
            foreach (RadComboBoxItem item in ddlSetDetails.Items)
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
        ddlSetDetails.DataSource = PhoenixRegistersSetDetails.ListSetDetails();
        ddlSetDetails.DataBind();
        foreach (RadComboBoxItem item in ddlSetDetails.Items)
        {
            if (item.Value == _selectedValue.ToString())
            {
                item.Selected = true;
                break;
            }
        }
    }
    protected void ddlSetDetails_DataBound(object sender, EventArgs e)
    {
        if (_appenddatabounditems)
            ddlSetDetails.Items.Insert(0, new RadComboBoxItem("--Select--", "Dummy"));
    } 
}
