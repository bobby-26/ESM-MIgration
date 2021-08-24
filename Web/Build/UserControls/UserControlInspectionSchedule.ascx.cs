using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.Inspection;
using Telerik.Web.UI;
public partial class UserControlInspectionSchedule : System.Web.UI.UserControl
{
    public event EventHandler TextChangedEvent;
    private bool _appenddatabounditems;
    private string _selectedValue = string.Empty;
    Guid? inspectionscheduleid = null;
    string vesselid = "";
    string inspectionschedulestatus = null;

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            bind();
        }
    }

    public DataSet InspectionScheduleList
    {
        set
        {
            ucInspectionSchedule.Items.Clear();
            ucInspectionSchedule.DataSource = value;
            ucInspectionSchedule.DataBind();
        }
    }

    public string Width
    {
        get
        {
            return ucInspectionSchedule.Width.ToString();
        }
        set
        {
            ucInspectionSchedule.Width = Unit.Parse(value);
        }
    }

    public string InspectionScheduleStatus
    {
        get
        {
            return inspectionschedulestatus.ToString();
        }
        set
        {
            inspectionschedulestatus = value.ToString();
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
            ucInspectionSchedule.CssClass = value;
        }
    }

    public string AutoPostBack
    {
        set
        {
            if (value.ToUpper().Equals("TRUE"))
                ucInspectionSchedule.AutoPostBack = true;
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

    public string SelectedInspectionSchedule
    {
        get
        {
            return ucInspectionSchedule.SelectedValue;
        }
        set
        {
            if (value == string.Empty)
            {
                ucInspectionSchedule.SelectedIndex = -1;
                ucInspectionSchedule.ClearSelection();
                ucInspectionSchedule.Text = string.Empty;
                return;
            }
            _selectedValue = value.ToString();
            ucInspectionSchedule.SelectedIndex = -1;
            foreach (RadComboBoxItem item in ucInspectionSchedule.Items)
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
            return ucInspectionSchedule.SelectedValue;
        }
        set
        {
            _selectedValue = value;
            ucInspectionSchedule.SelectedIndex = -1;
            foreach (RadComboBoxItem item in ucInspectionSchedule.Items)
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
        ucInspectionSchedule.SelectedIndex = -1;
        ucInspectionSchedule.DataSource = PhoenixInspectionSchedule.ListInspectionSchedule(inspectionscheduleid, General.GetNullableInteger(vesselid), inspectionschedulestatus);
        ucInspectionSchedule.DataBind();
        foreach (RadComboBoxItem item in ucInspectionSchedule.Items)
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

    protected void ucInspectionSchedule_TextChanged(object sender, EventArgs e)
    {
        OnTextChangedEvent(e);
    }

    protected void ucInspectionSchedule_DataBound(object sender, EventArgs e)
    {
        if (_appenddatabounditems)
            ucInspectionSchedule.Items.Insert(0, new RadComboBoxItem("--Select--", "Dummy"));
    }

    public bool Enabled
    {
        set
        {
            ucInspectionSchedule.Enabled = value;
        }
    }
}
