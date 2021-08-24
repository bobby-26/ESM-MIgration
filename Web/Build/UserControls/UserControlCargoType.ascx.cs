using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.Registers;
using Telerik.Web.UI;
public partial class UserControlCargoType : System.Web.UI.UserControl
{
    string showyesno = "1";
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

    public string CargoTypeShowYesNo
    {
        get
        {
            return showyesno;
        }
        set
        {
            showyesno = value;
        }
    }
    public DataSet CargoTypeList
    {
        set
        {
            ddlCargoType.DataBind();
            ddlCargoType.Items.Clear();
            ddlCargoType.DataSource = value;
            ddlCargoType.DataBind();
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
            ddlCargoType.CssClass = value;
        }
    }


    public string AutoPostBack
    {
        set
        {
            if (value.ToUpper().Equals("TRUE"))
                ddlCargoType.AutoPostBack = true;
        }
    }


    public string SelectedCargoType
    {
        get
        {

            return ddlCargoType.SelectedValue;
        }
        set
        {
            if (value == string.Empty)
            {
                ddlCargoType.SelectedIndex = -1;
                ddlCargoType.ClearSelection();
                ddlCargoType.Text = string.Empty;
                return;
            }
            _selectedValue = value;
            ddlCargoType.SelectedIndex = -1;
            foreach (RadComboBoxItem item in ddlCargoType.Items)
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
                ddlCargoType.Enabled = true;
            else
                ddlCargoType.Enabled = false;
        }
    }
    public void bind()
    {
        ddlCargoType.DataSource = PhoenixRegistersCargo.ListCargoType(General.GetNullableInteger(showyesno));
        ddlCargoType.DataBind();
        foreach (RadComboBoxItem item in ddlCargoType.Items)
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
            return ddlCargoType.SelectedValue.ToString();
        }
        set
        {
            _selectedValue = value;
            ddlCargoType.SelectedIndex = -1;
            foreach (RadComboBoxItem item in ddlCargoType.Items)
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

    protected void ddlCargoType_TextChanged(object sender, EventArgs e)
    {
        OnTextChangedEvent(e);
    }

    protected void ddlCargoType_DataBound(object sender, EventArgs e)
    {
        if (_appenddatabounditems)
            ddlCargoType.Items.Insert(0, new RadComboBoxItem("--Select--", "Dummy"));
    } 
}
