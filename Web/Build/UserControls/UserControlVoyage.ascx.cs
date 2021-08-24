using System;
using System.Data;
using System.Web.UI.WebControls;
using SouthNests.Phoenix.VesselPosition;
using Telerik.Web.UI;

public partial class UserControlVoyage : System.Web.UI.UserControl
{
    public event EventHandler TextChangedEvent;
    private bool _appenddatabounditems;
    private string _selectedValue = "";
    private int? vesselid = null;
    private int? listclosed = null;

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            bind();
        }
    }

    public DataSet VoyageList
    {
        set
        {
            ddlVoyage.DataBind();
            ddlVoyage.Items.Clear();
            ddlVoyage.DataSource = value;
            ddlVoyage.DataBind();
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

    public string CssClass
    {
        set
        {
            ddlVoyage.CssClass = value;
        }
    }

    public string AutoPostBack
    {
        set
        {
            if (value.ToUpper().Equals("TRUE"))
                ddlVoyage.AutoPostBack = true;
        }
    }

    public string SelectedVoyage
    {
        get
        {
            return ddlVoyage.SelectedValue;
        }
        set
        {
            if (value == string.Empty)
            {
                ddlVoyage.SelectedIndex = -1;
                ddlVoyage.ClearSelection();
                ddlVoyage.Text = string.Empty;
                return;
            }
            _selectedValue = value;
            ddlVoyage.SelectedIndex = -1;
            foreach (RadComboBoxItem item in ddlVoyage.Items)
            {
                if (item.Value == _selectedValue.ToString())
                {
                    item.Selected = true;
                    break;
                }
            }
        }
    }

    public string Enabled
    {
        set
        {

            if (value.ToUpper().Equals("TRUE"))
                ddlVoyage.Enabled = true;
            else
                ddlVoyage.Enabled = false;
        }
    }

    public int? VesselId
    {
        get
        {
            return vesselid;
        }
        set
        {
            vesselid = value;
        }
    }

    public int? ListClosed
    {
        get
        {
            return listclosed;
        }
        set
        {
            listclosed = value;
        }
    }

    public void bind()
    {
        ddlVoyage.DataSource = PhoenixVesselPositionVoyageData.ListVoyageData(
            vesselid
            , listclosed);

        ddlVoyage.DataBind();
        foreach (RadComboBoxItem item in ddlVoyage.Items)
        {
            if (item.Value == _selectedValue.ToString())
            {
                item.Selected = true;
                break;
            }
        }
    }

    public string SelectedValue
    {
        get
        {
            return ddlVoyage.SelectedValue.ToString();
        }
        set
        {
            _selectedValue = value;
            ddlVoyage.SelectedIndex = -1;
            foreach (RadComboBoxItem item in ddlVoyage.Items)
            {
                if (item.Value == _selectedValue.ToString())
                {
                    item.Selected = true;
                    break;
                }
            }
        }
    }

    protected void OnTextChangedEvent(EventArgs e)
    {
        if (TextChangedEvent != null)
            TextChangedEvent(this, e);
    }

    protected void ddlVoyage_TextChanged(object sender, EventArgs e)
    {
        OnTextChangedEvent(e);
    }

    protected void ddlVoyage_DataBound(object sender, EventArgs e)
    {
        if (_appenddatabounditems)
            ddlVoyage.Items.Insert(0, new RadComboBoxItem("--Select--", "Dummy"));
    } 
}
