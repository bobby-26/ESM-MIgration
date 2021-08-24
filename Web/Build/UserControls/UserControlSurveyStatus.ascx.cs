using System;
using System.Data;
using System.Web.UI.WebControls;
using Telerik.Web.UI;

public partial class UserControlSurveyStatus : System.Web.UI.UserControl
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

    public DataSet StatusList
    {
        set
        {
            ucSurveyStatus.Items.Clear();
            ucSurveyStatus.DataSource = value;
            ucSurveyStatus.DataBind();
        }
    }

    public string CssClass
    {
        set
        {
            ucSurveyStatus.CssClass = value;
        }
    }

    public bool AppendDataBoundItems
    {
        set
        {
           _appenddatabounditems = value;

        }
    }
    public bool Enabled
    {
        set
        {
            ucSurveyStatus.Enabled = value;
        }
    }
    public bool AutoPostBack
    {
        set
        {            
            ucSurveyStatus.AutoPostBack = value;
        }
    }

    public string SelectedStatus
    {
        get
        {

            return ucSurveyStatus.SelectedValue;
        }
        set
        {
            if (value.Trim().Equals("") || value == null)
            {
                ucSurveyStatus.ClearSelection();
                ucSurveyStatus.Text = string.Empty;
                return;
            }
            ucSurveyStatus.SelectedValue = value;
        }
    }
    private void bind()
    {
        ucSurveyStatus.DataSource = PhoenixPlannedMaintenanceSurveySchedule.SurveyStatusList();
        ucSurveyStatus.DataBind();
        foreach (RadComboBoxItem item in ucSurveyStatus.Items)
        {
            if (item.Value == _selectedValue.ToString())
            {
                item.Selected = true;
                break;
            }
        }
    }
    public int SelectedValue
    {
        get
        {

            return (int.Parse(ucSurveyStatus.SelectedValue));
        }
        set
        {
            _selectedValue = value;
            ucSurveyStatus.SelectedIndex = -1;
            foreach (RadComboBoxItem item in ucSurveyStatus.Items)
            {
                if (item.Value == _selectedValue.ToString())
                {
                    item.Selected = true;
                    break;
                }
            }
        }
    }

    protected void ucSurveyStatus_DataBound(object sender, EventArgs e)
    {
        if (_appenddatabounditems)
            ucSurveyStatus.Items.Insert(0, new RadComboBoxItem("--Select--", ""));
    }
    public string Width
    {
        get
        {
            return ucSurveyStatus.Width.ToString();
        }
        set
        {
            ucSurveyStatus.Width = Unit.Parse(value);
        }
    }

    protected void OnTextChangedEvent(EventArgs e)
    {
        if (TextChangedEvent != null)
            TextChangedEvent(this, e);
    }

    protected void ucSurveyStatus_TextChanged(object sender, EventArgs e)
    {
        OnTextChangedEvent(e);
    }

}
