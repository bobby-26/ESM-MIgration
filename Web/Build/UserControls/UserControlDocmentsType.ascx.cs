using System;
using System.Data;
using System.Configuration;
using System.Collections.Generic;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Data.SqlClient;
using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.Registers;
using SouthNests.Phoenix.Common;
using Telerik.Web.UI;

public partial class UserControlDocmentsType : System.Web.UI.UserControl
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

    public DataSet DocmentsTypeList
    {
        set
        {
            ddlDocumentType.Items.Clear();
            ddlDocumentType.DataSource = value;
            ddlDocumentType.DataBind();
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
            ddlDocumentType.CssClass = value;
        }
    }


    public bool AutoPostBack
    {
        set
        {
             ddlDocumentType.AutoPostBack = value;
        }
    }



    public string SelectedDocumentType
    {
        get
        {
            return ddlDocumentType.SelectedValue;
        }
        set
        {
            if (value == string.Empty)
            {
                ddlDocumentType.SelectedIndex = -1;
                ddlDocumentType.ClearSelection();
                ddlDocumentType.Text = string.Empty;
                return;
            }
            _selectedValue = Int32.Parse(value);
            ddlDocumentType.SelectedIndex = -1;
            foreach (RadComboBoxItem item in ddlDocumentType.Items)
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
        ddlDocumentType.DataSource = PhoenixCommanDocuments.ListDocumentType();
        ddlDocumentType.DataBind();
        foreach (RadComboBoxItem item in ddlDocumentType.Items)
        {
            if (item.Value == _selectedValue.ToString())
            {
                item.Selected = true;
                break;
            }
        }
    }
     protected void ddlDocumentType_DataBound(object sender, EventArgs e)
    {
        if (_appenddatabounditems)
            ddlDocumentType.Items.Insert(0, new RadComboBoxItem("--Select--", ""));
    } 
}


