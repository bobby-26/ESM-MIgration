using SouthNests.Phoenix.Common;
using System;
using System.Data;
using System.Web.UI.WebControls;
using Telerik.Web.UI;

public partial class UserControlVesselApplication : System.Web.UI.UserControl
{
    public event EventHandler TextChangedEvent;

    private bool _appenddatabounditems;
    private int _selectedValue=-1;
    
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            Bind();
        }
    }

    public DataSet VesselList
    {
        set
        {
            ddlVessel.Items.Clear();
            ddlVessel.DataSource = value;
            ddlVessel.DataBind();
            foreach (RadComboBoxItem item in ddlVessel.Items)
            {
                if (item.Value == _selectedValue.ToString())
                {
                    item.Selected = true;
                    break;
                }
            }
        }
    }

    public bool AppendDataBoundItems
    {
        set
        {            
            _appenddatabounditems = value;
        }
    }
    public bool VesselsOnly
    {
        get;
        set;
    }    

    public string CssClass
    {
        set
        {
            ddlVessel.CssClass = value;
        }
        get { return ddlVessel.CssClass; }
    }

    public bool AutoPostBack
    {
        set
        {
            ddlVessel.AutoPostBack = value;
        }
    }

    public string SelectedVesselName
    {
        get
        {
            return ddlVessel.SelectedItem.Text;
        }
    }

    public string SelectedVessel
    {
        get
        {
            return ddlVessel.SelectedValue;
        }
        set
        {
            if (value == string.Empty || value== "Dummy")
            {
                ddlVessel.SelectedIndex = -1;
                ddlVessel.ClearSelection();
                ddlVessel.Text = string.Empty;
                return;
            }
            _selectedValue = Int32.Parse(value);
            ddlVessel.SelectedIndex = -1;
            foreach (RadComboBoxItem item in ddlVessel.Items)
            {
                if (item.Value == _selectedValue.ToString())
                {
                    item.Selected = true;
                    break;
                }
            }

        }
    }
    public PhoenixApplication Module
    {
        get;
        set;
    }
    public bool Enabled
    {
        set
        {
            ddlVessel.Enabled = value;
        }
    }

    public int SelectedValue
    {
        get
        {
            return (int.Parse(ddlVessel.SelectedValue));
        }
        set
        {
            _selectedValue = value;
            ddlVessel.SelectedIndex = -1;
            foreach (RadComboBoxItem item in ddlVessel.Items)
            {
                if (item.Value == _selectedValue.ToString())
                {
                    item.Selected = true;
                    break;
                }
            }
        }
    }
    public void Bind()
    {

        ddlVessel.DataSource = PhoenixCommonRegisters.ListApplicationVessel((int)Module);
        ddlVessel.DataBind();

        foreach (RadComboBoxItem item in ddlVessel.Items)
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

    protected void ddlVessel_TextChanged(object sender, EventArgs e)
    {
        OnTextChangedEvent(e);
    }

    protected void ddlVessel_DataBound(object sender, EventArgs e)
    {
        if (_appenddatabounditems)
            ddlVessel.Items.Insert(0, new RadComboBoxItem("--Select--", ""));
       
    }

    public Unit Width
    {
        get
        {
            return ddlVessel.Width;
        }
        set
        {
            ddlVessel.Width = value;
            
        }
    }
    public Unit Height
    {
        get
        {
            return ddlVessel.Height;
        }
        set
        {
            ddlVessel.Height = value;
        }
    }   
}
