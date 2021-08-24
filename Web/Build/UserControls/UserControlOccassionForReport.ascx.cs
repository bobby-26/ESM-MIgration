using System;
using System.Data;
using System.Web.UI.WebControls;
using SouthNests.Phoenix.Registers;
using SouthNests.Phoenix.CrewManagement;
using SouthNests.Phoenix.Framework;
using Telerik.Web.UI;
public partial class UserControlOccassionForReport : System.Web.UI.UserControl
{   
    public event EventHandler TextChangedEvent;
    private bool _appenddatabounditems;
    private int _selectedValue = -1;
    string rankcategory = string.Empty;
    string activeyn = string.Empty;
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            bind();
        }
    }

    public void bind()
    {

        int? _category = null;
        if (General.GetNullableInteger(rankcategory) == null)        
            _category = null;        
        else        
            _category = int.Parse(rankcategory);
        
        ddlOccassionForReport.DataSource = PhoenixRegistersMiscellaneousAppraisalOccasion.ListMiscellaneousAppraisalOccasion(_category, General.GetNullableInteger(activeyn));


        ddlOccassionForReport.DataBind();
        foreach (RadComboBoxItem item in ddlOccassionForReport.Items)
        {
            if (item.Value == _selectedValue.ToString())
            {
                item.Selected = true;
                break;
            }
        }
    }

    public string Category
    {
        set { rankcategory = value; }
    }

    public string ActiveYN
    {
        set { activeyn = value; }
    }

    public DataSet OccassionList
    {
        set
        {
            ddlOccassionForReport.DataSource = value;
            ddlOccassionForReport.DataBind();
        }
    }

    public string  SelectedOccassion
    {
        get
        {

            return ddlOccassionForReport.SelectedValue;
        }
        set
        {
            if (value == string.Empty)
            {
                ddlOccassionForReport.SelectedIndex = -1;
                ddlOccassionForReport.ClearSelection();
                ddlOccassionForReport.Text = string.Empty;
                return;
            }
            _selectedValue = Int32.Parse(value);
            ddlOccassionForReport.SelectedIndex = -1;
            foreach (RadComboBoxItem item in ddlOccassionForReport.Items)
            {
                if (item.Value == _selectedValue.ToString())
                {
                    item.Selected = true;
                    break;
                }
            }
        }
    }

    public string CssClass
    {
        set
        {
            ddlOccassionForReport.CssClass = value;
        }
    }

    public bool AppendDataBoundItems
    {
        set
        {
             _appenddatabounditems = value;
        }
    }

    public bool AutoPostBack
    {
        set
        {
            ddlOccassionForReport.AutoPostBack = value;
        }
    }

    protected void ddlOccassionForReport_DataBound(object sender, EventArgs e)
    {
        if (_appenddatabounditems)
            ddlOccassionForReport.Items.Insert(0, new RadComboBoxItem("--Select--", "Dummy"));
    }

    protected void OnTextChangedEvent(EventArgs e)
    {
        if (TextChangedEvent != null)
            TextChangedEvent(this, e);
    }
    public bool Enabled
    {
        set
        {
            ddlOccassionForReport.Enabled = value;
        }
    }
}
