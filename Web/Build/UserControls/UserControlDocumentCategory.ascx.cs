using System;
using System.Data;
using System.Web.UI;
using System.Web.UI.WebControls;
using SouthNests.Phoenix.Registers;
using SouthNests.Phoenix.Common;
using SouthNests.Phoenix.Framework;
using Telerik.Web.UI;

public partial class UserControlDocumentCategory : System.Web.UI.UserControl
{
    public event EventHandler TextChangedEvent;
    private string _selectedValue = "";    

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            bind();
        }
    }
    public DataTable DocumentCategoryList
    {
        set
        {
            ddlDocumentCategory.Items.Clear();
            ddlDocumentCategory.DataSource = value;
            ddlDocumentCategory.DataBind();
            foreach (RadComboBoxItem item in ddlDocumentCategory.Items)
            {
                if (item.Value == _selectedValue.ToString())
                {
                    item.Selected = true;
                    break;
                }
            }
        }
    }   
    public string CssClass
    {
        set
        {
            ddlDocumentCategory.CssClass = value;
        }
        get { return ddlDocumentCategory.CssClass; }
    }

    public string AutoPostBack
    {
        set
        {
            if (value.ToUpper().Equals("TRUE"))
                ddlDocumentCategory.AutoPostBack = true;
        }
    }

    public string SelectedDocumentCategoryName
    {
        get
        {
            return ddlDocumentCategory.SelectedItem.Text;
        }
    }
    public string SelectedDocumentCategoryID
    {
        get
        {
            return ddlDocumentCategory.SelectedValue;
        }
        set
        {
            if (value == string.Empty || value == "Dummy")
            {
                ddlDocumentCategory.SelectedIndex = -1;
                ddlDocumentCategory.ClearSelection();
                ddlDocumentCategory.Text = string.Empty;
                return;
            }
            _selectedValue = value;
            ddlDocumentCategory.SelectedIndex = -1;
            foreach (RadComboBoxItem item in ddlDocumentCategory.Items)
            {
                if (item.Value == _selectedValue.ToString())
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
            return ddlDocumentCategory.SelectedValue.ToString();
        }
        set
        {
            _selectedValue = value;
            ddlDocumentCategory.SelectedIndex = -1;
            foreach (RadComboBoxItem item in ddlDocumentCategory.Items)
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
        ddlDocumentCategory.DataSource = PhoenixRegistersDocumentCategory.ListDocumentCategory(PhoenixSecurityContext.CurrentSecurityContext.UserCode
            , null
            , null
            , null);
        ddlDocumentCategory.DataBind();

        foreach (RadComboBoxItem item in ddlDocumentCategory.Items)
        {
            if (item.Value == _selectedValue.ToString())
            {
                item.Selected = true;
                break;
            }
        }
    }

    protected void ddlDocumentCategory_DataBound(object sender, EventArgs e)
    {
            ddlDocumentCategory.Items.Insert(0, new RadComboBoxItem("--Select--", "Dummy"));     
    }
    protected void OnTextChangedEvent(EventArgs e)
    {
        if (TextChangedEvent != null)
            TextChangedEvent(this, e);
    }
    protected void ddlDocumentCategory_TextChanged(object sender, EventArgs e)
    {
        OnTextChangedEvent(e);
    }
    public Unit Width
    {
        set
        {
            ddlDocumentCategory.Width = Unit.Parse(value.ToString());
        }
        get
        {
            return ddlDocumentCategory.Width;
        }
    }
}
