using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.Registers;
using Telerik.Web.UI;

public partial class UserControlVesselCode : System.Web.UI.UserControl
{
    public event EventHandler TextChangedEvent;
    private bool _appenddatabounditems;
    private string _selectedValue = "Dummy";

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            bind();
        }
    }

    public DataSet VesseCodelList
    {
        set
        {
            ddlVesselCode.Items.Clear();
            ddlVesselCode.DataSource = value;
            ddlVesselCode.DataBind();
            foreach (RadComboBoxItem item in ddlVesselCode.Items)
            {
                if (item.Value == _selectedValue)
                {
                    item.Selected = true;
                    break;
                }
            }
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
            ddlVesselCode.CssClass = value;
        }
        get { return ddlVesselCode.CssClass; }
    }

    public string AutoPostBack
    {
        set
        {
            if (value.ToUpper().Equals("TRUE"))
                ddlVesselCode.AutoPostBack = true;
        }
    }

    public string SelectedVesselCode
    {
        get
        {
            return ddlVesselCode.SelectedValue;
        }
        set
        {
            if (value == string.Empty || value == "Dummy")
            {
                ddlVesselCode.SelectedIndex = -1;
                ddlVesselCode.ClearSelection();
                ddlVesselCode.Text = string.Empty;
                return;
            }
            _selectedValue = value;
            ddlVesselCode.SelectedIndex = -1;
            foreach (RadComboBoxItem item in ddlVesselCode.Items)
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
            ddlVesselCode.Enabled = value;
        }
    }

    public int VesselId
    {
        get;
        set;
    }

    public string SelectedValue
    {
        get
        {
            return ddlVesselCode.SelectedValue;
        }
        set
        {
            _selectedValue = value;
            ddlVesselCode.SelectedIndex = -1;
            foreach (RadComboBoxItem item in ddlVesselCode.Items)
            {
                if (item.Value == _selectedValue.ToString())
                {
                    item.Selected = true;
                    break;
                }
            }
        }
    }

    public void bind()
    {
        ddlVesselCode.DataSource = PhoenixRegistersVessel.ListVesselCode(General.GetNullableInteger(VesselId.ToString()));
        ddlVesselCode.DataBind();


        foreach (RadComboBoxItem item in ddlVesselCode.Items)
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

    protected void ddlVesselCode_TextChanged(object sender, EventArgs e)
    {
        OnTextChangedEvent(e);
    }

    protected void ddlVesselCode_DataBound(object sender, EventArgs e)
    {
        if (_appenddatabounditems)
            ddlVesselCode.Items.Insert(0, new RadComboBoxItem("--Select--", "Dummy"));
    }
}
