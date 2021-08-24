using System;
using System.Data;
using System.Web.UI.WebControls;
using SouthNests.Phoenix.Common;
using Telerik.Web.UI;

public partial class UserControlDocmentFields : System.Web.UI.UserControl
{
    private bool _appenddatabounditems;
    private int _selectedValue = -1;

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            bind();
        }
    }

    public DataSet DocmentFieldsList
    {
        set
        {
            ddlDocumentFields.Items.Clear();
            ddlDocumentFields.DataSource = value;
            ddlDocumentFields.DataBind();
        }
    }
    public bool  AppendDataBoundItems
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
            ddlDocumentFields.CssClass = value;
        }
    }


    public bool  AutoPostBack
    {
        set
        {
             ddlDocumentFields.AutoPostBack = value;
        }
    }



    public string SelectedDocmentFields
    {
        get
        {
            return ddlDocumentFields.SelectedValue;
        }
        set
        {
            if (value == string.Empty)
            {
                ddlDocumentFields.SelectedIndex = -1;
                ddlDocumentFields.ClearSelection();
                ddlDocumentFields.Text = string.Empty;
                return;
            }
            _selectedValue = Int32.Parse(value);
            ddlDocumentFields.SelectedIndex = -1;
            foreach (RadComboBoxItem item in ddlDocumentFields.Items)
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
        ddlDocumentFields.DataSource = PhoenixCommanDocuments.ListDocumentNumberFields(null);
        ddlDocumentFields.DataBind();
    }
    protected void ddlDocumentFields_DataBound(object sender, EventArgs e)
    {
        if (_appenddatabounditems)
            ddlDocumentFields.Items.Insert(0, new RadComboBoxItem("--Select--", ""));
    } 
}


