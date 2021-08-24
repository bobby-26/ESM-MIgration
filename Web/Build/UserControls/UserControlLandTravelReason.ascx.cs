using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.Registers;
using SouthNests.Phoenix.CrewManagement;
using Telerik.Web.UI;
public partial class UserControlLandTravelReason : System.Web.UI.UserControl
{
    public event EventHandler TextChangedEvent;
    private bool _appenddatabounditems;
    private string _selectedValue = string.Empty;
    string _reasonfor = null;

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
            bind();
    }

    public DataSet ReasonList
    {
        set
        {
            ddllandtravelreason.Items.Clear();
            ddllandtravelreason.DataSource = value;
            ddllandtravelreason.DataBind();
        }
    }
    public string ReasonFor
    {
        set
        {
            _reasonfor = value;
        }
    }
    public string CssClass
    {
        set
        {
            ddllandtravelreason.CssClass = value;
        }
    }
    public bool Enabled
    {
        set
        {
            ddllandtravelreason.Enabled = value;
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

    public string SelectedReason
    {
        get
        {
            return ddllandtravelreason.SelectedValue;
        }
        set
        {
            if (value == string.Empty)
            {
                ddllandtravelreason.SelectedIndex = -1;
                ddllandtravelreason.ClearSelection();
                ddllandtravelreason.Text = string.Empty;
                return;
            }
            _selectedValue = value.ToString();
            ddllandtravelreason.SelectedIndex = -1;
            foreach (RadComboBoxItem item in ddllandtravelreason.Items)
            {
                if (item.Value == _selectedValue.ToString())
                {
                    item.Selected = true;
                    break;
                }
            }
        }
    }

    public string SelectedValue
    {
        get
        {
            return ddllandtravelreason.SelectedValue;
        }
        set
        {
            _selectedValue = value;
            ddllandtravelreason.SelectedIndex = -1;
            foreach (RadComboBoxItem item in ddllandtravelreason.Items)
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
        ddllandtravelreason.DataSource = PhoenixRegistersTravelReason.ListLandTravelReason();
        ddllandtravelreason.DataBind();
        foreach (RadComboBoxItem item in ddllandtravelreason.Items)
        {
            if (item.Value == _selectedValue.ToString())
            {
                item.Selected = true;
                break;
            }
        }
    }
    protected void ddllandtravelreason_DataBound(object sender, EventArgs e)
    {
        if (_appenddatabounditems)
            ddllandtravelreason.Items.Insert(0, new RadComboBoxItem("--Select--", "Dummy"));
    }
    protected void ddlLandTravelReason_TextChanged(object sender, EventArgs e)
    {
        OnTextChangedEvent(e);
    }
    protected void OnTextChangedEvent(EventArgs e)
    {
        if (TextChangedEvent != null)
            TextChangedEvent(this, e);
    }
    public string AutoPostBack
    {
        set
        {
            if (value.ToUpper().Equals("TRUE"))
                ddllandtravelreason.AutoPostBack = true;
        }
    }

    public string Width
    {
        get
        {
            return ddllandtravelreason.Width.ToString();
        }
        set
        {
            ddllandtravelreason.Width = Unit.Parse(value);
        }
    }
}
