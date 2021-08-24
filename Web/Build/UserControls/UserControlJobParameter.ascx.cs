using SouthNests.Phoenix.PlannedMaintenance;
using System;
using System.Data;
using Telerik.Web.UI;

public partial class UserControlJobParameter : System.Web.UI.UserControl
{    
    private int _selectedValue = -1;

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            bind();
        }
    }

    public DataTable JobParameterList
    {
        set
        {
            ddlJobParameter.Items.Clear();
            ddlJobParameter.DataSource = value;
            ddlJobParameter.DataBind();
        }
    }
    public bool AppendDataBoundItems
    {
        set;
        get;
    }

    public string CssClass
    {
        set
        {
            ddlJobParameter.CssClass = value;
        }
    }


    public bool AutoPostBack
    {
        set
        {            
            ddlJobParameter.AutoPostBack = value;
        }
    }

    public string SelectedJobParameter
    {
        get
        {
            return ddlJobParameter.SelectedValue;
        }
        set
        {
            if (value == string.Empty)
            {
                ddlJobParameter.SelectedIndex = -1;
                ddlJobParameter.ClearSelection();
                ddlJobParameter.Text = string.Empty;
                return;
            }
            _selectedValue = Int32.Parse(value);
            ddlJobParameter.SelectedIndex = -1;
            foreach (RadComboBoxItem item in ddlJobParameter.Items)
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
        ddlJobParameter.DataSource = PhoenixPlannedMaintenanceJobParameter.List(1);
        ddlJobParameter.DataBind();
        foreach (RadComboBoxItem item in ddlJobParameter.Items)
        {
            if (item.Value == _selectedValue.ToString())
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
            ddlJobParameter.Enabled = value;
        }
    }
    protected void ddlJobParameter_DataBound(object sender, EventArgs e)
    {
        if (AppendDataBoundItems)
            ddlJobParameter.Items.Insert(0, new RadComboBoxItem("--Select--", ""));
    }
}


