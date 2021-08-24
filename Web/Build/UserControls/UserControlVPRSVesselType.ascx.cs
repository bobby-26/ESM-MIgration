using System;
using System.Data;
using System.Web.UI.WebControls;
using SouthNests.Phoenix.Registers;
using Telerik.Web.UI;
public partial class UserControlVPRSVesselType : System.Web.UI.UserControl
{
    public event EventHandler TextChangedEvent;
    private bool _appenddatabounditems;
    private string _selectedValue = "";

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            bind();
        }
    }

    public DataSet VesselTypeList
    {
        set
        {
            ddlVesselType.DataBind();
            ddlVesselType.Items.Clear();
            ddlVesselType.DataSource = value;
            ddlVesselType.DataBind();
        }
    }

    public bool AppendDataBoundItems
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
            ddlVesselType.CssClass = value;
        }
    }

    public bool AutoPostBack
    {
        set
        {
            ddlVesselType.AutoPostBack = true;
        }
    }

    public string SelectedVesselType
    {
        get
        {
            return ddlVesselType.SelectedValue;
        }
        set
        {
            if (value == string.Empty)
            {
                ddlVesselType.SelectedIndex = -1;
                ddlVesselType.ClearSelection();
                ddlVesselType.Text = string.Empty; 
                return;
            }
            _selectedValue = value;
            ddlVesselType.SelectedIndex = -1;
            foreach (RadComboBoxItem item in ddlVesselType.Items)
            {
                if (item.Value == _selectedValue.ToString())
                {
                    item.Selected = true;
                    break;
                }
            }
        }
    }

    public bool Enabled
    {
        set
        {
            ddlVesselType.Enabled = value;
        }
    }

    public void bind()
    {
        ddlVesselType.DataSource = PhoenixRegistersVPRSVesselType.ListVesselType();
        ddlVesselType.DataBind();
        foreach (RadComboBoxItem item in ddlVesselType.Items)
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

    protected void ddlVesselType_TextChanged(object sender, EventArgs e)
    {
        OnTextChangedEvent(e);
    }

    protected void ddlVesselType_DataBound(object sender, EventArgs e)
    {
        if (_appenddatabounditems)
            ddlVesselType.Items.Insert(0, new RadComboBoxItem("--Select--", "Dummy"));
    }
}
