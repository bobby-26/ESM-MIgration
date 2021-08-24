using SouthNests.Phoenix.CrewManagement;
using SouthNests.Phoenix.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Telerik.Web.UI;
public partial class UserControls_UserControlInstituteBatchList : System.Web.UI.UserControl
{
    public event EventHandler TextChangedEvent;
    private bool _appenddatabounditems;
    private string _selectedValue = null;
    string courseInstituteid;
    string batchstatus;
   
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            bind();
        }
    }
    protected void ddlBatchList_DataBound(object sender, EventArgs e)
    {
        if (_appenddatabounditems)
            ddlBatchList.Items.Insert(0, new RadComboBoxItem("--Select--", "Dummy"));
    }

    public void OnTextChangedEvent(EventArgs e)
    {
        if (TextChangedEvent != null)
            TextChangedEvent(this, e);
    }
    protected void ddlBatchList_TextChanged(object sender, EventArgs e)
    {
        OnTextChangedEvent(e);
    }

    public string SelectedBatch
    {
        get
        {
            return ddlBatchList.SelectedValue;
        }
        set
        {
            if (value == string.Empty)
            {
                ddlBatchList.SelectedIndex = -1;
                ddlBatchList.ClearSelection();
                ddlBatchList.Text = string.Empty;
                return;
            }
            _selectedValue = value;
            ddlBatchList.SelectedIndex = -1;
            foreach (RadComboBoxItem item in ddlBatchList.Items)
            {
                if (item.Value == _selectedValue)
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
            return (ddlBatchList.SelectedValue);
        }
        set
        {
            _selectedValue = value;
            ddlBatchList.SelectedIndex = -1;
            foreach (RadComboBoxItem item in ddlBatchList.Items)
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
        ddlBatchList.DataSource = PhoenixCrewInstituteBatch.CrewInstituteBatchList(General.GetNullableGuid(courseInstituteid));
        ddlBatchList.DataBind();
        foreach (RadComboBoxItem item in ddlBatchList.Items)
        {
            if (item.Value == _selectedValue)
            {
                item.Selected = true;
                break;
            }
        }
    }

    public bool Enabled
    {
        set
        {
            ddlBatchList.Enabled = value;
        }
    }
    public string CssClass
    {
        set
        {
            ddlBatchList.CssClass = value;
        }
    }
    public string CourseInstituteId
    {
        get
        {
            return courseInstituteid;
        }
        set
        {
            courseInstituteid = value;
        }
    }
    public string BatchStatus
    {
        get
        {
            return batchstatus;
        }
        set
        {
            batchstatus = value;
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
                ddlBatchList.AutoPostBack = true;
        }
    } 
}