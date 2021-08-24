using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.Inspection;
using Telerik.Web.UI;

public partial class UserControlAuditSchedule : System.Web.UI.UserControl
{
    public event EventHandler TextChangedEvent;
    private bool _appenddatabounditems;
    private string _selectedValue = string.Empty;
    Guid? auditscheduleid = null;
    string vesselid = "";
    string auditschedulestatus = null;

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            bind();
        }
    }

    public DataSet AuditScheduleList
    {
        set
        {
            ucAuditSchedule.Items.Clear();
            ucAuditSchedule.DataSource = value;
            ucAuditSchedule.DataBind();
        }
    }

    public string Width
    {
        get
        {
            return ucAuditSchedule.Width.ToString();
        }
        set
        {
            ucAuditSchedule.Width = Unit.Parse(value);
        }
    }

    public string AuditScheduleStatus
    {
        get
        {
            return auditschedulestatus.ToString();
        }
        set
        {
            auditschedulestatus = value.ToString();
        }
    }

    public string VesselId
    {
        get
        {
            return vesselid.ToString();
        }
        set
        {
            vesselid = value.ToString();
        }
    }

    public string CssClass
    {
        set
        {
            ucAuditSchedule.CssClass = value;
        }
    }

    public string AutoPostBack
    {
        set
        {
            if (value.ToUpper().Equals("TRUE"))
                ucAuditSchedule.AutoPostBack = true;
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

    public string SelectedAuditSchedule
    {
        get
        {

            return ucAuditSchedule.SelectedValue;
        }
        set
        {
            ucAuditSchedule.SelectedIndex = -1;

            if (value == string.Empty || General.GetNullableInteger(value) == null)
            {
                ucAuditSchedule.SelectedIndex = -1;
                ucAuditSchedule.ClearSelection();
                ucAuditSchedule.Text = string.Empty;
                return;
            }
            _selectedValue = value.ToString();
            foreach ( RadComboBoxItem item in ucAuditSchedule.Items)
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
            return ucAuditSchedule.SelectedValue;
        }
        set
        {
            _selectedValue = value;
            ucAuditSchedule.SelectedIndex = -1;
            foreach (RadComboBoxItem item in ucAuditSchedule.Items)
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
        ucAuditSchedule.SelectedIndex = -1;
        ucAuditSchedule.DataSource = PhoenixInspectionAuditSchedule.ListAuditSchedule(PhoenixSecurityContext.CurrentSecurityContext.UserCode, auditscheduleid, General.GetNullableInteger(vesselid), auditschedulestatus);
        ucAuditSchedule.DataBind();
        foreach (RadComboBoxItem item in ucAuditSchedule.Items)
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

    protected void ucAuditSchedule_TextChanged(object sender, EventArgs e)
    {
        OnTextChangedEvent(e);
    }

    protected void ucAuditSchedule_DataBound(object sender, EventArgs e)
    {
        if (_appenddatabounditems)
            ucAuditSchedule.Items.Insert(0, new RadComboBoxItem("--Select--", "Dummy"));
    }

    public bool Enabled
    {
        set
        {
            ucAuditSchedule.Enabled = value;
        }
    }
}
