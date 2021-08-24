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

public partial class UserControlTravelReason : System.Web.UI.UserControl
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
            ddltravelreason.Items.Clear();
            ddltravelreason.DataSource = value;
            ddltravelreason.DataBind();
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
            ddltravelreason.CssClass = value;
        }
    }
    public bool Enabled
    {
        set
        {
            ddltravelreason.Enabled = value;
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
    public Unit Width
    {
        set
        {
            ddltravelreason.Width = Unit.Parse(value.ToString());
        }
        get
        {
            return ddltravelreason.Width;
        }
    }
    public string SelectedReason
    {
        get
        {
            return ddltravelreason.SelectedValue;
        }
        set
        {
            if (value == string.Empty)
            {
                ddltravelreason.SelectedIndex = -1;
                ddltravelreason.ClearSelection();
                ddltravelreason.Text = string.Empty;
                return;
            }
            _selectedValue = value.ToString();
            ddltravelreason.SelectedIndex = -1;
            foreach (RadComboBoxItem item in ddltravelreason.Items)
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
            return ddltravelreason.SelectedValue;
        }
        set
        {
            _selectedValue = value;
            ddltravelreason.SelectedIndex = -1;
            foreach (RadComboBoxItem item in ddltravelreason.Items)
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
        ddltravelreason.DataSource = PhoenixCrewTravelRequest.ListTravelReason(General.GetNullableInteger(_reasonfor));
        ddltravelreason.DataBind();
        foreach (RadComboBoxItem item in ddltravelreason.Items)
        {
            if (item.Value == _selectedValue.ToString())
            {
                item.Selected = true;
                break;
            }
        }
    }
    protected void ddltravelreason_DataBound(object sender, EventArgs e)
    {
        if (_appenddatabounditems)
            ddltravelreason.Items.Insert(0, new RadComboBoxItem("--Select--", "Dummy"));
    }
    protected void ddlTravelReason_TextChanged(object sender, EventArgs e)
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
                ddltravelreason.AutoPostBack = true;
        }
    }
}
