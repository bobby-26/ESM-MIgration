using System;
using System.Data;
using System.Web.UI;
using System.Web.UI.WebControls;
using SouthNests.Phoenix.Registers;
using Telerik.Web.UI;

public partial class UserControls_UserControlQualification : System.Web.UI.UserControl
{
    private bool _appenddatabounditems;
    private int _selectedValue = -1;

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!Page.IsPostBack)
        {
            bind();
        }
    }

    public DataSet QualificationList
    {
        set
        {
            ddlQualification.DataSource = value;
            ddlQualification.DataBind();
        }
    }

    public string CssClass
    {
        set
        {
            ddlQualification.CssClass = value;
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

    public string AutoPostBack
    {
        set
        {
            if (value.ToUpper().Equals("TRUE"))
                ddlQualification.AutoPostBack = true;
        }
    }

    public string SelectedQualification
    {
        get
        {
            return ddlQualification.SelectedValue;
        }
        set
        {
            if (value == string.Empty)
            {
                ddlQualification.SelectedIndex = -1;
                ddlQualification.ClearSelection();
                ddlQualification.Text = string.Empty;
                return;
            }
            _selectedValue = Int32.Parse(value);
            ddlQualification.SelectedIndex = -1;
            foreach (RadComboBoxItem item in ddlQualification.Items)
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
        ddlQualification.DataSource = PhoenixRegistersDocumentQualification.ListDocumentQualification();
        ddlQualification.DataBind();
        foreach (RadComboBoxItem item in ddlQualification.Items)
        {
            if (item.Value == _selectedValue.ToString())
            {
                item.Selected = true;
                break;
            }
        }
    }
    protected void ddlQualification_DataBound(object sender, EventArgs e)
    {
        if (_appenddatabounditems)
            ddlQualification.Items.Insert(0, new RadComboBoxItem("--Select--", "Dummy"));
    } 
}
