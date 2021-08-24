using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.Registers;
using SouthNests.Phoenix.Inspection;
using Telerik.Web.UI;

public partial class UserControlInspection : System.Web.UI.UserControl
{
    int? inspectiontypeid = null;
    int? inspectioncategoryid = null;
    int? externalaudittype = null;
    public event EventHandler TextChangedEvent;
    private bool _appenddatabounditems;
    private string _selectedValue = string.Empty;

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            bind();
        }
    }

    public DataSet InspectionList
    {
        set
        {
            ddlInspection.Items.Clear();
            ddlInspection.DataSource = value;
            ddlInspection.DataBind();
        }
    }

    public string InspectionType
    {
        get
        {
            return inspectiontypeid.ToString();
        }
        set
        {
            inspectiontypeid = General.GetNullableInteger(value);
        }
    }

    public string InspectionCategory
    {
        get
        {
            return inspectioncategoryid.ToString();
        }
        set
        {
            inspectioncategoryid = General.GetNullableInteger(value);
        }
    }

    public string ExternalAuditType
    {
        get
        {
            return externalaudittype.ToString();
        }
        set
        {
            externalaudittype = General.GetNullableInteger(value);
        }
    }

    public string CssClass
    {
        set
        {
            ddlInspection.CssClass = value;
        }
    }    

    public bool Enabled
    {
        set
        {
            ddlInspection.Enabled = value;
        }
        get
        {
            return ddlInspection.Enabled;
        }
    }

    public new bool Visible
    {
        set
        {
            ddlInspection.Visible = value;
        }
        get
        {
            return ddlInspection.Visible;
        }
    }

    public string AutoPostBack
    {
        set
        {
            if (value.ToUpper().Equals("TRUE"))
                ddlInspection.AutoPostBack = true;
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

    public string SelectedInspection
    {
        get
        {

            return ddlInspection.SelectedValue;
        }
        set
        {
            if (value == string.Empty)
            {
                ddlInspection.SelectedIndex = -1;
                ddlInspection.ClearSelection();
                ddlInspection.Text = string.Empty;
                return;
            }
            _selectedValue = value.ToString();
            ddlInspection.SelectedIndex = -1;
            foreach (RadComboBoxItem item in ddlInspection.Items)
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
            return ddlInspection.SelectedValue;
        }
        set
        {
            _selectedValue = value;
            ddlInspection.SelectedIndex = -1;
            foreach (RadComboBoxItem item in ddlInspection.Items)
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
        ddlInspection.SelectedIndex = -1;
        ddlInspection.DataSource = PhoenixInspection.ListInspection(inspectiontypeid, inspectioncategoryid, externalaudittype);
        ddlInspection.DataBind();
        foreach (RadComboBoxItem item in ddlInspection.Items)
        {
            if (item.Value == _selectedValue.ToString())
            {
                item.Selected = true;
                break;
            }
        }
    }
    protected void OnTextChangedEvent(EventArgs e)
    {
        if (TextChangedEvent != null)
            TextChangedEvent(this, e);
    }

    protected void ddlInspection_TextChanged(object sender, EventArgs e)
    {
        OnTextChangedEvent(e);
    }

    protected void ddlInspection_DataBound(object sender, EventArgs e)
    {
        if (_appenddatabounditems)
            ddlInspection.Items.Insert(0, new RadComboBoxItem("--Select--", "Dummy"));
    }
    public string Width
    {
        get
        {
            return ddlInspection.Width.ToString();
        }
        set
        {            
            ddlInspection.Width = Unit.Parse(value);
        }
    }
}
