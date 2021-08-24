using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.Registers;
using SouthNests.Phoenix.CrewOffshore;
using Telerik.Web.UI;
public partial class UserControlDMRCharter : System.Web.UI.UserControl
{
    public event EventHandler TextChangedEvent;
    private bool _appenddatabounditems;
    private bool _corporate;
    private int? vesselid = null;
    private int _selectedValue = -1;

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            foreach (RadComboBoxItem item in ddlDMRCharter.Items)
            {
                if (item.Value == _selectedValue.ToString())
                {
                    item.Selected = true;
                    break;
                }
            }

        }
    }

  

    public DataSet CharterList
    {
        set
        {
            ddlDMRCharter.Items.Clear();
            ddlDMRCharter.DataSource = value;
            ddlDMRCharter.DataBind();

            foreach (RadComboBoxItem item in ddlDMRCharter.Items)
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

    public bool AppendCorporate
    {
        set
        {
            _corporate = value;
        }
    }

    public string CssClass
    {
        set
        {
            ddlDMRCharter.CssClass = value;
        }
    }

    public bool AutoPostBack
    {
        set
        {
            ddlDMRCharter.AutoPostBack = value;
        }
    }

    public bool Enabled
    {
        set
        {
            ddlDMRCharter.Enabled = value;
        }
    }

    protected void OnTextChangedEvent(EventArgs e)
    {
        if (TextChangedEvent != null)
            TextChangedEvent(this, e);
    }

    protected void ddlDMRCharter_TextChanged(object sender, EventArgs e)
    {
        OnTextChangedEvent(e);
    }

    public string SelectedCharter
    {
        get
        {

            return ddlDMRCharter.SelectedItem.Text;
        }
        set
        {
            ddlDMRCharter.SelectedIndex = -1;
            if (value != null && value != "" && value != "0")
                ddlDMRCharter.SelectedItem.Text = value;
        }
    }
    public string SelectedValue
    {
        get
        {
            return ddlDMRCharter.SelectedValue;
        }
        set
        {
            if (value == string.Empty)
            {
                ddlDMRCharter.SelectedIndex = -1;
                ddlDMRCharter.ClearSelection();
                ddlDMRCharter.Text = string.Empty;
                return;
            }
            _selectedValue = Int32.Parse(value);
            ddlDMRCharter.SelectedIndex = -1;
            foreach (RadComboBoxItem item in ddlDMRCharter.Items)
            {
                if (item.Value == _selectedValue.ToString())
                {
                    item.Selected = true;
                    break;
                }
            }
        }

    }

    protected void ddlDMRCharter_DataBound(object sender, EventArgs e)
    {
        if (_appenddatabounditems)
        {
            if (!_corporate)
            {
                ddlDMRCharter.Items.Insert(0, new RadComboBoxItem("--Select--", "Dummy"));
            }
            else
                ddlDMRCharter.Items.Insert(0, new RadComboBoxItem("--Manager--", "Dummy"));
        }
    }

    public string Width
    {
        get
        {
            return ddlDMRCharter.Width.ToString();
        }
        set
        {
            ddlDMRCharter.Width = Unit.Parse(value);
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

}
