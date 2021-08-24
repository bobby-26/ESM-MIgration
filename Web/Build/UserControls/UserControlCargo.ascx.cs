using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.Registers;
using Telerik.Web.UI;
public partial class UserControlCargo : System.Web.UI.UserControl
{
    public event EventHandler TextChangedEvent;
    private bool _appenddatabounditems;
    private string _selectedValue = "";
    private string _cargotype = "";
    private int? vesselid = null;

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            bind();
        }
    }

    public DataSet CargoList
    {
        set
        {
            ddlCargo.DataBind();
            ddlCargo.Items.Clear();
            ddlCargo.DataSource = value;
            ddlCargo.DataBind();
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
            ddlCargo.CssClass = value;
        }
    }

    public string AutoPostBack
    {
        set
        {
            if (value.ToUpper().Equals("TRUE"))
                ddlCargo.AutoPostBack = true;
        }
    }

    public string SelectedCargo
    {
        get
        {
            return ddlCargo.SelectedValue;
        }
        set
        {
            if (value == string.Empty)
            {
                ddlCargo.SelectedIndex = -1;
                ddlCargo.ClearSelection();
                ddlCargo.Text = string.Empty;
                return;
            }
            _selectedValue = value;
            ddlCargo.SelectedIndex = -1;
            foreach (RadComboBoxItem item in ddlCargo.Items)
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
                ddlCargo.Enabled = true;
            else
                ddlCargo.Enabled = false;
        }
    }

    public void bind()
    {
        ddlCargo.DataSource = PhoenixRegistersCargo.ListCargo(General.GetNullableGuid(_cargotype), vesselid);
        ddlCargo.DataBind();

        foreach (RadComboBoxItem item in ddlCargo.Items)
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
            return ddlCargo.SelectedValue.ToString();
        }
        set
        {
            _selectedValue = value;
            ddlCargo.SelectedIndex = -1;
            foreach (RadComboBoxItem item in ddlCargo.Items)
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

    protected void ddlCargo_TextChanged(object sender, EventArgs e)
    {
        OnTextChangedEvent(e);
    }

    protected void ddlCargo_DataBound(object sender, EventArgs e)
    {
        if (_appenddatabounditems)
            ddlCargo.Items.Insert(0, new RadComboBoxItem("--Select--", "Dummy"));
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
}
