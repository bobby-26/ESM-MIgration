using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.Registers;
using Telerik.Web.UI;
public partial class UserControlVesselType : System.Web.UI.UserControl
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

    public DataSet VesselTypeList
    {
        set
        {
            ddlVesselType.Items.Clear();
            ddlVesselType.DataSource = value;
            ddlVesselType.DataBind();
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

    public bool Enabled
    {
        set
        {
            ddlVesselType.Enabled = value;
        }
    }

    public string CssClass
    {
        set
        {
            ddlVesselType.CssClass = value;
        }
    }


    public string AutoPostBack
    {
        set
        {
            if (value.ToUpper().Equals("TRUE"))
                ddlVesselType.AutoPostBack = true;
        }
    }

    public string SelectedVesseltype
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
            _selectedValue = Int32.Parse(value);
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

    private void bind()
    {
        ddlVesselType.DataSource = PhoenixRegistersVesselType.ListVesselType(0);
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

    protected void ddlVesselType_DataBound(object sender, EventArgs e)
    {
        if (_appenddatabounditems)
            ddlVesselType.Items.Insert(0, new RadComboBoxItem("--Select--", "Dummy"));
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

    public string Width
    {
        get
        {
            return ddlVesselType.Width.ToString();
        }
        set
        {
            ddlVesselType.Width = Unit.Parse(value);
        }
    }
}
