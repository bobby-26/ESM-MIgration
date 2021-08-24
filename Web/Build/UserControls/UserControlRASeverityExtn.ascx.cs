using System;
using System.Data;
using System.Web.UI.WebControls;
using SouthNests.Phoenix.Inspection;
using Telerik.Web.UI;

public partial class UserControlRASeverityExtn : System.Web.UI.UserControl
{
    private bool _appenddatabounditems;
    private int _selectedValue = -1;

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
            bind();
    }

    public DataSet SeverityList
    {
        set
        {
            ddlSeverity.Items.Clear();
            ddlSeverity.DataSource = value;
            ddlSeverity.DataBind();
        }
    }

    public string CssClass
    {
        set
        {
            ddlSeverity.CssClass = value;
        }
    }
    public bool Enabled
    {
        set
        {
            ddlSeverity.Enabled = value;
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

    public string SelectedSeverity
    {
        get
        {
            return ddlSeverity.SelectedValue;
        }
        set
        {
            if (value == string.Empty)
            {
                ddlSeverity.SelectedIndex = -1;
                ddlSeverity.ClearSelection();
                ddlSeverity.Text = string.Empty;
                return;
            }
            _selectedValue = Int32.Parse(value);
            ddlSeverity.SelectedIndex = -1;
            foreach (RadComboBoxItem item in ddlSeverity.Items)
            {
                if (item.Value == _selectedValue.ToString())
                {
                    item.Selected = true;
                    break;
                }
            }
        }
    }

    public int SelectedValue
    {
        get
        {
            return (int.Parse(ddlSeverity.SelectedValue));
        }
        set
        {
            _selectedValue = value;
            ddlSeverity.SelectedIndex = -1;
            foreach (RadComboBoxItem item in ddlSeverity.Items)
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
        ddlSeverity.DataSource = PhoenixInspectionRiskAssessmentSeverityExtn.ListRiskAssessmentSeverity();
        ddlSeverity.DataBind();
        foreach (RadComboBoxItem item in ddlSeverity.Items)
        {
            if (item.Value == _selectedValue.ToString())
            {
                item.Selected = true;
                break;
            }
        }
    }
    protected void ddlSeverity_DataBound(object sender, EventArgs e)
    {
        if (_appenddatabounditems)
            ddlSeverity.Items.Insert(0, new RadComboBoxItem("--Select--", "Dummy"));
    }

    public Unit Width
    {
        get
        {
            return ddlSeverity.Width;
        }
        set
        {
            ddlSeverity.Width = value;

        }
    }

}